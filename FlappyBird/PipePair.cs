using System;
using System.Drawing;

namespace FlappyBird
{
    public class PipePair : GameEntity
    {
        public int GapY { get; private set; }
        public int GapHeight { get; set; }
        public float Speed { get; set; }
        public bool IsScored { get; set; }
        private readonly int _groundY;

        public PipePair(float startX, int gapY, int gapHeight, int groundY, float speed = 3.5f)
            : base(startX, 0, 64, groundY)
        {
            GapY = gapY;
            GapHeight = gapHeight;
            _groundY = groundY;
            Speed = speed;
            IsScored = false;
        }

        public Rectangle TopPipeBounds => new Rectangle((int)X, 0, Width, GapY);
        public Rectangle BottomPipeBounds => new Rectangle((int)X, GapY + GapHeight, Width, Math.Max(0, _groundY - (GapY + GapHeight)));

        public override void Update() => X -= Speed;

        public void Reposition(float newX, int newGapY)
        {
            X = newX;
            GapY = newGapY;
            IsScored = false;
        }

        public bool CollidesWith(Rectangle targetBounds) =>
            TopPipeBounds.IntersectsWith(targetBounds) || BottomPipeBounds.IntersectsWith(targetBounds);

        public override void Draw(Graphics g)
        {
            using (Brush pipeBrush = new SolidBrush(Color.FromArgb(115, 191, 46)))
            using (Pen borderPen = new Pen(Color.FromArgb(45, 90, 20), 2f))
            {
                int x = (int)X;
                g.FillRectangle(pipeBrush, TopPipeBounds);
                g.DrawRectangle(borderPen, TopPipeBounds);

                Rectangle topCap = new Rectangle(x - 4, Math.Max(0, GapY - 24), Width + 8, 24);
                g.FillRectangle(pipeBrush, topCap);
                g.DrawRectangle(borderPen, topCap);

                g.FillRectangle(pipeBrush, BottomPipeBounds);
                g.DrawRectangle(borderPen, BottomPipeBounds);

                Rectangle botCap = new Rectangle(x - 4, GapY + GapHeight, Width + 8, 24);
                g.FillRectangle(pipeBrush, botCap);
                g.DrawRectangle(borderPen, botCap);
            }
        }
    }
}
