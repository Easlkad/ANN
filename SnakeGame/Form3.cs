using SnakeGame.Tensorflow;
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

namespace SnakeGame
{
    
    public partial class Form3: Form
    {
        private SnakeGame game;
        private Snake ai;
        private System.Windows.Forms.Timer timer;
        private int score = 0 ;
        private int gameNumber = 0;
        private List<(NDArray state, int action, float reward, NDArray nextState, bool gameOver)> gameExperience;

        public Form3()
        {
            //InitializeComponent();
            //this.Text = "AI Snake Game";
            //this.Size = new Size(420, 450);
            
            //this.MaximizeBox = false;
            //this.StartPosition = FormStartPosition.CenterScreen;
            //this.DoubleBuffered = true;
            //game = new SnakeGame();
            //ai = new Snake();
            //gameExperience = new List<(NDArray state, int action, float reward, NDArray nextState, bool gameOver)>();

            //InitializeGame();
        }

        //private void InitializeGame()
        //{
        //     game.Reset();
        //    gameExperience.Clear();
        //    score = 0;
        //    if(timer == null)
        //    {
        //        timer = new System.Windows.Forms.Timer { Interval = 10 };
        //        timer.Tick += Update;
        //        timer.Start();
        //    }
        //    else
        //    {
        //        timer.Stop();
        //        timer.Start();
        //    }
        //    Invalidate();
        //}
        //private void Update(object sender, EventArgs e)
        //{
        //    NDArray state = game.GetState();
        //    int action = ai.ChooseAction(state); // AI decides the next move

        //    var (reward, gameOver, newScore) = game.PlayStep(action);
        //    score = newScore;
        //    gameExperience.Add((state, action, reward, game.GetState(), gameOver));
           
        //    var uiContext = SynchronizationContext.Current;
        //    if (gameOver)
        //    {
        //        ai.StoreExperience(gameExperience);
        //        ai.ChangeEpsilon();
        //        ai.TrainModel();
        //        gameNumber++;// Train the AI after the game ends
        //        InitializeGame(); // Restart the game
        //    }
           

        //    Invalidate(); // Refresh UI
        //}
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);

        //    Graphics g = e.Graphics;
        //    foreach (Point p in game.snake)
        //    {
        //        g.FillRectangle(Brushes.Green, p.X, p.Y, 40, 40);
        //    }
        //    g.FillRectangle(Brushes.Red, game.food.X, game.food.Y, 40, 40);

        //    using (Font font = new Font("Arial", 12))
        //    {
        //        g.DrawString($"Score: {score}", font, Brushes.Green, 10, 10);
        //    }
        //    using (Font font = new Font("Arial", 12))
        //    {
        //        g.DrawString($"Game Number: {gameNumber}", font, Brushes.Green, 10, 30);
        //    }
        //}
    }
    
}
