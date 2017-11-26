using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		Bootloader = 6
	}
}
