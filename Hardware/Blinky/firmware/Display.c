/*
 * BISS.Blinky: A USB device for notifying the user of a new BISS message.
 *
 * Copyright (C) 2017-2018 Michael Bemmerl
 *
 * SPDX-License-Identifier: MIT
 */

#include "Display.h"

// PC6 = OC.1A = R
// PC5 = OC.1B = G
// PB7 = OC.1C = B

void Display_Setup(void)
{
	// Fast PWM, 8-bit, clear on match, set on top, with prescaler of 1024
	TCCR1A |= _BV(COM1A1) | _BV(COM1B1) | _BV(COM1C1) | _BV(WGM10);
	TCCR1B |= _BV(WGM12) | _BV(CS12) | _BV(CS10);
	
	// Set the pins as outputs (this also puts a low signal to the pin)
	DDRC |= _BV(PC6) | _BV(PC5);
	DDRB |= _BV(PB7);
	
	Display_Update();
}

void Display_Update()
{
	OCR1A = settings.Color.R;
	OCR1B = settings.Color.G;
	OCR1C = settings.Color.B;
	
	// Enable / disable the display depending if the LED should be active
	if (OCR1A > 0 || OCR1B > 0 || OCR1C > 0)
		Display_Enable();
	else if (OCR1A == 0 && OCR1B == 0 && OCR1C == 0)
		Display_Disable();
}

void Display_Enable(void)
{
	// Start the timer
	TCCR1B |= _BV(CS12) | _BV(CS10);
}

void Display_Disable(void)
{
	// Stop the timer
	TCCR1B &= ~(_BV(CS10) | _BV(CS12));
	
	OCR1A = 0;
	OCR1B = 0;
	OCR1C = 0;
}
