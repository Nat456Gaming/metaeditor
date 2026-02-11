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
        public MetaEditor()
        {
            InitializeComponent();
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
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".tiff", ".exif"};
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
    }
}