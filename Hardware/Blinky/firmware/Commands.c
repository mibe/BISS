#include "Commands.h"

static void Command_Trigger(uint8_t enableTouchSensor)
{
	Blinker_Enable(enableTouchSensor);
}

static void Command_SetSettings(uint8_t r, uint8_t g, uint8_t b, uint8_t blinkInterval)
{
	if (settings.Color.R != r || settings.Color.G != g || settings.Color.B != b)
	{
		settings.Color.R = r;
		settings.Color.G = g;
		settings.Color.B = b;
		Display_Update();
	}
	
	settings.BlinkInterval = blinkInterval;
}

static void Command_GetSettings(uint8_t* r, uint8_t* g, uint8_t* b, uint8_t* blinkInterval)
{
	*r = settings.Color.R;
	*g = settings.Color.G;
	*b = settings.Color.B;
	*blinkInterval = settings.BlinkInterval;
}

static void Command_SaveSettings(void)
{
	// Disable the blinker after saving, too.
	Settings_Save();
	Blinker_Disable();
}

static void Command_ResetSettings(void)
{
	Settings_Clear();
}

static void Command_Bootloader(void)
{
	Bootloader_Execute();
}

uint8_t Command_Handle(uint8_t* fromHost, uint8_t* toHost)
{
	// The command ID is always the first byte of the package.
	uint8_t cmdId = fromHost[0];
	
	// Clear array; this notation had the least memory impact.
	toHost[0] = cmdId;
	toHost[1] = 0;
	toHost[2] = 0;
	toHost[3] = 0;
	toHost[4] = 0;
	toHost[5] = 0;
	toHost[6] = 0;
	toHost[7] = 0;
	
	switch(cmdId)
	{
		case CMD_Trigger:
			Command_Trigger(fromHost[1]);
			break;
		case CMD_SetSettings:
			Command_SetSettings(fromHost[1], fromHost[2], fromHost[3], fromHost[4]);
			break;
		case CMD_GetSettings:
			Command_GetSettings(&toHost[1], &toHost[2], &toHost[3], &toHost[4]);
			break;
		case CMD_SaveSettings:
			Command_SaveSettings();
			break;
		case CMD_ResetSettings:
			Command_ResetSettings();
			break;
		case CMD_Bootloader:
			Command_Bootloader();
		default:
			return 1;
	}
				
	return 0;
}
