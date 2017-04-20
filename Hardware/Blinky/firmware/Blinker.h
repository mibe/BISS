#ifndef _BLINKER_H_
#define _BLINKER_H_

#include <stdint.h>
#include <avr/interrupt.h>

#include "Display.h"
#include "Settings.h"

#define BLINKER_STATR GPIOR0		// GPIO register for display status
#define BLINKER_STATB 1				// Bit for that in that GPIO register

void Blinker_Enable(void);
void Blinker_Disable(void);

#endif