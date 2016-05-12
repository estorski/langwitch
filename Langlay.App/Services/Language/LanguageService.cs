﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Product.Common;

namespace Product
{
    public class LanguageService : ILanguageService
    {
        private IDictionary<string, IntPtr> CultureToLastUsedLayout
            = new Dictionary<string, IntPtr>();

        private IOverlayService OverlayService { get; set; }
        private IConfigService ConfigService { get; set; }
        public ILanguageSetterService LanguageSetterService { get; set; }

        public LanguageService(
            IConfigService configService, IOverlayService overlayService)
        {
            ConfigService = configService;
            OverlayService = overlayService;
        }

        private void CheckServicesAreSet()
        {
            if (ConfigService == null)
                throw new NullReferenceException("ConfigService must not be null");
            if (OverlayService == null)
                throw new NullReferenceException("OverlayService");
            if (LanguageSetterService == null)
                throw new NullReferenceException("LanguageSetterService");
        }

        private IDictionary<IntPtr, InputLayout> _inputLayouts
            = new Dictionary<IntPtr, InputLayout>();

        public IList<InputLayout> GetInputLayouts()
        {
            return InputLanguage.InstalledInputLanguages
              .Cast<InputLanguage>()
              .Select(x =>
              {
                  if (!_inputLayouts.ContainsKey(x.Handle))
                  {
                      // Surprisingly, this is quite expensive. According to
                      // ILSpy there're lots of unsafe method and registry
                      // usage each time you read the property and sub-properties.
                      _inputLayouts[x.Handle] = new InputLayout(x);
                  }
                  return _inputLayouts[x.Handle];
              })
              .ToList();
        }

        private IList<InputLayout> GetLayoutsByLanguage(
            string languageName, IList<InputLayout> inputLayouts = null)
        {
            if (inputLayouts == null)
                inputLayouts = GetInputLayouts();
            return inputLayouts.Where(x => x.LanguageName == languageName).ToList();
        }

        private string GetNextInputLayoutName(
            string currentLanguageName, string currentLayoutName, bool doWrap,
            IList<InputLayout> inputLayouts = null)
        {
            var layoutNames = GetLayoutsByLanguage(currentLanguageName, inputLayouts)
                .Select(x => x.Name)
                .ToList();
            var indexOfNext = layoutNames.IndexOf(currentLayoutName) + 1;
            if (indexOfNext >= layoutNames.Count)
            {
                if (doWrap)
                    return layoutNames[0];
                else
                    return null;
            }
            return layoutNames[indexOfNext];
        }

        public InputLayout GetCurrentLayout()
        {
            var currentLayoutHandle = Win32.GetKeyboardLayout(
               Win32.GetWindowThreadProcessId(Win32.GetForegroundWindow(), IntPtr.Zero));
            return GetInputLayouts()
                .FirstOrDefault(x => x.Handle == currentLayoutHandle);
        }

        private string GetNextInputLanguageName(
            string currentLanguageName)
        {
            var inputLayouts = GetInputLayouts();
            var languageNames = inputLayouts.Select(x => x.LanguageName).Distinct().ToList();
            var indexOfNext = languageNames.IndexOf(currentLanguageName) + 1;
            if (indexOfNext >= languageNames.Count)
                indexOfNext = 0;
            return languageNames[indexOfNext];
        }

        private IntPtr GetDefaultLayoutForLanguage(
            string languageName)
        {
            // Avoid re-evaluating properties
            var inputLayouts = GetInputLayouts();
            var firstLanguageLayout = inputLayouts.FirstOrDefault(x => x.LanguageName == languageName);
            if (firstLanguageLayout == null)
                firstLanguageLayout = inputLayouts.FirstOrDefault();
            if (firstLanguageLayout == null)
                throw new NullReferenceException("Not a single language/layout installed in the system");

            return firstLanguageLayout.Handle;
        }

        private InputLayout GetLayoutByLanguageAndLayoutName(
            string languageName, string layoutName)
        {
            var inputLayouts = GetInputLayouts();
            return inputLayouts.FirstOrDefault(x => x.LanguageName == languageName && x.Name == layoutName);
        }

        private string GetLanguageName(InputLayout layout)
        {
            return ConfigService.DoShowLanguageNameInNative
                ? layout.LanguageNameThreeLetterNative.ToUpper()
                : layout.LanguageNameThreeLetter.ToUpper();
        }

        private void PushToOverlay()
        {
            var currentLayout = GetCurrentLayout();
            if (currentLayout == null)
                throw new NullReferenceException("currentLayout must not be null");
            OverlayService.PushMessage(
                GetLanguageName(currentLayout),
                currentLayout.Name);
        }

        protected void SwitchLanguage(bool restoreSavedLayout)
        {
            CheckServicesAreSet();

            var inputLayouts = GetInputLayouts();
            var currentLayout = GetCurrentLayout();
            if (currentLayout == null)
                throw new NullReferenceException("currentLayout must not be null");

            // Here we save the layout last used within the language, so
            // that it could be restored later.
            CultureToLastUsedLayout[currentLayout.LanguageName] = currentLayout.Handle;

            var nextLanguageName = GetNextInputLanguageName(
                currentLayout.LanguageName);
            IntPtr layoutToSet;
            if (restoreSavedLayout && CultureToLastUsedLayout.ContainsKey(nextLanguageName))
                layoutToSet = CultureToLastUsedLayout[nextLanguageName];
            else
                layoutToSet = GetDefaultLayoutForLanguage(
                    nextLanguageName);
            LanguageSetterService.SetCurrentLayout(layoutToSet);
            Thread.Sleep(10);
            PushToOverlay();
        }

        protected bool SwitchLayout(bool doWrap)
        {
            CheckServicesAreSet();
            var result = false;
            var inputLayouts = GetInputLayouts();
            var currentLayout = GetCurrentLayout();
            if (currentLayout == null)
                throw new NullReferenceException("currentLayout must not be null");

            var nextLayoutName = GetNextInputLayoutName(
                currentLayout.LanguageName, currentLayout.Name, doWrap);

            if (!string.IsNullOrEmpty(nextLayoutName))
            {
                var layoutToSet = GetLayoutByLanguageAndLayoutName(
                    currentLayout.LanguageName, nextLayoutName).Handle;
                LanguageSetterService.SetCurrentLayout(layoutToSet);

                CultureToLastUsedLayout[currentLayout.LanguageName] = layoutToSet;
                PushToOverlay();

                result = true;
            }
            return result;
        }

        protected void SwitchLanguageAndLayout()
        {
            if (!SwitchLayout(false))
                SwitchLanguage(false);
        }

        public void ConductSwitch(KeyboardSwitch keyboardSwitch)
        {
            switch (keyboardSwitch)
            {
                case KeyboardSwitch.Language:
                    SwitchLanguage(false);
                    break;

                case KeyboardSwitch.LanguageRestoreLayout:
                    SwitchLanguage(true);
                    break;

                case KeyboardSwitch.LayoutNoWrap:
                    SwitchLayout(false);
                    break;

                case KeyboardSwitch.Layout:
                    SwitchLayout(true);
                    break;

                case KeyboardSwitch.LanguageAndLayout:
                    SwitchLanguageAndLayout();
                    break;
            }
        }
    }
}