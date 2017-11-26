#ifndef _COMMANDS_H_
#define _COMMANDS_H_

#import "Settings.h"
#import "Display.h"
#import "Blinker.h"
#import "Bootloader.h"

#define CMD_Trigger 1
#define CMD_SetSettings 2
#define CMD_GetSettings 3
#define CMD_SaveSettings 4
#define CMD_ResetSettings 5
#define CMD_Bootloader 6
#define CMD_TurnOff 7

uint8_t Command_Handle(uint8_t* fromHost, uint8_t* toHost);
#endif