using SnakeGame.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace SnakeGame.MVVM.View
{
    public partial class GameOverView : UserControl
    {
        public GameOverView(int finalScore)
        {
            InitializeComponent();
            DataContext = new GameOverViewModel(finalScore);
            Loaded += GameOVerView_Loaded;
        }

        private void GameOVerView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is GameOverViewModel gameOverViewModel)
            {
                this.Focus();
                this.KeyDown += gameOverViewModel.OnKeyDown;
            }    
        }
    }
}
