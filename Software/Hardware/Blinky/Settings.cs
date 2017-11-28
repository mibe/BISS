using System.Drawing;

namespace BISS.Hardware.Blinky
{
	public class Settings
	{
		/// <summary>
		/// Gets or sets the color used by the blink algorithm.
		/// </summary>
		public Color Color
		{ get; set; }

		/// <summary>
		/// Gets or sets the blink rate. A value of zero means a fast blink rate, whereas a value of
		/// 255 means a slow blink rate.
		/// </summary>
		public byte BlinkInterval
		{ get; set; }

		/// <summary>
		/// Timeout in seconds before the blink algorithm is turned off. This value is offset by 45 seconds.
		/// This means a value of zero would imply a timeout of 45 seconds, while a value of 255 would imply
		/// a timeout of 300 seconds.
		/// </summary>
		public byte BlinkTimeout
		{ get; set; }

		/// <summary>
		/// Initializes a new instance of the Settings class.
		/// </summary>
		/// <param name="color">Color to be used in this instance.</param>
		/// <param name="blinkInterval">The blink rate to be used in this instance.</param>
		/// <param name="blinkTimeout">The timeout value to be used in this instance.</param>
		public Settings(Color color, byte blinkInterval, byte blinkTimeout)
		{
			this.Color = color;
			this.BlinkInterval = blinkInterval;
			this.BlinkTimeout = blinkTimeout;
		}

		/// <summary>
		/// Initializes a new instance of the Settings class.
		/// </summary>
		/// <param name="r">The red component value of the color of this instance.</param>
		/// <param name="g">The green component value of the color of this instance.</param>
		/// <param name="b">The blue component value of the color of this instance.</param>
		/// <param name="blinkInterval">The blink rate to be used in this instance.</param>
		/// <param name="blinkTimeout">The timeout value to be used in this instance.</param>
		public Settings(byte r, byte g, byte b, byte blinkInterval, byte blinkTimeout)
			: this(Color.FromArgb(r, g, b), blinkInterval, blinkTimeout)
		{ }
	}
}
