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
            PictureBox.Margin = new Padding(0, 0, 0, 3);
            PictureBox.Name = "PictureBox";
            PictureBox.Size = new Size(286, 250);
            PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBox.TabIndex = 0;
            PictureBox.TabStop = false;
            // 
            // TextDataDiaplay
            // 
            TextDataDiaplay.AutoEllipsis = true;
            TextDataDiaplay.AutoSize = true;
            TextDataDiaplay.Location = new Point(0, 250);
            TextDataDiaplay.Margin = new Padding(0);
            TextDataDiaplay.MaximumSize = new Size(286, 0);
            TextDataDiaplay.Name = "TextDataDiaplay";
            TextDataDiaplay.Size = new Size(59, 25);
            TextDataDiaplay.TabIndex = 1;
            TextDataDiaplay.Text = "label1";
            // 
            // OverlayImage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(286, 333);
            ControlBox = false;
            Controls.Add(TextDataDiaplay);
            Controls.Add(PictureBox);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
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