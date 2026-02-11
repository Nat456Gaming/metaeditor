namespace metaeditor
{
    partial class MetaEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            UseRegex = new CheckBox();
            FileSelection = new TextBox();
            SelPathButton = new Button();
            PathBox = new TextBox();
            MatchCase = new CheckBox();
            ApplyButton = new Button();
            FilesView = new ListView();
            folderBrowserDialog1 = new FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.Fixed3D;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(UseRegex);
            splitContainer1.Panel1.Controls.Add(FileSelection);
            splitContainer1.Panel1.Controls.Add(SelPathButton);
            splitContainer1.Panel1.Controls.Add(PathBox);
            splitContainer1.Panel1.Controls.Add(MatchCase);
            splitContainer1.Panel1.Controls.Add(ApplyButton);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(FilesView);
            splitContainer1.Size = new Size(1018, 631);
            splitContainer1.SplitterDistance = 307;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 0;
            // 
            // UseRegex
            // 
            UseRegex.AutoSize = true;
            UseRegex.Location = new Point(8, 67);
            UseRegex.Margin = new Padding(2);
            UseRegex.Name = "UseRegex";
            UseRegex.Size = new Size(248, 24);
            UseRegex.TabIndex = 10;
            UseRegex.Text = "Utiliser les expressions régulières";
            UseRegex.UseVisualStyleBackColor = true;
            UseRegex.CheckedChanged += UseRegex_CheckedChanged;
            // 
            // FileSelection
            // 
            FileSelection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileSelection.Location = new Point(8, 38);
            FileSelection.Margin = new Padding(2);
            FileSelection.Name = "FileSelection";
            FileSelection.PlaceholderText = "Fichiers à sélectionner";
            FileSelection.Size = new Size(291, 27);
            FileSelection.TabIndex = 9;
            FileSelection.TextChanged += FileSelection_TextChanged;
            // 
            // SelPathButton
            // 
            SelPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SelPathButton.Location = new Point(259, 7);
            SelPathButton.Margin = new Padding(2);
            SelPathButton.Name = "SelPathButton";
            SelPathButton.Size = new Size(39, 27);
            SelPathButton.TabIndex = 8;
            SelPathButton.Text = ". . .";
            SelPathButton.UseVisualStyleBackColor = true;
            SelPathButton.Click += SelPathButton_Click;
            // 
            // PathBox
            // 
            PathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PathBox.Location = new Point(8, 9);
            PathBox.Margin = new Padding(2);
            PathBox.Name = "PathBox";
            PathBox.PlaceholderText = "Chemin d'accès";
            PathBox.Size = new Size(249, 27);
            PathBox.TabIndex = 6;
            PathBox.TextChanged += PathBox_TextChanged;
            // 
            // MatchCase
            // 
            MatchCase.AutoSize = true;
            MatchCase.Location = new Point(8, 94);
            MatchCase.Margin = new Padding(2);
            MatchCase.Name = "MatchCase";
            MatchCase.Size = new Size(151, 24);
            MatchCase.TabIndex = 5;
            MatchCase.Text = "Respecter la casse";
            MatchCase.UseVisualStyleBackColor = true;
            MatchCase.CheckedChanged += MatchCase_CheckedChanged;
            // 
            // ApplyButton
            // 
            ApplyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ApplyButton.Location = new Point(211, 592);
            ApplyButton.Margin = new Padding(2);
            ApplyButton.Name = "ApplyButton";
            ApplyButton.Size = new Size(90, 27);
            ApplyButton.TabIndex = 0;
            ApplyButton.Text = "Appliquer";
            ApplyButton.UseVisualStyleBackColor = true;
            ApplyButton.Click += ApplyButton_Click;
            // 
            // FilesView
            // 
            FilesView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            FilesView.Location = new Point(3, 0);
            FilesView.Name = "FilesView";
            FilesView.Size = new Size(698, 624);
            FilesView.TabIndex = 10;
            FilesView.UseCompatibleStateImageBehavior = false;
            FilesView.View = View.Tile;
            // 
            // MetaEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1018, 631);
            Controls.Add(splitContainer1);
            Name = "MetaEditor";
            Text = "MetaEditor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Button ApplyButton;
        private CheckBox MatchCase;
        private Button SelPathButton;
        private TextBox PathBox;
        private FolderBrowserDialog folderBrowserDialog1;
        private CheckBox UseRegex;
        private TextBox FileSelection;
        private ListView FilesView;
    }
}
