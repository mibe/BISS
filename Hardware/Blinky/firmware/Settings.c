#include "Settings.h"

static Settings_t EEMEM s_settings = {0, SETTINGS_HEADER, SETTINGS_VERSION, {0, 0, 0}, 0};

void Settings_Load(void)
{
	eeprom_read_block(&settings, &s_settings, sizeof(Settings_t));
	
	if ((settings.Header != SETTINGS_HEADER) || (settings.Version != SETTINGS_VERSION))
	{
		// Looks like an uninitialized EEPROM. So we're using defaults here.
		settings.Color = (struct Display_Color_t) SETTINGS_DEFAULT_COLOR;
		settings.BlinkInterval = SETTINGS_DEFAULT_INTERVAL;
	}
}

void Settings_Save(void)
{
	settings.Header = SETTINGS_HEADER;
	settings.Version = SETTINGS_VERSION;
	eeprom_write_block(&settings, &s_settings, sizeof(Settings_t));
}