using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EGLCort2
{
    class Box
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Color Cor { get; set; }

        public Box(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
            Cor = Color.Black;
        }
    }
}
