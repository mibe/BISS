using System;

namespace BISS.Networking
{
	/// <summary>
	/// Provides methods for building a valid packet of the BISS protocol for sending
	/// over the network.
	/// </summary>
	public class PacketBuilder
	{
		static PacketBuilder instance;
		readonly Random random;

		public PacketBuilder()
		{
			this.random = new Random();
		}

		/// <summary>
		/// Gets an instance of PacketBuilder.
		/// </summary>
		public static PacketBuilder Instance
		{
			get
			{
				if (instance == null)
					instance = new PacketBuilder();

				return instance;
			}
		}

		/// <summary>
		/// Builds a packet of the BISS protocol using the message type specified
		/// in <paramref name="messageType"/>.
		/// </summary>
		/// <param name="messageType">Message type for this packet.</param>
		/// <returns>Packet with the specified information encapsulated.</returns>
		public Packet Build(MessageType messageType)
		{
			byte[] rand = new byte[2];
			this.random.NextBytes(rand);

			return new Packet(messageType, BitConverter.ToUInt16(rand, 0));
		}
	}
}
