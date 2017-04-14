#include "Settings.h"

static Settings_t EEMEM s_settings = {0, SETTINGS_HEADER, SETTINGS_VERSION, {0, 0, 0}, 0};

Settings_t Settings_Load(void)
{
	Settings_t temp;
	eeprom_read_block(&temp, &s_settings, sizeof(Settings_t));
	
	if ((temp.Header != SETTINGS_HEADER) || (temp.Version != SETTINGS_VERSION))
	{
		// Looks like an uninitialized EEPROM. So we're using defaults here.
		temp.Color = (struct Display_Color_t) SETTINGS_DEFAULT_COLOR;
		temp.BlinkInterval = SETTINGS_DEFAULT_INTERVAL;
	}
	
	return temp;
}

void Settings_Save(Settings_t settings)
{
	settings.Header = SETTINGS_HEADER;
	settings.Version = SETTINGS_VERSION;
	eeprom_write_block(&settings, &s_settings, sizeof(Settings_t));
}