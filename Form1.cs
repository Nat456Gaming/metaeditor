using System.Collections.Specialized;
using System.Security;

namespace metaeditor
{
    public partial class MetaEditor : Form
    {
        public MetaEditor()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                PathBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //Affectation du chemin d'accès à variable locale
            string rootPath = PathBox.Text;
            //Test si le chemin existe
            if (Directory.Exists(rootPath))
            {
                //Effacer le TreeView
                treeView1.Nodes.Clear();
                //Extension de fichier que l'on veut
                string[] imageExtensions = { ".jpg", ".jpeg", ".png" };
                //Création de noeuds
                TreeNode rootNode = new(Path.GetFileName(rootPath))
                {
                    Tag = rootPath
                };
                treeView1.Nodes.Add(rootNode);
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
                                using (Image img = Image.FromFile(file))
                                {
                                    text += $" ({img.Width}x{img.Height}) => {String.Join(",",img.PropertyIdList)}";
                                }
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
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }
    }
}
