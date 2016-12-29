using System;

namespace BISS.Networking
{
	/// <summary>
	/// Provides data for the <see cref="Receiver.PacketReceived"/> event.
	/// </summary>
	public class PacketReceivedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the packet which was received.
		/// </summary>
		public Packet ReceivedPacket
		{ get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PacketReceivedEventArgs"/> class.
		/// </summary>
		/// <param name="receivedPacket">Instance of the packet which was received.</param>
		public PacketReceivedEventArgs(Packet receivedPacket)
		{
			if (receivedPacket == null)
				throw new ArgumentNullException("receivedPacket");

			this.ReceivedPacket = receivedPacket;
		}
	}
}
