#ifndef _DISPLAY_H_
#define _DISPLAY_H_

#include <avr/io.h>

typedef struct __attribute__((__packed__))
{
	uint8_t R;
	uint8_t G;
	uint8_t B;
} Display_Color_t;

void Display_Setup(void);
void Display_SetValue(Display_Color_t color);
Display_Color_t Display_GetValue(void);
void Display_Enable(void);
void Display_Disable(void);

#endif