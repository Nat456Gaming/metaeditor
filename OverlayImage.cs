using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace metaeditor
{
    public partial class OverlayImage : Form
    {
        private readonly MetaEditor _metaeditor;

        public OverlayImage(MetaEditor metaEditor)
        {
            InitializeComponent();
            _metaeditor = metaEditor;
        }

        public void SetImage(string path, List<int> displayedIds)
        {
            PictureBox.Load(path);

            Image image = Image.FromFile(path);

            String displayText = "";
            foreach (int i in image.PropertyIdList)
            {
                if (displayedIds.Contains(i))
                {
                    System.Drawing.Imaging.PropertyItem? property = image.GetPropertyItem(i);
                    if (property != null && property.Value != null)
                    {
                        //Int16 value = BitConverter.ToInt16(property.Value);
                        //displayText += i.ToString() + ": ";
                        displayText += "-" + _metaeditor.propertyIds[i] + " " + _metaeditor.propertyTypes[property.Type - 1] + ": ";
                        switch (property.Type)
                        {
                            case 2:
                                displayText += System.Text.Encoding.UTF8.GetString(property.Value, 0, property.Len - 1) + "\n";
                                break;
                            case 3:
                                UInt16[] val_uint16 = new UInt16[property.Len / 2];
                                for (int j = 0; j < property.Len / 2; j++)
                                {
                                    byte[] val = { property.Value[2 * j], property.Value[2 * j + 1] };
                                    val_uint16[j] = BitConverter.ToUInt16(val);
                                }
                                displayText += String.Join("-", val_uint16) + "\n";
                                break;
                            case 4:
                                UInt32[] val_uint32 = new UInt32[property.Len / 2];
                                for (int j = 0; j < property.Len / 4; j++)
                                {
                                    byte[] val = { property.Value[4 * j], property.Value[4 * j + 1], property.Value[4 * j + 2], property.Value[4 * j + 3] };
                                    val_uint32[j] = BitConverter.ToUInt32(val);
                                }
                                displayText += String.Join("-", val_uint32) + "\n";
                                break;
                            case 7:
                                Int32[] val_int32 = new Int32[property.Len / 2];
                                for (int j = 0; j < property.Len / 4; j++)
                                {
                                    byte[] val = { property.Value[4 * j], property.Value[4 * j + 1], property.Value[4 * j + 2], property.Value[4 * j + 3] };
                                    val_int32[j] = BitConverter.ToInt32(val);
                                }
                                displayText += String.Join("-", val_int32) + "\n";
                                break;
                            default:
                                displayText += " \n";
                                break;
                        }
                        /*
                        if (property.Type == 2)
                        {
                            displayText += System.Text.Encoding.UTF8.GetString(property.Value,0,property.Len-1) + "\n";
                        }
                        else
                        {
                            displayText += _metaeditor.propertyTypes[property.Type - 1] + " \n";
                        }*/
                    }
                }
            }
            TextDataDiaplay.Text = displayText;
        }
    }
}
