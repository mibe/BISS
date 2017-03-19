using System;
using System.Net.Sockets;

namespace BISS.Networking
{
	/// <summary>
	/// Receives a packet.
	/// </summary>
	public class Receiver : Base
	{
		readonly UdpClient client;
		IAsyncResult asyncResult;

		/// <summary>
		/// Occurs when a packet was successfully received.
		/// </summary>
		public event EventHandler<PacketReceivedEventArgs> PacketReceived;

		/// <summary>
		/// Occurs when received data was invalid.
		/// </summary>
		public event EventHandler ErrorReceived;

		public Receiver()
		{
			this.client = CreateClient();
		}

		/// <summary>
		/// Start the receiving of packets.
		/// </summary>
		public void StartReceiving()
		{
			this.asyncResult = this.client.BeginReceive(receive, new object());
		}

		private void receive(IAsyncResult asyncResult)
		{
			byte[] datagram = this.client.EndReceive(asyncResult, ref this.BroadcastEndPoint);

			// Convert the received bytes into a packet
			Packet receivedPacket = Packet.Parse(datagram);

			// Is it a valid packet?
			if (receivedPacket != null)
				OnPacketReceived(receivedPacket);
			else
				OnErrorReceived();

			// Start receiving again
			StartReceiving();
		}

		/// <summary>
		/// Raises the <see cref="PacketReceived"/> event.
		/// </summary>
		/// <param name="receivedPacket">Packet which was received.</param>
		protected virtual void OnPacketReceived(Packet receivedPacket)
		{
			if (receivedPacket == null)
				throw new ArgumentNullException("receivedPacket");

			if (PacketReceived != null)
				PacketReceived(this, new PacketReceivedEventArgs(receivedPacket));
		}

		/// <summary>
		/// Raises the <see cref="ErrorReceived"/> event.
		/// </summary>
		protected virtual void OnErrorReceived()
		{
			if (ErrorReceived != null)
				ErrorReceived(this, EventArgs.Empty);
		}
	}
}
