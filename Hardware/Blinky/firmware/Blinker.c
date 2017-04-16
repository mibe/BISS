#include "Blinker.h"

volatile static uint8_t setPoint;
volatile static uint8_t counter;

void Blinker_SetInterval(uint8_t interval)
{
	setPoint = interval;
}

void Blinker_Enable(void)
{
	// Normal mode, with prescaler of 1024; overflow interrupt enabled
	TCCR0B |= _BV(CS02) | _BV(CS00);
	TIMSK0 |= _BV(TOIE0);
}

void Blinker_Disable(void)
{
	// stop timer by setting no clock source
	TCCR0B &= ~_BV(CS02) & ~_BV(CS00);
}

ISR(TIMER0_OVF_vect)
{
	// Overflow of Timer0 happened. Increment our internal counter.
	counter++;
	
	// If the setpoint is reached, toggle the display.
	if (counter >= setPoint)
	{
		// Bit 0 of GPIO register 0 is used as a status bit.
		if (~BLINKER_STATR & BLINKER_STATB)
		{
			Display_Enable();
			BLINKER_STATR |= BLINKER_STATB;
		}
		else
		{
			Display_Disable();
			BLINKER_STATR &= ~(BLINKER_STATB);
		}			
		
		// Reset internal counter.
		// This makes it practically a timer in CTC mode with a larger prescaler.
		counter = 0;
	}
}