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

        public void SetImage(string path)
        {
            PictureBox.Load(path);

            Image image = Image.FromFile(path);

            String displayText = "";
            foreach (int i in image.PropertyIdList)
            {
                System.Drawing.Imaging.PropertyItem? property = image.GetPropertyItem(i);
                if (property != null && property.Value != null)
                {
                    //Int16 value = BitConverter.ToInt16(property.Value);
                    displayText += i.ToString() + ": ";
                    if (property.Type == 2)
                    {
                        displayText += System.Text.Encoding.UTF8.GetString(property.Value,0,property.Len-1) + "\n";
                    }
                    else
                    {
                        displayText += _metaeditor.propertyTypes[property.Type - 1] + " \n";
                    }
                }
            }
            TextDataDiaplay.Text = displayText;
        }
    }
}
