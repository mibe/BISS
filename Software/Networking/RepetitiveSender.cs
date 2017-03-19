using System;
using System.Net;
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

		/// <summary>
		/// Initializes a new instance of the <see cref="RepetitiveSender"/> class with the
		/// specified repetitions and delay.
		/// </summary>
		/// <param name="repetitions">The number of repetitions.</param>
		/// <param name="delay">The delay in seconds between the repetitions.</param>
		public RepetitiveSender(uint repetitions, uint delay)
		{
			this.Repetitions = repetitions;
			this.Delay = delay;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RepetitiveSender"/> class. Nine repetitions
		/// and a delay of one second is used.
		/// </summary>
		public RepetitiveSender()
			: this(9, 1)
		{ }

		/// <summary>
		/// Repetitively transmit the packet in <paramref name="packet"/>.
		/// </summary>
		/// <param name="packet">Packet, which should be transmitted.</param>
		/// <param name="ipAddress">IP address of the local endpoint.</param>
		/// <returns>TRUE if all packets were successfully sent.</returns>
		public override bool Send(Packet packet, IPAddress ipAddress)
		{
			if (packet == null)
				throw new ArgumentNullException("packet");

			bool result = true;

			// Generate a UDP client used for all transmisions.
			using (UdpClient client = CreateClient(ipAddress))
			{
				for (int a = 0; a < Transmissions; a++)
				{
					bool tempResult = base.Send(client, packet);

					// only set result to false when previous results were true
					if (result)
						result = tempResult;

					// No delay after the last repetition.
					if (a + 1 < Transmissions && Delay != 0)
						System.Threading.Thread.Sleep((int)Delay * 1000);
				}
			}

			return result;
		}
	}
}
