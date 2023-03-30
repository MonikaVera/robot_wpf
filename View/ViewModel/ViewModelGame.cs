using Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.ViewModel
{
    public class ViewModelGame : ViewModelBase
    {
        public ViewModelGame(Game game) { _model = game; }
        #region Properties
        public ObservableCollection<ViewModelField> Fields;
        public Game _model;
        #endregion
        #region Commands
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand ExitGameCommand { get; private set; }
        public DelegateCommand NewExtraTask { get; private set; }
        public DelegateCommand PressUp { get; private set; }
        public DelegateCommand PressDown { get; private set; }
        public DelegateCommand PressRight { get; private set; }
        public DelegateCommand PressLeft { get; private set; }
        public DelegateCommand ConnectRobot { get; private set; }
        public DelegateCommand DisConnectRobot { get; private set; }
        public DelegateCommand ConnectCubes { get; private set; }
        public DelegateCommand DisConnectCubes { get; private set; }
        public DelegateCommand RotateRobot { get; private set; }
        #endregion
        #region Events
        public event EventHandler ExitGame;
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;
        #endregion
        #region Methods
        private void Model_MoveRobot(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_RotateRobot(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_ConnectRobot(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_DisConnectRobot(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_ConnectCubes(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_DisConnectCubes(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_Clean(object obj, RobotEventArgs e) {/*code*/; }
        private void OnPressUp() {/*code*/; }
        private void OnPressDown() {/*code*/; }
        private void OnPressRight() {/*code*/; }
        private void OnPressLeft() {/*code*/; }
        private void OnConnectRobot() {/*code*/; }
        private void OnDisConnectRobot() {/*code*/; }
        private void OnConnectCubes() {/*code*/; }
        private void OnDisConnectCubes() {/*code*/; }
        private void OnRotateRobot() {/*code*/; }
        private void ReFresh() {/*code*/; }
        private void OnLoadGame() {/*code*/; }
        private void OnNewGame() {/*code*/; }
        private void OnSaveGame() {/*code*/; }
        private void OnExitGame() {/*code*/; }
        private void OnExtraTask() {/*code*/; }
        #endregion
    }
}
