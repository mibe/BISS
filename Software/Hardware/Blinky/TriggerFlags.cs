using System;

namespace BISS.Hardware.Blinky
{
	[Flags()]
	public enum TriggerFlags : byte
	{
		TouchSensor = 1,
		Timeout = 2,
		AuxOutput = 4
	}
}
