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
#define BLINKER_TOUCH PB5			// Pin wired to the touch sensor output
#define BLINKER_ENABLE_TOUCH 1		// Bit for touch sensor enabled
#define BLINKER_ENABLE_TIMEOUT 2	// Bit for timeout enabled

void Blinker_Enable(uint8_t enableTouchSensor);
void Blinker_Disable(void);

#endif