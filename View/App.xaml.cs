using Model.Model;
using View.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Model.Persistence;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;
using System.Threading;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Game _model = null!;
        private NoticeBoard _board = null!;
        private ViewModelGame _viewModel = null!;
        private PlayerMode _playerMode = null!;
        private ViewerMode _viewerMode = null!;
        private Diary _diary = null!;
        private MainPage _mainPage = null!;
        private MainWindow _mainWindow = null!;
        MyDataAccess _dataAccess = null!;
        private DispatcherTimer _timer = null!;
        private OpenFileDialog? _openFileDialog;
        private SaveFileDialog? _saveFileDialog;
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            _dataAccess = new MyDataAccess();
            _model = new Game(_dataAccess);
            _model.GameOver += new EventHandler<GameEventArgs>(Model_GameOver);
            _model.NewRound += new EventHandler<GameEventArgs>(Model_NewRound);
            _model.UpdateFields += new EventHandler<ActionEventArgs>(Model_UpdateFields);

            _viewModel = new ViewModelGame(_model);
            _viewModel.PlayerModeClick += new EventHandler(ViewModel_PlayerMode);
            _viewModel.ViewerModeClick += new EventHandler(ViewModel_ViewerMode);
            _viewModel.ViewerModeBackClick += new EventHandler(ViewModel_ViewerModeBack);
            _viewModel.DiaryClick += new EventHandler(ViewModel_Diary);
            _viewModel.ExitClick += new EventHandler(ViewModel_Exit);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame); // kezeljük a nézetmodell eseményeit
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            _mainWindow = new MainWindow();
            _mainPage = new MainPage();
           // _mainWindow.DataContext = _viewModel;
            _mainWindow.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            _mainPage.DataContext = _viewModel;
            _mainWindow.Content = _mainPage;
            _mainWindow.Show();


            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            

        }

        private void Timer_Tick(object sender, EventArgs e) 
        {
            _model.AdvanceTime();
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Biztos, hogy ki akarsz lépni?", "CyberChallenge játék", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást
            }

        }
        private void ViewModel_PlayerMode(object? sender, EventArgs e)
        {
            //frame.NavigationService.Navigate(new PlayerMode2());
            _model.NewGame();
            _viewModel.GenerateTable();
            _viewModel.GenerateTasks();
            _playerMode = new PlayerMode();
            _playerMode.DataContext = _viewModel;
            _mainWindow.Content = _playerMode;
            _timer.Start();
        }

        private void ViewModel_Exit(object? sender, EventArgs e)
        {
            Shutdown(); // a teljes alkalmazás bezárása
        }

        private void ViewModel_ViewerMode(object? sender, EventArgs e)
        {
            _model.NewGame();
            _viewModel.GenerateTableVM();
            _viewModel.GenerateTasks();//nem

            _viewerMode = new ViewerMode();
            _viewerMode.DataContext = _viewModel;
            _mainWindow.Content = _viewerMode;
        }
        private void ViewModel_Diary(object? sender, EventArgs e)
        {
            int i = 1;
            _model.NewGame();
            //while (File.Exists("file" + i + ".txt"))
            {
                 _model.LoadGameAsync("file" + i + ".txt");
                _viewModel.GenerateTableVM();

                _diary = new Diary();
                _diary.DataContext = _viewModel;
                _mainWindow.Content = _diary;
              //  ++i;
             //   Thread.Sleep(10000);//will sleep for 10 sec
            }

        }
        private void ViewModel_ViewerModeBack(object? sender, EventArgs e)
        {
            _mainWindow.Content = _mainPage;
        }


        private void Model_GameOver(object sender, GameEventArgs e) 
        {
            _timer.Stop();
            
            if (e.WinnerTeam == 1)
            {
                MessageBox.Show("Az 1-es csapat nyert!" + Environment.NewLine +
                                "Összesen " + e.Team1Points + " pontot gyűjtöttek össze.",
                                "CyberChallenge játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);

            }
            else
            {
                MessageBox.Show("A 2-es csapat nyert!" + Environment.NewLine +
                               "Összesen " + e.Team2Points + " pontot gyűjtöttek össze.",
                               "CyberChallenge játék",
                               MessageBoxButton.OK,
                               MessageBoxImage.Asterisk);
            }

            //TODO vissza a fooldalra
            _mainWindow.Content = _mainPage;

        }


        private void Model_UpdateFields(object sender, ActionEventArgs e)
        {
            _timer.Stop();
           
                if (e.CanExecute == false)
                {
                MessageBox.Show("A művelet nem végrehajtható.", "CyberChallenge játék",
                           MessageBoxButton.OK,
                           MessageBoxImage.Asterisk);
                }
           /*     else
            {
                MessageBox.Show("A művelet sikeres volt", "CyberChallenge játék",
                           MessageBoxButton.OK,
                           MessageBoxImage.Asterisk);
            }*/
            _timer.Start();
            
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            _timer.Stop();
            if (_openFileDialog == null)
            {
                _openFileDialog = new OpenFileDialog();
                _openFileDialog.Title = "Robot - Játék betöltése";
                _openFileDialog.Filter = "Szövegfájlok|*.txt";
            }

            // nyithatunk új nézetet
            if (_openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _model.LoadGameAsync(_openFileDialog.FileName); // játék betöltése
                }
                catch (RobotDataException)
                {
                    MessageBox.Show("Hiba keletkezett a betöltés során.", "Robot", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            _timer.Start();
        }
        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object? sender, System.EventArgs e)
        {
            _timer.Stop();
            if (_saveFileDialog == null)
            {
                _saveFileDialog = new SaveFileDialog();
                _saveFileDialog.Title = "Robot - Játék mentése";
                _saveFileDialog.Filter = "Szövegfájlok|*.txt";
            }

            if (_saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    await _model.SaveGameAsync(_saveFileDialog.FileName); // játék mentése
                }
                catch (RobotDataException)
                {
                    MessageBox.Show("Hiba keletkezett a mentés során.", "Robot", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            _timer.Start();
        }

        private void Model_NewRound(object sender, GameEventArgs e)
        {
            /*MessageBox.Show("Új kör kezdődött.", "CyberChallenge játék", MessageBoxButton.OK,
                               MessageBoxImage.Asterisk);*/
        }

       
    }
}
