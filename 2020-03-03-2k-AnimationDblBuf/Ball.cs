using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2020_03_03_2k_AnimationDblBuf
{
    class Ball
    {
        private int width;
        private int height;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int BallD { get; private set; }
        public Color FgColor { get; private set; }
        public Color BgColor { get; private set; }
        private static Random rand = null;
        private int dx, dy;
        private Thread t = null;
        private bool stop = false;
        private int counter = 0;
        private static int maxCount = 10;
        public bool IsAlive
        {
            get { return t!=null && t.IsAlive; }
        }

        public Ball(Rectangle r)
        {
            Update(r);
            if (rand == null) rand = new Random();
            BallD = rand.Next(20, 70);
            X = (width - BallD) / 2;
            Y = (height - BallD) / 2;
            FgColor = Color.FromArgb(rand.Next(25, 255),
                rand.Next(255),
                rand.Next(255),
                rand.Next(255)
            );
            BgColor = Color.FromArgb(rand.Next(25, 255),
                rand.Next(255),
                rand.Next(255),
                rand.Next(255)
            );
            do
            {
                dx = rand.Next(-5, 5);
                dy = rand.Next(-5, 5);
            } while (dx * dy == 0);
        }

        public void Update(Rectangle r)
        {
            width = r.Width;
            height = r.Height;
        }

        private void Move()
        {
            while (!stop && counter<maxCount)
            {
                Thread.Sleep(30);
                X += dx;
                Y += dy;
                if (X > width - BallD || X < 0)
                {
                    if (X > width - BallD)
                    {
                        X = width - BallD;
                    }
                    dx = -dx;
                    counter++;
                }

                if (Y > height - BallD || Y < 0)
                {
                    if (Y > height - BallD)
                    {
                        Y = height - BallD;
                    }
                    dy = -dy;
                    counter++;
                }
            }
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Move);
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
