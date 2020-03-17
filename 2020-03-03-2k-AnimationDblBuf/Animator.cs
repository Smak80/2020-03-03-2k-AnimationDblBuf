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
        private List<Ring> rings = new List<Ring>();
        private Thread t;
        private bool stop = false;
        private BufferedGraphics bg;
        private bool bgChanged = false;
        private object obj = new object();
        public Animator(Graphics g, Rectangle r)
        {
            Update(g, r);
        }

        public void Update(Graphics g, Rectangle r)
        {
            mainG = g;
            width = r.Width;
            height = r.Height;
            Monitor.Enter(obj);
            bgChanged = true;
            bg = BufferedGraphicsManager.Current.Allocate(
                mainG,
                new Rectangle(0, 0, width, height)
            );
            Monitor.Exit(obj);
            Monitor.Enter(balls);
            foreach (var b in balls)
            {
                b.Update(r);
            }
            Monitor.Exit(balls);
            Monitor.Enter(rings);
            foreach (var rng in rings)
            {
                rng.Update(r);
            }
            Monitor.Exit(rings);
        }

        private void Animate()
        {
            while (!stop)
            {
                Monitor.Enter(obj);
                bgChanged = false;
                Graphics g = bg.Graphics;
                Monitor.Exit(obj);
                g.Clear(Color.White);
                Monitor.Enter(rings);
                int cnt = rings.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (!rings[i].IsAlive)
                    {
                        rings.Remove(rings[i]);
                        i--;
                        cnt--;
                    }
                }
                foreach (var r in rings)
                {
                    Brush br = new SolidBrush(r.RingColor);
                    g.FillEllipse(br, r.X, r.Y, 2*r.Radius, 2*r.Radius);
                }
                Monitor.Exit(rings);
                Monitor.Enter(balls);
                cnt = balls.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (!balls[i].IsAlive)
                    {
                        balls.Remove(balls[i]);
                        i--;
                        cnt--;
                    }
                }
                foreach (var b in balls)
                {
                    Brush br = new SolidBrush(b.BgColor);
                    g.FillEllipse(br, b.X, b.Y, b.BallD, b.BallD);
                    Pen p = new Pen(b.FgColor, 2);
                    g.DrawEllipse(p,  b.X, b.Y, b.BallD, b.BallD);
                }
                Monitor.Exit(balls);
                Monitor.Enter(obj);
                if (!bgChanged)
                {
                    try
                    {
                        bg.Render();
                    }
                    catch (Exception e)
                    {
                    }
                }
                Monitor.Exit(obj);
                
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
            var rect = new Rectangle(0, 0, width, height);
            Ball b = new Ball(rect);
            Ring r = new Ring(b.BgColor, rect);
            b.Start();
            r.Start();
            Monitor.Enter(balls);
            balls.Add(b);
            Monitor.Exit(balls);
            Monitor.Enter(rings);
            rings.Add(r);
            Monitor.Exit(rings);
        }

        public void Stop()
        {
            stop = true;
            Monitor.Enter(balls);
            foreach (var b in balls)
            {
                b.Stop();
            }
            balls.Clear();
            Monitor.Exit(balls);
        }
    }
}