﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Product.Common;

namespace Product
{
    public class MouseHooker : IDisposable
    {
        private IntPtr HookHandle = IntPtr.Zero;

        #region Events
        public MouseEventHandler2 ButtonDown;
        public MouseEventHandler2 ButtonUp;
        public MouseEventHandler2 MouseMove;
        #endregion

        /// <summary>
        /// Strong reference to a native callback method.
        /// </summary>
        private Win32.MouseHookProc HookProcedureHolder;

        public MouseHooker(bool doHookImmediately = true)
        {
            // This is a c# hack in order to keep a firm reference to a dynamically created delegate
            // so that it won't be collected by GC.
            HookProcedureHolder = HookProcedure;
            if (doHookImmediately)
                SetHook();
        }

        /// <summary>
        /// Installs the global hook
        /// </summary>
        public void SetHook()
        {
            var hInstance = Win32.LoadLibrary("User32");
            HookHandle = Win32.SetWindowsHookEx(
                Win32.WH_MOUSE_LL, HookProcedureHolder, hInstance, 0);
        }

        /// <summary>
        /// Uninstalls the global hook
        /// </summary>
        public void UnsetHook()
        {
            Win32.UnhookWindowsHookEx(HookHandle);
        }

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The mousehook event information</param>
        /// <returns></returns>
        private int HookProcedure(int code, int wParam, ref Win32.MouseInfo lParam)
        {
            if (code >= 0)
            {
                MouseEventArgs2 args = null;
                if (ButtonDown != null)
                {
                    if (wParam == Win32.WM_LBUTTONDOWN)
                    {
                        args = new MouseEventArgs2(MouseButtons.Left, lParam.pt);
                    }
                    if (wParam == Win32.WM_RBUTTONDOWN)
                    {
                        args = new MouseEventArgs2(MouseButtons.Right, lParam.pt);
                    }
                    if (wParam == Win32.WM_MBUTTONDOWN)
                    {
                        args = new MouseEventArgs2(MouseButtons.Middle, lParam.pt);
                    }
                    if (args != null)
                        ButtonDown(this, args);
                }
                if (args == null && ButtonUp != null)
                {
                    if (wParam == Win32.WM_LBUTTONUP)
                    {
                        args = new MouseEventArgs2(MouseButtons.Left, lParam.pt);
                    }
                    if (wParam == Win32.WM_RBUTTONUP)
                    {
                        args = new MouseEventArgs2(MouseButtons.Right, lParam.pt);
                    }
                    if (wParam == Win32.WM_MBUTTONUP)
                    {
                        args = new MouseEventArgs2(MouseButtons.Middle, lParam.pt);
                    }
                    if (args != null)
                        ButtonUp(this, args);
                }

                if (args == null && MouseMove != null)
                {
                    if (wParam == Win32.WM_MOUSEMOVE)
                    {
                        args = new MouseEventArgs2(MouseButtons.None, lParam.pt);
                        MouseMove(this, args);
                    }
                }
                if (args != null && args.Handled)
                    return 1;
                else
                {
                    Trace.WriteLine("Not handled " + Win32.MessageToString(wParam) + ": ");
                }
            }
            return Win32.CallNextHookEx(HookHandle, code, wParam, ref lParam);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                UnsetHook();
                disposedValue = true;
            }
        }

        ~MouseHooker()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}