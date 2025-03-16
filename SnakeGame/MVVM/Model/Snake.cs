using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.MVVM.Model
{
    public class Snake
    {
        public List<Rectangle> SnakeBody;
        private Point PlayerPosition;

        public Snake(int gridSize, Canvas gameCanvas)
        {
            SnakeBody = new List<Rectangle>();
            PlayerPosition = new Point();

            PlayerPosition.X = ((int)(gameCanvas.Width / gridSize) / 2) - 3;
            PlayerPosition.Y = (int)(gameCanvas.Height / gridSize) / 2;
        }

        public Rectangle CreateSnakePart(int gridSize, Canvas gameCanvas)
        {
            Rectangle snakePart = new Rectangle
            {
                Width = gridSize,
                Height = gridSize,
                Fill = Brushes.Green,
                Stroke = Brushes.Black,
                RadiusX = 2,
                RadiusY = 2
            };
            gameCanvas.Children.Add(snakePart);
            return snakePart;
        }

        public void GrowSnake(int gridSize, Canvas gameCanvas)
        {
            Rectangle newPart = CreateSnakePart(gridSize, gameCanvas);
            Rectangle lastPart = SnakeBody[SnakeBody.Count - 1];

            Point lastPartPosition = new Point(Canvas.GetLeft(lastPart), Canvas.GetTop(lastPart));

            Canvas.SetLeft(newPart, lastPartPosition.X);
            Canvas.SetTop(newPart, lastPartPosition.Y);

            SnakeBody.Add(newPart);
        }

        public void StartingSnakePosition(int gridSize)
        {
            Canvas.SetLeft(SnakeBody[0], PlayerPosition.X * gridSize);
            Canvas.SetTop(SnakeBody[0], PlayerPosition.Y * gridSize);
        }

        public bool MoveWithCollisionDetection(Key currentDirection, int gridSize, Canvas gameCanvas, Food food, Snake snake, Action IncreaseScore)
        {
            Point newPlayerPosition = PlayerPosition;

            switch (currentDirection)
            {
                case Key.Up: newPlayerPosition.Y--; break;
                case Key.Down: newPlayerPosition.Y++; break;
                case Key.Right: newPlayerPosition.X++; break;
                case Key.Left: newPlayerPosition.X--; break;
            }

            if (CheckCollisionWithWallsAndSnakeBody(gridSize, gameCanvas, newPlayerPosition))
            {
                return true;
            }

            UpdateSnakePosition(gridSize, gameCanvas, newPlayerPosition, food, snake, IncreaseScore);
            gameCanvas.Focus();
            return false;
        }

        public void UpdateSnakePosition(int gridSize, Canvas gameCanvas, Point newPlayerPosition, Food food, Snake snake, Action IncreaseScore)
        {
            for (int i = SnakeBody.Count - 1; i > 0; i--)
            {
                Canvas.SetLeft(SnakeBody[i], Canvas.GetLeft(SnakeBody[i - 1]));
                Canvas.SetTop(SnakeBody[i], Canvas.GetTop(SnakeBody[i - 1]));
            }

            Canvas.SetLeft(SnakeBody[0], newPlayerPosition.X * gridSize);
            Canvas.SetTop(SnakeBody[0], newPlayerPosition.Y * gridSize);

            CheckCollisionWithFood(gridSize, gameCanvas, newPlayerPosition, food, snake, IncreaseScore);
            PlayerPosition.X= newPlayerPosition.X;
            PlayerPosition.Y = newPlayerPosition.Y;
        }

        public bool CheckCollisionWithWallsAndSnakeBody(int gridSize, Canvas gameCanvas, Point newPlayerPosition)
        {
            if (newPlayerPosition.X < 0 || newPlayerPosition.Y < 0 ||
                newPlayerPosition.X * gridSize >= gameCanvas.Width ||
                newPlayerPosition.Y * gridSize >= gameCanvas.Height)
                return true;

            foreach (var part in SnakeBody)
            {
                if (Canvas.GetLeft(part) == newPlayerPosition.X * gridSize && 
                    Canvas.GetTop(part) == newPlayerPosition.Y * gridSize)
                    return true;
            }
            return false;
        }

        public void CheckCollisionWithFood(int gridSize, Canvas gameCanvas, Point newPlayerPosition, Food food, Snake snake, Action IncreaseScore)
        {
            if (newPlayerPosition.X == food.FoodPosition.X && newPlayerPosition.Y == food.FoodPosition.Y)
            {
                gameCanvas.Children.Remove(food.FoodBody);
                GrowSnake(gridSize, gameCanvas);
                food.FoodSpawn(gridSize, gameCanvas, snake);
                IncreaseScore();
            }
        }
    }
}
