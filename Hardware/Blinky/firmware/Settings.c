#include "Settings.h"

static Settings_t EEMEM s_settings = {0, SETTINGS_HEADER, SETTINGS_VERSION, {0, 0, 0}, 0};

void Settings_Load(void)
{
	eeprom_read_block(&settings, &s_settings, sizeof(Settings_t));
	
	if ((settings.Header != SETTINGS_HEADER) || (settings.Version != SETTINGS_VERSION))
	{
		// Looks like an uninitialized EEPROM. So we're using defaults here.
		Settings_Clear();
	}
}

void Settings_Save(void)
{
	settings.Header = SETTINGS_HEADER;
	settings.Version = SETTINGS_VERSION;
	eeprom_write_block(&settings, &s_settings, sizeof(Settings_t));
}

uint8_t Settings_State(void)
{
	uint8_t byte = eeprom_read_byte(0x01);
	
	if (byte != SETTINGS_HEADER)
		return SETTINGS_STATE_Empty;
	
	if (settings.Color == (struct Color_t) SETTINGS_DEFAULT_COLOR
		&& settings.BlinkInterval == SETTINGS_DEFAULT_INTERVAL)
		return SETTINGS_STATE_Defaults;
	
	return SETTINGS_STATE_NonDefaults;
}

void Settings_Clear(void)
{
	settings.Color = (struct Color_t) SETTINGS_DEFAULT_COLOR;
	settings.BlinkInterval = SETTINGS_DEFAULT_INTERVAL;
}