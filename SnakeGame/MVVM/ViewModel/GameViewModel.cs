using SnakeGame.MVVM.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame.MVVM.ViewModel
{
    public class GameViewModel
    {
        private Canvas gameCanvas;
        private const int gridSize = 20;
        private DispatcherTimer gameTimer;
        private Key currentDirection;

        private int gameSpeed = 100;
        private bool gameOver = false;
        private int score;

        private Snake snake;
        private Food food;

        public event Action<int> ScoreUpdated;

        public void InitializeGame(Canvas canvas)
        {
            currentDirection = Key.Right;
            gameCanvas = canvas;
            snake = new Snake(gridSize, gameCanvas);
            food = new Food();
            gameCanvas.Children.Clear();
            snake.SnakeBody.Clear();
            score = 0;
            ScoreUpdated?.Invoke(score);
            StartGame();
        }

        private void StartGame()
        {
            Rectangle head = snake.CreateSnakePart(gridSize, gameCanvas);
            snake.SnakeBody.Add(head);
            snake.StartingSnakePosition(gridSize);
            food.FoodSpawn(gridSize, gameCanvas, snake);
            StartGameLoop();
        }

        private void StartGameLoop()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(gameSpeed);
            gameTimer.Tick += (sender, e) => GameTimerTick();
            gameTimer.Start();
        }

        private void GameTimerTick()
        {
            gameOver = snake.MoveWithCollisionDetection(currentDirection, gridSize, gameCanvas, food, snake, IncreaseScore);

            if (gameOver)
            {
                gameTimer.Stop();
                int finalScore = score;

                if (Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
                {
                    mainViewModel.GameOverCommand.Execute(finalScore);
                }
            }
        }

        private void IncreaseScore()
        {
            score += 10;
            ScoreUpdated?.Invoke(score);
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Up && currentDirection != Key.Down) ||
                (e.Key == Key.Down && currentDirection != Key.Up) ||
                (e.Key == Key.Left && currentDirection != Key.Right) ||
                (e.Key == Key.Right && currentDirection != Key.Left))
            {
                currentDirection = e.Key;;
            }
        }
    }
}
