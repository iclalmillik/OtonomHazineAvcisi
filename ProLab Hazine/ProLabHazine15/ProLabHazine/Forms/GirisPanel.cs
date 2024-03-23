using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProLabHazine
{
    public partial class GirisPanel : Form
    {
        AnaForm form;
        int xSize = 0;
        int ySize = 0;

        public GirisPanel()
        {
            InitializeComponent();
        }

        private void btnCreateNewMap_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtBoxMapXAxis.Text, out xSize) && int.TryParse(txtBoxMapYAxis.Text, out xSize))
            {
                xSize = int.Parse(txtBoxMapXAxis.Text);
                ySize = int.Parse(txtBoxMapYAxis.Text);

                if(xSize > 1450 || xSize > 800)
                {
                    xSize = 1450;
                    ySize = 800;
                }

                form = new AnaForm(xSize, ySize);
                form.Show();
            }
            else
                MessageBox.Show("Geçerli Harita Boyutu Girmelisiniz", "Harita Boyutu Hatası", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            form.StartGame();
        }
    }
}
