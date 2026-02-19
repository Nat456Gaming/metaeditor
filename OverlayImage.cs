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
        private MetaEditor _metaeditor;

        public OverlayImage(MetaEditor metaEditor)
        {
            InitializeComponent();
            _metaeditor = metaEditor;
        }

        public void SetImage(string path)
        {
            PictureBox.Load(path);
        }
    }
}
