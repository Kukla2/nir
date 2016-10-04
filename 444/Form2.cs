using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace _444
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

  

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //SolidBrush br = new SolidBrush(Color.Purple);
            float x = 0.0F;
            float y = 0.0F;
            float wid = 100.0F;
            float hei = 300.0F;
            //g.DrawEllipse(new Pen(Color.Purple,15.0F), 0, 0, 100, 300);
            PointF[] points = 
            {
                new PointF(10.0F,10.0F),
                new PointF(15.0F,19.0F),
                new PointF(25.0F,5.0F),
                new PointF(1.0F,10.0F),
            };

            g.DrawLines(new Pen(Color.Black, 1.0F), points);
        }
    }
}
