using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.MVVM.Model
{
    public class Food
    {
        public Rectangle FoodBody;
        public Point FoodPosition;
        private Random random = new Random();

        public void FoodSpawn(int gridSize, Canvas gameCanvas, Snake snake)
        {
            do
            {
                FoodPosition = new Point(random.Next(0, (int)(gameCanvas.Width / gridSize)),
                                     random.Next(0, (int)(gameCanvas.Height / gridSize)));
            } while (IsFoodInsideSnake(gridSize, gameCanvas, snake));

            FoodBody = CreateFood(gridSize, gameCanvas);
            Canvas.SetLeft(FoodBody, FoodPosition.X * gridSize);
            Canvas.SetTop(FoodBody, FoodPosition.Y * gridSize);
        }

        private bool IsFoodInsideSnake(int gridSize, Canvas gameCanvas, Snake snake)
        {
            foreach (var part in snake.SnakeBody)
            {
                double snakePartX = Canvas.GetLeft(part) / gridSize;
                double snakePartY = Canvas.GetTop(part) / gridSize;

                if (snakePartX == FoodPosition.X && snakePartY == FoodPosition.Y)
                {
                    return true;
                }
            }
            return false;
        }

        private Rectangle CreateFood(int gridSize, Canvas gameCanvas)
        {
            Rectangle food = new Rectangle
            {
                Width = gridSize,
                Height = gridSize,
                Fill = Brushes.Red,
                RadiusX = 20,
                RadiusY = 20,
                Stroke = Brushes.Black,
            };
            gameCanvas.Children.Add(food);
            return food;
        }
    }
}
