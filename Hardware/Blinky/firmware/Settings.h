#ifndef _SETTINGS_H_
#define _SETTINGS_H_

#include <avr/eeprom.h>

#define EEPROM_DEF 0xFF
#define SETTINGS_HEADER 0x42	// 'B'
#define SETTINGS_VERSION 0x00

#define SETTINGS_DEFAULT_COLOR {10, 10, 10}
#define SETTINGS_DEFAULT_INTERVAL 31
#define SETTINGS_DEFAULT_TIMEOUT 76

typedef struct __attribute__((__packed__))
{
	uint8_t R;
	uint8_t G;
	uint8_t B;
} Color_t;

typedef struct
{
	uint8_t Reserved;		// 0x00			reserved (EEPROM corruption)
	uint8_t Header;			// 0x01			'B' (0x42)
	uint8_t Version;		// 0x02			memory layout version, zero-based.
	Color_t Color;			// 0x03 - 0x05	LED color
	uint8_t BlinkInterval;	// 0x06			blink interval
	uint8_t BlinkTimeout;	// 0x07			blinker timeout
} Settings_t;

enum SettingsState_t
{
	SETTINGS_STATE_Empty = 0,			// EEPROM is empty / uninitialized
	SETTINGS_STATE_Defaults = 1,		// EEPROM contains default settings
	SETTINGS_STATE_NonDefaults = 2		// EEPROM contains non-default settings
};

Settings_t settings;

void Settings_Load(void);
void Settings_Save(void);
uint8_t Settings_State(void);
void Settings_Clear(void);

#endif