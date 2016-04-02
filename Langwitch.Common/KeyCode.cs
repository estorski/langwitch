﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Product.Common
{
    [Flags]
    public enum KeyCode : byte
    {
        //
        // Summary:
        //     No key pressed.
        None = 0,

        //
        // Summary:
        //     The BACKSPACE key.
        [Display(Name = "Backspace")]
        BackSpace = 8,
        //
        // Summary:
        //     The TAB key.
        Tab = 9,
        //
        // Summary:
        //     The LINEFEED key.
        LineFeed = 10,
        //
        // Summary:
        //     The CLEAR key.
        Clear = 12,
        //
        // Summary:
        //     The ENTER key.
        Enter = 13,
        //
        // Summary:
        //     The PAUSE key.
        Pause = 19,
        //
        // Summary:
        //     The CAPS LOCK key.
        [Display(Name = "Caps Lock")]
        CapsLock = 20,
        //
        // Summary:
        //     The ESC key.
        Escape = 27,
        //
        // Summary:
        //     The SPACEBAR key.
        Space = 32,
        //
        // Summary:
        //     The PAGE UP key.
        PageUp = 33,
        //
        // Summary:
        //     The PAGE DOWN key.
        PageDown = 34,
        //
        // Summary:
        //     The END key.
        End = 35,
        //
        // Summary:
        //     The HOME key.
        Home = 36,
        //
        // Summary:
        //     The LEFT ARROW key.
        Left = 37,
        //
        // Summary:
        //     The UP ARROW key.
        Up = 38,
        //
        // Summary:
        //     The RIGHT ARROW key.
        Right = 39,
        //
        // Summary:
        //     The DOWN ARROW key.
        Down = 40,
        //
        // Summary:
        //     The PRINT SCREEN key.
        [Display(Name = "Print Screen")]
        PrintScreen = 44,
        //
        // Summary:
        //     The INS key.
        Insert = 45,
        //
        // Summary:
        //     The DEL key.
        Delete = 46,
        //
        // Summary:
        //     The 0 key.
        [Display(Name = "Digit 0")]
        D0 = 48,
        //
        // Summary:
        //     The 1 key.
        [Display(Name = "Digit 1")]
        D1 = 49,
        //
        // Summary:
        //     The 2 key.
        [Display(Name = "Digit 2")]
        D2 = 50,
        //
        // Summary:
        //     The 3 key.
        [Display(Name = "Digit 3")]
        D3 = 51,
        //
        // Summary:
        //     The 4 key.
        [Display(Name = "Digit 4")]
        D4 = 52,
        //
        // Summary:
        //     The 5 key.
        [Display(Name = "Digit 5")]
        D5 = 53,
        //
        // Summary:
        //     The 6 key.
        [Display(Name = "Digit 6")]
        D6 = 54,
        //
        // Summary:
        //     The 7 key.
        [Display(Name = "Digit 7")]
        D7 = 55,
        //
        // Summary:
        //     The 8 key.
        [Display(Name = "Digit 8")]
        D8 = 56,
        //
        // Summary:
        //     The 9 key.
        [Display(Name = "Digit 9")]
        D9 = 57,
        //
        // Summary:
        //     The A key.
        A = 65,
        //
        // Summary:
        //     The B key.
        B = 66,
        //
        // Summary:
        //     The C key.
        C = 67,
        //
        // Summary:
        //     The D key.
        D = 68,
        //
        // Summary:
        //     The E key.
        E = 69,
        //
        // Summary:
        //     The F key.
        F = 70,
        //
        // Summary:
        //     The G key.
        G = 71,
        //
        // Summary:
        //     The H key.
        H = 72,
        //
        // Summary:
        //     The I key.
        I = 73,
        //
        // Summary:
        //     The J key.
        J = 74,
        //
        // Summary:
        //     The K key.
        K = 75,
        //
        // Summary:
        //     The L key.
        L = 76,
        //
        // Summary:
        //     The M key.
        M = 77,
        //
        // Summary:
        //     The N key.
        N = 78,
        //
        // Summary:
        //     The O key.
        O = 79,
        //
        // Summary:
        //     The P key.
        P = 80,
        //
        // Summary:
        //     The Q key.
        Q = 81,
        //
        // Summary:
        //     The R key.
        R = 82,
        //
        // Summary:
        //     The S key.
        S = 83,
        //
        // Summary:
        //     The T key.
        T = 84,
        //
        // Summary:
        //     The U key.
        U = 85,
        //
        // Summary:
        //     The V key.
        V = 86,
        //
        // Summary:
        //     The W key.
        W = 87,
        //
        // Summary:
        //     The X key.
        X = 88,
        //
        // Summary:
        //     The Y key.
        Y = 89,
        //
        // Summary:
        //     The Z key.
        Z = 90,
        //
        // Summary:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        [Display(Name = "Left Windows")]
        LWin = 91,
        //
        // Summary:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        [Display(Name = "Right Windows")]
        RWin = 92,
        //
        // Summary:
        //     The computer sleep key.
        Sleep = 95,
        //
        // Summary:
        //     The 0 key on the numeric keypad.
        [Display(Name = "Numpad 0")]
        NumPad0 = 96,
        //
        // Summary:
        //     The 1 key on the numeric keypad.
        [Display(Name = "Numpad 1")]
        NumPad1 = 97,
        //
        // Summary:
        //     The 2 key on the numeric keypad.
        [Display(Name = "Numpad 2")]
        NumPad2 = 98,
        //
        // Summary:
        //     The 3 key on the numeric keypad.
        [Display(Name = "Numpad 3")]
        NumPad3 = 99,
        //
        // Summary:
        //     The 4 key on the numeric keypad.
        [Display(Name = "Numpad 4")]
        NumPad4 = 100,
        //
        // Summary:
        //     The 5 key on the numeric keypad.
        [Display(Name = "Numpad 5")]
        NumPad5 = 101,
        //
        // Summary:
        //     The 6 key on the numeric keypad.
        [Display(Name = "Numpad 6")]
        NumPad6 = 102,
        //
        // Summary:
        //     The 7 key on the numeric keypad.
        [Display(Name = "Numpad 7")]
        NumPad7 = 103,
        //
        // Summary:
        //     The 8 key on the numeric keypad.
        [Display(Name = "Numpad 8")]
        NumPad8 = 104,
        //
        // Summary:
        //     The 9 key on the numeric keypad.
        [Display(Name = "Numpad 9")]
        NumPad9 = 105,
        //
        // Summary:
        //     The multiply key.
        [Display(Name = "Numpad *")]
        Multiply = 106,
        //
        // Summary:
        //     The add key.
        [Display(Name = "Numpad +")]
        Add = 107,
        //
        // Summary:
        //     The separator key.
        [Display(Name = "|")]
        Separator = 108,
        //
        // Summary:
        //     The subtract key.
        [Display(Name = "Numpad -")]
        Subtract = 109,
        //
        // Summary:
        //     The decimal key.
        [Display(Name = "Numpad .")]
        Decimal = 110,
        //
        // Summary:
        //     The divide key.
        [Display(Name = "Numpad /")]
        Divide = 111,
        //
        // Summary:
        //     The F1 key.
        F1 = 112,
        //
        // Summary:
        //     The F2 key.
        F2 = 113,
        //
        // Summary:
        //     The F3 key.
        F3 = 114,
        //
        // Summary:
        //     The F4 key.
        F4 = 115,
        //
        // Summary:
        //     The F5 key.
        F5 = 116,
        //
        // Summary:
        //     The F6 key.
        F6 = 117,
        //
        // Summary:
        //     The F7 key.
        F7 = 118,
        //
        // Summary:
        //     The F8 key.
        F8 = 119,
        //
        // Summary:
        //     The F9 key.
        F9 = 120,
        //
        // Summary:
        //     The F10 key.
        F10 = 121,
        //
        // Summary:
        //     The F11 key.
        F11 = 122,
        //
        // Summary:
        //     The F12 key.
        F12 = 123,
        //
        // Summary:
        //     The F13 key.
        F13 = 124,
        //
        // Summary:
        //     The F14 key.
        F14 = 125,
        //
        // Summary:
        //     The F15 key.
        F15 = 126,
        //
        // Summary:
        //     The F16 key.
        F16 = 127,
        //
        // Summary:
        //     The F17 key.
        F17 = 128,
        //
        // Summary:
        //     The F18 key.
        F18 = 129,
        //
        // Summary:
        //     The F19 key.
        F19 = 130,
        //
        // Summary:
        //     The F20 key.
        F20 = 131,
        //
        // Summary:
        //     The F21 key.
        F21 = 132,
        //
        // Summary:
        //     The F22 key.
        F22 = 133,
        //
        // Summary:
        //     The F23 key.
        F23 = 134,
        //
        // Summary:
        //     The F24 key.
        F24 = 135,
        //
        // Summary:
        //     The NUM LOCK key.
        [Display(Name = "Num Lock")]
        NumLock = 144,
        //
        // Summary:
        //     The SCROLL LOCK key.
        [Display(Name = "Scroll Lock")]
        Scroll = 145,
        //
        // Summary:
        //     The left SHIFT key.
        [Display(Name = "Left Shift")]
        LShiftKey = 160,
        //
        // Summary:
        //     The right SHIFT key.
        [Display(Name = "Right Shift")]
        RShiftKey = 161,
        //
        // Summary:
        //     The left CTRL key.
        [Display(Name = "Left Control")]
        LControlKey = 162,
        //
        // Summary:
        //     The right CTRL key.
        [Display(Name = "Right Control")]
        RControlKey = 163,
        //
        // Summary:
        //     The left ALT key.
        [Display(Name = "Left Alt")]
        LMenu = 164,
        //
        // Summary:
        //     The right ALT key.
        [Display(Name = "Right Alt")]
        RMenu = 165,
        //
        // Summary:
        //     The volume mute key (Windows 2000 or later).
        VolumeMute = 173,
        //
        // Summary:
        //     The volume down key (Windows 2000 or later).
        VolumeDown = 174,
        //
        // Summary:
        //     The volume up key (Windows 2000 or later).
        VolumeUp = 175,
        //
        // Summary:
        //     The media next track key (Windows 2000 or later).
        MediaNextTrack = 176,
        //
        // Summary:
        //     The media previous track key (Windows 2000 or later).
        MediaPreviousTrack = 177,
        //
        // Summary:
        //     The media Stop key (Windows 2000 or later).
        MediaStop = 178,
        //
        // Summary:
        //     The media play pause key (Windows 2000 or later).
        MediaPlayPause = 179,
        //
        // Summary:
        //     The launch mail key (Windows 2000 or later).
        LaunchMail = 180,
        //
        // Summary:
        //     The select media key (Windows 2000 or later).
        SelectMedia = 181,
        //
        // Summary:
        //     The start application one key (Windows 2000 or later).
        LaunchApplication1 = 182,
        //
        // Summary:
        //     The start application two key (Windows 2000 or later).
        LaunchApplication2 = 183,
        //
        // Summary:
        //     The OEM Semicolon key on a US standard keyboard (Windows 2000 or later).
        [Display(Name = "Semicolon")]
        OemSemicolon = 186,
        //
        // Summary:
        //     The OEM 1 key.
        Oem1 = 186,
        //
        // Summary:
        //     The OEM plus key on any country/region keyboard (Windows 2000 or later).
        Oemplus = 187,
        //
        // Summary:
        //     The OEM comma key on any country/region keyboard (Windows 2000 or later).
        Oemcomma = 188,
        //
        // Summary:
        //     The OEM minus key on any country/region keyboard (Windows 2000 or later).
        OemMinus = 189,
        //
        // Summary:
        //     The OEM period key on any country/region keyboard (Windows 2000 or later).
        OemPeriod = 190,
        //
        // Summary:
        //     The OEM question mark key on a US standard keyboard (Windows 2000 or later).
        OemQuestion = 191,
        //
        // Summary:
        //     The OEM tilde key on a US standard keyboard (Windows 2000 or later).
        Oemtilde = 192,
        //
        // Summary:
        //     The OEM open bracket key on a US standard keyboard (Windows 2000 or later).
        OemOpenBrackets = 219,
        //
        // Summary:
        //     The OEM pipe key on a US standard keyboard (Windows 2000 or later).
        OemPipe = 220,
        //
        // Summary:
        //     The OEM close bracket key on a US standard keyboard (Windows 2000 or later).
        OemCloseBrackets = 221,
        //
        // Summary:
        //     The OEM singled/double quote key on a US standard keyboard (Windows 2000 or later).
        OemQuotes = 222,
        //
        // Summary:
        //     The OEM 8 key.
        Oem8 = 223,
        //
        // Summary:
        //     The OEM angle bracket or backslash key on the RT 102 key keyboard (Windows 2000
        //     or later).
        OemBackslash = 226,
        //
        // Summary:
        //     The PLAY key.
        Play = 250,
        //
        // Summary:
        //     The ZOOM key.
        Zoom = 251,
        //
        // Summary:
        //     A constant reserved for future use.
        NoName = 252,
    }
}
