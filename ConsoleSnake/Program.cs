using System;
using System.Collections.Generic;
using System.Threading;

class SnakeGame
{
    static int width = 20, height = 10;
    static List<(int x, int y)> snake = new List<(int, int)> { (10, 5) };
    static (int x, int y) food;
    static int dx = 1, dy = 0; // Moving right

    static void Main()
    {
        Console.CursorVisible = false;
        SpawnFood();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && dy == 0) { dx = 0; dy = -1; }
                else if (key == ConsoleKey.DownArrow && dy == 0) { dx = 0; dy = 1; }
                else if (key == ConsoleKey.LeftArrow && dx == 0) { dx = -1; dy = 0; }
                else if (key == ConsoleKey.RightArrow && dx == 0) { dx = 1; dy = 0; }
            }

            MoveSnake();
            Draw();
            Thread.Sleep(200);
        }
    }

    static void SpawnFood()
    {
        Random rand = new Random();
        food = (rand.Next(1, width - 1), rand.Next(1, height - 1));
    }

    static void MoveSnake()
    {
        var newHead = (snake[0].x + dx, snake[0].y + dy);
        if (newHead.Item1 <= 0 || newHead.Item2 <= 0 || newHead.Item1 >= width || newHead.Item2 >= height || snake.Contains(newHead))
        {
            Console.Clear();
            Console.WriteLine("Game Over!");
            Environment.Exit(0);
        }

        snake.Insert(0, newHead);
        if (newHead == food)
            SpawnFood();
        else
            snake.RemoveAt(snake.Count - 1);
    }

    static void Draw()
    {
        Console.Clear();
        Console.SetCursorPosition(food.x, food.y);
        Console.Write("O"); // Food

        foreach (var segment in snake)
        {
            Console.SetCursorPosition(segment.x, segment.y);
            Console.Write("#"); // Snake body
        }
    }
}
