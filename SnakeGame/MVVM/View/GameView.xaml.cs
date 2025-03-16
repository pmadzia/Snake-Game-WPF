using SnakeGame.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace SnakeGame.MVVM.View
{
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
            Loaded += GameView_Loaded;
        }

        private void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is GameViewModel gameViewModel)
            {
                gameViewModel.InitializeGame(GameCanvas);
                GameCanvas.Focus();
                GameCanvas.KeyDown += gameViewModel.OnKeyDown;
                gameViewModel.ScoreUpdated += UpdateScoreUI;
            }
        }

        private void UpdateScoreUI(int newScore)
        {
            ScoreText.Text = $"Score: {newScore}";
        }
    }
}
