#include "Commands.h"

static void Command_Trigger(void)
{
	Blinker_Enable();
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

uint8_t Command_Handle(uint8_t* fromHost, uint8_t* toHost)
{
	// The command ID is always the first byte of the package.
	uint8_t cmdId = fromHost[0];
	toHost[0] = cmdId;
	
	switch(cmdId)
	{
		case CMD_Trigger:
			Command_Trigger();
			break;
		case CMD_SetSettings:
			Command_SetSettings(fromHost[1], fromHost[2], fromHost[3], fromHost[4]);
			break;
		case CMD_SaveSettings:
			Command_SaveSettings();
			break;
		case CMD_ResetSettings:
			Command_ResetSettings();
		default:
			return 1;
	}
				
	return 0;
}
