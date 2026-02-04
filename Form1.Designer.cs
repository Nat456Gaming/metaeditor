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
            logs = new Label();
            UseRegex = new CheckBox();
            FileSelection = new TextBox();
            SelPathButton = new Button();
            PathBox = new TextBox();
            MatchCase = new CheckBox();
            ApplyButton = new Button();
            FilesList = new TreeView();
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
            splitContainer1.Panel1.Controls.Add(logs);
            splitContainer1.Panel1.Controls.Add(UseRegex);
            splitContainer1.Panel1.Controls.Add(FileSelection);
            splitContainer1.Panel1.Controls.Add(SelPathButton);
            splitContainer1.Panel1.Controls.Add(PathBox);
            splitContainer1.Panel1.Controls.Add(MatchCase);
            splitContainer1.Panel1.Controls.Add(ApplyButton);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(FilesList);
            splitContainer1.Size = new Size(1272, 789);
            splitContainer1.SplitterDistance = 384;
            splitContainer1.TabIndex = 0;
            // 
            // logs
            // 
            logs.AutoSize = true;
            logs.Location = new Point(3, 498);
            logs.Name = "logs";
            logs.Size = new Size(59, 25);
            logs.TabIndex = 11;
            logs.Text = "label1";
            // 
            // UseRegex
            // 
            UseRegex.AutoSize = true;
            UseRegex.Location = new Point(10, 84);
            UseRegex.Name = "UseRegex";
            UseRegex.Size = new Size(295, 29);
            UseRegex.TabIndex = 10;
            UseRegex.Text = "Utiliser les expressions régulières";
            UseRegex.UseVisualStyleBackColor = true;
            UseRegex.CheckedChanged += UseRegex_CheckedChanged;
            // 
            // FileSelection
            // 
            FileSelection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileSelection.Location = new Point(10, 47);
            FileSelection.Name = "FileSelection";
            FileSelection.PlaceholderText = "Fichiers à sélectionner";
            FileSelection.Size = new Size(364, 31);
            FileSelection.TabIndex = 9;
            FileSelection.TextChanged += FileSelection_TextChanged;
            // 
            // SelPathButton
            // 
            SelPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SelPathButton.Location = new Point(325, 9);
            SelPathButton.Margin = new Padding(2);
            SelPathButton.Name = "SelPathButton";
            SelPathButton.Size = new Size(49, 34);
            SelPathButton.TabIndex = 8;
            SelPathButton.Text = ". . .";
            SelPathButton.UseVisualStyleBackColor = true;
            SelPathButton.Click += SelPathButton_Click;
            // 
            // PathBox
            // 
            PathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PathBox.Location = new Point(10, 11);
            PathBox.Margin = new Padding(2);
            PathBox.Name = "PathBox";
            PathBox.PlaceholderText = "Chemin d'accès";
            PathBox.Size = new Size(311, 31);
            PathBox.TabIndex = 6;
            PathBox.TextChanged += PathBox_TextChanged;
            // 
            // MatchCase
            // 
            MatchCase.AutoSize = true;
            MatchCase.Location = new Point(10, 118);
            MatchCase.Margin = new Padding(2);
            MatchCase.Name = "MatchCase";
            MatchCase.Size = new Size(179, 29);
            MatchCase.TabIndex = 5;
            MatchCase.Text = "Respecter la casse";
            MatchCase.UseVisualStyleBackColor = true;
            MatchCase.CheckedChanged += MatchCase_CheckedChanged;
            // 
            // ApplyButton
            // 
            ApplyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ApplyButton.Location = new Point(265, 741);
            ApplyButton.Margin = new Padding(2);
            ApplyButton.Name = "ApplyButton";
            ApplyButton.Size = new Size(112, 34);
            ApplyButton.TabIndex = 0;
            ApplyButton.Text = "Appliquer";
            ApplyButton.UseVisualStyleBackColor = true;
            ApplyButton.Click += ApplyButton_Click;
            // 
            // FilesList
            // 
            FilesList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            FilesList.Location = new Point(4, 51);
            FilesList.Margin = new Padding(4);
            FilesList.Name = "FilesList";
            FilesList.Size = new Size(872, 729);
            FilesList.TabIndex = 9;
            // 
            // MetaEditor
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1272, 789);
            Controls.Add(splitContainer1);
            Margin = new Padding(4);
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
        private TreeView FilesList;
        private CheckBox UseRegex;
        private TextBox FileSelection;
        private Label logs;
    }
}
