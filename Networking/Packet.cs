namespace BISS.Networking
{
	/// <summary>
	/// Represents a single transmission packet of the BISS protocol.
	/// </summary>
	public class Packet
	{
		readonly MessageType messageType;
		readonly ushort packetIdentifier;

		/// <summary>
		/// First byte of the packet
		/// </summary>
		const byte StartOfPacket = 0x02;	// STX;

		/// <summary>
		/// Last byte of the packet
		/// </summary>
		const byte EndOfPacket = 0x03;		// ETX;

		/// <summary>
		/// Protocol version used
		/// </summary>
		const byte ProtocolVersion = 0x01;

		/// <summary>
		/// Magic string
		/// </summary>
		public const string Magic = "BISS";

		public Packet(MessageType messageType, ushort packetIdentifier)
		{
			this.messageType = messageType;
			this.packetIdentifier = packetIdentifier;
		}

		/// <summary>
		/// Generates the raw bytes of the packet for transmission over the network.
		/// </summary>
		/// <returns>Raw bytes of the packet.</returns>
		internal byte[] GenerateDatagram()
		{
			byte[] data = new byte[10];

			// Start
			data[0] = StartOfPacket;

			// Magic word
			data[1] = (byte)Magic[0];
			data[2] = (byte)Magic[1];
			data[3] = (byte)Magic[2];
			data[4] = (byte)Magic[3];

			// Used protocol version
			data[5] = ProtocolVersion;

			// Packet identifier
			data[6] = (byte)(this.packetIdentifier >> 8);
			data[7] = (byte)(this.packetIdentifier & 0xFF);

			// Message type
			data[8] = (byte)this.messageType;

			// End
			data[9] = EndOfPacket;

			return data;
		}
	}
}
