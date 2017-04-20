#ifndef _DISPLAY_H_
#define _DISPLAY_H_

#include <avr/io.h>

#include "Settings.h"

void Display_Setup(void);
void Display_Update(void);
void Display_Enable(void);
void Display_Disable(void);

#endif