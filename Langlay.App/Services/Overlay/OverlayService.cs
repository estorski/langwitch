﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Product {
    public class OverlayService : IOverlayService, ILifecycled {
        private IEventService EventService { get; set; }
        private IConfigService ConfigService { get; set; }

        private IDictionary<string, OverlayForm> Overlays { get; set; }

        public OverlayService() {
            Overlays = new Dictionary<string, OverlayForm>();
        }

        private OverlayForm CreateOverlay(Screen screen) {
            var overlayForm = new OverlayForm {
                MillisecondsToKeepVisible = ConfigService.OverlayDuration,
                OpacityWhenVisible = ConfigService.OverlayOpacity,
                ScalingPercent = ConfigService.OverlayScale,
                DisplayLocation = ConfigService.OverlayLocation,
                RoundCorners = ConfigService.DoShowOverlayRoundCorners,
                Screen = screen
            };

            overlayForm.InitializeRenderingCoefficient();

            return overlayForm;
        }

        #region Start/Stop
        public bool IsStarted { get; private set; }

        public void Start() {
            ConfigService = ServiceRegistry.Instance.Get<IConfigService>();
            if (!IsStarted && ConfigService.DoShowOverlay) {
                IsStarted = true;
                foreach (var screen in Screen.AllScreens) {
                    if (!ConfigService.DoShowOverlayOnMainDisplayOnly || screen.Primary)
                        Overlays[screen.DeviceName] = CreateOverlay(screen);
                }
                StartTimer();

                EventService = ServiceRegistry.Instance.Get<IEventService>();
                EventService.KeyboardInput += EventService_Input;
                EventService.MouseInput += EventService_Input;
                SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            }
        }

        public void Stop() {
            if (IsStarted) {
                IsStarted = false;

                SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;

                EventService.KeyboardInput -= EventService_Input;
                EventService.MouseInput -= EventService_Input;

                StopTimer();

                if (_lastInputElapsed.IsRunning)
                    _lastInputElapsed.Stop();
                _previousLayoutHandle = null;

                foreach (var pair in Overlays) {
                    if (pair.Value != null) {
                        pair.Value.Dispose();
                    }
                }
                Overlays.Clear();
            }
        }

        #endregion Start/Stop

        #region Timer

        private Timer LanguageCheckTimer { get; set; }
        private const uint PeriodToCheckForLayoutSwitch = 1000;
        private Stopwatch _lastInputElapsed = new Stopwatch() { };

        private void EventService_Input() {
            _lastInputElapsed.Restart();
            if (GetIsTimerPaused())
                ResumeTimer();
        }

        private void StartTimer() {
            LanguageCheckTimer = new Timer { Interval = 50 };
            LanguageCheckTimer.Tick += LanguageCheckTimer_Tick;
            LanguageCheckTimer.Start();
        }

        private void StopTimer() {
            if (LanguageCheckTimer != null) {
                LanguageCheckTimer.Stop();
                LanguageCheckTimer.Tick -= LanguageCheckTimer_Tick;
                LanguageCheckTimer.Dispose();
                LanguageCheckTimer = null;
            }
        }

        private void PauseTimer() {
            if (LanguageCheckTimer != null) {
                LanguageCheckTimer.Stop();
                LanguageCheckTimer_Tick(LanguageCheckTimer, EventArgs.Empty);
            }
        }

        private void ResumeTimer() {
            if (LanguageCheckTimer != null) {
                LanguageCheckTimer.Start();
            }
        }

        private bool GetIsTimerPaused() {
            return LanguageCheckTimer != null && !LanguageCheckTimer.Enabled;
        }

        private IntPtr? _previousLayoutHandle;

        private void DoOnTimer() {
            var languageService = ServiceRegistry.Instance.Get<ILanguageService>();
            var currentLayoutHandle = languageService.GetCurrentLayoutHandle();
            if (currentLayoutHandle != IntPtr.Zero) {
                if (_previousLayoutHandle != null
                    && _previousLayoutHandle != currentLayoutHandle) {
                    var currentLayout = languageService.GetCurrentLayout();
                    if (currentLayout != null)
                        PushMessage(GetLanguageName(currentLayout), currentLayout.Name);
                }
                _previousLayoutHandle = currentLayoutHandle;
            }
        }

        private void OnTimer() {
            if (_lastInputElapsed.IsRunning
                && _lastInputElapsed.ElapsedMilliseconds < PeriodToCheckForLayoutSwitch) {
                DoOnTimer();
            } else {
                _lastInputElapsed.Stop();
                if (!GetIsTimerPaused())
                    PauseTimer();
            }
        }

        private void LanguageCheckTimer_Tick(object sender, System.EventArgs e) {
            // Use the condition to make sure we don't get any "old" timer
            // influencing our overlay.
            if (sender == this.LanguageCheckTimer) {
                try {
                    OnTimer();
                } catch (Exception ex) {
#if TRACE
                    Trace.TraceError(ex.ToString());
#endif
                }
            }
        }

        #endregion Timer

        private string GetLanguageName(InputLayout layout) {
            return ConfigService.DoShowLanguageNameInNative
                ? layout.LanguageNameThreeLetterNative.ToUpper()
                : layout.LanguageNameThreeLetter.ToUpper();
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, System.EventArgs e) {
            if (IsStarted) {
                Stop();
                Start();
            }
        }

        public void PushMessage(string languageName, string layoutName) {
            if (IsStarted) {
                foreach (var overlay in Overlays.Values) {
                    overlay.PushMessage(languageName, layoutName);
                }
            }
        }
    }
}