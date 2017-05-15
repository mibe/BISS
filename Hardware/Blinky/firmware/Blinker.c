#include "Blinker.h"

void Blinker_Enable(void)
{
	// Normal mode, with prescaler of 1024; overflow interrupt enabled
	TCCR0B |= _BV(CS02) | _BV(CS00);
	TIMSK0 |= _BV(TOIE0);
	
	// Watch for changes on the touch sensor input pin
	PCICR |= _BV(PCIE0);
	PCMSK0 |= _BV(BLINKER_TOUCH);
}

void Blinker_Disable(void)
{
	// stop timer by setting no clock source & disable pin change interrupt
	TCCR0B &= ~_BV(CS02) & ~_BV(CS00);
	PCICR = 0;
	Display_Disable();
}

ISR(TIMER0_OVF_vect)
{
	static uint8_t counter = 0;
	
	// Overflow of Timer0 happened. Increment our internal counter.
	counter++;
	
	// If the setpoint is reached, toggle the display.
	if (counter >= settings.BlinkInterval)
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

ISR(PCINT0_vect)
{
	Blinker_Disable();
}