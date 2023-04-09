using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Documents;
using Model.Persistence;

namespace Model.Model
{
    public class Game
    {
        public Game(MyDataAccess _dataAccess)
        {
            _width = 10;
            _height = 12;
            _board = new Board(_width, _height);
            _robot = new Robot(1, 1, Direction.EAST);
            _board.SetValue(1, 1, _robot);
            _noticeBoard = new NoticeBoard();
            this._dataAccess = _dataAccess;
            _round = 1;
            _gameOverTurn = 50;
        }

        #region Fields

        private Robot _robot;
        private Board _board;
        private NoticeBoard _noticeBoard;
        private int _gameOverTurn;
        private int _gameTime;
        private IDataAccess _dataAccess;
        private string _filepath;
        private Team _team1;
        private Team _team2;
        private int _round;
        private int _width;
        private int _height;
        private int? _actionFieldX;
        private int? _actionFieldY;
        private Direction? _actionDirection;

        #endregion

        #region Properties
        public Robot Robot { get { return _robot; } }

        public Board Board { get { return _board; } }

        public NoticeBoard NoticeBoard { get { return _noticeBoard; } }

        public int Round { get { return _round; } set { _round = value; } }

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

        #region  Public  Methods

        public void AdvanceTime()
        {
            if (IsGameOver) // ha mr vge, nem folytathatjuk
            {
                OnGameOver(false, _team1);
                return;
            }
            else if (IsRoundOver)
            {
                _round++;
                OnNewRound(false, _team1);
                return;
            }

            _gameTime--;

        }

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

        public void ChooseActionField (int x, int y)
        {
            _actionFieldX = x;
            _actionFieldY = y;
            CalculateDirection();
        }


        #endregion

        #region Private Methods

        private Direction? CalculateDirection() 
        {
            if (_actionFieldX == null || _actionFieldY == null)
                return null;

            if (_robot.X == _actionFieldX)
            {
                if (_robot.Y == _actionFieldY - 1)
                {
                    return Direction.NORTH;
                }
                else if (_robot.Y == _actionFieldY + 1)
                {
                    return Direction.SOUTH;
                }
                else { return null; }
            }
            else if (_robot.Y == _actionFieldY)
            {
                if (_robot.X == _actionFieldX - 1)
                {
                    return Direction.WEST;
                }
                else if (_robot.X == _actionFieldX + 1)
                {
                    return Direction.EAST;
                }
                else { return null; }
            }
            else
            {
                return null;
            }
        }

        private void OnGameOver(bool end, Team team)
        {
            if (team == _team1)
            {
                GameOver?.Invoke(this, new GameEventArgs(end, 1, _round));
            }
            else
            {
                GameOver?.Invoke(this, new GameEventArgs(end, 2, _round));
            }

        }

        private void OnNewRound(bool end, Team team)
        {
            _round++;
            if (team == _team1)
            {
                NewRound?.Invoke(this, new GameEventArgs(end, 1, _round));
            }
            else
            {
                NewRound?.Invoke(this, new GameEventArgs(end, 2, _round));
            }
        }

        /*private void MoveRobot_(int x, int y, Direction dir, Robot robot)
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


        }*/
        private void RotateRobot(Direction dir) { /*code*/ ; }
        private void DisConnectRobot(Direction dir) { /*code*/ ; }
        private void ConnectCubes(int x, int y) { /*code*/ ; }
        private void DisConnectCubes(int x, int y) { /*code*/ ; }
        private void Clean(int x, int y) { /*code*/ ; }


        #endregion

        #region Move
        public void MoveRobot(Robot robot, Direction dir)
        {
            if (dir == Direction.EAST)
            {
                if (CanMoveToEast(robot))
                {
                    MoveToEast(robot);
                }
            }
            else if (dir == Direction.WEST)
            {
                if (CanMoveToWest(robot))
                {
                    MoveToWest(robot);
                }
            }
            else if (dir == Direction.NORTH)
            {
                if (CanMoveToNorth(robot))
                {
                    MoveToNorth(robot);
                }
            }
            else if (dir == Direction.SOUTH)
            {
                if (CanMoveToSouth(robot))
                {
                    MoveToSouth(robot);
                }
            }
            if (_round == _gameOverTurn)
                OnGameOver(true, _team1);

            OnNewRound(false, _team1);
        }

        private bool CanMoveToEast(Robot robot)
        {
            List<MyTuple> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].X + 1 >= _board.Width
                    && !(_board.GetFieldValue(connections[i].X + 1, connections[i].Y) is Empty))
                {
                    return false;
                }
            }
            return true;
        }
        private bool CanMoveToWest(Robot robot)
        {
            List<MyTuple> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].X - 1 < 0
                    && !(_board.GetFieldValue(connections[i].X - 1, connections[i].Y) is Empty))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanMoveToNorth(Robot robot)
        {
            List<MyTuple> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].Y - 1 < 0
                    && !(_board.GetFieldValue(connections[i].X, connections[i].Y - 1) is Empty))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanMoveToSouth(Robot robot)
        {
            List<MyTuple> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].Y + 1 >= _board.Height
                    && !(_board.GetFieldValue(connections[i].X, connections[i].Y + 1) is Empty))
                {
                    return false;
                }
            }
            return true;
        }
        private void MoveToEast(Robot robot)
        {
            List<MyTuple> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }
            robot.ToEast();
            List<MyTuple> connectionsNew = robot.AllConnections();
            _board.SetValue(robot.X + 1, robot.Y, new Robot(robot.X + 1, robot.Y, robot.Direction, connectionsNew));
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Cube(connections[i].X, connections[i].Y, 1, Color.RED));
            }
        }

        private void MoveToWest(Robot robot)
        {
            List<MyTuple> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }
            robot.ToWest();
            List<MyTuple> connectionsNew = robot.AllConnections();
            _board.SetValue(robot.X - 1, robot.Y, new Robot(robot.X - 1, robot.Y, robot.Direction, connectionsNew));
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Cube(connections[i].X, connections[i].Y, 1, Color.RED));
            }
        }

        private void MoveToNorth(Robot robot)
        {
            List<MyTuple> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }
            robot.ToNorth();
            List<MyTuple> connectionsNew = robot.AllConnections();
            _board.SetValue(robot.X, robot.Y - 1, new Robot(robot.X, robot.Y - 1, robot.Direction, connectionsNew));
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Cube(connections[i].X, connections[i].Y, 1, Color.RED));
            }
        }

        private void MoveToSouth(Robot robot)
        {
            List<MyTuple> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }
            robot.ToSouth();
            List<MyTuple> connectionsNew = robot.AllConnections();
            _board.SetValue(robot.X, robot.Y + 1, new Robot(robot.X, robot.Y + 1, robot.Direction, connectionsNew));
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Cube(connections[i].X, connections[i].Y, 1, Color.RED));
            }
        }


        #endregion

        #region Rotate

        public void RotateRobot(Robot robot, Angle angle) { /*code*/ }

        #endregion

        #region ConnectRobot
        public void ConnectRobot(Robot robot)
        {
            if (_actionDirection == null)
            {
                return;
            }

            if (_actionDirection == Direction.EAST)
            {
                int x = robot.X + 1;
                while (robot.IsConnected(new MyTuple(x, robot.Y)))
                {
                    x = x + 1;
                }
                if (x < _board.Width && _board.GetFieldValue(x, robot.Y) is Cube)
                {
                    robot.AddConnection(new MyTuple(x, robot.Y));
                }
            }
            else if (_actionDirection == Direction.WEST)
            {
                int x = robot.X - 1;
                while (robot.IsConnected(new MyTuple(x, robot.Y)))
                {
                    x = x - 1;
                }
                if (x >= 0 && _board.GetFieldValue(x, robot.Y) is Cube)
                {
                    robot.AddConnection(new MyTuple(x, robot.Y));
                }
            }
            else if (_actionDirection == Direction.NORTH)
            {
                int y = robot.Y - 1;
                while (robot.IsConnected(new MyTuple(robot.X, y)))
                {
                    y = y - 1;
                }
                if (y >= 0 && _board.GetFieldValue(robot.X, y) is Cube)
                {
                    robot.AddConnection(new MyTuple(robot.X, y));
                }
            }
            else if (_actionDirection == Direction.SOUTH)
            {
                int y = robot.Y + 1;
                while (robot.IsConnected(new MyTuple(robot.X, y)))
                {
                    y = y + 1;
                }
                if (y < _board.Height && _board.GetFieldValue(robot.X, y) is Cube)
                {
                    robot.AddConnection(new MyTuple(robot.X, y));
                }
            }

            if (_round == _gameOverTurn)
                OnGameOver(true, _team1);

            OnNewRound(false, _team1);

        }
        #endregion

        #region DisconnectRobot

        public void DisconnectRobot(Robot robot) { /*code*/ }

        #endregion

        #region ConnectCubes

        public void ConnectCubes(Robot robot) { /*code*/ }

        #endregion

        #region DisconnectCubes

        public void DisconnectCubes(Robot robot) { /*code*/ }

        #endregion

        #region Clean

        public void Clean(Robot robot) { /*code*/ }

        #endregion

        #region Wait

        public void Wait(Robot robot) { /*code*/}

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
