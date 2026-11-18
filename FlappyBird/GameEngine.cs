using System;
using System.Collections.Generic;
using System.Drawing;

namespace FlappyBird
{
    public class GameEngine
    {
        public Bird PlayerBird { get; }
        public List<PipePair> Pipes { get; }
        public int Score { get; private set; }
        public int HighScore { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool IsPaused { get; private set; }
        public int ViewportWidth { get; }
        public int ViewportHeight { get; }
        public int GroundY { get; }

        private readonly Random _random = new Random();
        private const int PipeGapHeight = 145;
        private const int PipeSpacing = 220;

        public GameEngine(int viewportWidth, int viewportHeight)
        {
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            GroundY = viewportHeight - 80;
            PlayerBird = new Bird(95f, 245f);
            Pipes = new List<PipePair>();
            InitializePipes();
        }

        private void InitializePipes()
        {
            Pipes.Clear();
            for (int i = 0; i < 3; i++)
            {
                float startX = ViewportWidth + 60 + (i * PipeSpacing);
                int gapY = _random.Next(90, GroundY - PipeGapHeight - 80);
                Pipes.Add(new PipePair(startX, gapY, PipeGapHeight, GroundY));
            }
        }

        public void TogglePause()
        {
            if (!IsGameOver) IsPaused = !IsPaused;
        }

        public void Restart()
        {
            Score = 0;
            IsGameOver = false;
            IsPaused = false;
            PlayerBird.Reset();
            InitializePipes();
        }

        public void Step()
        {
            if (IsGameOver || IsPaused) return;

            PlayerBird.Update();
            if (PlayerBird.Y < 0) PlayerBird.Y = 0;

            if (PlayerBird.Y + PlayerBird.Height >= GroundY)
            {
                PlayerBird.Y = GroundY - PlayerBird.Height;
                TriggerGameOver();
                return;
            }

            Rectangle birdRect = PlayerBird.Bounds;
            foreach (var pipe in Pipes)
            {
                pipe.Update();
                if (pipe.CollidesWith(birdRect))
                {
                    TriggerGameOver();
                    return;
                }
                if (!pipe.IsScored && pipe.X + pipe.Width < PlayerBird.X)
                {
                    pipe.IsScored = true;
                    Score++;
                    if (Score > HighScore) HighScore = Score;
                }
            }

            foreach (var pipe in Pipes)
            {
                if (pipe.X + pipe.Width < -10)
                {
                    float maxRightX = 0f;
                    foreach (var other in Pipes)
                        if (other.X > maxRightX) maxRightX = other.X;

                    int nextGapY = _random.Next(90, GroundY - PipeGapHeight - 80);
                    pipe.Reposition(maxRightX + PipeSpacing, nextGapY);
                }
            }
        }

        private void TriggerGameOver()
        {
            IsGameOver = true;
            if (Score > HighScore) HighScore = Score;
        }
    }
}
