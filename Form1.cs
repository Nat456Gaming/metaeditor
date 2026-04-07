using System;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Net;
using System.Reflection.Metadata;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Forms.Design;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace metaeditor
{
    public partial class MetaEditor : Form
    {
        //Déclaration du dictionnaire des handlers à "$"
        private readonly Dictionary<string, Func<string, string, string?>> handlers;

        //Creation de la fenetre pour affichage MouseMove
        private readonly OverlayImage _previewForm;
        //Mise en place d'une sécurité pour éviter d'afficher en continu pour MouseMove
        private ListViewItem? _lastItem;

        //Nombre d'objets PropertyEditor créés
        private int _PropertyEditorNb;
        //ID séectionnés dans les ComboBox
        private readonly List<int> _displayedIds;
        //Valeurs insérées dans les TextInput
        private readonly List<string> _NewValue;

        public MetaEditor()
        {
            InitializeComponent();
            _previewForm = new OverlayImage(this);

            _lastItem = null;
            _PropertyEditorNb = 0;
            _displayedIds = [];
            _NewValue = [];

            //Définition du dictionnaire des endlers à "$"
            handlers = new Dictionary<string, Func<string, string, string?>>
            {
                { "FOLDER", HandleFolder },
                { "DATE", HandleDate },
                { "FILENAME", HandleFilename },
                { "PROPERTY", HandleProperty }
            };
        }

        //Types de métadonnées (soustraire 1)
        public String[] propertyTypes = ["Bytes", "ASCII", "UInt16 (Short)", "UInt32 (Long)", "UInt32 Fraction (Rational)", "Any", "Int32 (SLong)", "", "", "Int32 Fraction (SRational)"];

        //ID métadonnées
        public readonly Dictionary<Int32, String> propertyIds = new()
        {
            { 0x0002, "GpsLatitude" },
            { 0x0004, "GpsLongitude" },
            { 0x0006, "GpsAltitude" },

            { 0x010D, "DocumentName" },
            { 0x010E, "ImageDescription" },
            { 0x010F, "EquipMake" },
            { 0x0110, "EquipModel" },

            { 0x0131, "SoftwareUsed" },
            { 0x0132, "DateTime" },

            { 0x013B, "Artist" },
            { 0x013C, "HostComputer" },

            { 0x0320, "ImageTitle" },

            { 0x5025, "ThumbnailImageDescription" },
            { 0x5026, "ThumbnailEquipMake" },
            { 0x5027, "ThumbnailEquipModel" },
            { 0x5032, "ThumbnailSoftwareUsed" },
            { 0x5033, "ThumbnailDateTime" },
            { 0x5034, "ThumbnailArtist" },
            { 0x503B, "ThumbnailCopyRight" },

            { 0x8298, "Copyright" },

            { 0x829A, "ExifExposureTime" },

            { 0x8822, "ExifExposureProg" },

            { 0x8827, "ExifISOSpeed" },

            { 0x9201, "ExifShutterSpeed" },
            { 0x9202, "ExifAperture" },
            { 0x9203, "ExifBrightness" },
            { 0x9204, "ExifExposureBias" },
            { 0x9205, "ExifMaxAperture" },
            { 0x9206, "ExifSubjectDist" },
            { 0x9207, "ExifMeteringMode" },
            { 0x9208, "ExifLightSource" },
            { 0x9209, "ExifFlash" },
            { 0x920A, "ExifFocalLenght" },

            { 0x927C, "ExifMakerNote" },

            { 0x9286, "ExifUserComment" }
        };

        public struct MetadataProperties(short i_type, int i_l_min, int i_l_max)
        {
            public short type = i_type;
            public int l_min = i_l_min;
            public int l_max = i_l_max;
        }

        public readonly Dictionary<Int32, MetadataProperties> propertyIdTypes = new()
        {
            { 0x0002, new MetadataProperties(5,3,3) },
            { 0x0004, new MetadataProperties(5,3,3) },
            { 0x0006, new MetadataProperties(5,3,3) },

            { 0x010D, new MetadataProperties(2,-1,-1) },
            { 0x010E, new MetadataProperties(2,-1,-1) },
            { 0x010F, new MetadataProperties(2,-1,-1) },
            { 0x0110, new MetadataProperties(2,-1,-1) },

            { 0x0131, new MetadataProperties(2,-1,-1) },
            { 0x0132, new MetadataProperties(2,20,20) },

            { 0x013B, new MetadataProperties(2,-1,-1) },
            { 0x013C, new MetadataProperties(2,-1,-1) },

            { 0x0320, new MetadataProperties(2,-1,-1) },

            { 0x5025, new MetadataProperties(2,-1,-1) },
            { 0x5026, new MetadataProperties(2,-1,-1) },
            { 0x5027, new MetadataProperties(2,-1,-1) },
            { 0x5032, new MetadataProperties(2,-1,-1) },
            { 0x5033, new MetadataProperties(2,20,20) },
            { 0x5034, new MetadataProperties(2,-1,-1) },
            { 0x503B, new MetadataProperties(2,-1,-1) },

            { 0x8298, new MetadataProperties(2,-1,-1) },

            { 0x829A, new MetadataProperties(5,1,1) },

            { 0x8822, new MetadataProperties(3,1,1) },

            { 0x8827, new MetadataProperties(3,0,-1) },

            { 0x9201, new MetadataProperties(10,1,1) },
            { 0x9202, new MetadataProperties(5,1,1) },
            { 0x9203, new MetadataProperties(10,1,1) },
            { 0x9204, new MetadataProperties(10,1,1) },
            { 0x9205, new MetadataProperties(5,1,1) },
            { 0x9206, new MetadataProperties(5,1,1) },
            { 0x9207, new MetadataProperties(3,1,1) },
            { 0x9208, new MetadataProperties(3,1,1) },
            { 0x9209, new MetadataProperties(3,1,1) },
            { 0x920A, new MetadataProperties(5,1,1) },

            { 0x927C, new MetadataProperties(6,-1,-1) },

            { 0x9286, new MetadataProperties(6,-1,-1) }
        };

        public static String DecodeProperty(System.Drawing.Imaging.PropertyItem? property)
        {
            if (property != null && property.Value != null)
            {
                switch (property.Type)
                {
                    case 1: //Bytes
                        return BitConverter.ToString(property.Value);

                    case 2: // ASCII
                        return System.Text.Encoding.UTF8.GetString(property.Value, 0, property.Len - 1);

                    case 3: // UInt16 (Short)
                        UInt16[] val_uint16 = new UInt16[property.Len / 2];
                        for (int j = 0; j < property.Len / 2; j++)
                        {
                            byte[] val = { property.Value[2 * j], property.Value[2 * j + 1] };
                            val_uint16[j] = BitConverter.ToUInt16(val);
                        }
                        return String.Join("-", val_uint16);

                    case 4: // UInt32 (Long)
                        UInt32[] val_uint32 = new UInt32[property.Len / 4];
                        for (int j = 0; j < property.Len / 4; j++)
                        {
                            byte[] val = { property.Value[4 * j], property.Value[4 * j + 1], property.Value[4 * j + 2], property.Value[4 * j + 3] };
                            val_uint32[j] = BitConverter.ToUInt32(val);
                        }
                        return String.Join("-", val_uint32);

                    case 5: // UInt32 Fraction (Rational)
                        UInt32[,] frac_uint32 = new UInt32[property.Len / 8, 2];
                        for (int j = 0; j < property.Len / 8; j ++)
                        {
                            byte[] val1 = { property.Value[8 * j], property.Value[8 * j + 1], property.Value[8 * j + 2], property.Value[8 * j + 3] };
                            frac_uint32[j,0] = BitConverter.ToUInt32(val1);
                            byte[] val2 = { property.Value[8 * j + 4], property.Value[8 * j + 5], property.Value[8 * j + 6], property.Value[8 * j + 7] };
                            frac_uint32[j,1] = BitConverter.ToUInt32(val2);
                        }
                        string[] joinustr = new string[property.Len / 8];
                        for (int j = 0;j < property.Len / 8; j++)
                        {
                            joinustr[j] = frac_uint32[j, 0] + "/" + frac_uint32[j, 1];
                        }

                        return String.Join("-", joinustr);

                    case 6: // Any
                        return BitConverter.ToString(property.Value);

                    case 7: // Int32 (SLong)
                        Int32[] val_int32 = new Int32[property.Len / 4];
                        for (int j = 0; j < property.Len / 4; j++)
                        {
                            byte[] val = { property.Value[4 * j], property.Value[4 * j + 1], property.Value[4 * j + 2], property.Value[4 * j + 3] };
                            val_int32[j] = BitConverter.ToInt32(val);
                        }
                        return String.Join("-", val_int32);

                    case 10: // Int32 Fraction (SRational)
                        Int32[,] frac_int32 = new Int32[property.Len / 8, 2];
                        for (int j = 0; j < property.Len / 8; j++)
                        {
                            byte[] val1 = { property.Value[8 * j], property.Value[8 * j + 1], property.Value[8 * j + 2], property.Value[8 * j + 3] };
                            frac_int32[j, 0] = BitConverter.ToInt32(val1);
                            byte[] val2 = { property.Value[8 * j + 4], property.Value[8 * j + 5], property.Value[8 * j + 6], property.Value[8 * j + 7] };
                            frac_int32[j, 1] = BitConverter.ToInt32(val2);
                        }
                        string[] joinstr = new string[property.Len / 8];
                        for (int j = 0; j < property.Len / 8; j++)
                        {
                            joinstr[j] = frac_int32[j, 0] + "/" + frac_int32[j, 1];
                        }

                        return String.Join("-", joinstr);

                    default:
                        return "";
                }
            }
            return "";
        }

        public bool EncodeProperty(String input, Int32 propertyId, Image image)
        {
            //PropertyItem does not have a constructor so we take one from the image
            System.Drawing.Imaging.PropertyItem property = image.PropertyItems[0];
            //we put the new type and new Id
            property.Type = this.propertyIdTypes[propertyId].type;
            property.Id = propertyId;

            //we encode the value depending on the type
            switch (property.Type)
            {
                case 2: // ASCII
                    if ((input.Length + 1 >= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_min < 0) && (input.Length + 1 <= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_max < 0))
                    {
                        property.Value = System.Text.Encoding.UTF8.GetBytes(input + '\0');
                        property.Len = input.Length + 1;
                        image.SetPropertyItem(property);
                        return true;
                    }
                    return false;

                case 3: // UInt16 (Short)
                    string[] values_uint16 = input.Split('-');
                    if ((values_uint16.Length >= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_min < 0) && (values_uint16.Length <= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_max < 0))
                    {
                        UInt16[] val_uint16 = new UInt16[values_uint16.Length];
                        for (int i = 0; i < values_uint16.Length; i++)
                        {
                            val_uint16[i] = UInt16.Parse(values_uint16[i].Trim(' '));
                        }
                        byte[] out_uint16 = new byte[2 * val_uint16.Length];
                        for (int i = 0; i < values_uint16.Length; i++)
                        {
                            byte[] val = BitConverter.GetBytes(val_uint16[i]);
                            out_uint16[2 * i] = val[0];
                            out_uint16[2 * i + 1] = val[1];
                        }
                        property.Value = out_uint16;
                        property.Len = out_uint16.Length;
                        image.SetPropertyItem(property);
                        return true;
                    }
                    return false;

                case 4: // UInt32 (Long)
                    string[] values_uint32 = input.Split('-');
                    if ((values_uint32.Length >= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_min < 0) && (values_uint32.Length <= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_max < 0))
                    {
                        UInt32[] val_uint32 = new UInt32[values_uint32.Length];
                        for (int i = 0; i < values_uint32.Length; i++)
                        {
                            val_uint32[i] = UInt32.Parse(values_uint32[i].Trim(' '));
                        }
                        byte[] out_uint32 = new byte[4 * val_uint32.Length];
                        for (int i = 0; i < values_uint32.Length; i++)
                        {
                            byte[] val = BitConverter.GetBytes(val_uint32[i]);
                            out_uint32[4 * i] = val[0];
                            out_uint32[4 * i + 1] = val[1];
                            out_uint32[4 * i + 2] = val[2];
                            out_uint32[4 * i + 3] = val[3];
                        }
                        property.Value = out_uint32;
                        property.Len = out_uint32.Length;
                        image.SetPropertyItem(property);
                        return true;
                    }
                    return false;

                case 5: // UInt32 Fraction (Rational)
                    string[] values_uint32_f = input.Split('-');
                    if ((values_uint32_f.Length >= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_min == -1) && (values_uint32_f.Length <= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_max == -1))
                    {
                        UInt32[] frac_uint32 = new UInt32[values_uint32_f.Length * 2];
                        for (int i = 0; i < values_uint32_f.Length; i++)
                        {
                            string[] temp = values_uint32_f[i].Split("/");
                            if (temp.Length != 2) return false;
                            frac_uint32[2 * i] = UInt32.Parse(temp[0].Trim(' '));
                            frac_uint32[2 * i + 1] = UInt32.Parse(temp[1].Trim(' '));
                        }
                        byte[] out_uint32_f = new byte[4 * frac_uint32.Length];
                        for (int i = 0; i < frac_uint32.Length; i++)
                        {
                            byte[] val = BitConverter.GetBytes(frac_uint32[i]);
                            out_uint32_f[4 * i] = val[0];
                            out_uint32_f[4 * i + 1] = val[1];
                            out_uint32_f[4 * i + 2] = val[2];
                            out_uint32_f[4 * i + 3] = val[3];
                        }
                        property.Value = out_uint32_f;
                        property.Len = out_uint32_f.Length;
                        image.SetPropertyItem(property);
                        return true;
                    }
                    return false;

                case 7: // Int32 (SLong)
                    string[] values_int32 = input.Split('-');
                    if ((values_int32.Length >= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_min == -1) && (values_int32.Length <= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_max == -1))
                    {
                        Int32[] val_int32 = new Int32[values_int32.Length];
                        for (int i = 0; i < values_int32.Length; i++)
                        {
                            val_int32[i] = Int32.Parse(values_int32[i].Trim(' '));
                        }
                        byte[] out_int32 = new byte[4 * val_int32.Length];
                        for (int i = 0; i < values_int32.Length; i++)
                        {
                            byte[] val = BitConverter.GetBytes(val_int32[i]);
                            out_int32[4 * i] = val[0];
                            out_int32[4 * i + 1] = val[1];
                            out_int32[4 * i + 2] = val[2];
                            out_int32[4 * i + 3] = val[3];
                        }
                        property.Value = out_int32;
                        property.Len = out_int32.Length;
                        image.SetPropertyItem(property);
                        return true;
                    }
                    return false;

                case 10: // Int32 Fraction (SRational)
                    string[] values_int32_f = input.Split('-');
                    if ((values_int32_f.Length >= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_min == -1) && (values_int32_f.Length <= this.propertyIdTypes[propertyId].l_min || this.propertyIdTypes[propertyId].l_max == -1))
                    {
                        Int32[] frac_int32 = new Int32[values_int32_f.Length * 2];
                        for (int i = 0; i < values_int32_f.Length; i++)
                        {
                            string[] temp = values_int32_f[i].Split("/");
                            if (temp.Length != 2) return false;
                            frac_int32[2 * i] = Int32.Parse(temp[0].Trim(' '));
                            frac_int32[2 * i + 1] = Int32.Parse(temp[1].Trim(' '));
                        }
                        byte[] out_int32_f = new byte[4 * frac_int32.Length];
                        for (int i = 0; i < frac_int32.Length; i++)
                        {
                            byte[] val = BitConverter.GetBytes(frac_int32[i]);
                            out_int32_f[4 * i] = val[0];
                            out_int32_f[4 * i + 1] = val[1];
                            out_int32_f[4 * i + 2] = val[2];
                            out_int32_f[4 * i + 3] = val[3];
                        }
                        property.Value = out_int32_f;
                        property.Len = out_int32_f.Length;
                        image.SetPropertyItem(property);
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        //Fonction qui permet d'avoir le nom du dossier en fonction du niveau de la variable $FOLDER
        private static string GetFolderName(string path, int level)
        {
            DirectoryInfo dir = new(path);

            for (int i = 0; i < level; i++)
            {
                if (dir.Parent != null)
                    dir = dir.Parent;
                else
                    return dir.Name; // ou gérer autrement (racine atteinte)
            }
            return dir.Name;
        }

        //Cette fonction permet d'englober les noms et les variables spéciaux du "$"
        private string? ResolveValue(string input, string imgpath)
        {
            //Test si c'est juste du texte sans "$"
            if (!input.StartsWith('$'))
            {
                return input;
            }
            //Il y a un '$', d'abord il faut extraire le nom du token (ex: FOLDER, DATE...)
            string token = new(input.Skip(1).TakeWhile(char.IsLetter).ToArray()); //Le skip ne prend pas le premier caractère qui est "$", ensuite il prend les caractères qui sont des lettres et enfin il le transforme en tableau donc en un string
            //Correspondance avec le dictionnaire
            if (handlers.TryGetValue(token, out var handler))
            {
                return handler(input,imgpath);
            }
            //Si rien n'est fonctionnel, alors affiche une erreur dans le TextBox (à changé après les tests)
            return null;
        }

        //Fonction qui gère "$FOLDER" qui est assigné dans le dictionnaire
        private string? HandleFolder(string input, string imgpath)
        {
            string numberPart = input.Substring(7); //prend seulement les caractères après les 7 premières caractères qui sont "$FOLDER"
            //Test si les caractères prises avec les 7 premières caractères sont un nombre
            if (int.TryParse(numberPart, out int level))
            {
                return GetFolderName(imgpath, level+1);
            }
            //Si rien n'est fonctionnel, alors affiche un erreur dans le TextBox (à changer après les tests)
            return null;
        }

        //Fonction qui gère "$DATE" qui est assigné dans le dictionnaire - ATTENTION ! j'ai mis input et rootpath mais je ne l'utilise pas
        private string? HandleDate(string input, string imgpath)
        {
            return DateTime.Now.ToString("dd-MM-yyyy");
        }

        //Fonction qui gère "$FILENAME" qui est assigné dans le dictionnaire - ATTENTION ! j'ai mis input mais je ne l'utilise pas
        private string? HandleFilename(string input, string imgpath)
        {
            return Path.GetFileName(imgpath);
        }

        //Fonction qui gère "$PROPERTY" qui est assigné dans le dictionnaire
        private string? HandleProperty(string input, string imgpath)
        {
            string propertyIdstr = input.Substring(9); //prend seulement les caractères après les 9 premières caractères qui sont "$PROPERTY"

            //Test si les caractères prises avec les 9 premières caractères sont un nombre
            if (int.TryParse(propertyIdstr, out int propertyId))
            {
                Image image = Image.FromFile(imgpath);
                if (image.PropertyIdList.Contains(propertyId))
                {
                    return DecodeProperty(image.GetPropertyItem(propertyId));
                }
                return null;
            }
            //Si rien n'est fonctionnel, alors affiche un erreur dans le TextBox (à changer après les tests)
            return null;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            //Parcours des images
            foreach (ListViewItem item in FilesView.Items)
            {
                //Test pour vérifier si leur nom est vert (images sélectionnées)
                if (item.ForeColor == Color.Green)
                {
                    String path = item.Tag.ToString();
                    System.IO.File.Move(path,path + ".old");
                    Image img = Image.FromFile(path + ".old");
                    //Parcours de displayedID et NewValue pour pouvoir appliquer toutes les modifications à chaque image
                    for (int i = 0; i < _PropertyEditorNb; i++)
                    {
                        //Test pour vérifier que l'Id de propriété est bien présent dans le dictonnaire (modification valide)
                        if (propertyIds.TryGetValue(_displayedIds[i], out string? a))
                        {
                            if (item.Tag != null)
                            {
                                //Décodage des variables '$'
                                string? value = ResolveValue(_NewValue[i], path);
                                if (value != null)
                                {
                                    // Encodage et application des propriétés
                                    if (!EncodeProperty(value, _displayedIds[i], img))
                                    {
                                        item.ForeColor = Color.Orange;
                                        throw new Exception("Error writing the property: " + this.propertyIds[_displayedIds[i]] + " with the value:" + value + " to image: " + item.Tag.ToString());
                                    }
                                    else
                                    {
                                        item.ForeColor = Color.Blue;
                                    }
                                }
                            }
                        }
                    }
                    bool exception = false;
                    try
                    {
                        img.Save(path);
                    }
                    catch
                    {
                        exception = true;
                        throw new Exception("Error writing the file " + path);
                    }
                    img.Dispose();
                    if (!exception)
                    {
                        System.IO.File.Delete(path + ".old");
                    }
                }
            }
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
                    ListViewGroup group = new(groupName);
                    FilesView.Groups.Add(group);
                    // Images du dossier A TESTER
                    foreach (string file in Directory.GetFiles(path))
                    {
                        string ext = Path.GetExtension(file).ToLower(); // passer les estension en minuscule pour une meilleur détection
                        if (!imageExtensions.Contains(ext)) //test si le fichier n'est pas une image, on l'ignore
                        {
                            continue;
                        }
                        string dimensions = "";
                        /*try
                        {
                            //using (Image img = Image.FromFile(file)) dimensions = $"{img.Width}x{img.Height}"; //Dimension de l'icone
                        }
                        catch { }*/
                        //Mise en place du ListView
                        ListViewItem item = new(Path.GetFileName(file));
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
            
                if ((FileName.Contains(selection) && !UseRegex.Checked) || (Regex.IsMatch(FileName, @selection) && UseRegex.Checked))
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
                if (item != _lastItem && item.Tag != null)
                {
                    _lastItem = item;
                    string? ImagePath = item.Tag.ToString(); //Chemin complet stocker dans l'image
                    if (File.Exists(ImagePath))
                    {
                        _previewForm.SetImage(ImagePath, _displayedIds);
                        //previewForm.Size = new Size(300, 300);
                        _previewForm.Show();
                    }
                    //Afficher la fenêtre a une position décalée par raport au souris
                    _previewForm.Location = new Point(Cursor.Position.X + 15, Cursor.Position.Y + 15);
                }
            }
            else
            {
                _previewForm.Remove();
                _lastItem = null;
            }
        }

        private void FilesView_MouseLeave(object sender, EventArgs e)
        {
            _previewForm.Remove();
            _lastItem = null;
        }
        private void AddPropertyEditorButton_Click(object sender, EventArgs e)
        {
            new PropertyEditor(PropertyListPanel, propertyIds, CurrentAutoScaleDimensions.Width, _PropertyEditorNb, _displayedIds, _NewValue);
            _PropertyEditorNb++;    
        }

        public class PropertyEditor
        {
            private System.Windows.Forms.Panel m_panel;
            private System.Windows.Forms.TextBox m_textBox;
            private System.Windows.Forms.Button m_close;
            private System.Windows.Forms.ComboBox m_comboBox;
            private System.Windows.Forms.FlowLayoutPanel m_PropertyListPanel;
            private int m_Id;
            private readonly List<int> m_displayedIds;
            private readonly List<string> m_NewValue;
            private readonly Dictionary<Int32, String> m_propertyIds;

            public PropertyEditor(System.Windows.Forms.FlowLayoutPanel PropertyListPanel, Dictionary<Int32, String> propertyIds, float CurrentAutoScaleDimensionsWidth, int PropertyEditorId, List<int> displayedIds, List<string> NewValue)
            {
                m_PropertyListPanel = PropertyListPanel;
                m_Id = PropertyEditorId;
                m_displayedIds = displayedIds;
                m_propertyIds = propertyIds;
                m_NewValue = NewValue;

                m_displayedIds.Add(0);
                m_NewValue.Add("");

                m_panel = new Panel();
                m_textBox = new System.Windows.Forms.TextBox();
                m_close = new System.Windows.Forms.Button();
                m_comboBox = new System.Windows.Forms.ComboBox();

                m_PropertyListPanel.Controls.Add(m_panel);

                int margin = 3, size = (int)(CurrentAutoScaleDimensionsWidth * 3.5), width = 300;

                // 
                // panel1
                // 
                m_panel.BorderStyle = BorderStyle.Fixed3D;
                m_panel.Controls.Add(m_textBox);
                m_panel.Controls.Add(m_close);
                m_panel.Controls.Add(m_comboBox);
                m_panel.Location = new Point(3, 3);
                m_panel.Name = "panel1";
                m_panel.Size = new Size(width, 2 * size + 4 * margin);
                m_panel.TabIndex = 0;
                // 
                // close
                // 
                //m_close.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                m_close.Location = new Point(width - margin - size, margin);
                m_close.Name = "close";
                m_close.Size = new Size(size, size);
                m_close.TabIndex = 1;
                m_close.Text = "x";
                m_close.UseVisualStyleBackColor = true;
                m_close.Click += Close_Click;
                //
                // comboBox1
                //
                //m_comboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                m_comboBox.FormattingEnabled = true;
                m_comboBox.Location = new Point(margin, margin);
                m_comboBox.Name = "comboBox1";
                m_comboBox.Size = new Size(width - 3 * margin - size, size);
                m_comboBox.TabIndex = 0;
                m_comboBox.DataSource = new BindingSource(m_propertyIds, "");
                m_comboBox.DisplayMember = "Value";
                m_comboBox.ValueMember = "Key";
                m_comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                //
                // textBox1
                //
                //m_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                m_textBox.Location = new Point(margin, size + 2 * margin);
                m_textBox.Name = "textBox1";
                m_textBox.Size = new Size(width - 2 * margin, size);
                m_textBox.TabIndex = 2;
                m_textBox.TextChanged += TextBox_TextChanged;
            }

            private void Close_Click(object? sender, EventArgs e)
            {
                m_PropertyListPanel.Controls.Remove(m_panel);
                m_panel.Controls.Remove(m_textBox);
                m_panel.Controls.Remove(m_close);
                m_panel.Controls.Remove(m_comboBox);
                m_displayedIds[m_Id] = 100000;
                m_NewValue[m_Id] = "";
            }

            private void ComboBox_SelectedIndexChanged(object? sender, EventArgs e)
            {
                if (m_comboBox.SelectedItem != null)
                    m_displayedIds[m_Id] = ((KeyValuePair<int, string>)m_comboBox.SelectedItem).Key;
            }

            //Fonction qui détecte les noms spéciaux après le symbole "$" en utilisant du Regex
            private bool IsValidVariable(string input)
            {
                return Regex.IsMatch(input, @"^\$(FOLDER\d+|DATE|FILENAME|PROPERTY\d+)$"); //Ajouter d'autre nom en fonction de notre besoin
            }

            private void TextBox_TextChanged(object? sender, EventArgs e)
            {
                m_NewValue[m_Id] = m_textBox.Text;
                string selection = m_textBox.Text;
                if (!selection.Contains("$"))
                {
                    m_textBox.ForeColor = Color.Black;
                }
                else if(IsValidVariable(selection))
                {
                    m_textBox.ForeColor = Color.Green;
                }
                else
                {
                    m_textBox.ForeColor = Color.Red;
                }
            }
        }
    }
}
