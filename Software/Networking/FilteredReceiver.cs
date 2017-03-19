using System;
using System.Collections.Generic;

namespace BISS.Networking
{
	/// <summary>
	/// Receives a packet but checks if this packet was already received before.
	/// </summary>
	public class FilteredReceiver : Receiver
	{
		readonly List<ushort> receivedIdentifiers;

		/// <summary>
		/// Occurs when a received packet was filtered.
		/// </summary>
		public event EventHandler<PacketReceivedEventArgs> PacketFiltered;

		public FilteredReceiver()
			: base()
		{
			this.receivedIdentifiers = new List<ushort>();
		}

		/// <summary>
		/// Raises the <see cref="PacketFiltered"/> event.
		/// </summary>
		/// <param name="filteredPacket">Packet which was filtered.</param>
		protected virtual void OnPacketFiltered(Packet filteredPacket)
		{
			if (filteredPacket == null)
				throw new ArgumentNullException("filteredPacket");

			if (PacketFiltered != null)
				PacketFiltered(this, new PacketReceivedEventArgs(filteredPacket));
		}

		/// <summary>
		/// Raises the <see cref="Receiver.PacketReceived"/> event, but only if the packet in
		/// <paramref name="receivedPacket"/> was not already received before.
		/// </summary>
		/// <param name="receivedPacket">Packet which was received.</param>
		protected override void OnPacketReceived(Packet receivedPacket)
		{
			if (receivedPacket == null)
				throw new ArgumentNullException("receivedPacket");

			// Check if the packet identifier was already received before
			if (this.receivedIdentifiers.Contains(receivedPacket.PacketIdentifier))
			{
				OnPacketFiltered(receivedPacket);
				return;
			}
			else
			{
				this.receivedIdentifiers.Add(receivedPacket.PacketIdentifier);
				base.OnPacketReceived(receivedPacket);
			}
		}
	}
}
