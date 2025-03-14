using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tensorflow.NumPy;
using Tensorflow;
using ScottPlot;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SnakeGame.TorchSharp;

namespace SnakeGame
{
     
    public partial class Form2: Form
    {
        private SnakeGame game;
        private DQNAgent ai;
        private System.Windows.Forms.Timer timer;
        private int score = 0 ;
        private int gameNumber = 0;
       

        public Form2()
        {
            InitializeComponent();
            this.Text = "AI Snake Game";
            this.Size = new Size(420, 450);
            
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            game = new SnakeGame();
            ai = new DQNAgent();
            

            InitializeGame();
        }

        private void InitializeGame()
        {
             game.Reset();
        
            score = 0;
            if(timer == null)
            {
                timer = new System.Windows.Forms.Timer { Interval = 1 };
                timer.Tick += Update;
                timer.Start();
            }
            else
            {
                timer.Stop();
                timer.Start();
            }
            Invalidate();
        }
        private void Update(object sender, EventArgs e)
        {
            using (var state = game.GetStateTensor())
            {
                // AI chooses action
                var action = ai.SelectAction(state);

                // Play step and get results
                var (reward, gameOver, newScore) = game.PlayStep((int)action);
                score = newScore;

                // Store experience
                using (var nextState = game.GetStateTensor())
                {
                    ai.StoreExperience(state, action, reward, nextState, gameOver);
                }

                // Train the model
                ai.OptimizeModel();

                // Restart game if over
                if (gameOver)
                {
                    gameNumber++;
                    InitializeGame();
                }
            }


            Invalidate(); // Refresh UI
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            foreach (Point p in game.snake)
            {
                g.FillRectangle(Brushes.Green, p.X, p.Y, 20, 20);
            }
            g.FillRectangle(Brushes.Red, game.food.X, game.food.Y, 20, 20);

            using (Font font = new Font("Arial", 12))
            {
                g.DrawString($"Score: {score}", font, Brushes.Green, 10, 10);
            }
            using (Font font = new Font("Arial", 12))
            {
                g.DrawString($"Game Number: {gameNumber}", font, Brushes.Green, 10, 30);
            }
        }
    }
}
