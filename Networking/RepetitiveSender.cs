using System;
using System.Net.Sockets;

namespace BISS.Networking
{
	/// <summary>
	/// Transmits a packet more than once over the network.
	/// </summary>
	public class RepetitiveSender : Sender
	{
		/// <summary>
		/// Gets or sets the number of repetitions of the transmission.
		/// </summary>
		public uint Repetitions
		{ get; set; }

		/// <summary>
		/// Gets or sets the number of seconds waited between transmissions.
		/// </summary>
		public uint Delay
		{ get; set; }

		/// <summary>
		/// Gets the total number of transmissions.
		/// <remarks>This is naturally one more than the number of repetitions.</remarks>
		/// </summary>
		public uint Transmissions
		{
			get
			{
				return this.Repetitions + 1;
			}
		}

		public RepetitiveSender()
		{
			// Defaults
			this.Repetitions = 9;
			this.Delay = 1;
		}

		/// <summary>
		/// Repetitively transmit the packet in <paramref name="packet"/>.
		/// </summary>
		/// <param name="packet">Packet, which should be transmitted.</param>
		public override void Send(Packet packet)
		{
			if (packet == null)
				throw new ArgumentNullException("packet");

			// Generate a UDP client used for all transmisions.
			using (UdpClient client = new UdpClient())
			{
				for (int a = 0; a < Transmissions; a++)
				{
					base.Send(client, packet);

					// No delay on the last repetition.
					if (a + 1 < Transmissions && Delay != 0)
						System.Threading.Thread.Sleep((int)Delay * 1000);
				}
			}
		}
	}
}
