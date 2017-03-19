using System;
using System.Net;
using System.Net.Sockets;

namespace BISS.Networking
{
	/// <summary>
	/// Base class for the Receiver and Sender classes. This class handles the raw network stuff.
	/// </summary>
	public abstract class Base
	{
		/// <summary>
		/// Port used for the BISS protocol.
		/// </summary>
		protected const int Port = 15000;

		/// <summary>
		/// Endpoint used for broadcasting.
		/// </summary>
		protected IPEndPoint BroadcastEndPoint;

		protected Base()
		{
			this.BroadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, Port);
		}

		/// <summary>
		/// Returns a new instance of <see cref="UdpClient"/>.
		/// </summary>
		/// <param name="bindIPAddress">The IP address which is used for the local endpoint.
		/// <see cref="IPAddress.Any"/> is used if this parameter is not set.</param>
		/// <returns>Instance of UdpClient.</returns>
		protected UdpClient CreateClient(IPAddress bindIPAddress = null)
		{
			if (bindIPAddress == null)
				bindIPAddress = IPAddress.Any;

			UdpClient result = new UdpClient();
			// Set ReuseAddress to support multiple receivers.
			result.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			result.Client.Bind(new IPEndPoint(bindIPAddress, Port));

			return result;
		}

		/// <summary>
		/// Check if the IP address speficied in <paramref name="ipAddress"/> is useable.
		/// Only IPv4 addresses are supported. Loopback and multicast addresses are not supported.
		/// </summary>
		/// <param name="ipAddress">IP address to be checked.</param>
		/// <returns>TRUE if the IP address is usable.</returns>
		protected bool IsUsableIPAddress(IPAddress ipAddress)
		{
			// UDP broadcast is only supported in IPv4.
			bool usable = ipAddress.AddressFamily == AddressFamily.InterNetwork;

			// Loopback addresses are not allowed either.
			usable = usable && !IPAddress.IsLoopback(ipAddress);

			// Same with link multicast addresses
			if (usable)
			{
				// Multicast addresses are defined by the four MSB of the address.
				// So get the first byte and shift it four bits to the right.
				byte[] bytes = ipAddress.GetAddressBytes();
				usable = bytes[0] >> 4 != 14;
			}

			return usable;
		}
	}
}
