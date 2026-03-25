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

    struct PropertyItem
    {
       
    }
    public partial class MetaEditor : Form
    {
        //Déclaration du dictionnaire de variable à "$"
        private readonly Dictionary<string, Func<string, string, string>> handlers;

        //Creation de la fenetre pour affichage MouseMove
        private readonly OverlayImage _previewForm;
        //Mise en place d'une sécurité pour éviter d'afficher en continu pour MouseMove
        private ListViewItem? _lastItem;

        private int _PropertyEditorNb;
        private List<int> _displayedIds;
        private List<string> _NewValue;

        public MetaEditor()
        {
            InitializeComponent();
            _previewForm = new OverlayImage(this);

            _lastItem = null;
            _PropertyEditorNb = 0;
            _displayedIds = [];
            _NewValue = [];

            //Définition du dictionnaire de variable à "$"
            handlers = new Dictionary<string, Func<string, string, string>>
            {
                { "FOLDER", HandleFolder },
                { "DATE", HandleDate },
                { "FILENAME", HandleFilename },
                { "PROPERTY", HandleProperty }
            };
        }

        //Types de métadonnées (soustraire 1)
        public String[] propertyTypes = ["Bytes", "ASCII", "UInt16", "UInt32", "UInt32 Fraction", "Any", "Int32", "", "", "Int32 Fraction"];

        //ID métadonnées
        public readonly Dictionary<Int32, String> propertyIds = new()
        {
            { 0x0000, "GpsVer" },
            { 0x0001, "GpsLatitudeRef" },
            { 0x0002, "GpsLatitude" },          //
            { 0x0003, "GpsLongitudeRef" },
            { 0x0004, "GpsLongitude" },         //
            { 0x0005, "GpsAltitudeRef" },
            { 0x0006, "GpsAltitude" },          //
            { 0x0007, "GpsGpsTime" },
            { 0x0008, "GpsGpsSatellites" },
            { 0x0009, "GpsGpsStatus" },
            { 0x000A, "GpsGpsMeasureMode" },
            { 0x000B, "GpsGpsDop" },
            { 0x000C, "GpsSpeedRef" },
            { 0x000D, "GpsSpeed" },
            { 0x000E, "GpsTrackRef" },
            { 0x000F, "GpsTrack" },
            { 0x0010, "GpsImgDirRef" },
            { 0x0011, "GpsImgDir" },
            { 0x0012, "GpsMapDatum" },
            { 0x0013, "GpsDestLatRef" },
            { 0x0014, "GpsDestLat" },
            { 0x0015, "GpsDestLongRef" },
            { 0x0016, "GpsDestLong" },
            { 0x0017, "GpsDestBearRef" },
            { 0x0018, "GpsDestBear" },
            { 0x0019, "GpsDestDistRef" },
            { 0x001A, "GpsDestDist" },

            { 0x001D, "29" },

            { 0x00FE, "NewSubfileType" },
            { 0x00FF, "SubfileType" },
            { 0x0100, "ImageWidth" },
            { 0x0101, "ImageHeight" },
            { 0x0102, "BitsPerSample" },
            { 0x0103, "TagCompression" },

            { 0x0106, "PhotometricInterp" },
            { 0x0107, "ThreshHolding" },
            { 0x0108, "CellWidth" },
            { 0x0109, "CellHeight" },
            { 0x010A, "FillOrder" },

            { 0x010D, "DocumentName" },         //
            { 0x010E, "ImageDescription" },     //
            { 0x010F, "EquipMade" },            //
            { 0x0110, "EquipModel" },           //
            { 0x0111, "StripOffsets" },
            { 0x0112, "Orientation" },

            { 0x0115, "SamplesPerPixel" },
            { 0x0116, "RowsPerStrip" },
            { 0x0117, "StripBytesCount" },
            { 0x0118, "MinSampleValue" },
            { 0x0119, "MaxSampleValue" },
            { 0x011A, "XResolution" },
            { 0x011B, "YResolution" },
            { 0x011C, "PlanarConfig" },
            { 0x011D, "PageName" },
            { 0x011E, "XPosition" },
            { 0x011F, "YPosition" },
            { 0x0120, "FreeOffset" },
            { 0x0121, "FreeByteCounts" },
            { 0x0122, "GrayResponseUnit" },
            { 0x0123, "GrayResponseCurve" },
            { 0x0124, "T4Option" },
            { 0x0125, "T6Option" },

            { 0x0128, "ResolutionUnit" },
            { 0x0129, "PageNumber" },

            { 0x012D, "TranferFunction" },

            { 0x0131, "SoftwareUsed" },         //
            { 0x0132, "DateTime" },             //

            { 0x013B, "Artist" },               //
            { 0x013C, "HostComputer" },         //
            { 0x013D, "Predictor" },
            { 0x013E, "WhitePoint" },
            { 0x013F, "PrimaryChromaticies" },
            { 0x0140, "ColorMap" },
            { 0x0141, "HalftoneHints" },
            { 0x0142, "TileWidth" },
            { 0x0143, "TileLenght" },
            { 0x0144, "TileOffset" },
            { 0x0145, "TileByteCounts" },

            { 0x014C, "InkSet" },
            { 0x014D, "InkNames" },
            { 0x014E, "NumberOfInks" },

            { 0x0150, "DotRange" },
            { 0x0151, "TargetPrinter" },
            { 0x0152, "ExtraSamples" },
            { 0x0153, "SampleFormat" },
            { 0x0154, "SMinSampleValue" },
            { 0x0155, "SMaxSampleValue" },
            { 0x0156, "TranferRange" },

            { 0x0200, "JPEGProc" },
            { 0x0201, "JPEGInterFormat" },
            { 0x0202, "JPEGInterLenght" },
            { 0x0203, "JPEGRestartInterval" },

            { 0x0205, "JPEGLosslessPredictors" },
            { 0x0206, "JPEGPointTransforms" },
            { 0x0207, "JPEGQTables" },
            { 0x0208, "JPEGDCTables" },
            { 0x0209, "JPEGACTables" },

            { 0x0211, "YCbCrCoefficients" },
            { 0x0212, "YCbCrSubsampling" },
            { 0x0213, "YCbCrPositionning" },
            { 0x0214, "REFBlackWhite" },

            { 0x0301, "TagGamma" },
            { 0x0302, "ICCProfileDescriptor" },
            { 0x0303, "SRGBRenderingIntent" },

            { 0x0320, "ImageTitle" },               //

            { 0x5001, "ResolutionXUnit" },
            { 0x5002, "ResolutionYUnit" },
            { 0x5003, "ResolutionXLenghtUnit" },
            { 0x5004, "ResolutionYLenghtUnit" },
            { 0x5005, "PrintFlags" },
            { 0x5006, "PrintFlagsVersion" },
            { 0x5007, "PrintFlagsCrop" },
            { 0x5008, "PrintFlagsBleedWith" },
            { 0x5009, "PrintFlagsBleedWidthScale" },
            { 0x500A, "HalftoneLPI" },
            { 0x500B, "HalftoneLPIUnit" },
            { 0x500C, "HalftoneDegree" },
            { 0x500D, "HalftoneShape" },
            { 0x500E, "HaltoneMisc" },
            { 0x500F, "HalftoneScreen" },
            { 0x5010, "JPEGQuality" },
            { 0x5011, "GridSize" },
            { 0x5012, "ThumbnailFormat" },
            { 0x5013, "ThumbnailWidth" },
            { 0x5014, "ThumbnailHeight" },
            { 0x5015, "ThumbnailColorDepth" },
            { 0x5016, "ThumbnailPlanes" },
            { 0x5017, "ThumbnailRawBytes" },
            { 0x5018, "ThumbnailSize" },
            { 0x5019, "ThumbnailCompressedSize" },
            { 0x501A, "ColorTransferFunction" },
            { 0x501B, "ThumbnailData" },

            { 0x5020, "ThumbnailImageWidth" },
            { 0x5021, "ThumbnailImageHeight" },
            { 0x5022, "ThumbnailBitsPerSample" },
            { 0x5023, "ThumbnailCompression" },
            { 0x5024, "ThumbnailPhotometricInterp" },
            { 0x5025, "ThumbnailImageDescription" },
            { 0x5026, "ThumbnailEquipMake" },               //
            { 0x5027, "ThumbnailEquipModel" },              //
            { 0x5028, "ThumbnailStripOffsets" },
            { 0x5029, "ThumbnailOrientation" },
            { 0x502A, "ThumbnailSamplesPerPixel" },
            { 0x502B, "ThumbnailRowsPerStrip" },
            { 0x502C, "ThumbnailStripBytesCount" },
            { 0x502D, "ThumbnailResolutionX" },
            { 0x502E, "ThumbnailResolutionY" },
            { 0x502F, "ThumbnailPlanarConfig" },
            { 0x5030, "ThumbnailResolutionUnit" },
            { 0x5031, "ThumbnailTransferFunction" },
            { 0x5032, "ThumbnailSoftwareUsed" },
            { 0x5033, "ThumbnailDateTime" },                //
            { 0x5034, "ThumbnailArtist" },                  //
            { 0x5035, "ThumbnailWhitePoint" },
            { 0x5036, "ThumbnailPrimaryChromaticies" },
            { 0x5037, "ThumbnailYCbCrCoefficients" },
            { 0x5038, "ThumbnailYCbCrSubsampling" },
            { 0x5039, "ThumbnailYCbCrPositioning" },
            { 0x503A, "ThumbnailRefBlackWhite" },
            { 0x503B, "ThumbnailCopyRight" },               //

            { 0x5041, "20545" },
            { 0x5042, "20546" },

            { 0x5090, "LuminanceTable" },
            { 0x5091, "ChrominanceTable" },

            { 0x5100, "FrameDelay" },
            { 0x5101, "LoopCount" },
            { 0x5102, "GlobalPalette" },
            { 0x5103, "IndexBackground" },
            { 0x5104, "IndexTransparent" },

            { 0x5110, "PixelUnit" },
            { 0x5111, "PixelPerUnitX" },
            { 0x5112, "PixelPerUnitY" },
            { 0x5113, "PaletteHistogram" },

            { 0x8298, "Copyright" },                        //

            { 0x829A, "ExifExposureTime" },

            { 0x829D, "ExifFNumber" },

            { 0x8769, "ExifIFD" },

            { 0x8773, "ICCProfile" },

            { 0x8822, "ExifExposureProg" },

            { 0x8824, "ExifSpectralSense" },
            { 0x8825, "GpsIFD" },

            { 0x8827, "ExifISOSpeed" },                     //
            { 0x8828, "ExifOECF" },

            { 0x8830, "34864" },

            { 0x9000, "ExifVer" },

            { 0x9003, "ExifDTOrig" },
            { 0x9004, "ExifDTDigitized" },

            { 0x9101, "ExifCompConfig" },
            { 0x9102, "ExifCompBPP" },

            { 0x9201, "ExifShutterSpeed" },                 //
            { 0x9202, "ExifAperture" },                     //
            { 0x9203, "ExifBrightness" },                   //
            { 0x9204, "ExifExposureBias" },                 //
            { 0x9205, "ExifMaxAperture" },                  //
            { 0x9206, "ExifSubjectDist" },                  //
            { 0x9207, "ExifMeteringMode" },                 //
            { 0x9208, "ExifLightSource" },                  //
            { 0x9209, "ExifFlash" },                        //
            { 0x920A, "ExifFocalLenght" },                  //

            { 0x927C, "ExifMakerMode" },

            { 0x9286, "ExifUserComment" },                  //
            { 0x9290, "ExifDTSubsec" },
            { 0x9291, "ExifDTOrigSS" },
            { 0x9292, "ExifDTDigSS" },

            { 0xA000, "ExifFPXVer" },
            { 0xA001, "ExifColorSpace" },
            { 0xA002, "ExifPixXDim" },
            { 0xA003, "ExifPixYDim" },
            { 0xA004, "ExifRelatedWav" },
            { 0xA005, "ExifInterop" },

            { 0xA20B, "ExifFlashEnergy" },
            { 0xA20C, "ExifSpacialFR" },

            { 0xA20E, "ExifFocalXRes" },
            { 0xA20F, "ExifFocalYRes" },
            { 0xA210, "ExifFocalResUnit" },

            { 0xA214, "ExifSubjectLoc" },
            { 0xA215, "ExifExposureIndex" },

            { 0xA217, "ExifSensingMethod" },

            { 0xA300, "ExifFileSource" },
            { 0xA301, "ExifSceneType" },
            { 0xA302, "ExifCfaPattern" },

            { 0xA401, "41985" },
            { 0xA402, "41986" },
            { 0xA403, "41987" },
            { 0xA404, "41988" },
            { 0xA405, "41989" },
            { 0xA406, "41990" },
            { 0xA407, "41991" },
            { 0xA408, "41992" },
            { 0xA409, "41993" },
            { 0xA40A, "41994" },
            { 0xA40B, "41995" },
            { 0xA40C, "41996" }
        };

        public static String DecodeProperty(System.Drawing.Imaging.PropertyItem? property)
        {
            if (property != null && property.Value != null)
            {
                switch (property.Type)
                {
                    case 2:
                        return System.Text.Encoding.UTF8.GetString(property.Value, 0, property.Len - 1) + "\n";

                    case 3:
                        UInt16[] val_uint16 = new UInt16[property.Len / 2];
                        for (int j = 0; j < property.Len / 2; j++)
                        {
                            byte[] val = { property.Value[2 * j], property.Value[2 * j + 1] };
                            val_uint16[j] = BitConverter.ToUInt16(val);
                        }
                        return String.Join("-", val_uint16) + "\n";

                    case 4:
                        UInt32[] val_uint32 = new UInt32[property.Len / 2];
                        for (int j = 0; j < property.Len / 4; j++)
                        {
                            byte[] val = { property.Value[4 * j], property.Value[4 * j + 1], property.Value[4 * j + 2], property.Value[4 * j + 3] };
                            val_uint32[j] = BitConverter.ToUInt32(val);
                        }
                        return String.Join("-", val_uint32) + "\n";

                    case 7:
                        Int32[] val_int32 = new Int32[property.Len / 2];
                        for (int j = 0; j < property.Len / 4; j++)
                        {
                            byte[] val = { property.Value[4 * j], property.Value[4 * j + 1], property.Value[4 * j + 2], property.Value[4 * j + 3] };
                            val_int32[j] = BitConverter.ToInt32(val);
                        }
                        return String.Join("-", val_int32) + "\n";

                    default:
                        return " \n";
                }
            }
            return "";
        }

        //Fonction qui permet d'avoir le nom du dossier en fonction du niveau de la variable $FOLDER
        private string GetFolderName(string path, int level)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            for (int i = 0; i < level; i++)
            {
                if (dir.Parent != null)
                    dir = dir.Parent;
                else
                    return ""; // ou gérer autrement (racine atteinte)
            }
            return dir.Name;
        }

        //Cette fonction permet d'englober les noms et les variables spéciaux du "$"
        private string ResolveValue(string input, string imgpath)
        {
            //Test si c'est juste du texte sans "$"
            if (!input.StartsWith("$"))
            {
                return input;
            }
            //Ici, il y'a "$", d'abord il faut extraire le nom du token (ex: FOLDER, DATE...)
            string token = new string(input.Skip(1).TakeWhile(char.IsLetter).ToArray()); //Le skip ne prend pas le premier caractère qui est "$", ensuite il prend les caractères qui sont des lettres et enfin il le transforme en tableau donc en un string
            //Correspondance avec le dictionnaire
            if (handlers.TryGetValue(token, out var handler))
            {
                return handler(input,imgpath);
            }
            //Si rien n'est fonctionnel, alors affiche une erreur dans le TextBox (à changé après les tests)
            return "UNKNOWN_TOKEN";
        }

        //Fonction qui gère "$FOLDER" qui est assigné dans le dictionnaire
        private string HandleFolder(string input, string imgpath)
        {
            string numberPart = input.Substring(7); //prend seulement les caractères après les 7 premières caractères qui sont "$FOLDER"
            //Test si les caractères prises avec les 7 premières caractères sont un nombre
            if (int.TryParse(numberPart, out int level))
            {
                return GetFolderName(imgpath, level+1);
            }
            //Si rien n'est fonctionnel, alors affiche un erreur dans le TextBox (à changer après les tests)
            return "ERROR_FOLDER";
        }

        //Fonction qui gère "$DATE" qui est assigné dans le dictionnaire - ATTENTION ! j'ai mis input et rootpath mais je ne l'utilise pas
        private string HandleDate(string input, string imgpath)
        {
            return DateTime.Now.ToString("dd-MM-yyyy");
        }

        //Fonction qui gère "$FILENAME" qui est assigné dans le dictionnaire - ATTENTION ! j'ai mis input mais je ne l'utilise pas
        private string HandleFilename(string input, string imgpath)
        {
            return Path.GetFileName(imgpath);
        }

        //Fonction qui gère "$PROPERTY" qui est assigné dans le dictionnaire
        private string HandleProperty(string input, string imgpath)
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
                return "ERROR_PROPERTY";
            }
            //Si rien n'est fonctionnel, alors affiche un erreur dans le TextBox (à changer après les tests)
            return "ERROR_PROPERTY";
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            //Parcours des items
            foreach (ListViewItem item in FilesView.Items)
            { 
                //Test pour vérifier s'ils sont verts (c'est seulement les verts qui faut changer)
                if(item.ForeColor == Color.Green)
                {
                    //Parcours de displayedID et NewValue pour pouvoir les appliqués
                    for(int i = 0; i < _PropertyEditorNb; i++)
                    {
                        //Test pour vérifier qu'il est bien présent dans le dictonnaire
                        if (propertyIds.TryGetValue(_displayedIds[i], out string a))
                        {
                            //Appel du fonction EncodeProperty
                        }
                    }
                }
            }
            /// Méthode pour appliquer les changements des propreiétés d'une ou plusieurs images
            if(FilesView.Items[0].Tag != null)
            {
                string value = _NewValue[0]; //Récuper la valeur à changer (pour l'instant le premier)
                Test_affiche.Text = ResolveValue(value, FilesView.Items[0].Tag.ToString());
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
                    ListViewGroup group = new ListViewGroup(groupName);
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
            
                if ((FileName.Contains(selection) && !UseRegex.Checked) || (Regex.IsMatch(FileName, '@'+selection) && UseRegex.Checked))
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
                _previewForm.Hide();
                _lastItem = null;
            }
        }

        private void FilesView_MouseLeave(object sender, EventArgs e)
        {
            _previewForm.Hide();
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
