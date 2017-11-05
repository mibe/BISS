#include "Blinker.h"

static uint8_t blinkCounter;
static uint8_t ovrflwToSec;
static uint16_t secondsCounter;

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

void Blinker_Enable(uint8_t blinkerSettings)
{
	blinkCounter = 0;
	ovrflwToSec = 0;
	secondsCounter = 0;
	
	// Normal mode, with prescaler of 1024; overflow interrupt enabled
	TCCR0B |= _BV(CS02) | _BV(CS00);
	TIMSK0 |= _BV(TOIE0);
	
	// Watch for changes on the touch sensor input pin
	if (blinkerSettings & BLINKER_ENABLE_TOUCH)
		BLINKER_STATR |= BLINKER_STAT_TOUCH;
	// Enable blinker timeout
	if (blinkerSettings & BLINKER_ENABLE_TIMEOUT)
		BLINKER_STATR |= BLINKER_STAT_TIMEOUT;
	
	 _display_enable();
}

void Blinker_Disable(void)
{
	// Stop timer by setting no clock source
	TCCR0B &= ~_BV(CS02) & ~_BV(CS00);
	
	// Disable the display and reset status bits.
	_display_disable();
	BLINKER_STATR &= ~BLINKER_STAT_TOUCH;
	BLINKER_STATR &= ~BLINKER_STAT_TIMEOUT;
}

ISR(TIMER0_OVF_vect)
{
	// Overflow of Timer0 happened. Increment our internal counter.
	blinkCounter++;
	
	// Timeout logic: 16.000.000 MHz F_CPU with a timer prescaler of 1024 and
	// an overflow of an 8 bit timer results in 61.03 interrupts per second.
	// The error of 0.0351... can be neglected here.
	// But timeout only if the bit in the status register is set.
	if ((BLINKER_STATR & BLINKER_STAT_TIMEOUT) && ++ovrflwToSec == 61)
	{
		// Increment the seconds counter and reset the overflow counter.
		secondsCounter++;
		ovrflwToSec = 0;
		
		// The maximum timeout is 300 seconds. But the timeout value is stored
		// in an 1 byte variable. This means the minimum timeout is 300-256=44
		// seconds. This has to be considered here.
		if (secondsCounter > 44)
			if (secondsCounter - 44 > settings.BlinkTimeout)
			{
				Blinker_Disable();
				return;
			}
	}
	
	// If the setpoint is reached, toggle the display.
	if (blinkCounter >= settings.BlinkInterval)
	{
		// Bit 0 of GPIO register 0 is used as a status bit to check whether
		// the display is enabled or not.
		if (~BLINKER_STATR & BLINKER_STAT_DISPLAY)
			_display_enable();
		else
			_display_disable();
		
		// Reset internal counter.
		// This makes it practically a timer in CTC mode with a larger prescaler.
		blinkCounter = 0;
	}
	
	// Check if the touch sensor is enabled and if yes, check if it sensed a touch.
	// (The output from the sensor is low-active.)
	if (BLINKER_STATR & BLINKER_STAT_TOUCH)
		if (!(PINB & _BV(BLINKER_TOUCH)))
			Blinker_Disable();
}
