#include "Blinker.h"

static uint8_t counter;

static void _display_enable(void)
{
	Display_Enable();
	BLINKER_STATR |= BLINKER_STAT_DISPLAY;
}

static void _display_disable(void)
{
	Display_Disable();
	BLINKER_STATR &= ~(BLINKER_STAT_DISPLAY);
}

void Blinker_Enable(uint8_t enableTouchSensor)
{
	counter = 0;
	
	// Normal mode, with prescaler of 1024; overflow interrupt enabled
	TCCR0B |= _BV(CS02) | _BV(CS00);
	TIMSK0 |= _BV(TOIE0);
	
	// Watch for changes on the touch sensor input pin
	if (enableTouchSensor)
		BLINKER_STATR |= BLINKER_STAT_TOUCH;
	
	 _display_enable();
}

void Blinker_Disable(void)
{
	// Stop timer by setting no clock source
	TCCR0B &= ~_BV(CS02) & ~_BV(CS00);
	
	// Disable the display and reset touch sensor enabled bit.
	_display_disable();
	BLINKER_STATR &= ~BLINKER_STAT_TOUCH;
}

ISR(TIMER0_OVF_vect)
{
	// Overflow of Timer0 happened. Increment our internal counter.
	counter++;
	
	// If the setpoint is reached, toggle the display.
	if (counter >= settings.BlinkInterval)
	{
		// Bit 0 of GPIO register 0 is used as a status bit to check whether
		// the display is enabled or not.
		if (~BLINKER_STATR & BLINKER_STAT_DISPLAY)
			_display_enable();
		else
			_display_disable();
		
		// Reset internal counter.
		// This makes it practically a timer in CTC mode with a larger prescaler.
		counter = 0;
	}
	
	// Check if the touch sensor is enabled and if yes, check if it sensed a touch.
	// (The output from the sensor is low-active.)
	if (BLINKER_STATR & BLINKER_STAT_TOUCH)
		if (!(PINB & _BV(BLINKER_TOUCH)))
			Blinker_Disable();
}
