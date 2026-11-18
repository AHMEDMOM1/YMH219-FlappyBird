using System.Drawing;

namespace FlappyBird
{
    public abstract class GameEntity
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        protected GameEntity(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public virtual Rectangle Bounds => new Rectangle((int)X, (int)Y, Width, Height);

        public abstract void Update();
        public abstract void Draw(Graphics g);
    }
}
