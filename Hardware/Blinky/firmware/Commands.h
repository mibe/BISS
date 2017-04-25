#ifndef _COMMANDS_H_
#define _COMMANDS_H_

#import "Settings.h"
#import "Display.h"
#import "Blinker.h"

#define CMD_Trigger 1
#define CMD_SetSettings 2
#define CMD_GetSettings 3
#define CMD_SaveSettings 4
#define CMD_ResetSettings 5
#define CMD_Bootloader 6

static void Command_Trigger(void);
static void Command_SetSettings(uint8_t r, uint8_t g, uint8_t b, uint8_t blinkInterval);

static void Command_SaveSettings(void);
static void command_ResetSettings(void);

uint8_t Command_Handle(uint8_t* fromHost, uint8_t* toHost);
#endif