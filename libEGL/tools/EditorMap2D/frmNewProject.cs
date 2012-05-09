using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EditorMapa2D
{
    public partial class frmNewProject : Form
    {
        private bool _isProject;

        public frmNewProject()
        {
            _isProject = false;
            InitializeComponent();
        }

        public bool isProject
        {
            get { return _isProject; }
        }

        public int mapW
        {
            get { return (int)nudMapWidth.Value; }
        }
        public int mapH
        {
            get { return (int)nudMapHeight.Value; }
        }
        public int tileW
        {
            get { return (int)nudTileWidth.Value; }
        }
        public int tileH
        {
            get { return (int)nudTileHeight.Value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _isProject = true;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            groupBox2.Text = "Mapa info (" + nudMapWidth.Value * nudTileWidth.Value + "x" + nudMapHeight.Value * nudTileHeight.Value + ")";
        }
    }
}
