namespace BISS.Hardware.Blinky
{
	/// <summary>
	/// Specifies commands available to send to the device.
	/// </summary>
	internal enum Command : byte
	{
		/// <summary>
		/// No command.
		/// </summary>
		None = 0,
		/// <summary>
		/// Trigger the blink algorithm.
		/// </summary>
		Trigger = 1,
		/// <summary>
		/// Set the device settings.
		/// </summary>
		SetSettings = 2,
		/// <summary>
		/// Get the device settings.
		/// </summary>
		GetSettings = 3,
		/// <summary>
		/// Save the device settings in the EEPROM of the device.
		/// </summary>
		SaveSettings = 4,
		/// <summary>
		/// Reset the device settings to the default values.
		/// </summary>
		ResetSettings = 5,
		/// <summary>
		/// Start the Bootloader for firmware update.
		/// </summary>
		Bootloader = 6,
		/// <summary>
		/// Turn off a triggered blink algorithm.
		/// </summary>
		TurnOff = 7,
		/// <summary>
		/// Request a heartbeat signal from the device.
		/// </summary>
		Ping = 8
	}
}
