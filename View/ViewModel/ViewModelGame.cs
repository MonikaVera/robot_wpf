﻿using Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace View.ViewModel
{
    public class ViewModelGame : ViewModelBase
    {
        public ObservableCollection<ViewModelField> Fields { get; set; }
        public ObservableCollection<ViewModelField> FieldsMap { get; set; }
        public ObservableCollection<ViewModelField> FieldsMapView { get; set; }
        public ObservableCollection<VMTasksFields> FieldsTasks { get; set; }

        private Game _model;
        private bool _canMove;

        #region Commands
        public DelegateCommand PlayerModeCommand { get; private set; }
        public DelegateCommand ViewerModeCommand { get; private set; }
        public DelegateCommand ViewerModeBack { get; }
        public DelegateCommand ViewerModeNext { get; }
        public DelegateCommand DiaryCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand KeyDownCommand { get; private set; }
        public DelegateCommand NextPlayerCommand { get; private set; }
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
        public event EventHandler? ViewerModeBackClick;
        public event EventHandler? ViewerModeNextClick;
        public event EventHandler? DiaryClick;
        public event EventHandler? ExitClick;

        public event EventHandler? StopTimer;
        public event EventHandler? StartTimer;



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

        public int Team1Points
        {
            get { return _model.Team1Points; }
            set
            {
                OnPropertyChanged(nameof(Team1Points));
            }
        }

        public int Team2Points
        {
            get { return _model.Team2Points; }
            set
            {
                OnPropertyChanged(nameof(Team2Points));
            }
        }



        public Game Game { get; set; }

        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }
      

        //  public int LocX { get { return _model.LocX; } }
        // public int LocY { get { return _model.LocY; } }
        #endregion

        public ViewModelGame(Game game)
        {
            //jatek csatlakoztatasa
            _model = game;
            _canMove = true;
            _model.GameAdvanced += new EventHandler<GameEventArgs>(Model_GameAdvanced);
            //_model.GameOver += new EventHandler<GameEventArgs>(Model_GameOver);
            _model.NewRound += new EventHandler<GameEventArgs>(Model_NewRound);
            _model.UpdateFields += new EventHandler<ActionEventArgs>(Model_UpdateFields);

            //parancsok kezelese
            // _model.FieldChanged += new EventHandler<Field>(Model_FieldChanged);
            PlayerModeCommand = new DelegateCommand(param => OnPlayerModeClick());
            ViewerModeCommand = new DelegateCommand(param => OnViewerModeClick());
            DiaryCommand = new DelegateCommand(param => OnDiaryClick());
            ExitCommand = new DelegateCommand(param => OnExitClick());
            ViewerModeBack = new DelegateCommand(param => OnViewerModeBackClick());
            ViewerModeNext = new DelegateCommand(param => OnViewerModeNextClick());
            KeyDownCommand = new DelegateCommand(param => KeyDown(Convert.ToString(param)));
            NextPlayerCommand = new DelegateCommand(param => NextPlayer());

            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());

        }

        #region Public Methods
        /*
        public void GenerateTable()
        {
            //jatektabla letrehozasa
            Fields = new ObservableCollection<ViewModelField>();
            for (int j = 0; j < _model.Board.Height; j++)
                for (int i = 0; i < _model.Board.Width; i++)
                {
                    ViewModelField field = new ViewModelField();
                    field.Number = j * _model.Board.Width + i;
                    field.SetPicture(_model.Board.GetFieldValue(i, j));
                    //field.Number = j * _model.Board.Width + i;
                    field.IndX = i;
                    field.IndY = j;
                    field.ChooseActionFieldCommand = new DelegateCommand(param => ChooseActionField(Convert.ToInt32(param)));

                    Fields.Add(field);
                }
        } */
        public void GenerateTable()
        {
            //jatektabla letrehozasa
            Fields = new ObservableCollection<ViewModelField>();
            FieldsMap = new ObservableCollection<ViewModelField>();
            int x = 0, y = 0;
            for (int j = _model.Robot.Y - 3; j <= _model.Robot.Y + 3; j++)
            {
                for (int i = _model.Robot.X - 3; i <= _model.Robot.X + 3; i++)
                    if (j >= 0 && i >= 0 && (Math.Abs(j - _model.Robot.Y) + Math.Abs(i - _model.Robot.X)) <= 3 && j < _model.Board.Height && i < _model.Board.Width)
                    {
                        ViewModelField field = new ViewModelField();
                        //field.Number = j * 7 + i;
                        field.SetPicture(_model.Board.GetFieldValue(i, j));
                        field.IndX = x;
                        field.IndY = y;
                        field.ChooseActionFieldCommand = new DelegateCommand(param => ChooseActionField(Convert.ToInt32(param)));
                        ++y;
                        Fields.Add(field);
                    }
                    else
                    {
                        ViewModelField field = new ViewModelField();
                        //field.SetText(_model.Board.GetFieldValue(0, 0));//Black
                        field.IndX = x;
                        field.IndY = y;
                        field.ChooseActionFieldCommand = new DelegateCommand(param => ChooseActionField(Convert.ToInt32(param)));
                        ++y;
                        Fields.Add(field);
                    }
                ++x;
            }

            for (int j = 0; j < _model.Board.Height; j++)
                for (int i = 0; i < _model.Board.Width; i++)
                    if ( (Math.Abs(j - _model.Robot.Y) + Math.Abs(i - _model.Robot.X)) <= 3)
                    {
                        ViewModelField fieldMap = new ViewModelField();
                        // fieldMap.Number = j * _model.Board.Width + i;
                        fieldMap.SetPicture(_model.Board.GetFieldValue(i, j));
                        fieldMap.IndX = i;
                        fieldMap.IndY = j;
                        fieldMap.ChooseActionFieldCommand = new DelegateCommand(param => ChooseActionField(Convert.ToInt32(param)));

                        FieldsMap.Add(fieldMap);
                    }
                    else
                    {
                        ViewModelField fieldMap = new ViewModelField();
                        // fieldMap.Number = j * _model.Board.Width + i;
                        fieldMap.SetText(_model.Board.GetFieldValue(i, j));
                        fieldMap.IndX = i;
                        fieldMap.IndY = j;
                        fieldMap.ChooseActionFieldCommand = new DelegateCommand(param => ChooseActionField(Convert.ToInt32(param)));

                        FieldsMap.Add(fieldMap);
                    }
            GenerateTableVM();

        }

        public void GenerateTableVM()
        {
            FieldsMapView = new ObservableCollection<ViewModelField>();
            //viewer mode fields
            for (int j = 0; j < _model.Board.Height; j++)
                for (int i = 0; i < _model.Board.Width; i++)
                {
                    ViewModelField fieldMapView = new ViewModelField();
                    fieldMapView.SetPicture(_model.Board.GetFieldValue(i, j));
                    fieldMapView.IndX = i;
                    fieldMapView.IndY = j;
                    fieldMapView.ChooseActionFieldCommand = new DelegateCommand(param => ChooseActionField(Convert.ToInt32(param)));

                    FieldsMapView.Add(fieldMapView);
                }
            _model.SaveGameAsync("file" + 1 + ".txt");
        }

        public void GenerateTasks()
        {
            //tasks letrehozasa
            FieldsTasks = new ObservableCollection<VMTasksFields>();
            // _model.NoticeBoard.GenerateTasks(3, 3);
            for (int j = 0; j < 3; j++)
                for (int i = 0; i < 3; i++)
                {
                    VMTasksFields fieldTasks = new VMTasksFields();
                    fieldTasks.SetImage(_model.NoticeBoard.Fields[i, j]);
                    fieldTasks.Number = j * 3 + i;
                    fieldTasks.X = i;
                    fieldTasks.Y = j;
                    // fieldTasks.CubeColor = CubeToField(_model.NoticeBoard.Fields[i, j]);
                    fieldTasks.FieldChangeCommand = new DelegateCommand(param => ChooseActionField(Convert.ToInt32(param)));

                    FieldsTasks.Add(fieldTasks);
                }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Tábla frissítése.
        /// </summary>
        private void RefreshTable()
        {
            int y = 0;
            for (int j = _model.Robot.Y - 3; j <= _model.Robot.Y + 3; j++)
            {
                for (int i = _model.Robot.X - 3; i <= _model.Robot.X + 3; i++)
                {
                    if (j >= 0 && i >= 0 && (Math.Abs(j - _model.Robot.Y) + Math.Abs(i - _model.Robot.X)) <= 3
                        && j < _model.Board.Height && i < _model.Board.Width)
                    {
                        ViewModelField field = Fields[y]; //x,y
                        field.SetPicture(_model.Board.GetFieldValue(i, j));
                        ViewModelField fieldMap = FieldsMap[j * _model.Board.Width + i];
                        fieldMap.SetPicture(_model.Board.GetFieldValue(i, j));

                        //viewer mode table refresh
                        ViewModelField fieldMapView = FieldsMapView[j * _model.Board.Width + i];
                        fieldMapView.SetPicture(_model.Board.GetFieldValue(i, j));
                        ++y;
                    }
                    else
                    {
                        ViewModelField field = Fields[y];
                        field.SetPicture(new None());
                        ++y;
                    }

                }
            }
            _model.SaveGameAsync("file" + (_model.Round+1)+".txt");

            // frissítjük a megszerzett kosarak számát és a játékidőt
            OnPropertyChanged(nameof(GameTime));
        }

        private void OnPlayerModeClick()
        {
            PlayerModeClick?.Invoke(this, EventArgs.Empty);
        }

        private void OnViewerModeClick()
        {
            ViewerModeClick?.Invoke(this, EventArgs.Empty);
        }

        private void OnDiaryClick()
        {
            DiaryClick?.Invoke(this, EventArgs.Empty);
        }

        private void OnViewerModeBackClick()
        {
            ViewerModeBackClick?.Invoke(this, EventArgs.Empty);
        }
        private void OnViewerModeNextClick()
        {
            ViewerModeNextClick?.Invoke(this, EventArgs.Empty);
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
            if (!_canMove)
            {
                return;
            }

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

            _canMove = false;
            OnPropertyChanged(nameof(Round));
            OnStopTimer();
        }

        private void NextPlayer()
        {
            if (_canMove)
            {
                return;
            }
            _model.NextPlayer();
            RefreshTable();

            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(Round));
            OnPropertyChanged(nameof(Team1Points));
            OnPropertyChanged(nameof(Team2Points));
            OnStartTimer();
            _canMove = true;
        }

        private void Model_GameAdvanced(object? sender, GameEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
        }

        private void Model_NewRound(object? sender, GameEventArgs e)
        {
            if (_canMove)
            {
                return;
            }
            _model.NextPlayer();
            RefreshTable();

            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(Round));
            OnPropertyChanged(nameof(Team1Points));
            OnPropertyChanged(nameof(Team2Points));

            _canMove = true;
            OnStartTimer();
        }

        private void Model_UpdateFields(object obj, ActionEventArgs e)
        {
            if (e.CanExecute == false)
            {
                return;
            }

            RefreshTable();
            /*if (e.Action == Model.Model.Action.Move)
            {
                ViewModelField fieldMap;
                if (e.Direction == Direction.EAST)
                {
                    fieldMap = FieldsMap[e.Robot.Y * _model.Board.Width + e.Robot.X-1];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X-1, e.Robot.Y));

                    foreach (XYcoordinates coord in e.Robot.AllConnections())
                    {
                        fieldMap = FieldsMap[coord.Y * _model.Board.Width + coord.X - 1];
                        fieldMap.SetPicture(_model.Board.GetFieldValue(coord.X - 1, coord.Y));
                    }
                }
                else if (e.Direction == Direction.WEST){
                    fieldMap = FieldsMap[e.Robot.Y * _model.Board.Width + e.Robot.X + 1];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X + 1, e.Robot.Y));

                    foreach (XYcoordinates coord in e.Robot.AllConnections())
                    {
                        fieldMap = FieldsMap[coord.Y * _model.Board.Width + coord.X + 1];
                        fieldMap.SetPicture(_model.Board.GetFieldValue(coord.X + 1, coord.Y));
                    }
                }
                else if (e.Direction == Direction.NORTH)
                {
                    fieldMap = FieldsMap[(e.Robot.Y + 1) * _model.Board.Width + e.Robot.X];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X, e.Robot.Y + 1));

                    foreach (XYcoordinates coord in e.Robot.AllConnections())
                    {
                        fieldMap = FieldsMap[(coord.Y + 1) * _model.Board.Width + coord.X];
                        fieldMap.SetPicture(_model.Board.GetFieldValue(coord.X, coord.Y+1));
                    }
                }
                else if (e.Direction == Direction.SOUTH)
                {
                    fieldMap = FieldsMap[(e.Robot.Y-1) * _model.Board.Width + e.Robot.X];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X, e.Robot.Y-1));
                    

                    foreach (XYcoordinates coord in e.Robot.AllConnections())
                    {
                        fieldMap = FieldsMap[(coord.Y-1) * _model.Board.Width + coord.X];
                        fieldMap.SetPicture(_model.Board.GetFieldValue(coord.X, coord.Y - 1));
                    }
                }
                foreach (XYcoordinates coord in e.Robot.AllConnections())
                {
                    if(Game.IsOnBoard(coord.X, coord.Y))
                    {
                        fieldMap = FieldsMap[coord.Y * _model.Board.Width + coord.X];
                        fieldMap.SetPicture(_model.Board.GetFieldValue(coord.X, coord.Y));
                    }
                }
                fieldMap = FieldsMap[e.Robot.Y * _model.Board.Width + e.Robot.X];
                fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X, e.Robot.Y));
            }
            else if (e.Action == Model.Model.Action.Turn)
            {
                ViewModelField fieldMap;
              
                if(e.Direction==Direction.WEST) //counterclockwise
                {
                    foreach (XYcoordinates coord in e.Robot.AllConnections())
                    {
                        int newX = e.Robot.X + e.Robot.Y - coord.Y;
                        int newY = -e.Robot.X + e.Robot.Y + coord.X;
                        fieldMap = FieldsMap[newX * _model.Board.Width + newY];
                        fieldMap.SetPicture(_model.Board.GetFieldValue(newX, newY));

                    }
                } 
                else
                {
                    foreach (XYcoordinates coord in e.Robot.AllConnections())
                    {
                        int newX = e.Robot.X - e.Robot.Y + coord.Y;
                        int newY = e.Robot.X + e.Robot.Y - coord.X;
                        fieldMap = FieldsMap[newX * _model.Board.Width + newY];
                        fieldMap.SetPicture(_model.Board.GetFieldValue(newX, newY));
                    }
                }
                foreach (XYcoordinates coord in e.Robot.AllConnections())
                {
                    fieldMap = FieldsMap[coord.Y * _model.Board.Width + coord.X];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(coord.X, coord.Y));

                }
                fieldMap = FieldsMap[e.Robot.Y * _model.Board.Width + e.Robot.X];
                fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X, e.Robot.Y));

            }
            else if (e.Action == Model.Model.Action.ConnectRobot)
            {
                //semmi sem kell
            }
            else if (e.Action == Model.Model.Action.ConnectCubes)
            {

            }
            else if (e.Action == Model.Model.Action.DisconnectRobot)
            {
                //semmi sem kell
            }
            else if (e.Action == Model.Model.Action.DisconnectCubes)
            {

            }
            else if (e.Action == Model.Model.Action.Clean)
            {
                ViewModelField fieldMap;
                if (e.Direction == Direction.EAST)
                {
                    fieldMap = FieldsMap[e.Robot.Y * _model.Board.Width + e.Robot.X+1];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X+1, e.Robot.Y));
                }
                else if(e.Direction == Direction.WEST)
                {
                    fieldMap = FieldsMap[e.Robot.Y * _model.Board.Width + e.Robot.X - 1];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X - 1, e.Robot.Y));
                }
                else if(e.Direction == Direction.NORTH)
                {
                    fieldMap = FieldsMap[(e.Robot.Y-1) * _model.Board.Width + e.Robot.X ];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X, e.Robot.Y-1));
                }
                else if(e.Direction == Direction.SOUTH)
                {
                    fieldMap = FieldsMap[(e.Robot.Y+1) * _model.Board.Width + e.Robot.X];
                    fieldMap.SetPicture(_model.Board.GetFieldValue(e.Robot.X, e.Robot.Y+1));
                }
            }
            else if (e.Action == Model.Model.Action.Wait)
            {

            }*/
          //  RefreshTable();
        }
        private void Model_UpdateTasks(object obj, ActionEventArgs e)
        {

        }

        private void OnStartTimer()
        {
            StartTimer?.Invoke(this, EventArgs.Empty);
        }

        private void OnStopTimer()
        {
            StopTimer?.Invoke(this, EventArgs.Empty);
        }



        private void ReFresh() {/*code*/; }
        private void OnLoadGame() { LoadGame?.Invoke(this, EventArgs.Empty); }
        private void OnNewGame() {/*code*/; }
        private void OnSaveGame() { SaveGame?.Invoke(this, EventArgs.Empty); }
        private void OnExitGame() {/*code*/; }
        private void OnExtraTask() {/*code*/; }

        #endregion


    }
}

