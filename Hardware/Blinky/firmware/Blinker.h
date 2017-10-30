#ifndef _BLINKER_H_
#define _BLINKER_H_

#include <stdint.h>
#include <avr/interrupt.h>

#include "Display.h"
#include "Settings.h"

#define BLINKER_STATR GPIOR0		// GPIO register for blinker status
#define BLINKER_STATB 1				// Bit for Display enabled
#define BLINKER_TOUCHB 2			// Bit for touch sensor enabled
#define BLINKER_TOUCH PB5			// Pin wired to the touch sensor output

void Blinker_Enable(uint8_t enableTouchSensor);
void Blinker_Disable(void);

#endif