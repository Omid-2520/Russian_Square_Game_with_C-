using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Russian_Square_Game
{
    internal class block_code
    {
        private int width;
        private Russian_Square frm;
        private Color squarecolor;
        public Panel panel;



        public block_code(Russian_Square frm, int w, Color squarecolor)
        {
            this.width = w;
            this.squarecolor = squarecolor;
            this.frm = frm;
            panel = new Panel();
            this.panel.BackColor = squarecolor;
            this.panel.BorderStyle = BorderStyle.Fixed3D;
            this.panel.Size = new Size(width, width);
            this.frm.Controls.Add(panel);
        }

        public void showSquare(int x, int y)
        {
            this.panel.Location = new Point(x, y);
        }
    }
}


