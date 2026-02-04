using System.Collections.Specialized;
using System.Reflection.Metadata;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Forms.Design;

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
                //Effacer le TreeView
                FilesList.Nodes.Clear();
                //Extension de fichier que l'on veut
                string[] imageExtensions = [".jpg", ".jpeg", ".png", ".tiff", ".exif"];
                //Création de noeuds
                TreeNode rootNode = new(Path.GetFileName(rootPath))
                {
                    Tag = rootPath
                };
                FilesList.Nodes.Add(rootNode);
                void LoadDirectory(string path, TreeNode parentNode)
                {
                    // Sous-dossiers
                    foreach (string dir in Directory.GetDirectories(path))
                    {
                        TreeNode dirNode = new(Path.GetFileName(dir))
                        {
                            Tag = dir
                        };
                        parentNode.Nodes.Add(dirNode);

                        LoadDirectory(dir, dirNode);
                    }
                    // Fichiers images
                    foreach (string file in Directory.GetFiles(path))
                    {
                        string ext = Path.GetExtension(file).ToLower();
                        if (imageExtensions.Contains(ext))
                        {
                            string text = Path.GetFileName(file);
                            try
                            {
                                using Image img = Image.FromFile(file);
                                text += $" ({img.Width}x{img.Height}) => {String.Join(",", img.PropertyIdList)}";
                            }
                            catch { }
                            TreeNode fileNode = new TreeNode(text);
                            fileNode.Tag = file;
                            parentNode.Nodes.Add(fileNode);
                        }
                    }
                }
                LoadDirectory(rootPath, rootNode);
                rootNode.Expand();
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
            /*foreach (var file in FilesList.Nodes)
            {
                if(file.GetType)
            }*/
            //logs.Text = FilesList.Nodes[0].Nodes[0].ForeColor.ToString();
            //FilesList.Nodes[0].Nodes[0].ForeColor = Color.Blue;
            logs.Text = selection;
            RecursiveSearch(FilesList.Nodes, selection);
        }

        private void RecursiveSearch(TreeNodeCollection rootNode, string selection)
        {
            foreach (TreeNode node in rootNode)
            {
                if(node.Nodes.Count > 0)
                {
                    RecursiveSearch(node.Nodes, selection);
                }
                else
                {
                    string FileName = node.Tag.ToString().Split('/')[^1];
                    if (MatchCase.Checked)
                    {
                        FileName = FileName.ToLower();
                        FileName = FileName.Replace(" ", string.Empty);
                    }
                    if(FileName.Contains(selection))
                    {
                        node.ForeColor = Color.Green;
                    }
                    else
                    {
                        node.ForeColor = Color.Red;
                    }
                }
            }
        }
    }
}
