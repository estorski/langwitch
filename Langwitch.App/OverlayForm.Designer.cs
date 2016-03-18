﻿namespace Langwitch
{
    partial class OverlayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelOverlay = new System.Windows.Forms.Label();
            this.timerOverlay = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // labelOverlay
            // 
            this.labelOverlay.AutoSize = true;
            this.labelOverlay.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelOverlay.Location = new System.Drawing.Point(0, 0);
            this.labelOverlay.Margin = new System.Windows.Forms.Padding(0);
            this.labelOverlay.MinimumSize = new System.Drawing.Size(20, 20);
            this.labelOverlay.Name = "labelOverlay";
            this.labelOverlay.Padding = new System.Windows.Forms.Padding(30, 10, 30, 10);
            this.labelOverlay.Size = new System.Drawing.Size(124, 68);
            this.labelOverlay.TabIndex = 0;
            this.labelOverlay.Text = "En";
            // 
            // timerOverlay
            // 
            this.timerOverlay.Interval = 50;
            this.timerOverlay.Tick += new System.EventHandler(this.timerOverlay_Tick);
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.Black;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(116, 76);
            this.ControlBox = false;
            this.Controls.Add(this.labelOverlay);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 100);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(50, 20);
            this.Name = "OverlayForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Overlay";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelOverlay;
        private System.Windows.Forms.Timer timerOverlay;
    }
}