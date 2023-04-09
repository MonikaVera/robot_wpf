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
        private Game _model;

        #region Commands
        public DelegateCommand PlayerModeCommand { get; private set; }
        public DelegateCommand ViewerModeCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand KeyDownCommand { get; private set; }
        public DelegateCommand ChooseActionFieldCommand { get; private set; }

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand ExitGameCommand { get; private set; }
        public DelegateCommand NewExtraTask { get; private set; }
        
        #endregion

        #region Events
        public event EventHandler? PlayerModeClick;
        public event EventHandler? ViewerModeClick;
        public event EventHandler? ExitClick;
        

        public event EventHandler ExitGame;
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;
        #endregion

        #region Properties
        public int Height { get { return _model.Board.Height; }
            set
            {
                OnPropertyChanged(nameof(Height));
            }
        }

        public int Width
        {
            get { return _model.Board.Width; }
            set
            {
                OnPropertyChanged(nameof(Width));
            }
        }

        public int Round
        {
            get { return _model.Round; }
            set
            {
                OnPropertyChanged(nameof(Round));
            }
        }



        public Game Game { get; set; }

        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }
      

        //  public int LocX { get { return _model.LocX; } }
        // public int LocY { get { return _model.LocY; } }
        #endregion

        public ViewModelGame(Game game)
        {
            _model = game;
            _model.GameAdvanced += new EventHandler<GameEventArgs>(Model_GameAdvanced);
            // _model.FieldChanged += new EventHandler<Field>(Model_FieldChanged);
            PlayerModeCommand = new DelegateCommand(param => OnPlayerModeClick());
            ViewerModeCommand = new DelegateCommand(param => OnViewerModeClick());
            ExitCommand = new DelegateCommand(param => OnExitClick());
            KeyDownCommand = new DelegateCommand(param => KeyDown(Convert.ToString(param)));
            

            //jatektabla letrehozasa
            Fields = new ObservableCollection<ViewModelField>();
            for (int j = 0; j < _model.Board.Height; j++)
                for (int i = 0; i < _model.Board.Width; i++)
                {
                    ViewModelField field = new ViewModelField();
                    field.SetText(_model.Board.GetFieldValue(i, j));
                    field.Number = j * _model.Board.Width + i;
                    field.IndX = i;
                    field.IndY = j;
                    field.ChooseActionFieldCommand = new DelegateCommand(param => ChooseActionField(Convert.ToInt32(param)));

                    Fields.Add(field);
                }

        }

        #region Private Methods
        private void OnPlayerModeClick()
        {
            PlayerModeClick?.Invoke(this, EventArgs.Empty);
        }

        private void OnViewerModeClick()
        {
            ViewerModeClick?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitClick()
        {
            ExitClick?.Invoke(this, EventArgs.Empty);
        }

        private void ChooseActionField(int number)
        {
            ViewModelField field = Fields[number];

            _model.ChooseActionField(field.IndX, field.IndY);
        }

        private void KeyDown(string? action)
        {
            // lekérdezzük, hogy hol helyezkedik épp a robot
            Int32 x = _model.Robot.X;
            Int32 y = _model.Robot.Y;


            if (action == "RIGHT" && (x + 1) < _model.Board.Width) // jobbra lépünk
            {
                _model.MoveRobot(_model.Robot, Direction.EAST); 
            }
            else if (action == "LEFT" && (x - 1) >= 0) // balra lépünk
            {
                _model.MoveRobot(_model.Robot, Direction.WEST); 
            }
            else if (action == "UP" && (y - 1) >= 0) // felfele lépünk
            {
                _model.MoveRobot(_model.Robot,Direction.NORTH); 
            }
            else if (action == "DOWN" && (y + 1) < _model.Board.Height) // lefele lépünk
            {
                _model.MoveRobot(_model.Robot,Direction.SOUTH); 
            }
            else if (action == "CLEAN") // tisztitunk
            {
                _model.Clean(_model.Robot); 
            }
            else if (action == "MERGE") // osszekapcsolunk kockakat
            {
                _model.ConnectCubes(_model.Robot); 
            }
            else if (action == "GET") // rakapcsolodunk egy kockara
            {
                _model.ConnectRobot(_model.Robot); 
            }
            else if (action == "PUTDOWN") // lekapcsolodunk egy kockarol
            {
                _model.DisconnectRobot(_model.Robot);
            }
            else if (action == "SPLIT") // szetkapcsolunk kockakat
            {
                _model.DisconnectCubes(_model.Robot);
            }
            else if (action == "TURNEAST") // oramutatoval megegyezo iranyu forgas
            {
                _model.RotateRobot(_model.Robot, Angle.Clockwise);
            }
            else if (action == "TURNWEST") // oramutatoval ellentetes iranyu forgas
            {
                _model.RotateRobot(_model.Robot, Angle.CounterClockwise);
            }
            else if (action == "WAIT") // varakozas
            {
                _model.Wait(_model.Robot);
            }


            OnPropertyChanged(nameof(Round));
        }

        private void Model_GameAdvanced(object? sender, GameEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
        }

        /// <summary>
        /// Modell mezőváltozásának eseménykezelése.
        /// </summary>
        private void Model_FieldChanged(object? sender, Field e)
        {
            // Fields.First(field => field.X == e.X && field.Y == e.Y).Player = PlayerToField(_model[e.X, e.Y]);
            // lineáris keresés a megadott sorra, oszlopra, majd a játékos átírása
        }

        private void Model_MoveRobot(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_RotateRobot(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_ConnectRobot(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_DisConnectRobot(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_ConnectCubes(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_DisConnectCubes(object obj, RobotEventArgs e) {/*code*/; }
        private void Model_Clean(object obj, RobotEventArgs e) {/*code*/; }
       
        private void ReFresh() {/*code*/; }
        private void OnLoadGame() {/*code*/; }
        private void OnNewGame() {/*code*/; }
        private void OnSaveGame() {/*code*/; }
        private void OnExitGame() {/*code*/; }
        private void OnExtraTask() {/*code*/; }

        #endregion


    }
}

