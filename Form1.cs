using System;
using System.Collections.Specialized;
using System.Reflection.Metadata;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Forms.Design;
using System.Xml.Linq;

namespace metaeditor
{
    public partial class MetaEditor : Form
    {
        //Types de métadonnées (soustraire 1)
        public String[] propertyTypes = ["Bytes", "ASCII", "UInt16", "UInt32", "UInt32 Fraction", "Any", "Int32", "", "", "Int32 Fraction"];

        //Creation de la fenetre pour affichage MouseMove
        private readonly OverlayImage _previewForm;
        //Mise en place d'une sécurité pour éviter d'afficher en continu pour MouseMove
        ListViewItem? _lastItem = null;

        public MetaEditor()
        {
            InitializeComponent();
            _previewForm = new OverlayImage(this);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {

        }

        private void SelPathButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                PathBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void PathBox_TextChanged(object sender, EventArgs e)
        {
            //Affectation du chemin d'accès à variable locale
            string rootPath = PathBox.Text;
            //Test si le chemin existe
            if (Directory.Exists(rootPath))
            {
                //Effacer le ListView
                FilesView.Items.Clear();
                FilesView.Groups.Clear();
                //Extension de fichier que l'on veut
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".tiff", ".exif" };
                //Création de noeuds
                void LoadDirectory(string path)
                {
                    // Créer un groupe pour le dossier courant
                    string groupName = Path.GetRelativePath(rootPath, path);
                    if (groupName == ".")
                    {
                        groupName = Path.GetFileName(path);
                    }
                    ListViewGroup group = new ListViewGroup(groupName);
                    FilesView.Groups.Add(group);
                    // Images du dossier A TESTER
                    foreach (string file in Directory.GetFiles(path))
                    {
                        string ext = Path.GetExtension(file).ToLower(); // avoir les extensions en minuscule exemple : au lieu de .PNG on aura .png
                        if (!imageExtensions.Contains(ext)) //test si le fichier n'est pas une image, on peut l'enlever si non nécessaire
                        {
                            continue;
                        }
                        string dimensions = "";
                        /*try
                        {
                            //using (Image img = Image.FromFile(file)) dimensions = $"{img.Width}x{img.Height}"; //Dimension de l'icone
                        }
                        catch { }*/
                        //Mise en place de la ListView
                        ListViewItem item = new ListViewItem(Path.GetFileName(file));
                        item.SubItems.Add(dimensions);
                        item.SubItems.Add(groupName);
                        item.SubItems.Add("Preview ..."); //futur nom
                        item.Tag = file;
                        item.Group = group;
                        //Finalisation
                        FilesView.Items.Add(item);
                    }
                    // Sous-dossiers
                    foreach (string dir in Directory.GetDirectories(path))
                    {
                        LoadDirectory(dir); //ATTENTION récursive
                    }
                }
                LoadDirectory(rootPath);
            }
            UpdateSelection();
        }

        private void MatchCase_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        private void FileSelection_TextChanged(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        private void UseRegex_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            string selection = FileSelection.Text;
            if (MatchCase.Checked)
            {
                selection = selection.Replace(" ", string.Empty);
                selection = selection.ToLower();
            }
            foreach (ListViewItem item in FilesView.Items)
            {
                string FileName = item.Text;
                if (MatchCase.Checked)
                {
                    FileName = FileName.ToLower();
                    FileName = FileName.Replace(" ", string.Empty);
                }

                if ((FileName.Contains(selection) && !UseRegex.Checked))
                {
                    item.ForeColor = Color.Green;
                }
                else
                {
                    item.ForeColor = Color.Red;
                }
            }
        }

        private void FilesView_MouseMove(object sender, MouseEventArgs e)
        {
            //Accès de l'item
            ListViewItem? item = FilesView.GetItemAt(e.X, e.Y);
            //Commencer l'affichage
            if (item != null)
            {
                //On recharge si on change d'item
                if (item != _lastItem)
                {
                    _lastItem = item;
                    string ImagePath = item.Tag.ToString(); //Chemin complet stocker dans l'image
                    if (File.Exists(ImagePath))
                    {
                        _previewForm.SetImage(ImagePath);
                        //previewForm.Size = new Size(300, 300);
                        _previewForm.Show();
                    }
                    //Afficher la fenêtre a une position décalée par raport au souris
                    _previewForm.Location = new Point(Cursor.Position.X + 15, Cursor.Position.Y + 15);
                }
            }
        }

        private void FilesView_MouseLeave(object sender, EventArgs e)
        {
            _previewForm.Hide();
            _lastItem = null;
        }
        private void AddPropertyEditorButton_Click(object sender, EventArgs e)
        {
            new PropertyEditor(PropertyListPanel);
        }

        public class PropertyEditor
        {
            private System.Windows.Forms.Panel m_panel;
            private System.Windows.Forms.TextBox m_textBox;
            private System.Windows.Forms.Button m_close;
            private System.Windows.Forms.ComboBox m_comboBox;
            private System.Windows.Forms.FlowLayoutPanel m_PropertyListPanel;

            public PropertyEditor(System.Windows.Forms.FlowLayoutPanel PropertyListPanel)
            {
                m_PropertyListPanel = PropertyListPanel;

                m_panel = new Panel();
                m_textBox = new System.Windows.Forms.TextBox();
                m_close = new System.Windows.Forms.Button();
                m_comboBox = new System.Windows.Forms.ComboBox();

                m_PropertyListPanel.Controls.Add(m_panel);

                // 
                // panel1
                // 
                m_panel.BorderStyle = BorderStyle.Fixed3D;
                m_panel.Controls.Add(m_textBox);
                m_panel.Controls.Add(m_close);
                m_panel.Controls.Add(m_comboBox);
                m_panel.Location = new Point(3, 3);
                m_panel.Name = "panel1";
                m_panel.Size = new Size(300, 55);
                m_panel.TabIndex = 0;
                // 
                // close
                // 
                //m_close.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                m_close.Location = new Point(274, 3);
                m_close.Name = "close";
                m_close.Size = new Size(23, 23);
                m_close.TabIndex = 1;
                m_close.Text = "x";
                m_close.UseVisualStyleBackColor = true;
                m_close.Click += m_close_Click;
                // 
                // comboBox1
                // 
                m_comboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                m_comboBox.FormattingEnabled = true;
                m_comboBox.Location = new Point(3, 3);
                m_comboBox.Name = "comboBox1";
                m_comboBox.Size = new Size(268, 23);
                m_comboBox.TabIndex = 0;
                // 
                // textBox1
                // 
                m_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                m_textBox.Location = new Point(3, 29);
                m_textBox.Name = "textBox1";
                m_textBox.Size = new Size(294, 23);
                m_textBox.TabIndex = 2;
            }

            private void m_close_Click(object sender, EventArgs e)
            {
                m_PropertyListPanel.Controls.Remove(m_panel);
                m_panel.Controls.Remove(m_textBox);
                m_panel.Controls.Remove(m_close);
                m_panel.Controls.Remove(m_comboBox);
            }
        }
    }
}
