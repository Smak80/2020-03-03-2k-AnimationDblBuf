using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace _2020_03_03_2k_AnimationDblBuf
{
    class Ring
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        private int width, height;
        private bool stop = false;
        private Thread t = null;
        public Color RingColor { get; private set; }
        public bool IsAlive
        {
            get { return t!=null && t.IsAlive; }
        }
        private int maxRadius
        {
            get { return (width > height) ? width : height; }
            set{}
        }

        public int Radius { get; private set; }
        public Ring(Color c, Rectangle r)
        {
            width = r.Width;
            height = r.Height;
            X = width / 2;
            Y = height / 2;
            RingColor = c;
            Radius = 0;
        }

        public void Update(Rectangle r)
        {
            width = r.Width;
            height = r.Height;
        }

        private void Enc()
        {
            while (!stop && Radius < maxRadius)
            {
                X -= 1;
                Y -= 1;
                Radius += 1;
                Thread.Sleep(5);
                RingColor = Color.FromArgb(
                    (int)((1.0 - (float)Radius/maxRadius)*255),
                    RingColor.R,
                    RingColor.G,
                    RingColor.B
                );
            }
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Enc);
                t = new Thread(th);
                t.Start();
            }
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
