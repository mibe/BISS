using System;
using System.Net;
using System.Net.Sockets;

namespace BISS.Networking
{
	/// <summary>
	/// Base class for the Receiver and Sender classes. This class handles the raw network stuff.
	/// </summary>
	public abstract class Socket
	{
		/// <summary>
		/// Port used for the BISS protocol.
		/// </summary>
		protected const int Port = 15000;

		/// <summary>
		/// Endpoint used for broadcasting.
		/// </summary>
		protected IPEndPoint EndPoint;

		protected Socket()
		{
			// FIXME: Determine Broadcast address of local subnet.
			this.EndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.255"), Port);
		}

		/// <summary>
		/// Returns a new instance of <see cref="UdpClient"/>.
		/// </summary>
		/// <param name="bindPort">Determines if the UdpClient instance should be binded to the port.</param>
		/// <returns>Instance of UdpClient.</returns>
		protected UdpClient CreateClient(bool bindPort = false)
		{
			if (bindPort)
				return new UdpClient(Port);
			else
				return new UdpClient();
		}
	}
}
