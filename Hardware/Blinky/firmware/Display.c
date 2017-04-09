#include "Display.h"

// PC6 = OC.1A = R
// PC5 = OC.1B = G
// PB7 = OC.1C = B

void Display_Setup(void)
{
	// Fast PWM, 8-bit, clear on match, set on top, with prescaler of 1024
	TCCR1A |= _BV(COM1A1) | _BV(COM1B1) | _BV(COM1C1) | _BV(WGM10);
	TCCR1B |= _BV(WGM12) | _BV(CS12) | _BV(CS10);
	
	Display_Color_t zero = {0, 0, 0};
	Display_SetValue(zero);
}

void Display_SetValue(Display_Color_t color)
{
	OCR1A = color.R;
	OCR1B = color.G;
	OCR1C = color.B;
	
	// Disable the outputs if no PWM is required.
	if (color.R == 0)
		DDRC &= ~_BV(PC6);
	
	if (color.G == 0)
		DDRC &= ~_BV(PC5);
	
	if (color.B == 0)
		DDRB &= ~_BV(PB7);
}

Display_Color_t Display_GetValue(void)
{
	Display_Color_t result;
	result.R = OCR1A;
	result.G = OCR1B;
	result.B = OCR1C;
	
	return result;
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
