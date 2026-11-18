using System.Drawing;
using System.Drawing.Drawing2D;

namespace FlappyBird
{
    public class Bird : GameEntity
    {
        public float VelocityY { get; private set; }
        public float Gravity { get; set; } = 0.65f;
        public float JumpStrength { get; set; } = -9.5f;
        public float MaxFallSpeed { get; set; } = 12.0f;

        private readonly float _initialX;
        private readonly float _initialY;

        public Bird(float startX, float startY)
            : base(startX, startY, 38, 30)
        {
            _initialX = startX;
            _initialY = startY;
            VelocityY = 0f;
        }

        public override void Update()
        {
            VelocityY += Gravity;
            if (VelocityY > MaxFallSpeed)
                VelocityY = MaxFallSpeed;
            Y += VelocityY;
        }

        public void Flap() => VelocityY = JumpStrength;

        public void Reset()
        {
            X = _initialX;
            Y = _initialY;
            VelocityY = 0f;
        }

        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int bx = (int)X;
            int by = (int)Y;

            using (Brush bodyBrush = new SolidBrush(Color.FromArgb(248, 216, 48)))
            using (Pen outlinePen = new Pen(Color.FromArgb(60, 50, 10), 2f))
            {
                g.FillEllipse(bodyBrush, bx, by, Width, Height);
                g.DrawEllipse(outlinePen, bx, by, Width, Height);

                using (Brush wingBrush = new SolidBrush(Color.FromArgb(255, 245, 180)))
                {
                    g.FillEllipse(wingBrush, bx + 5, by + 11, 16, 12);
                    g.DrawEllipse(Pens.DarkGoldenrod, bx + 5, by + 11, 16, 12);
                }

                g.FillEllipse(Brushes.White, bx + 23, by + 4, 11, 11);
                g.DrawEllipse(outlinePen, bx + 23, by + 4, 11, 11);
                g.FillEllipse(Brushes.Black, bx + 28, by + 7, 4, 5);

                Point[] beakPoints = { new Point(bx + 34, by + 14), new Point(bx + 46, by + 18), new Point(bx + 34, by + 23) };
                using (Brush beakBrush = new SolidBrush(Color.FromArgb(240, 100, 30)))
                {
                    g.FillPolygon(beakBrush, beakPoints);
                    g.DrawPolygon(outlinePen, beakPoints);
                }
            }
        }
    }
}
