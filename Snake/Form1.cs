using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using Tensorflow;
using Tensorflow.NumPy;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Layers;
using Tensorflow.Keras.Models;
using Tensorflow.Keras.ArgsDefinition;
using Tensorflow.Keras.Optimizers;
using Tensorflow.Keras;
namespace Snake
{

    public partial class Form1: Form
    {
        private Timer timer;
        private List<Point> snake = new List<Point>();
        private Point food;
        private int direction = 0;
        private int gridSize = 20;
        private Sequential model;
        public Form1()
        {
            this.Text = "Snake";
            this.Size = new Size(800, 600);
            this.DoubleBuffered = true;

            timer = new Timer { Interval = 100 };
            timer.Tick += Update;
            timer.Start();
            snake.Add(new Point(5, 5));
            SpawnFood();

            this.Paint += Draw;


            model = BuildModel();
        }

        private Sequential BuildModel()
        {
            var model = new Sequential(new SequentialArgs
            {
                Layers = new List<ILayer>
                {
                    new Dense(new DenseArgs{Units = 128, Activation = tf.keras.activations.Relu, InputShape = new Shape(1)}),
                    new Dense(new DenseArgs{Units = 64, Activation = tf.keras.activations.Relu}),
                    new Dense(new DenseArgs{Units = 32, Activation = tf.keras.activations.Relu}),
                    new Dense(new DenseArgs { Units = 3, Activation = tf.keras.activations.Softmax }) // Left, Right, Forward

                }
            });

            model.compile(optimizer: new Adam(0.001f), loss: tf.keras.losses.MeanSquaredError());
            return model;
        }
        private void Update(object sender, EventArgs e)
        {
            var state = GetGameState();
            int action = ChooseBestAction(state);

            UpdateDirection(action);

            MoveSnake();
            Invalidate();
        }
        private void SpawnFood()
        {
            Random rand = new Random();
            food = new Point(rand.Next(0, this.Width / gridSize), rand.Next(0, this.Height / gridSize));
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            foreach(Point point in snake)
            {
                foreach (Point p in snake)
                    graphics.FillRectangle(Brushes.Green, p.X * gridSize, p.Y * gridSize, gridSize, gridSize);
                graphics.FillRectangle(Brushes.Red, food.X * gridSize, food.Y * gridSize, gridSize, gridSize);
            }
        }

        private NDArray GetGameState()
        {
            var head = snake[0];
            return np.array(new float[,] { { head.X, head.Y, food.X, food.Y,direction } }).reshape(new Shape(1,5));

        }

        private int ChooseBestAction(NDArray state)
        {
            float epsilon = 0.1f;
            Random rand = new Random();
            if (rand.NextDouble() < epsilon)
                return rand.Next(0, 3);
            var prediction = model.Apply(state);
            return np.argmax(prediction.numpy());
        }

        private void UpdateDirection(int action)
        {
            if (action == 0) direction = (direction + 3) % 4; // Left
            if(action == 1 ) direction = (direction + 1) % 4; // Right
            // If action == 2 (Forward), keep direction unchanged
        }

        private void MoveSnake()
        {
            Point head = snake[0];
            Point newHead = head;

            switch (direction)
            {
                case 0: newHead.X++; break;
                case 1: newHead.Y++; break;
                case 2: newHead.X--; break;
                case 3: newHead.Y--; break;
            }

            if(newHead == food)
            {
                snake.Insert(0, food);
                SpawnFood();
            }
            else
            {
                snake.Insert(0, newHead);
                snake.RemoveAt(snake.Count - 1);
            }
        }

        private void TrainModel(NDArray state , int action, float reward, NDArray newState)
        {
            var target = model.Apply(state).numpy();
            target[0, action] = reward;
            model.fit(state, target, epochs: 1, verbose: 0);
        }

        private (NDArray, float, bool) TakeStep(int action)
        {
            UpdateDirection(action);
            MoveSnake();
            bool done = false;
            float reward = -0.1f;

            if (snake[0] == food)
            {
                reward = 10;
                SpawnFood();
            }
            else if (snake[0].X < 0 || snake[0].Y < 0 || snake[0].X >= this.Width / gridSize || snake[0].Y >= this.Height / gridSize)
            {
                reward = -10f; // Penalty for hitting walls
                done = true;
            }
            else if (snake.Skip(1).Contains(snake[0]))
            {
                reward = -10f; // Penalty for hitting itself
                done = true;
            }
            return (GetGameState(), reward, done);
        }

        private void TrainAI()
        {
            for (int episode = 0; episode < 1000; episode++)
            {
                var state = GetGameState();
                for (int step = 0; step < 500; step++)
                {
                    int action = ChooseBestAction(state);
                    var (newState, reward, done) = TakeStep(action);    
                    TrainModel(state, action, reward, newState);

                    if (done) break;
                }
            }
        }
        }
}
