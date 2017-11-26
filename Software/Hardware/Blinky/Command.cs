namespace BISS.Hardware.Blinky
{
	internal enum Command : byte
	{
		None = 0,
		Trigger = 1,
		SetSettings = 2,
		GetSettings = 3,
		SaveSettings = 4,
		ResetSettings = 5,
		Bootloader = 6,
		TurnOff = 7
	}
}
