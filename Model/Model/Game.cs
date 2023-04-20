using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Documents;
using Model.Persistence;
using System.Linq;
using System.Diagnostics.Eventing.Reader;

namespace Model.Model
{
    public class Game
    {
        public Game(MyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _gameOverTurn = 50;
            _width = 10;
            _height = 12;
            //NewGame();
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
        private int _team1points;
        private int _team2points;

        #endregion

        #region Properties
        public Robot Robot { get { return _robot; } }

        public Board Board { get { return _board; } }

        public NoticeBoard NoticeBoard { get { return _noticeBoard; } }

        public int Round { get { return _round; } set { _round = value; } }

        public int Team1Points { get { return _team1points; } set { _team1points = value; } }
        public int Team2Points { get { return _team2points; } set { _team2points = value; } }

        public int GameTime { get { return _gameTime; } }

        public bool IsGameOver { get { return _round == _gameOverTurn; } } 

        public bool IsRoundOver { get { return _gameTime == 0; } }

        #endregion

        #region Events 

        public event EventHandler<GameEventArgs> GameAdvanced;

        public event EventHandler<GameEventArgs> GameOver;

        public event EventHandler<RobotEventArgs> RobotAction;

        public event EventHandler<GameEventArgs> NewRound;

        public event EventHandler<ActionEventArgs> UpdateFields;

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
           
            if (IsRoundOver)
            {
                OnNewRound(_team1);
                return;
            }

            _gameTime--;
            OnGameAdvanced();

        }

        public void NewGame()
        {
            _board = new Board(_width, _height);
            _robot = new Robot(1, 1, Direction.EAST);
            _board.SetValue(1, 1, _robot);
            _noticeBoard = new NoticeBoard();
            _gameTime = 30;
            _round = 1;
            _team1points = 0;
            _team2points = 0;
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
                GameOver?.Invoke(this, new GameEventArgs(end, 1, _round, _gameTime, _team1points, _team2points));
            }
            else
            {
                GameOver?.Invoke(this, new GameEventArgs(end, 2, _round, _gameTime, _team1points, _team2points));
            }

        }

        private void OnNewRound(Team team)
        {
            _round++;
            _gameTime = 30;
            if (team == _team1)
            {
                NewRound?.Invoke(this, new GameEventArgs(false, 1, _round, _gameTime, _team1points, _team2points));
            }
            else
            {
                NewRound?.Invoke(this, new GameEventArgs(false, 2, _round, _gameTime, _team1points, _team2points));
            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this,new GameEventArgs(false, 1, _round, _gameTime, _team1points, _team2points));
        }

        private void OnUpdateFields(Robot robot, Direction direction, Action action, bool canExecute)
        {
            UpdateFields?.Invoke(this, new ActionEventArgs(robot,direction,action,canExecute));
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
                    robot.X++;
                    _board.SetValue(robot.X, robot.Y, robot);
                   // robot.Direction = Direction.EAST;
                    OnUpdateFields(robot, Direction.EAST, Action.Move, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.EAST, Action.Move, false);
                }
            }
            else if (dir == Direction.WEST)
            {
                if (CanMoveToWest(robot))
                {
                    MoveToWest(robot);
                    robot.X--;
                    _board.SetValue(robot.X, robot.Y, robot);
                    //robot.Direction = Direction.WEST;
                    OnUpdateFields(robot, Direction.WEST, Action.Move, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.WEST, Action.Move, false);
                }
            }
            else if (dir == Direction.NORTH)
            {
                if (CanMoveToNorth(robot))
                {
                    MoveToNorth(robot);
                    robot.Y--;
                    _board.SetValue(robot.X, robot.Y, robot);
                    //robot.Direction = Direction.NORTH;
                    OnUpdateFields(robot, Direction.NORTH, Action.Move, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.NORTH, Action.Move, false);
                }
            }
            else if (dir == Direction.SOUTH)
            {
                if (CanMoveToSouth(robot))
                {
                    MoveToSouth(robot);
                    robot.Y++;
                    _board.SetValue(robot.X, robot.Y, robot);
                    OnUpdateFields(robot, Direction.SOUTH, Action.Move, true);
                    //robot.Direction = Direction.SOUTH;
                }
                else
                {
                    OnUpdateFields(robot, Direction.SOUTH, Action.Move, false);
                }
            }
            if (_round == _gameOverTurn)
            {
                OnGameOver(true, _team1);
                return;
            }
               

            OnNewRound(_team1);
        }

        private bool CanMoveToEast(Robot robot)
        {
            if ((robot.X + 1) >= _board.Width
                    || !((_board.GetFieldValue(robot.X + 1, robot.Y) is Empty)
                    || ((_board.GetFieldValue(robot.X + 1, robot.Y) is Cube) && 
                    robot.IsConnected(new XYcoordinates(robot.X+1, robot.Y)))))
            {
                return false;
            }

            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if ((connections[i].X + 1) >= _board.Width
                    || !((_board.GetFieldValue(connections[i].X + 1, connections[i].Y) is Empty)
                    || ((_board.GetFieldValue(connections[i].X + 1, connections[i].Y) is Robot)
                    && (connections[i].X + 1==robot.X && connections[i].Y==robot.Y))
                    || ((_board.GetFieldValue(connections[i].X + 1, connections[i].Y) is Cube) &&
                    robot.IsConnected(new XYcoordinates(connections[i].X + 1, connections[i].Y)))
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        private bool CanMoveToWest(Robot robot)
        {
            if ((robot.X - 1) < 0
                    || !((_board.GetFieldValue(robot.X - 1, robot.Y) is Empty)
                    || ((_board.GetFieldValue(robot.X - 1, robot.Y) is Cube) &&
                    robot.IsConnected(new XYcoordinates(robot.X - 1, robot.Y)))))
            {
                return false;
            }

            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if ((connections[i].X - 1) < 0
                    || !((_board.GetFieldValue(connections[i].X - 1, connections[i].Y) is Empty)
                    || ((_board.GetFieldValue(connections[i].X - 1, connections[i].Y) is Robot)
                    && (connections[i].X - 1 == robot.X && connections[i].Y == robot.Y))
                    || ((_board.GetFieldValue(connections[i].X - 1, connections[i].Y) is Cube) &&
                    robot.IsConnected(new XYcoordinates(connections[i].X - 1, connections[i].Y)))
                    ))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanMoveToNorth(Robot robot)
        {
            if ((robot.Y - 1) < 0
                    || !((_board.GetFieldValue(robot.X, robot.Y - 1) is Empty)
                    || ((_board.GetFieldValue(robot.X, robot.Y-1) is Cube) &&
                    robot.IsConnected(new XYcoordinates(robot.X, robot.Y-1)))))
            {
                return false;
            }

            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if ((connections[i].Y - 1) < 0
                    || !((_board.GetFieldValue(connections[i].X, connections[i].Y - 1) is Empty)
                    || ((_board.GetFieldValue(connections[i].X, connections[i].Y-1) is Robot)
                    && (connections[i].X == robot.X && connections[i].Y - 1 == robot.Y))
                    || ((_board.GetFieldValue(connections[i].X, connections[i].Y - 1) is Cube) &&
                    robot.IsConnected(new XYcoordinates(connections[i].X, connections[i].Y - 1)))
                    ))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanMoveToSouth(Robot robot)
        {
            if (((robot.Y + 1) >= _board.Height)
                    || !((_board.GetFieldValue(robot.X, robot.Y+1) is Empty)
                    || ((_board.GetFieldValue(robot.X, robot.Y+1) is Cube) &&
                    robot.IsConnected(new XYcoordinates(robot.X, robot.Y+1)))))
            {
                return false;
            }

            _board.SetValue(robot.X, robot.Y, new Empty(robot.X, robot.Y));
            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if ((connections[i].Y + 1) >= _board.Height
                    || !((_board.GetFieldValue(connections[i].X, connections[i].Y + 1) is Empty)
                    || ((_board.GetFieldValue(connections[i].X, connections[i].Y+1) is Robot)
                    && (connections[i].X == robot.X && connections[i].Y + 1 == robot.Y))
                    || ((_board.GetFieldValue(connections[i].X, connections[i].Y + 1) is Cube) &&
                    robot.IsConnected(new XYcoordinates(connections[i].X, connections[i].Y + 1)))
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        private void MoveToEast(Robot robot)
        {
            _board.SetValue(robot.X, robot.Y, new Empty(robot.X, robot.Y));
            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }
            robot.ToEast();
            List<XYcoordinates> connectionsNew = robot.AllConnections();
            _board.SetValue(robot.X + 1, robot.Y, robot);
            for (int i = 0; i < connectionsNew.Count; i++)
            {
                _board.SetValue(connectionsNew[i].X, connectionsNew[i].Y, new Cube(connectionsNew[i].X, connectionsNew[i].Y, 1, Color.RED));
            }
        }

        private void MoveToWest(Robot robot)
        {
            _board.SetValue(robot.X, robot.Y, new Empty(robot.X, robot.Y));
            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }
            robot.ToWest();
            List<XYcoordinates> connectionsNew = robot.AllConnections();
            _board.SetValue(robot.X - 1, robot.Y, robot);
            for (int i = 0; i < connectionsNew.Count; i++)
            {
                _board.SetValue(connectionsNew[i].X, connectionsNew[i].Y, new Cube(connectionsNew[i].X, connectionsNew[i].Y, 1, Color.RED));
            }
        }

        private void MoveToNorth(Robot robot)
        {
            _board.SetValue(robot.X, robot.Y, new Empty(robot.X, robot.Y));
            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }
            robot.ToNorth();
            List<XYcoordinates> connectionsNew = robot.AllConnections();
            _board.SetValue(robot.X, robot.Y - 1, robot);
            for (int i = 0; i < connectionsNew.Count; i++)
            {
                _board.SetValue(connectionsNew[i].X, connectionsNew[i].Y, new Cube(connectionsNew[i].X, connectionsNew[i].Y, 1, Color.RED));
            }
        }

        private void MoveToSouth(Robot robot)
        {
            _board.SetValue(robot.X, robot.Y, new Empty(robot.X, robot.Y)); 
            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }
            robot.ToSouth();
            List<XYcoordinates> connectionsNew = robot.AllConnections();
            _board.SetValue(robot.X, robot.Y + 1, robot);
            for (int i = 0; i < connectionsNew.Count; i++)
            {
                _board.SetValue(connectionsNew[i].X, connectionsNew[i].Y, new Cube(connectionsNew[i].X, connectionsNew[i].Y, 1, Color.RED));
            }
        }


        #endregion

        #region Rotate

        public void RotateRobot(Robot robot, Angle angle) {
            if ((angle == Angle.Clockwise && CanRotateClockwise(robot))
                || (angle == Angle.CounterClockwise && CanRotateCounterClockwise(robot)))
            {
                RotateAll(robot, angle);
                if(angle==Angle.Clockwise)
                {
                    OnUpdateFields(robot, Direction.EAST, Action.Turn, true);
                } 
                else
                {
                    OnUpdateFields(robot, Direction.WEST, Action.Turn, true);
                }
            }
        }

        private void RotateAll(Robot robot, Angle angle)
        {
            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                _board.SetValue(connections[i].X, connections[i].Y, new Empty(connections[i].X, connections[i].Y));
            }

            if(angle==Angle.Clockwise)
            {   
                robot.RotateClockwise();
                switch(robot.Direction)
                {
                    case Direction.EAST: robot.Direction = Direction.SOUTH;
                            break;
                    case Direction.SOUTH: robot.Direction = Direction.WEST;
                        break;
                    case Direction.WEST: robot.Direction = Direction.NORTH;
                        break;
                    case Direction.NORTH: robot.Direction = Direction.EAST;
                        break;
                }
            } 
            else
            {
                robot.RotateCounterClockwise();
                switch (robot.Direction)
                {
                    case Direction.EAST:
                        robot.Direction = Direction.NORTH;
                        break;
                    case Direction.SOUTH:
                        robot.Direction = Direction.EAST;
                        break;
                    case Direction.WEST:
                        robot.Direction = Direction.SOUTH;
                        break;
                    case Direction.NORTH:
                        robot.Direction = Direction.WEST;
                        break;
                }
            }
            
            List<XYcoordinates> connectionsNew = robot.AllConnections();

            for (int i = 0; i < connectionsNew.Count; i++)
            {
                _board.SetValue(connectionsNew[i].X, connectionsNew[i].Y, new Cube(connectionsNew[i].X, connectionsNew[i].Y, 1, Color.RED));
            }
        }

        private bool CanRotateClockwise(Robot robot)
        {
            for (int i = 0; i < (robot.AllConnections()).Count(); i++)
            {
                int newX = robot.X + robot.Y - (robot.AllConnections())[i].Y;
                int newY = -robot.X + robot.Y + (robot.AllConnections())[i].X;
                if(newX<0 || newY<0 || newX>=_board.Width || newY>=_board.Height || 
                    !(_board.GetFieldValue(newX,newY) is Empty))
                {
                    return false;
                }

            }
            return true;
        }

        private bool CanRotateCounterClockwise(Robot robot)
        {
            for (int i = 0; i < (robot.AllConnections()).Count(); i++)
            {
                int newX = robot.X - robot.Y + (robot.AllConnections())[i].Y;
                int newY = robot.X + robot.Y - (robot.AllConnections())[i].X;
                if (newX < 0 || newY < 0 || newX >= _board.Width || newY >= _board.Height ||
                    !(_board.GetFieldValue(newX, newY) is Empty))
                {
                    return false;
                }

            }
            return true;
        }

    #endregion

        #region ConnectRobot
        public void ConnectRobot(Robot robot)
        {
            _actionDirection = robot.Direction;
            if (_actionDirection == null)
            {
                return;
            }

            if (_actionDirection == Direction.EAST)
            {
                int x = robot.X + 1;
                while (robot.IsConnected(new XYcoordinates(x, robot.Y)))
                {
                    x = x + 1;
                }
                if (x < _board.Width && _board.GetFieldValue(x, robot.Y) is Cube)
                {
                    robot.AddConnection(new XYcoordinates(x, robot.Y));
                    OnUpdateFields(robot, Direction.EAST, Action.ConnectRobot, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.EAST, Action.ConnectRobot, false);
                }
            }
            else if (_actionDirection == Direction.WEST)
            {
                int x = robot.X - 1;
                while (robot.IsConnected(new XYcoordinates(x, robot.Y)))
                {
                    x = x - 1;
                }
                if (x >= 0 && _board.GetFieldValue(x, robot.Y) is Cube)
                {
                    robot.AddConnection(new XYcoordinates(x, robot.Y));
                    OnUpdateFields(robot, Direction.WEST, Action.ConnectRobot, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.WEST, Action.ConnectRobot, false);
                }
            }
            else if (_actionDirection == Direction.NORTH)
            {
                int y = robot.Y - 1;
                while (robot.IsConnected(new XYcoordinates(robot.X, y)))
                {
                    y = y - 1;
                }
                if (y >= 0 && _board.GetFieldValue(robot.X, y) is Cube)
                {
                    robot.AddConnection(new XYcoordinates(robot.X, y));
                    OnUpdateFields(robot, Direction.NORTH, Action.ConnectRobot, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.NORTH, Action.ConnectRobot, false);
                }
            }
            else if (_actionDirection == Direction.SOUTH)
            {
                int y = robot.Y + 1;
                while (robot.IsConnected(new XYcoordinates(robot.X, y)))
                {
                    y = y + 1;
                }
                if (y < _board.Height && _board.GetFieldValue(robot.X, y) is Cube)
                {
                    robot.AddConnection(new XYcoordinates(robot.X, y));
                    OnUpdateFields(robot, Direction.SOUTH, Action.ConnectRobot, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.SOUTH, Action.ConnectRobot, false);
                }
            }

            if (_round == _gameOverTurn)
            {
                OnGameOver(true, _team1);
                return;
            }
               

            OnNewRound(_team1);

        }
        #endregion

        #region DisconnectRobot

        public void DisconnectRobot(Robot robot) {
            Robot.clearConnections();
        }

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
            public GameEventArgs(bool isGameOver, int winnerTeam, int currentRound, int gameTime, int team1points, int team2points)
            {
                _isGameOver = isGameOver;
                _winnerTeam = winnerTeam;
                _currentRound = currentRound;
                 _gameTime = gameTime;
                _team1points = team1points;
                _team2points = team2points;
        }
            private bool _isGameOver;
            private int _winnerTeam;
            private int _currentRound;
            private int _gameTime;
            private int _team1points;
            private int _team2points;

        public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }
            public int WinnerTeam { get { return _winnerTeam; } set { _winnerTeam = value; } }
            public int CurrentRound { get { return _currentRound; } set { _currentRound = value; } }
            public int GameTime { get { return _gameTime; } set { _gameTime = value; } }
            public int Team1Points { get { return _team1points; } set { _team1points = value; } }
            public int Team2Points { get { return _team2points; } set { _team2points = value; } }


    }


    
}
