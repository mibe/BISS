using System.Drawing;

namespace BISS.Hardware.Blinky
{
	public class Settings
	{
		public Color Color
		{ get; set; }

		public byte BlinkInterval
		{ get; set; }

		public byte BlinkTimeout
		{ get; set; }

		public Settings(Color color, byte blinkInterval, byte blinkTimeout)
		{
			this.Color = color;
			this.BlinkInterval = blinkInterval;
			this.BlinkTimeout = blinkTimeout;
		}

		public Settings(byte r, byte g, byte b, byte blinkInterval, byte blinkTimeout)
			: this(Color.FromArgb(r, g, b), blinkInterval, blinkTimeout)
		{ }
	}
}
