using System;
using System.IO;
using System.Threading.Tasks;
using Model.Persistence;

namespace Model.Model
{
    public class Game
    {
        public Game(MyDataAccess _dataAccess)
        {
            _board = new Board(10, 12);
            _noticeBoard = new NoticeBoard();
            this._dataAccess = _dataAccess;

        }

        #region Fields

        private Robot _robot;
        private Board _board;
        private NoticeBoard _noticeBoard;
        private int _turns;
        private int _gameTime;
        private IDataAccess _dataAccess;
        private string _filepath;
        private Team _team1;
        private Team _team2;
        private int _round;
        private int _width = 12;
        private int _height = 15;

        #endregion

        #region Properties
        public Robot Robot { get { return _robot; } }

        public Board Board { get { return _board; } }

        public NoticeBoard NoticeBoard { get { return _noticeBoard; } }

        public int Turns { get { return _turns; } set { _turns = value; } }

        public int GameTime { get { return _gameTime; } }

        public bool IsGameOver { get { return false; } } //TODO

        public bool IsRoundOver { get { return _gameTime == 1; } }

        #endregion

        #region Events 

        public event EventHandler<GameEventArgs> GameAdvanced;

        public event EventHandler<GameEventArgs> GameOver;

        public event EventHandler<RobotEventArgs> RobotAction;

        public event EventHandler<GameEventArgs> NewRound;

        /*public event EventHandler<RobotEventArgs> MoveRobot_;
        public event EventHandler<RobotEventArgs> RotateRobot_;
        public event EventHandler<RobotEventArgs> ConnectRobot_;
        public event EventHandler<RobotEventArgs> DisConnectRobot_;
        public event EventHandler<RobotEventArgs> ConnectCubes_;
        public event EventHandler<RobotEventArgs> DisConnectCubes_;
        public event EventHandler<RobotEventArgs> Clean_;*/



        #endregion



        #region  Public Methods

        public void AdvanceTime()
        {
            if (IsGameOver) // ha mr vge, nem folytathatjuk
            {
                OnGameOver(false, _team1, _round);
                return;
            }
            else if (IsRoundOver)
            {
                _round++;
                OnNewRound(false, _team1, _round);
                return;
            }

            _gameTime--;

            OnGameAdvanced();
        }

        public void OnGameAdvanced() {/*code*/ ; }

        public void NewGame()
        {
            _robot = new Robot(1, 1, Direction.EAST);
            _gameTime = 30;
            _round = 1;
            _board = new Board(_width, _height);
            _board.SetValue(1, 1, _robot);

        }
        public async Task LoadGameAsync(string _filepath) {/*code*/ ; }
        public async Task SaveGameAsync(string _filepath) {/*code*/ ; }

        public void MoveRobot(Robot robot, Direction dir)
        {
            if (dir == Direction.EAST)
            {
                MoveRobot(robot.X + 1, robot.Y, dir, robot);
            }
            else if (dir == Direction.WEST)
            {
                MoveRobot(robot.X - 1, robot.Y, dir, robot);
            }
            else if (dir == Direction.NORTH)
            {
                MoveRobot(robot.X, robot.Y - 1, dir, robot);
            }
            else if (dir == Direction.SOUTH)
            {
                MoveRobot(robot.X, robot.Y + 1, dir, robot);
            }

        }
        public bool rotateRobot(Robot robot, Angle angle) { /*code*/ return true; }
        public bool connectRobot(Robot robot, Direction dir) { /*code*/ return true; }
        public bool disConnectRobot(Robot robot, Direction dir) { /*code*/ return true; }
        public bool connectCubes(Robot robot, RelDistance distance) { /*code*/ return true; }
        public bool disConnectCubes(Robot robot, RelDistance distance) { /*code*/ return true; }
        public bool clean(Robot robot, Direction dir) { /*code*/ return true; }

        #endregion

        #region Private Methods

        private void OnGameOver(bool end, Team team, int round)
        {
            if (team == _team1)
            {
                GameOver?.Invoke(this, new GameEventArgs(end, 1, round));
            }
            else
            {
                GameOver?.Invoke(this, new GameEventArgs(end, 2, round));
            }

        }

        private void OnNewRound(bool end, Team team, int round)
        {
            if (team == _team1)
            {
                NewRound?.Invoke(this, new GameEventArgs(end, 1, round));
            }
            else
            {
                NewRound?.Invoke(this, new GameEventArgs(end, 2, round));
            }
        }

        private void MoveRobot(int x, int y, Direction dir, Robot robot)
        {
            if (x < 0 || x >= _board.Width)
                return;
            if (y < 0 || y >= _board.Height)
                return;

            Int32 prevX = robot.X;
            Int32 prevY = robot.Y;

            if (!(prevX == x || prevY == y) || x < prevX - 1 || x > prevX + 1 || y > prevY + 1 || y < prevY - 1)
                return; //ellenrzs, hogy kizrlag vzszintesen vagy fgglegesen lp

            if (_board.GetFieldValue(x, y) is Obstacle || _board.GetFieldValue(x, y) is Cube || _board.GetFieldValue(x, y) is Exit) // ha a mezn akadly van, nem lphetnk
                return;



            //mezk State-jeinek frisstse a lpsnek megfelelen
            robot.SetXY(x, y);
            _board.SetValue(prevX, prevY, new Empty(prevX, prevY));
            _board.SetValue(x, y, robot);

            //OnRobotAction(prevX, prevY, x, y);

            OnGameAdvanced();


        }
        private void RotateRobot(Direction dir) { /*code*/ ; }
        private void ConnectRobot(Direction dir) { /*code*/ ; }
        private void DisConnectRobot(Direction dir) { /*code*/ ; }
        private void ConnectCubes(int x, int y) { /*code*/ ; }
        private void DisConnectCubes(int x, int y) { /*code*/ ; }
        private void Clean(int x, int y) { /*code*/ ; }


        #endregion








    }

    public class GameEventArgs : EventArgs
    {
        public GameEventArgs(bool isGameOver, int winnerTeam, int currentRound)
        {
            _isGameOver = isGameOver;
            _winnerTeam = winnerTeam;
            _currentRound = currentRound;
        }
        private bool _isGameOver;
        private int _winnerTeam;
        private int _currentRound;
        public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }
        public int WinnerTeam { get { return _winnerTeam; } set { _winnerTeam = value; } }
        public int CurrentRound { get { return _currentRound; } set { _currentRound = value; } }
    }


}
