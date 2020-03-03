using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2020_03_03_2k_AnimationDblBuf
{
    class Animator
    {
        private Graphics mainG;
        private int width, height;
        private List<Ball> balls = new List<Ball>();
        private Thread t;
        private bool stop = false;
        private BufferedGraphics bg;
        public Animator(Graphics g, Rectangle r)
        {
            Update(g, r);
        }

        public void Update(Graphics g, Rectangle r)
        {
            mainG = g;
            width = r.Width;
            height = r.Height;
            bg = BufferedGraphicsManager.Current.Allocate(
                mainG,
                new Rectangle(0, 0, width, height)
            );
            foreach (var b in balls)
            {
                b.Update(r);
            }
        }

        private void Animate()
        {
            while (!stop)
            {
                Graphics g = bg.Graphics;
                g.Clear(Color.White);
                foreach (var b in balls)
                {
                    Brush br = new SolidBrush(b.BgColor);
                    g.FillEllipse(br, b.X, b.Y, b.BallD, b.BallD);
                    Pen p = new Pen(b.FgColor, 2);
                    g.DrawEllipse(p,  b.X, b.Y, b.BallD, b.BallD);
                }

                try
                {
                    bg.Render();
                }
                catch (Exception e){}
                Thread.Sleep(30);
            }
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Animate);
                t = new Thread(th);
                t.Start();
            }
            Ball b = new Ball(new Rectangle(0, 0, width, height));
            b.Start();
            balls.Add(b);
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
