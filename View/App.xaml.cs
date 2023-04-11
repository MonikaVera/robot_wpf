﻿using Model.Model;
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
        private PlayerMode2 _playerMode = null!;
        private ViewerMode2 _viewerMode = null!;
        private MainPage _mainPage = null!;
        private MainWindow _mainWindow = null!;
        MyDataAccess _dataAccess = null!;
        private DispatcherTimer _timer = null!;
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
            _viewModel.ExitClick += new EventHandler(ViewModel_Exit);

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
            _playerMode = new PlayerMode2();
            _playerMode.DataContext = _viewModel;
            _mainWindow.Content = _playerMode;
            _timer.Start();
        }

        private void ViewModel_Exit(object? sender, EventArgs e)
        {
            //TODO
            //_mainWindow.
            //_mainWindow.Content = null;
        }

        private void ViewModel_ViewerMode(object? sender, EventArgs e)
        {
            _model.NewGame();
            _viewModel.GenerateTable();
            _viewModel.GenerateTasks();//nem

            _viewerMode = new ViewerMode2();
            _viewerMode.DataContext = _viewModel;
            _mainWindow.Content = _viewerMode;
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
                else
            {
                MessageBox.Show("A művelet sikeres volt", "CyberChallenge játék",
                           MessageBoxButton.OK,
                           MessageBoxImage.Asterisk);
            }
            _timer.Start();
            
        }

        private void Model_NewRound(object sender, GameEventArgs e)
        {
            /*MessageBox.Show("Új kör kezdődött.", "CyberChallenge játék", MessageBoxButton.OK,
                               MessageBoxImage.Asterisk);*/
        }

        private async void ViewModel_LoadGame(object sender, EventArgs e) {/*code*/; }
        private async void ViewModel_SaveGame(object sender, EventArgs e) {/*code*/; }
       
    }
}
