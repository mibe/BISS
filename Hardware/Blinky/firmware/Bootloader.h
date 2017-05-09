#ifndef _BOOTLOADER_H_
#define _BOOTLOADER_H_

#include <avr/wdt.h>
#include <LUFA/Common/Common.h>
#include <LUFA/Drivers/USB/USB.h>

#define MAGIC_BOOT_KEY            0xDEADBEEF
#define BOOTLOADER_START_ADDRESS  ((FLASH_SIZE_BYTES - BOOTLOADER_SEC_SIZE_BYTES) >> 1)

void Bootloader_Jump_Check(void) ATTR_INIT_SECTION(3);
void Bootloader_Execute(void);

#endif