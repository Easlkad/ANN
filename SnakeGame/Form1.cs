using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        private List<Point> snake = new List<Point>();
        private Point food;
        private int direction = 0; // 0: Right, 1: Down, 2: Left, 3: Up
        private int gridSize = 20;
        private bool gameOver = false;
        private readonly Size gameSize = new Size(400, 400); // Fixed game size
        private Button restartButton;
        private int score = 0;
        private SnakeAI ai;

        public Form1()
        {
            this.Size = new Size(gameSize.Width + 16, gameSize.Height + 38); // Account for window borders
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.DoubleBuffered = true; // Prevent flickering
            this.Text = "Snake Game";
            
            this.KeyDown += Form1_KeyDown;

            // Initialize restart button
            restartButton = new Button
            {
                Text = "Restart Game",
                Size = new Size(100, 30),
                Location = new Point((gameSize.Width - 100) / 2, gameSize.Height - 40),
                Visible = false // Only show when game is over
            };
            restartButton.Click += RestartGame;
            this.Controls.Add(restartButton);
            ai = new SnakeAI();
            InitializeGame();
        }

        private void InitializeGame()
        {
            snake.Clear();
            snake.Add(new Point(5, 5));
            direction = 0;
            score = 0;
            gameOver = false;
            
            timer = new System.Windows.Forms.Timer { Interval = 100 };
            timer.Tick += Update;
            timer.Start();

            SpawnFood();
            restartButton.Visible = false;
            Invalidate();
        }

        private void RestartGame(object sender, EventArgs e)
        {
            InitializeGame();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameOver)
            {
                if (e.KeyCode == Keys.R)
                {
                    InitializeGame();
                    return;
                }
            }

            switch (e.KeyCode)
            {
                case Keys.Left when direction != 0: direction = 2; break;
                case Keys.Right when direction != 2: direction = 0; break;
                case Keys.Up when direction != 1: direction = 3; break;
                case Keys.Down when direction != 3: direction = 1; break;
            }
        }

        private void Update(object sender, EventArgs e)
        {
            if (gameOver) return;

            // Move snake
            Point head = snake[0];
            Point newHead = new Point(head.X, head.Y);

            switch (direction)
            {
                case 0: newHead.X++; break; // Right
                case 1: newHead.Y++; break; // Down
                case 2: newHead.X--; break; // Left
                case 3: newHead.Y--; break; // Up
            }

            // Check collisions
            if (newHead.X < 0 || newHead.Y < 0 || 
                newHead.X >= gameSize.Width / gridSize || 
                newHead.Y >= gameSize.Height / gridSize ||
                snake.Contains(newHead))
            {
                HandleGameOver();
                return;
            }

            // Check food
            if (newHead == food)
            {
                snake.Insert(0, newHead);
                score += 10;
                SpawnFood();
            }
            else
            {
                snake.Insert(0, newHead);
                snake.RemoveAt(snake.Count - 1);
            }

            Invalidate();
        }

        private void HandleGameOver()
        {
            gameOver = true;
            timer.Stop();
            restartButton.Visible = true;
            Invalidate();
        }

        private void SpawnFood()
        {
            Random rand = new Random();
            do
            {
                food = new Point(
                    rand.Next(0, gameSize.Width / gridSize),
                    rand.Next(0, gameSize.Height / gridSize)
                );
            } while (snake.Contains(food));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Draw grid background
            for (int x = 0; x < gameSize.Width; x += gridSize)
            {
                for (int y = 0; y < gameSize.Height; y += gridSize)
                {
                    g.DrawRectangle(Pens.LightGray, x, y, gridSize, gridSize);
                }
            }

            // Draw snake
            foreach (Point p in snake)
            {
                g.FillRectangle(Brushes.Green, 
                    p.X * gridSize, p.Y * gridSize, 
                    gridSize - 1, gridSize - 1);
            }

            // Draw food
            g.FillRectangle(Brushes.Red, 
                food.X * gridSize, food.Y * gridSize, 
                gridSize - 1, gridSize - 1);

            // Draw score
            using (Font scoreFont = new Font("Arial", 12))
            {
                g.DrawString($"Score: {score}", scoreFont, Brushes.Black, 10, 10);
            }

            if (gameOver)
            {
                // Draw semi-transparent overlay
                using (SolidBrush overlay = new SolidBrush(Color.FromArgb(128, Color.Black)))
                {
                    g.FillRectangle(overlay, 0, 0, gameSize.Width, gameSize.Height);
                }

                // Draw game over text
                string gameOverText = "Game Over!";
                string scoreText = $"Final Score: {score}";
                string restartText = "Press 'R' or click Restart to play again";
                
                using (Font gameOverFont = new Font("Arial", 24, FontStyle.Bold))
                using (Font scoreFont = new Font("Arial", 16))
                {
                    // Draw game over with white color for better visibility
                    SizeF gameOverSize = g.MeasureString(gameOverText, gameOverFont);
                    SizeF scoreSize = g.MeasureString(scoreText, scoreFont);
                    SizeF restartSize = g.MeasureString(restartText, scoreFont);

                    float centerX = gameSize.Width / 2;
                    float centerY = gameSize.Height / 2 - 40;

                    g.DrawString(gameOverText, gameOverFont, Brushes.White,
                        centerX - gameOverSize.Width / 2,
                        centerY - gameOverSize.Height);

                    g.DrawString(scoreText, scoreFont, Brushes.White,
                        centerX - scoreSize.Width / 2,
                        centerY + 10);

                    g.DrawString(restartText, scoreFont, Brushes.White,
                        centerX - restartSize.Width / 2,
                        centerY + 40);
                }
            }
        }
    }
}