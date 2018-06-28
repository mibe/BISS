#
#             LUFA Library
#     Copyright (C) Dean Camera, 2015.
#
#  dean [at] fourwalledcubicle [dot] com
#           www.lufa-lib.org
#
# --------------------------------------
#         LUFA Project Makefile.
# --------------------------------------

# Run "make help" for target help.

MCU          = atmega32u2
ARCH         = AVR8
BOARD        = NONE
F_CPU        = 16000000
F_USB        = $(F_CPU)
# Sizes are in bytes, not words!
# ATmega32U2 has 32 KiB of flash; bootloader takes 4 KiB
FLASH_SIZE   = 0x8000
BL_SEC_SIZE  = 0x1000
OPTIMIZATION = s
TARGET       = Blinky
SRC          = $(TARGET).c Bootloader.c Commands.c Blinker.c Settings.c Display.c Descriptors.c $(LUFA_SRC_USB)
LUFA_PATH    = LUFA/LUFA
CC_FLAGS     = -DUSE_LUFA_CONFIG_HEADER -IConfig/ -DFLASH_SIZE_BYTES=$(FLASH_SIZE) -DBOOTLOADER_SEC_SIZE_BYTES=$(BL_SEC_SIZE)
LD_FLAGS     =

# Default target
all:

# Include LUFA build script makefiles
include $(LUFA_PATH)/Build/lufa_core.mk
include $(LUFA_PATH)/Build/lufa_sources.mk
include $(LUFA_PATH)/Build/lufa_build.mk
include $(LUFA_PATH)/Build/lufa_cppcheck.mk
include $(LUFA_PATH)/Build/lufa_doxygen.mk
include $(LUFA_PATH)/Build/lufa_dfu.mk
include $(LUFA_PATH)/Build/lufa_hid.mk
include $(LUFA_PATH)/Build/lufa_avrdude.mk
include $(LUFA_PATH)/Build/lufa_atprogram.mk
