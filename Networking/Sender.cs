using System;
using System.Net;
using System.Net.Sockets;

namespace BISS.Networking
{
	/// <summary>
	/// Transmits a packet over the network.
	/// </summary>
	public class Sender
	{
		readonly IPEndPoint endpoint;

		public Sender()
		{
			// FIXME: Determine Broadcast address of local subnet.
			this.endpoint = new IPEndPoint(IPAddress.Parse("192.168.1.255"), 15000);
		}

		/// <summary>
		/// Transmits the specified packet over the network.
		/// </summary>
		/// <param name="packet">Packet to be transmitted.</param>
		public virtual void Send(Packet packet)
		{
			if (packet == null)
				throw new ArgumentNullException("packet");

			using (UdpClient client = new UdpClient())
			{
				Send(client, packet);
			}
		}

		/// <summary>
		/// Transmit the specified packet over the network using the UDP client in <paramref name="client"/>.
		/// </summary>
		/// <param name="client">UDP client used for transmitting.</param>
		/// <param name="packet">Packet to be transmitted.</param>
		protected void Send(UdpClient client, Packet packet)
		{
			if (client == null)
				throw new ArgumentNullException("client");

			// Generate the raw byte data and send them
			byte[] data = packet.GenerateDatagram();
			client.Send(data, data.Length, this.endpoint);
		}
	}
}
