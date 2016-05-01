﻿using Microsoft.Win32;
using Product.Common;

namespace Product
{
    public class SystemSettingService : ISystemSettingService
    {
        private const string KeyboardLayoutKey = "HKEY_CURRENT_USER\\Keyboard Layout";
        private const string ToggleKey = KeyboardLayoutKey + "\\Toggle";
        private const int ShiftFlag = 0x00010000;

        public bool GetIsShiftToDisableCapsLock()
        {
            var value = Utils.ParseInt(Registry.GetValue(
                KeyboardLayoutKey, "Attributes", null), 0);

            if ((value & ShiftFlag) == ShiftFlag)
                return true;
            return false;
        }

        public WindowsSequenceCode? GetLanguageSwitchSequence()
        {
            var result =
                (WindowsSequenceCode?) Utils.ParseInt(Registry.GetValue(
                    ToggleKey,
                    "Language Hotkey", null));
            // Fallback to perhaps "old"-Windows-version key for the language sequence
            if (result == null)
            {
                result = (WindowsSequenceCode?) Utils.ParseInt(Registry.GetValue(
                    ToggleKey,
                    "Hotkey", null));
                if (result == null)
                {
                    // this is by default (on Win10 at least)
                    result = WindowsSequenceCode.AltShift;
                }
            }
            return result;
        }

        public WindowsSequenceCode? GetLayoutSwitchSequence()
        {
            return (WindowsSequenceCode?) Utils.ParseInt(Registry.GetValue(
                ToggleKey,
                "Layout Hotkey", null));
        }
    }
}
