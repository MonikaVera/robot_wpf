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
        private MainWindow _view;
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


        }
        private void Model_GameOver(object sender, GameEventArgs e) {/*code*/; }
        private void ViewModel_ExitGame(object sender, EventArgs e) {/*code*/; }
        private async void ViewModel_LoadGame(object sender, EventArgs e) {/*code*/; }
        private async void ViewModel_SaveGame(object sender, EventArgs e) {/*code*/; }
        private void ViewModel_NewGame(object sender, EventArgs e) {/*code*/; }
        private void View_Closing(object sender, CancelEventArgs e) {/*code*/; }

    }
}
