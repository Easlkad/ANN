using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TorchSharp;
using static TorchSharp.torch;

namespace SnakeGame
{
    class SnakeGame
    {
        public List<Point> snake;
        public Point food;
        private int direction;
        private readonly int gridSize = 20;
        private readonly int width = 400, height = 400;
        private int frameIteration;
        public int score;

        public SnakeGame()
        {
            Reset();
        }
        public void Reset()
        {
            direction = 0;
            snake = new List<Point>{ new Point(width / 2, height / 2)};
            score = 0;
            frameIteration = 0;
            SpawnFood();

        }

        private void SpawnFood()
        {
            Random rand = new Random();
            food = new Point(rand.Next(0, width / gridSize) * gridSize, rand.Next(0, height / gridSize) * gridSize);
            if (snake.Contains(food)) SpawnFood();

        }
        public (float,bool,int) PlayStep(int action)
        {
            float distanceBefore = (Math.Abs(snake[0].X - food.X) + Math.Abs(snake[0].Y - food.Y));
            frameIteration++;
            Move(action);
            float distanceAfter = (Math.Abs(snake[0].X - food.X) + Math.Abs(snake[0].Y - food.Y));
            float reward = -0.05f;
            bool gameOver = false;

            if (IsCollision())
            {
                gameOver = true;
                reward = -10;
                return (reward, gameOver, score);
            }

            if (snake[0] == food)
            {
                score += 1;
                reward = 50;
                SpawnFood();
            }
            else
            {
                if(distanceAfter < distanceBefore) reward += 0.5f;
                else reward += -0.5f;
                
            }
            return (reward, gameOver, score);
        }
        private void Move(int action)
        {
            int[] dx = { 20, 0, -20, 0 };
            int[] dy = { 0, 20, 0, -20 };

            if (action == 1) direction = (direction + 1) % 4;
            if (action == 2) direction = (direction - 1 + 4) % 4;
            int newX = snake[0].X + dx[direction];
            int newY = snake[0].Y + dy[direction];
            Point newHead = new Point(newX, newY);
            snake.Insert(0, newHead);

            if (newHead == food)
            {
                score += 1;
                SpawnFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1); // 🔴 Remove last tail segment
            }
        }
        private bool IsCollision()
        {
            Point head = snake[0];
            
            return head.X < 0 || head.Y < 0 || head.X >= width  || head.Y >= height || snake.Skip(1).Contains(head);
        }
        public Tensor GetStateTensor()
        {
            var head = snake[0];

            var state = new float[]
            {
                IsCollision() ? 1f : 0f,
                (direction == 3) ? 1f : 0f, // Up
                (direction == 1) ? 1f : 0f, // Down
                (direction == 2) ? 1f : 0f, // Left
                (direction == 0) ? 1f : 0f, // Right
                (food.X > head.X) ? 1f: 0f,   // -1/0/1 encoding
                 (food.Y > head.Y) ? 1f: 0f,
                 head.X / (float)width,      // Normalized coordinates
                head.Y / (float)height,
                food.X / (float)width,
                food.Y / (float)height
            };
            return torch.tensor(state, dtype: torch.float32).unsqueeze(0);
        }

    }
}
