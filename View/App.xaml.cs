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

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Game _model;
        private NoticeBoard _board;
        private ViewModelGame _viewModel;
        private PlayerMode2 _playerMode;
        private ViewerMode2 _viewerMode;
        private MainWindow _mainWindow;
        MyDataAccess _dataAccess;
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            _dataAccess = new MyDataAccess();
            _model = new Game(_dataAccess);

            _viewModel = new ViewModelGame(_model);

            //_playerMode = new PlayerMode2();
            //_playerMode.DataContext = _viewModel;

            _mainWindow = new MainWindow();
            _viewModel.PlayerModeClick += new EventHandler(MainWindow_PlayerMode);
            _viewModel.ViewerModeClick += new EventHandler(MainWindow_ViewerMode);
            _viewModel.ExitClick += new EventHandler(ViewerMode_Exit);
            _mainWindow.DataContext = _viewModel;
            _mainWindow.Show();
            

        }

        private void MainWindow_PlayerMode(object? sender, EventArgs e)
        {
            //frame.NavigationService.Navigate(new PlayerMode2());
            _playerMode = new PlayerMode2();
            _playerMode.DataContext = _viewModel;
            _mainWindow.Content = _playerMode;
        }

        private void ViewerMode_Exit(object? sender, EventArgs e)
        {
            //TODO
           //_mainWindow.
            //_mainWindow.Content = null;
        }

        private void MainWindow_ViewerMode(object? sender, EventArgs e)
        {
            //frame.NavigationService.Navigate(new PlayerMode2());
            _viewerMode = new ViewerMode2();
            _viewerMode.DataContext = _viewModel;
            _mainWindow.Content = _viewerMode;
        }


        private void Model_GameOver(object sender, GameEventArgs e) {/*code*/; }
        private void ViewModel_ExitGame(object sender, EventArgs e) {/*code*/; }
        private async void ViewModel_LoadGame(object sender, EventArgs e) {/*code*/; }
        private async void ViewModel_SaveGame(object sender, EventArgs e) {/*code*/; }
        private void ViewModel_NewGame(object sender, EventArgs e) {/*code*/; }
        private void View_Closing(object sender, CancelEventArgs e) {/*code*/; }

    }
}
