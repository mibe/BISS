using System;
using System.Collections.Generic;
using System.Drawing;

namespace BISS.Hardware.Blinky
{
	public class Settings : IEquatable<Settings>
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

		/// <summary>
		/// Determines whether the specified object is equal to the current instance.
		/// </summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>TRUE if the specified object is equal to the current object; otherwise, FALSE.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			return Equals(obj as Settings);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>TRUE if the current object is equal to the <paramref name="other"/> parameter;
		/// otherwise, FALSE.</returns>
		public bool Equals(Settings other)
		{
			if (other == null)
				return false;

			return other.BlinkInterval == BlinkInterval &&
				other.BlinkTimeout == BlinkTimeout &&
				other.Color == Color;
		}

		public override int GetHashCode()
		{
			var hashCode = -1424713856;
			hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(Color);
			hashCode = hashCode * -1521134295 + BlinkInterval.GetHashCode();
			hashCode = hashCode * -1521134295 + BlinkTimeout.GetHashCode();
			return hashCode;
		}

		/// <summary>
		/// Defines an implicit conversion of a Settings instance to a <see cref="Byte"/> array.
		/// </summary>
		/// <param name="instance">The Settings instance to convert.</param>
		public static implicit operator byte[](Settings instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			byte[] result = new byte[5];
			result[0] = instance.Color.R;
			result[1] = instance.Color.G;
			result[2] = instance.Color.B;
			result[3] = instance.BlinkInterval;
			result[4] = instance.BlinkTimeout;

			return result;
		}
	}
}
