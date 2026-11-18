using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlappyBird
{
    public class MainForm : Form
    {
        private readonly GameEngine _engine;
        private readonly System.Windows.Forms.Timer _gameTimer;

        public MainForm()
        {
            Text = "Flappy Bird - OOP Assignment 1";
            ClientSize = new Size(500, 620);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            KeyPreview = true;

            _engine = new GameEngine(ClientSize.Width, ClientSize.Height);
            _gameTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _gameTimer.Tick += (s, e) => { _engine.Step(); Invalidate(); };
            _gameTimer.Start();

            KeyDown += OnMainFormKeyDown;
            MouseDown += OnMainFormMouseDown;
        }

        private void OnMainFormKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                if (!_engine.IsGameOver && !_engine.IsPaused) _engine.PlayerBird.Flap();
            }
            else if (e.KeyCode == Keys.P)
            {
                _engine.TogglePause();
                Invalidate();
            }
            else if (e.KeyCode == Keys.R || (e.KeyCode == Keys.Enter && _engine.IsGameOver))
            {
                _engine.Restart();
                Invalidate();
            }
        }

        private void OnMainFormMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_engine.IsGameOver) _engine.Restart();
                else if (!_engine.IsPaused) _engine.PlayerBird.Flap();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(112, 197, 206));

            foreach (var pipe in _engine.Pipes) pipe.Draw(g);

            int gy = _engine.GroundY;
            using (Brush groundBrush = new SolidBrush(Color.FromArgb(222, 216, 149)))
            using (Brush grassBrush = new SolidBrush(Color.FromArgb(115, 191, 46)))
            {
                g.FillRectangle(groundBrush, 0, gy, ClientSize.Width, ClientSize.Height - gy);
                g.FillRectangle(grassBrush, 0, gy, ClientSize.Width, 14);
            }

            _engine.PlayerBird.Draw(g);

            using (Font scoreFont = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (Font hintFont = new Font("Segoe UI", 9.5f, FontStyle.Regular))
            {
                string hudText = $"Score: {_engine.Score}   |   High Score: {_engine.HighScore}";
                g.DrawString(hudText, scoreFont, Brushes.White, 16, 13);
                g.DrawString(hudText, scoreFont, Brushes.Black, 15, 12);
                g.DrawString("Controls: [SPACE / Click] Flap   |   [P] Pause   |   [R] Restart", hintFont, Brushes.DarkSlateGray, 15, gy + 32);
            }

            if (_engine.IsGameOver)
            {
                Rectangle panelRect = new Rectangle(85, 155, 330, 220);
                using (Brush panelBrush = new SolidBrush(Color.FromArgb(248, 242, 200)))
                using (Pen borderPen = new Pen(Color.FromArgb(90, 75, 40), 3f))
                using (Font titleFont = new Font("Segoe UI", 20f, FontStyle.Bold))
                using (Font scoreFont = new Font("Segoe UI", 13f, FontStyle.Bold))
                {
                    g.FillRectangle(panelBrush, panelRect);
                    g.DrawRectangle(borderPen, panelRect);
                    g.DrawString("GAME OVER", titleFont, Brushes.Firebrick, 162, 175);
                    g.DrawString($"Final Score: {_engine.Score}", scoreFont, Brushes.Black, 180, 228);
                    g.DrawString($"Best Score:  {_engine.HighScore}", scoreFont, Brushes.Black, 180, 260);
                    g.DrawString("Press [R] or [ENTER] to Retry", scoreFont, Brushes.DarkGreen, 130, 315);
                }
            }
        }
    }
}
