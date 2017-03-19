using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace BISS.Networking
{
	/// <summary>
	/// Transmits a packet over the network.
	/// </summary>
	public class Sender : Base
	{
		/// <summary>
		/// Transmits the specified packet over the network.
		/// </summary>
		/// <param name="packet">Packet to be transmitted.</param>
		/// <param name="ipAddress">IP address of the local endpoint.</param>
		/// <returns>TRUE if the packet was successfully sent.</returns>
		public virtual bool Send(Packet packet, IPAddress ipAddress)
		{
			if (packet == null)
				throw new ArgumentNullException("packet");
			if (ipAddress == null)
				throw new ArgumentNullException("ipAddress");

			if (!IsUsableIPAddress(ipAddress))
				throw new ArgumentException(String.Format("The specified IP address ({0}) is not usable.", ipAddress),
					"ipAddress");

			using (UdpClient client = CreateClient(ipAddress))
			{
				client.EnableBroadcast = true;
				client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);

				return Send(client, packet);
			}
		}

		/// <summary>
		/// Transmit the specified packet over the network using the UDP client in <paramref name="client"/>.
		/// </summary>
		/// <param name="client">UDP client used for transmitting.</param>
		/// <param name="packet">Packet to be transmitted.</param>
		/// <returns>TRUE if the packet was successfully sent.</returns>
		protected bool Send(UdpClient client, Packet packet)
		{
			if (client == null)
				throw new ArgumentNullException("client");

			// Generate the raw byte data and send them
			byte[] data = packet.GenerateDatagram();
			int sent = client.Send(data, data.Length, this.BroadcastEndPoint);

			return data.Length == sent;
		}
	}
}
