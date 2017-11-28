using System;

namespace BISS.Hardware.Blinky
{
	/// <summary>
	/// Specifies options available to the Trigger command.
	/// </summary>
	[Flags()]
	public enum TriggerOptions : byte
	{
		/// <summary>
		/// All options a deactivated.
		/// </summary>
		None = 0,
		/// <summary>
		/// Enables the touch sensor input to disable the blink algorithm.
		/// </summary>
		TouchSensor = 1,
		/// <summary>
		/// Enables the timeout of the blink algorithm.
		/// </summary>
		Timeout = 2,
		/// <summary>
		/// Enables the auxiliary output signal.
		/// </summary>
		AuxOutput = 4
	}
}
