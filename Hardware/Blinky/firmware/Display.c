#include "Display.h"

// PC6 = OC.1A = R
// PC5 = OC.1B = G
// PB7 = OC.1C = B

void Display_Setup(void)
{
	// Fast PWM, 8-bit, clear on match, set on top, with prescaler of 1024
	TCCR1A |= _BV(COM1A1) | _BV(COM1B1) | _BV(COM1C1) | _BV(WGM10);
	TCCR1B |= _BV(WGM12) | _BV(CS12) | _BV(CS10);

	Display_Update();
}

void Display_Update()
{
	OCR1A = settings.Color.R;
	OCR1B = settings.Color.G;
	OCR1C = settings.Color.B;
	
	// Disable the outputs if no PWM is required.
	if (settings.Color.R == 0)
		DDRC &= ~_BV(PC6);
	
	if (settings.Color.G == 0)
		DDRC &= ~_BV(PC5);
	
	if (settings.Color.B == 0)
		DDRB &= ~_BV(PB7);
}

void Display_Enable(void)
{
	// Set the port pins as outputs if the output compare registers are set.
	if (OCR1A > 0)
		DDRC |= _BV(PC6);
	
	if (OCR1B > 0)
		DDRC |= _BV(PC5);
	
	if (OCR1C > 0)
		DDRB |= _BV(PB7);
}

void Display_Disable(void)
{
	// Disable all outputs
	DDRC &= ~_BV(PC6) & ~_BV(PC5);
	DDRB &= ~_BV(PB7);
}
