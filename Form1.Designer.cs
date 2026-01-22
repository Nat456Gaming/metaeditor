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
            SelPathButton = new Button();
            PathBox = new TextBox();
            checkBox3 = new CheckBox();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            textBox1 = new TextBox();
            ApplyButton = new Button();
            listView1 = new ListView();
            treeView1 = new TreeView();
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
            splitContainer1.Panel1.Controls.Add(SelPathButton);
            splitContainer1.Panel1.Controls.Add(PathBox);
            splitContainer1.Panel1.Controls.Add(checkBox3);
            splitContainer1.Panel1.Controls.Add(checkBox2);
            splitContainer1.Panel1.Controls.Add(checkBox1);
            splitContainer1.Panel1.Controls.Add(textBox1);
            splitContainer1.Panel1.Controls.Add(ApplyButton);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(listView1);
            splitContainer1.Panel2.Controls.Add(treeView1);
            splitContainer1.Size = new Size(1272, 789);
            splitContainer1.SplitterDistance = 384;
            splitContainer1.TabIndex = 0;
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
            SelPathButton.Click += button2_Click;
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
            PathBox.TextChanged += textBox2_TextChanged;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(11, 292);
            checkBox3.Margin = new Padding(2);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(179, 29);
            checkBox3.TabIndex = 5;
            checkBox3.Text = "Respecter la casse";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(11, 258);
            checkBox2.Margin = new Padding(2);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(334, 29);
            checkBox2.TabIndex = 4;
            checkBox2.Text = "Correspondre à toutes les occurences";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(11, 222);
            checkBox1.Margin = new Padding(2);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(295, 29);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "Utiliser les expressions régulières";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(11, 185);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(316, 31);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
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
            ApplyButton.Click += button1_Click_1;
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listView1.Location = new Point(4, 4);
            listView1.Margin = new Padding(4);
            listView1.Name = "listView1";
            listView1.Size = new Size(872, 39);
            listView1.TabIndex = 10;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // treeView1
            // 
            treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeView1.Location = new Point(4, 51);
            treeView1.Margin = new Padding(4);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(872, 729);
            treeView1.TabIndex = 9;
            treeView1.AfterSelect += treeView1_AfterSelect_1;
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
        private TextBox textBox1;
        private CheckBox checkBox3;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private Button SelPathButton;
        private TextBox PathBox;
        private FolderBrowserDialog folderBrowserDialog1;
        private TreeView treeView1;
        private ListView listView1;
    }
}
