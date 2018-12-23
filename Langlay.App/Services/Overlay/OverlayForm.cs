﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Product.Common;

namespace Product {
    public partial class OverlayForm : Form {
        private Stopwatch PeriodElapsed { get; set; }
        public uint MillisecondsToKeepVisible { get; set; }
        public uint OpacityWhenVisible { get; set; }
        public uint ScalingPercent { get; set; }

        public bool RoundCorners { get; set; }
        public OverlayLocation DisplayLocation { get; set; }

        public Screen Screen { get; set; }

        private const long MillisecondsToFadeOut = 200;
        public string LanguageName { get; set; }
        public string LayoutName { get; set; }

        private Font LanguageFont { get; set; }
        private Brush LanguageBrush { get; set; }
        private Font LayoutFont { get; set; }
        private Brush LayoutBrush { get; set; }

        private float RenderingCoefficient { get; set; }

        private int MinWidth;
        private int ScreenMargin;
        private IntPtr RegionHandle { get; set; }

        public OverlayForm() {
            InitializeComponent();
            PeriodElapsed = new Stopwatch();
            LanguageBrush = new SolidBrush(Color.White);
            LayoutBrush = new SolidBrush(Color.Gray);
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams baseParams = base.CreateParams;
                baseParams.ExStyle |=
                    (Win32.WS_EX_NOACTIVATE | Win32.WS_EX_TOOLWINDOW | Win32.WS_EX_TOPMOST);
                baseParams.ExStyle &= ~Win32.WS_EX_APPWINDOW;
                return baseParams;
            }
        }

        private void ResetAndRun() {
            Visible = false;
            if (timerOverlay.Enabled)
                timerOverlay.Stop();

            PeriodElapsed.Restart();

            UpdateRegionAndPosition();

            Visible = true;
            OnTimer();
            timerOverlay.Start();
        }

        private int ToPixels(float relativeValue) {
            return (int) (relativeValue * RenderingCoefficient);
        }

        internal void InitializeRenderingCoefficient() {
            if (Screen == null)
                throw new NullReferenceException("Screen property must not be null");
            var minAxisValue = Math.Min(Screen.Bounds.Width, Screen.Bounds.Height);
            RenderingCoefficient = (float) minAxisValue / 768 * ((float) ScalingPercent / 100);

            MinWidth = ToPixels(140);
            ScreenMargin = ToPixels(20);

            LanguageFont = new Font(Font.FontFamily, ToPixels(48), GraphicsUnit.Pixel);
            LayoutFont = new Font(Font.FontFamily, ToPixels(24), GraphicsUnit.Pixel);
        }

        private Graphics _graphicsForMeasuring;

        private void UpdateRegionAndPosition() {
            if (RenderingCoefficient == 0)
                InitializeRenderingCoefficient();

            if (_graphicsForMeasuring == null)
                _graphicsForMeasuring = CreateGraphics();

            var sizeLanguage = _graphicsForMeasuring.MeasureString(LanguageName, LanguageFont);
            var sizeLayout = _graphicsForMeasuring.MeasureString(LayoutName, LayoutFont);
            var size = new Size(
                Math.Max((int) Math.Max(sizeLanguage.Width, sizeLayout.Width) + ToPixels(40), MinWidth),
                (int) sizeLanguage.Height + (int) sizeLayout.Height + ToPixels(20));

            var position = new Point();
            var screenBounds = Screen.Bounds;
            switch (DisplayLocation) {
                case OverlayLocation.TopLeft:
                case OverlayLocation.MiddleLeft:
                case OverlayLocation.BottomLeft:
                    position.X = screenBounds.Left + ScreenMargin;
                    break;

                case OverlayLocation.TopCenter:
                case OverlayLocation.MiddleCenter:
                case OverlayLocation.BottomCenter:
                    position.X = screenBounds.Left + ((screenBounds.Width - size.Width) / 2);
                    break;

                case OverlayLocation.TopRight:
                case OverlayLocation.MiddleRight:
                case OverlayLocation.BottomRight:
                    position.X = screenBounds.Left + screenBounds.Width - size.Width - ScreenMargin;
                    break;
            }

            switch (DisplayLocation) {
                case OverlayLocation.TopLeft:
                case OverlayLocation.TopCenter:
                case OverlayLocation.TopRight:
                    position.Y = screenBounds.Top + ScreenMargin;
                    break;

                case OverlayLocation.MiddleLeft:
                case OverlayLocation.MiddleCenter:
                case OverlayLocation.MiddleRight:
                    position.Y = screenBounds.Top + (screenBounds.Height - size.Height) / 2;
                    break;

                case OverlayLocation.BottomLeft:
                case OverlayLocation.BottomCenter:
                case OverlayLocation.BottomRight:
                    position.Y = screenBounds.Top + screenBounds.Height - size.Height - ScreenMargin;
                    break;
            }

            this.Bounds = new Rectangle(position, size);
            if (RoundCorners)
                SetRoundedRegion();
        }

        private void SetRoundedRegion() {
            if (Region != null) {
                Region.Dispose();
                Region = null;
            }
            if (RegionHandle != IntPtr.Zero) {
                // Make sure we free this unmanaged resource whenever we
                // don't use it anymore
                Win32.DeleteObject(RegionHandle);
                RegionHandle = IntPtr.Zero;
            }
            RegionHandle = Win32.CreateRoundRectRgn(0, 0, Width, Height, ToPixels(20), ToPixels(20));
            Region = System.Drawing.Region.FromHrgn(RegionHandle);
            UpdateBounds();
        }

        public void PushMessage(string languageName, string layoutName) {
            LanguageName = string.Empty;
            LayoutName = string.Empty;

            // Make sure the window has redrawn itself empty
            Invalidate();
            Visible = true;
            Application.DoEvents();
            Visible = false;

            // Put the new content into the window
            LanguageName = languageName;
            LayoutName = layoutName;

            ResetAndRun();
        }

        private double GetOpacity(long elapsed) {
            if (elapsed <= MillisecondsToKeepVisible) {
                return (double) OpacityWhenVisible / 100;
            } else if (elapsed <= MillisecondsToKeepVisible + MillisecondsToFadeOut) {
                return ((double) Math.Max(MillisecondsToKeepVisible + MillisecondsToFadeOut - elapsed, 0))
                    / MillisecondsToFadeOut
                    * OpacityWhenVisible / 100;
            } else {
                return 0;
            }
        }

        private void OnTimer() {
            Opacity = GetOpacity(PeriodElapsed.ElapsedMilliseconds);
            if (Opacity == 0) {
                timerOverlay.Stop();
                PeriodElapsed.Stop();
                Visible = false;
            }
        }

        private void timerOverlay_Tick(object sender, EventArgs e) {
            try {
                OnTimer();
            } catch (Exception ex) {
#if TRACE
                Trace.TraceError(ex.ToString());
#endif
            }
        }

        private void OverlayForm_Paint(object sender, PaintEventArgs e) {
            var sizeLanguage = e.Graphics.MeasureString(LanguageName, LanguageFont);
            var sizeLayout = e.Graphics.MeasureString(LayoutName, LayoutFont);
            var pointLanguage = new PointF((Width - sizeLanguage.Width) / 2, (Height - sizeLanguage.Height - sizeLayout.Height) / 2);
            var pointLayout = new PointF((Width - sizeLayout.Width) / 2, pointLanguage.Y + sizeLanguage.Height);

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            e.Graphics.DrawString(this.LanguageName, this.LanguageFont, this.LanguageBrush, pointLanguage);
            e.Graphics.DrawString(this.LayoutName, this.LayoutFont, this.LayoutBrush, pointLayout);
        }
    }
}