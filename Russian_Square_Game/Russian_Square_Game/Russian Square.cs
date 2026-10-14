using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Russian_Square_Game
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public partial class Russian_Square : Form
    {
        public Russian_Square()
        {
            InitializeComponent();
        }
        Game_logic game;
        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Dispose();
            button2.Dispose();
            game = new Game_logic(this);
            game.start();
        }

        private void Russian_Square_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    game.moveSquare(Direction.Up);
                    break;
                case Keys.S:
                    game.moveSquare(Direction.Down);
                    break;
                case Keys.D:
                    game.moveSquare(Direction.Right);
                    break;
                case Keys.A:
                    game.moveSquare(Direction.Left);
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
            }
        }
    }
    
}
