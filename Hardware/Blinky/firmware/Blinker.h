#ifndef _BLINKER_H_
#define _BLINKER_H_

#include <stdint.h>
#include <avr/interrupt.h>

#include "Display.h"
#include "Settings.h"

#define BLINKER_STATR GPIOR0		// GPIO register for blinker status
#define BLINKER_STAT_DISPLAY 1		// Bit for Display enabled in status register
#define BLINKER_STAT_TOUCH 2		// Bit for touch sensor enabled in status register
#define BLINKER_STAT_TIMEOUT 4		// Bit for timeout enabled in status register

#define BLINKER_TOUCH PB5			// Pin of the touch sensor input
#define BLINKER_AUX PB6				// Pin of the AUX output

#define BLINKER_ENABLE_TOUCH 1		// Bit for touch sensor enabled
#define BLINKER_ENABLE_TIMEOUT 2	// Bit for timeout enabled
#define BLINKER_ENABLE_AUX 4		// Bit for AUX output enabled

void Blinker_Setup(void);
void Blinker_Enable(uint8_t enableTouchSensor);
void Blinker_Disable(void);

#endif