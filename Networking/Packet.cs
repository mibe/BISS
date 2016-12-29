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
		/// Gets the message type of this packet.
		/// </summary>
		public MessageType MessageType
		{
			get
			{
				return this.messageType;
			}
		}

		/// <summary>
		/// Gets the identifier of this packet.
		/// </summary>
		public ushort PacketIdentifier
		{
			get
			{
				return this.packetIdentifier;
			}
		}

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

		/// <summary>
		/// Initializes a new instance of the <see cref="Packet"/> class with the specified
		/// message type and packet identifier.
		/// </summary>
		/// <param name="messageType">Message type of this packet.</param>
		/// <param name="packetIdentifier">Packet identifier of this packet.</param>
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

		/// <summary>
		/// Converts the raw data bytes into a packet.
		/// </summary>
		/// <param name="datagram">Raw bytes representing the packet.</param>
		/// <returns>Instance of <see cref="Packet"/> or NULL if the conversion was unsuccessfull.</returns>
		public static Packet Parse(byte[] datagram)
		{
			// Wrong length
			if (datagram.Length != 10)
				return null;

			// Wrong start byte
			if (datagram[0] != StartOfPacket)
				return null;

			// Wrong magic bytes
			if (datagram[1] != (byte)Magic[0] || datagram[2] != (byte)Magic[1]
				|| datagram[3] != (byte)Magic[2] || datagram[4] != (byte)Magic[3])
				return null;

			// Unsupported protocol version
			if (datagram[5] != ProtocolVersion)
				return null;

			// Wrong end byte
			if (datagram[9] != EndOfPacket)
				return null;

			MessageType messageType = (MessageType)datagram[8];
			ushort packetIdentifier = (ushort)(datagram[6] << 8);
			packetIdentifier += datagram[7];

			return new Packet(messageType, packetIdentifier);
		}
	}
}
