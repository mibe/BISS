#ifndef _SETTINGS_H_
#define _SETTINGS_H_

#include <avr/eeprom.h>
// for Display_Color_t type
#include "Display.h"

#define EEPROM_DEF 0xFF
#define SETTINGS_HEADER 0x42	// 'B'
#define SETTINGS_VERSION 0x00

typedef struct
{
	uint8_t Reserved;		// 0x00			reserved (EEPROM corruption)
	uint8_t Header;			// 0x01			'B' (0x42)
	uint8_t Version;		// 0x02			memory layout version, zero-based.
	Display_Color_t Color;	// 0x03 - 0x05	LED color
	uint8_t Frequency;		// 0x06			blink frequency
} Settings_t;

Settings_t Settings_Load(void);
void Settings_Save(Settings_t settings);

#endif