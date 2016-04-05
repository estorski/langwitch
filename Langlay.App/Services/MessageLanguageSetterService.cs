﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Product.Common;

namespace Product
{
    public class MessageLanguageSetterService : ILanguageSetterService
    {
        public bool SetCurrentLayout(IntPtr handle)
        {
            var foregroundWindowHandle = Win32.GetForegroundWindow();

            // TODO: this does not work with Skype, for some reason.
            IntPtr result;
            Win32.SendMessageTimeout(
                foregroundWindowHandle, Win32.WM_INPUTLANGCHANGEREQUEST, 0, handle.ToInt32(),
                Win32.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 500, out result);
            return true;
        }
    }
}