using SnakeGame.Core;
using SnakeGame.MVVM.View;
using System.Windows;
using System.Windows.Input;

namespace SnakeGame.MVVM.ViewModel
{
    internal class GameOverViewModel : ObservableObject
    {
        private int finalScore;

        public int FinalScore
        {
            get 
            { 
                return finalScore; 
            }
            set
            {
                finalScore = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RestartGameCommand { get; set; }

        public GameOverViewModel(int score)
        {
            FinalScore = score;

            RestartGameCommand = new RelayCommand(o =>
            {
                if (Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
                {
                    mainViewModel.CurrentView = new GameView { DataContext = mainViewModel.GameVM };
                }
            });
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                RestartGameCommand.Execute(null);
            }
        }
    }
}
