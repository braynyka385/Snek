using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snek
{
    public partial class Form1 : Form
    {

        Pen gridPen = new Pen(Color.Black, 2);
        Brush snakeBrush = new SolidBrush(Color.Green);
        Brush foodBrush = new SolidBrush(Color.Red);

        byte gridScale = 25;
        bool[] pressedKey = new bool[4];
        List<Snake> snakeParts = new List<Snake>();
        List<Food> foodItems = new List<Food>();
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Snake s = new Snake(100, 200);

            snakeParts.Add(s);


            int[] foodLoc = FoodPos(0, true);

            Food f = new Food(foodLoc[0], foodLoc[1]);
            foodItems.Add(f);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    if (!pressedKey[1])
                    {
                        pressedKey[0] = true;
                        pressedKey[1] = false;
                        pressedKey[2] = false;
                        pressedKey[3] = false;
                    }
                    
                    break;
                case Keys.S:
                    if (!pressedKey[0])
                    {
                        pressedKey[0] = false;
                        pressedKey[1] = true;
                        pressedKey[2] = false;
                        pressedKey[3] = false;
                    }
                    
                    break;
                case Keys.A:
                    if (!pressedKey[3])
                    {
                        pressedKey[0] = false;
                        pressedKey[1] = false;
                        pressedKey[2] = true;
                        pressedKey[3] = false;
                    }
                    
                    break;
                case Keys.D:
                    if (!pressedKey[2])
                    {
                        pressedKey[0] = false;
                        pressedKey[1] = false;
                        pressedKey[2] = false;
                        pressedKey[3] = true;
                    }
                    
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {

            for (int i = foodItems.Count - 1; i >= 0; i--)
            {
                if (foodItems[i].x == snakeParts[0].x && foodItems[i].y == snakeParts[0].y)
                {
                    foodItems.RemoveAt(i);
                    Snake s = new Snake(snakeParts[0].x, snakeParts[0].y);
                    int[] foodLoc = FoodPos(0, false);
                    Food q = new Food(foodLoc[0], foodLoc[1]);
                    foodItems.Add(q);
                    snakeParts.Add(s);
                }
            }
            for (int i = snakeParts.Count - 1; i > 0; i--)
            {
                snakeParts[i].x = snakeParts[i - 1].x;
                snakeParts[i].y = snakeParts[i - 1].y;
            }

            if (pressedKey[0])
            {
                snakeParts[0].y -= gridScale;
            }
            else if (pressedKey[1])
            {
                snakeParts[0].y += gridScale;
            }
            else if (pressedKey[2])
            {
                snakeParts[0].x -= gridScale;
            }
            else if (pressedKey[3])
            {
                snakeParts[0].x += gridScale;
            }

            if (snakeParts[0].x == this.Width && pressedKey[3])
            {
                snakeParts[0].x = 0;
            }
            else if (snakeParts[0].x == -gridScale && pressedKey[2])
            {
                snakeParts[0].x = this.Width - gridScale;
            }
            else if (snakeParts[0].y == -gridScale && pressedKey[0])
            {
                snakeParts[0].y = this.Height - gridScale;
            }
            else if (snakeParts[0].y == this.Height && pressedKey[1])
            {
                snakeParts[0].y = 0;
            }

            

            for (int i = snakeParts.Count - 1; i > 0; i--)
            {
                if (snakeParts[i].x == snakeParts[0].x && snakeParts[i].y == snakeParts[0].y)
                {
                    Application.Exit();
                }
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Snake s in snakeParts)
            {
                e.Graphics.FillRectangle(snakeBrush, s.x, s.y, gridScale, gridScale);
            }

            foreach (Food f in foodItems)
            {
                e.Graphics.FillRectangle(foodBrush, f.x, f.y, gridScale, gridScale);
            }
            for (int x = 0; x < this.Width; x += gridScale)
            {
                e.Graphics.DrawLine(gridPen, x, 0, x, this.Height);
            }
            for (int y = 0; y < this.Height; y += gridScale)
            {
                e.Graphics.DrawLine(gridPen, 0, y, this.Width, y);
            }
        }

        private int[] FoodPos(int i, bool isOG)
        {
            int[] output = new int[2];
            int x = random.Next(0, this.Width);
            while (x % gridScale != 0)
            {
                x = random.Next(0, this.Width);
            }

            int y = random.Next(0, this.Height);
            while (y % gridScale != 0)
            {
                y = random.Next(0, this.Height);
            }
            output[0] = x;
            output[1] = y;
            foreach (Snake s in snakeParts)
            {
                if (x == s.x && y == s.y)
                {
                    output = FoodPos(i, isOG);
                }
            }
            
            return output;
            
        }

        
    }

    public class Snake
    {
        public int x;
        public int y;

        public Snake(int v1, int v2)
        {
            this.x = v1;
            this.y = v2;
        }
    }
    public class Food
    {
        public int x;
        public int y;
        public Food(int v1, int v2)
        {
            this.x = v1;
            this.y = v2;
        }
    }
}
