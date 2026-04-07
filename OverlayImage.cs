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
                if (displayedIds.Contains(i) && i != 0)
                {
                    displayText += "-" + _metaeditor.propertyIds[i] + ": ";
                    displayText += MetaEditor.DecodeProperty(image.GetPropertyItem(i));
                    displayText += "\n";
                }
            }
            TextDataDiaplay.Text = displayText;
            image.Dispose();
        }

        public void Remove()
        {
            
            if(PictureBox.Image != null)
            {
                this.Hide();
                PictureBox.Image.Dispose();
            }
        }
    }
}