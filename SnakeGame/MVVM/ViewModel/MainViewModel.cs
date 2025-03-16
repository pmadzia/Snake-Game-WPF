using SnakeGame.Core;
using SnakeGame.MVVM.View;
using System.Windows;

namespace SnakeGame.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        /* Commands */
        public RelayCommand MoveWindowCommand { get; set; }
        public RelayCommand ShutdownWindowCommand { get; set; }
        public RelayCommand MinimizeWindowCommand { get; set; }
        public RelayCommand StartGameCommand { get; set; }

        public RelayCommand GameOverCommand { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MenuViewModel MenuVM { get; set; }
        public GameViewModel GameVM { get; set; }

        public MainViewModel()
        {
            MenuVM = new MenuViewModel();
            GameVM = new GameViewModel();
            CurrentView = MenuVM;

            MoveWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.DragMove(); });
            ShutdownWindowCommand = new RelayCommand(o => { Application.Current.Shutdown(); });
            MinimizeWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });
            StartGameCommand = new RelayCommand(o => { CurrentView = new GameView { DataContext = GameVM }; });
            GameOverCommand = new RelayCommand(o =>
            {
                if (o is int finalScore)
                {
                    CurrentView = new GameOverView(finalScore);
                }
            });
        }
    }
}
