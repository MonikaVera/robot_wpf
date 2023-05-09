using Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
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
        private int round=1;
        private int roundTask = 1;
        private string str = "no";
        private int size = 0;
        private string _chatText;
        public string Connect { get { return str; } set { OnPropertyChanged(nameof(str)); } }
        public int Size { get { return size; } set { OnPropertyChanged(nameof(size)); } }
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

        public DelegateCommand ConnectChatButtonCommand { get; private set; }
        public DelegateCommand DisconnectChatButtonCommand { get; private set; }
        public DelegateCommand ExitChatButtonCommand { get; private set; }
        public DelegateCommand FollowChatButtonCommand { get; private set; }
        public DelegateCommand HelloChatButtonCommand { get; private set; }
        public DelegateCommand PraiseChatButtonCommand { get; private set; }
       

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

        public int Health
        {
            get { //return _model.Robot.getHealthAt(_model.Robot.RobotNumber);
                return 1; }
            set
            {
                OnPropertyChanged(nameof(Health));
            }
        }

        public int Number
        {
            get
            { return (_model.Robot.RobotNumber+1);}
            set
            {
                OnPropertyChanged(nameof(Number));
            }
        }

        public string ChatText
        {
            get { return _chatText; }
            set
            {
                if (_chatText != value)
                {
                    _chatText = value;
                    OnPropertyChanged();
                }
            }
        }



        public Game Game { get; set; }

        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }


        public string TaskName
        {
            get { return _model.NoticeBoard.TaskName; }
            set
            {
                OnPropertyChanged(nameof(TaskName));
            }
        }

        public string TaskReward
        {
            get { return _model.NoticeBoard.TaskReward + " points"; }
            set
            {
                OnPropertyChanged(nameof(TaskReward));
            }
        }

        public String TaskDeadline
        {
            get { return _model.NoticeBoard.Deadline+" rounds"; }
            set
            {
                OnPropertyChanged(nameof(TaskDeadline));
            }
        }

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
            ConnectChatButtonCommand = new DelegateCommand(param => SetChat(Convert.ToString(param)));
            DisconnectChatButtonCommand = new DelegateCommand(param => SetChat(Convert.ToString(param)));
            ExitChatButtonCommand = new DelegateCommand(param => SetChat(Convert.ToString(param)));
            FollowChatButtonCommand = new DelegateCommand(param => SetChat(Convert.ToString(param)));
            HelloChatButtonCommand = new DelegateCommand(param => SetChat(Convert.ToString(param)));
            PraiseChatButtonCommand = new DelegateCommand(param => SetChat(Convert.ToString(param)));

        }


        private XYcoordinates? firstCube=null;
        private XYcoordinates? lastCube=null;
        private bool _round=true;
        public void OnClickField(int param)
        {
            if(_round)
            {
                firstCube = new XYcoordinates(param % 7 - 3, param / 7 - 3);
                _round= false;
            }
            else
            {
                lastCube = new XYcoordinates(param % 7 - 3, param / 7 - 3);
                _round = true;
            }
            /*MessageBox.Show(param.ToString() +' '+ (param%7).ToString() + ' ' + (param/7).ToString());*/
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
            _model.SaveGameAsync("file" + 1 + ".txt");
            foreach (ViewModelField field in Fields)
            {
                field.ChooseActionFieldCommand = new DelegateCommand(param => OnClickField(Convert.ToInt32(param)));
            }
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
            if(round < _model.Round)
            {
                ++round;
                _model.SaveGameAsync("file" + (_model.Round ) + ".txt");
            }

            if  ( (_model.NoticeBoard.Deadline + roundTask) == round )
            {
                roundTask = round;
                _model.NoticeBoard.GenerateTasks(3,3);
                for (int j = 0; j < 3; j++)
                    for (int i = 0; i < 3; i++)
                    {
                        VMTasksFields taskfield = FieldsTasks[j * 3 + i];
                        taskfield.SetImage(_model.NoticeBoard.Fields[i, j]);
                    }

            }


            for (int j = _model.Robot.Y - 3; j <= _model.Robot.Y + 3; j++)
            {
                for (int i = _model.Robot.X - 3; i <= _model.Robot.X + 3; i++)
                {
                    if (j >= 0 && i >= 0 && (Math.Abs(j - _model.Robot.Y) + Math.Abs(i - _model.Robot.X)) <= 3
                        && j < _model.Board.Height && i < _model.Board.Width)
                    {
                        ViewModelField field = Fields[y]; //x,y
                        field.SetPicture(_model.Board.GetFieldValue(i, j));

                        //make Connections
                        List<XYcoordinates> connect = _model.Robot.AllConnections();
                        if( connect.Contains(new XYcoordinates(i, j)) )
                        {
                           /* Fields[y].BorderThickness = new Thickness(2.0);
                            Fields[y].BorderBrush = Brushes.Red; */
                            field.BorderThickness = new Thickness(2.0);
                            str = "yes";
                            size = 2;
                        }
                        else
                        {
                            str = "no";
                            size = 0;
                           // Fields[y].BorderThickness = new Thickness(0.0);
                        }
                        
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

            // frissítjük a megszerzett kosarak számát és a játékidőt
            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(Connect));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(TaskDeadline));
            OnPropertyChanged(nameof(TaskName));
            OnPropertyChanged(nameof(TaskReward));
            OnPropertyChanged(nameof(Health));
            OnPropertyChanged(nameof(Number));
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
            MessageBox.Show(number.ToString());
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
                if(firstCube!=null && lastCube!=null)
                {
                    _model.ConnectCubes(_model.Robot, firstCube, lastCube);
                   // _model.ConnectCubes(_model.Robot);
                }
                 
            }
            else if (action == "GET") // rakapcsolodunk egy kockara
            {
              /* if( _model.IsConected(new XYcoordinates(_model.Robot.X+1, _model.Robot.Y)) )
                {
                   // ViewModelField fieldMap = FieldsMap[ (_model.Robot.X + 1) + _model.Robot.Y ]; 
                    //!!!!
                }*/
                _model.ConnectRobot(_model.Robot); 
            }
            else if (action == "PUTDOWN") // lekapcsolodunk egy kockarol
            {
                _model.DisconnectRobot(_model.Robot);
            }
            else if (action == "SPLIT") // szetkapcsolunk kockakat
            {
                if (firstCube != null && lastCube != null)
                {
                    _model.DisconnectCubes(_model.Robot, firstCube, lastCube);
                    //_model.DisconnectCubes(_model.Robot);
                }
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

            if (_model.NextTeam1)
            {
                ChatText = _model.ChatTeam2;
            }
            else
            {
                ChatText = _model.ChatTeam1;
            }

            OnPropertyChanged(nameof(Connect));
            OnPropertyChanged(nameof(Size));

            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(Round));
            OnPropertyChanged(nameof(Team1Points));
            OnPropertyChanged(nameof(Team2Points));
            OnPropertyChanged(nameof(Number));
            OnPropertyChanged(nameof(Health));
            OnStartTimer();
            _canMove = true;
        }

        private void SetChat(string? key)
        {
            if (_model.NextTeam1)
            {
                _model.ChatTeam2 += " Player" + (_model.Robot.RobotNumber - 3) + ": " + key + "\n";
                ChatText = _model.ChatTeam2;
            }
            else
            {
                _model.ChatTeam1 += " Player" + (_model.Robot.RobotNumber + 1) + ": " + key + "\n";
                ChatText = _model.ChatTeam1;
            }
        }

        private void Model_GameAdvanced(object? sender, GameEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(Connect));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(Health));
            OnPropertyChanged(nameof(Number));
        }

        private void Model_NewRound(object? sender, GameEventArgs e)
        {
            if (_canMove)
            {
                return;
            }
            _model.NextPlayer();
            RefreshTable();

            OnPropertyChanged(nameof(Connect));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(Health));
            OnPropertyChanged(nameof(Number));

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

