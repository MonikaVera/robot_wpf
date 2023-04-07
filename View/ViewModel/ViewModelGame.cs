using Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace View.ViewModel
{
    public class ViewModelGame : ViewModelBase
    {
        public ObservableCollection<ViewModelField> Fields { get; set; }

        public ViewModelGame(Game game)
        {
            _model = game;
            _model.GameAdvanced += new EventHandler<GameEventArgs>(Model_GameAdvanced);
            // _model.FieldChanged += new EventHandler<Field>(Model_FieldChanged);

            //jatektabla letrehozasa
            Fields = new ObservableCollection<ViewModelField>();
            for (int i = 0; i < _model.Board.Width; i++)
                for (int j = 0; j < _model.Board.Height; j++)
                {
                    ViewModelField field = new ViewModelField();
                    field.SetText(_model.Board.GetFieldValue(i, j));
                    field.IndX = i;
                    field.IndY = j;
                    Fields.Add(field);
                }

        }

        private void Model_GameAdvanced(object? sender, GameEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(Round));
            //  OnPropertyChanged(nameof(LocX));
            //  OnPropertyChanged(nameof(LocY));
        }

        /// <summary>
        /// Modell mezőváltozásának eseménykezelése.
        /// </summary>
        private void Model_FieldChanged(object? sender, Field e)
        {
            // Fields.First(field => field.X == e.X && field.Y == e.Y).Player = PlayerToField(_model[e.X, e.Y]);
            // lineáris keresés a megadott sorra, oszlopra, majd a játékos átírása
        }

        #region Properties
        public Game _model;

        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }
        public Int32 Round { get { return _model.Turns; } }

        //  public int LocX { get { return _model.LocX; } }
        // public int LocY { get { return _model.LocY; } }

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

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>

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

