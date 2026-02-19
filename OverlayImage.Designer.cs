namespace metaeditor
{
    partial class OverlayImage
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
            PictureBox = new PictureBox();
            TextDataDiaplay = new Label();
            ((System.ComponentModel.ISupportInitialize)PictureBox).BeginInit();
            SuspendLayout();
            // 
            // PictureBox
            // 
            PictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PictureBox.Location = new Point(0, 0);
            PictureBox.Margin = new Padding(0, 0, 0, 2);
            PictureBox.Name = "PictureBox";
            PictureBox.Size = new Size(200, 150);
            PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBox.TabIndex = 0;
            PictureBox.TabStop = false;
            // 
            // TextDataDiaplay
            // 
            TextDataDiaplay.AutoEllipsis = true;
            TextDataDiaplay.AutoSize = true;
            TextDataDiaplay.Location = new Point(0, 150);
            TextDataDiaplay.Margin = new Padding(0);
            TextDataDiaplay.MaximumSize = new Size(200, 0);
            TextDataDiaplay.Name = "TextDataDiaplay";
            TextDataDiaplay.Size = new Size(38, 15);
            TextDataDiaplay.TabIndex = 1;
            TextDataDiaplay.Text = "label1";
            // 
            // OverlayImage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(200, 200);
            ControlBox = false;
            Controls.Add(TextDataDiaplay);
            Controls.Add(PictureBox);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OverlayImage";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "OverlayImage";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)PictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox PictureBox;
        private Label TextDataDiaplay;
    }
}