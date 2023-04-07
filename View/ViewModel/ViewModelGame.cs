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
        #region Properties

        public Game _model;

        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }
        public Int32 Round { get { return _model.Turns; } }

        //  public int LocX { get { return _model.LocX; } } //coll
        // public int LocY { get { return _model.LocY; } } //row
        Boolean _canMove = true;
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
        public ObservableCollection<GameFields> Fields;
        public ViewModelGame(Game game) 
        { 
            _model = game;
            _model.GameAdvanced += new EventHandler<GameEventArgs>(Model_GameAdvanced);
           // _model.FieldChanged += new EventHandler<Field>(Model_FieldChanged);
            NewGameCommand = new DelegateCommand(param => OnNewGame()); 
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());

            NewExtraTask = new DelegateCommand(param => OnExtraTask());
            PressUp  = new DelegateCommand(param => OnPressUp());
            PressDown = new DelegateCommand(param => OnPressDown());
            PressRight = new DelegateCommand(param => OnPressRight());
            PressLeft  = new  DelegateCommand(param => OnPressLeft());
            ConnectRobot = new DelegateCommand(param => OnConnectRobot());
            DisConnectRobot = new DelegateCommand(param => OnDisConnectRobot());
            ConnectCubes  = new DelegateCommand(param => OnConnectCubes());
            DisConnectCubes = new DelegateCommand(param => OnDisConnectCubes());
            RotateRobot = new DelegateCommand(param => OnRotateRobot());



            for (Int32 i = 0; i<_model.board.Height; i++) // inicializáljuk a mezőket
               {
                   for (Int32 j = 0; j<_model.board.Width; j++)
                   {
                       GameFields field = new GameFields();
                       field.TextAndImage(_model.board.GetFieldValue(i, j));
                       field.X = j;
                       field.Y = i;
                       field.IsLocked = false;
                       field.StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)));
                       Fields.Add(field);
                       
                   }
                }
            ReFresh();
        }

        private void StepGame(int v)
        {
            throw new NotImplementedException();
        }

        private void OnNewExtraTask()
        {
            throw new NotImplementedException();
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

   

        private void KeyDown(String? direction)
        {
            if (!_canMove)
                return;

            // lekérdezzük, hogy hol helyezkedik épp a játékos
         /*   int x; // = _model.Robot.getX();
            int y; // = _model.Robot.getY();


            if (direction == "RIGHT" && (y + 1) < _model.board.Width) // jobbra lépünk
            {
                _model.Step(x, y + 1); // lépés a játékban
            }
            else if (direction == "LEFT" && (y - 1) >= 0) // balra lépünk
            {
                _model.Step(x, y - 1); // lépés a játékban
            }
            else if (direction == "UP" && (x - 1) >= 0) // felfele lépünk
            {
                _model.Step(x - 1, y); // lépés a játékban
            }
            else if (direction == "DOWN" && (x + 1) < _model.board.Height) // lefele lépünk
            {
                _model.Step(x + 1, y); // lépés a játékban
            } */

            OnPropertyChanged();
        }

        

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
