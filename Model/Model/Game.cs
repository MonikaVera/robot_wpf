using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Model.Persistence;
using System.Linq;
using System.Diagnostics.Eventing.Reader;
using static System.Net.Mime.MediaTypeNames;
using System.Windows;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace Model.Model
{
    /// <summary>
    /// Robots Game type.
    /// </summary>
    public class Game
    {

        #region Fields

        private Robot _robot = null!; // the current robot on the board
        private Board _board = null!; // the board of the game
        private NoticeBoard _noticeBoard = null!; // the notice board which contains the guests
        private int _gameOverTurn; // the number of turns when the game ends
        private int _gameTime; // the amount of time in a round
        private IDataAccess _dataAccess; // for load and save methods
        private Team _team1 = null!; // the first team on the board
        private Team _team2 = null!; // the second team on the board
        private int _round; // the current round
        private int _width; // the width of the board
        private int _height; // the height of the board
        private int? _actionFieldX; // the x coordinate of the action
        private int? _actionFieldY; // the y coordinate of the action
        private Direction? _actionDirection; // the direction of the action
        private int _team1points; // the first team's points
        private int _team2points; // the second team's points
        private int _teamMembers; // the number of robots in a team
        private int _nextPlayerFromTeam1; // the next number of robot in first team
        private int _nextPlayerFromTeam2; // the next number of robot in second team
        private bool _nextTeam1; // whether the next team is the first or not
        private string _chatTeam1 = null!; // the content of the first team's chat
        private string _chatTeam2 = null!; // the content of the second team's chat
        private List<Board> _robotsMap = new List<Board>(); // the map of each robot with the fields they can view

        #endregion

        #region Properties

        /// <summary>
        /// Query or setting of the robot on the board.
        /// </summary>
        public Robot Robot { get { return _robot; } set { _robot = value; } }

        /// <summary>
        /// Query or setting of the board.
        /// </summary>
        public Board Board { get { return _board; } set { _board = value; } }

        /// <summary>
        /// Query or setting of the first team.
        /// </summary>
        public Team Team1 { get { return _team1; } set { _team1 = value; } }

        /// <summary>
        /// Query or setting of the second team.
        /// </summary>
        public Team Team2 { get { return _team2; } set { _team2 = value; } }

        /// <summary>
        /// Query or setting of the notice board.
        /// </summary>
        public NoticeBoard NoticeBoard { get { return _noticeBoard; } }

        /// <summary>
        /// Query or setting of the current round.
        /// </summary>
        public int Round { get { return _round; } set { _round = value; } }

        /// <summary>
        /// Query of the robots map with the fields they can currently see.
        /// </summary>
        public List<Board> RobotsMap { get { return _robotsMap; } }

        /// <summary>
        /// Query or setting of the first team points.
        /// </summary>
        public int Team1Points { get { return _team1points; } set { _team1points = value; } }

        /// <summary>
        /// Query or setting of the second team points.
        /// </summary>
        public int Team2Points { get { return _team2points; } set { _team2points = value; } }

        /// <summary>
        /// Query or setting of amount of time in a round.
        /// </summary>
        public int GameTime { get { return _gameTime; } }

        /// <summary>
        /// Query of whether it's game over.
        /// </summary>
        public bool IsGameOver { get { return _round == _gameOverTurn; } }

        /// <summary>
        /// Query of whether it's round over.
        /// </summary>
        public bool IsRoundOver { get { return _gameTime == 0; } }

        /// <summary>
        /// Query of which robot is the next from the first team.
        /// </summary>
        public int NextPlayerFromTeam1 { get { return _nextPlayerFromTeam1; } }

        /// <summary>
        /// Query of which robot is the next from the second team.
        /// </summary>
        public int NextPlayerFromTeam2 { get { return _nextPlayerFromTeam2; } }

        /// <summary>
        /// Query of whether the next team is the first team.
        /// </summary>
        public bool NextTeam1 { get { return _nextTeam1; } }

        /// <summary>
        /// Query or setting of the content of the first team's chat.
        /// </summary>
        public string ChatTeam1 { get { return _chatTeam1; } set { _chatTeam1 = value; } }

        /// <summary>
        /// Query or setting of the content of the second team's chat.
        /// </summary>
        public string ChatTeam2 { get { return _chatTeam2; } set { _chatTeam2 = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the Game class.
        /// </summary>
        /// <param name="dataAccess">The data access for the Load and Save methods.</param>
        public Game(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _gameOverTurn = 50;
            _width = 10;
            _height = 12;
        }

        #endregion 

        #region Events 

        /// <summary>
        /// The event of whether the game continues.
        /// </summary>
        public event EventHandler<GameEventArgs>? GameAdvanced;

        /// <summary>
        /// The event of game over.
        /// </summary>
        public event EventHandler<GameEventArgs>? GameOver;

        /// <summary>
        /// The event of starting a new round.
        /// </summary>
        public event EventHandler<GameEventArgs>? NewRound;

        /// <summary>
        /// The event of updating the fields.
        /// </summary>
        public event EventHandler<ActionEventArgs>? UpdateFields;

        #endregion

        #region  Public  Methods

        /// <summary>
        /// Advances time else when time is 0, starts a new round.
        /// </summary>
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

        /// <summary>
        /// Starts a new game.
        /// </summary>
        public void NewGame()
        {
            _teamMembers = 4;
            _board = new Board(_width, _height);
            _team1 = new Team(CreateTeam(_teamMembers, 0), _teamMembers, 0);
            _team2 = new Team(CreateTeam(_teamMembers, 1), _teamMembers, 1);
            _robot = _team1.GetRobot(0);
            _noticeBoard = new NoticeBoard();
            _gameTime = 30;
            _round = 1;
            _team1points = 0;
            _team2points = 0;
            _nextPlayerFromTeam1 = 1;
            _nextPlayerFromTeam2 = 0;
            _chatTeam1 = "";
            _chatTeam2 = "";
            _nextTeam1 = true;
        }

        /// <summary>
        /// Loads a game.
        /// </summary>
        /// <param name="filepath">The relative path to the file from where we want to load a game.</param>
        public void LoadGame(string filepath)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            Board board = _dataAccess.Load(filepath, _board.Height, _board.Width);
            _board = board;
        }

        /// <summary>
        /// Saves a game.
        /// </summary>
        /// <param name="filepath">The relative path to the file where we want to save the game.</param>
        public async Task SaveGameAsync(string filepath)
        {
            if (_dataAccess == null)
                return;

            await _dataAccess.SaveAsync(filepath, _board);

        }

        /// <summary>
        /// Calculates the field for action.
        /// </summary>
        /// <param name="x">The X coordinate of the field on which we want to execute an action.</param>
        /// <param name="y">The Y coordinate of the field on which we want to execute an action.</param>
        public void ChooseActionField(int x, int y)
        {
            _actionFieldX = x;
            _actionFieldY = y;
            CalculateDirection();
        }

        /// <summary>
        /// Switches to the next player.
        /// </summary>
        public void NextPlayer()
        {
            _robot = NextRobot();

            if (_nextPlayerFromTeam1 == 0 && _nextPlayerFromTeam2 == 0 && _nextTeam1)
            {
                _round++;
            }

            NextTeam();
            _gameTime = 30;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="number">The number of robots we want to create the team with.</param>
        /// <param name="teamNumber">The identifier of the team.</param>
        /// <returns>The created array of robots.</returns>
        private Robot[] CreateTeam(int number, int teamNumber)
        {
            Robot[] robots = new Robot[number];
            for (int i = 0; i < number; i++)
            {
                Robot robot2 = RandomRobot(i + teamNumber * number);
                robots[i] = robot2;
            }

            return robots;
        }

        /// <summary>
        /// Creates a new robot to a random field.
        /// </summary>
        /// <param name="i">The identifier of the robot we want to create.</param>
        /// <returns>The created robot.</returns>
        private Robot RandomRobot(int i)
        {
            Random rnd = new Random();
            int x;
            int y;
            do
            {
                x = rnd.Next(1, _board.Width - 1);
                y = rnd.Next(1, _board.Height - 1);
            }
            while (!(_board.GetFieldValue(x, y) is Empty)); //until the chosen field is not empty

            Direction direction = (Direction)rnd.Next(0, 4);
            Robot robot = new Robot(x, y, direction, i);
            _board.SetValue(x, y, robot);


            for (int a = 0; a < i; a++)
                if (_robotsMap[a].GetFieldValue(x, y) is not None)
                {
                    _robotsMap[a].SetValue(x, y, robot); //initialization of the robot's map

                }

            createMap(x, y); //creation of the map

            return robot;
        }

        /// <summary>
        /// Creates the manhattan map for a robot.
        /// </summary>
        /// <param name="x">The X coordinate of the robot we want to create the map</param>
        /// <param name="y">The Y coordinate of the robot we want to create the map</param>
        private void createMap(int x, int y)
        {
            //robot map
            Board robotsMap = new Board(_board.Width, _board.Height);
            for (int j = 0; j < _board.Height; j++)
                for (int l = 0; l < _board.Width; l++)
                {
                    robotsMap.SetValue(l, j, new None());
                }

            for (int j = y - 3; j <= y + 3; j++)
            {
                for (int l = x - 3; l <= x + 3; l++)
                {
                    if (j >= 0 && l >= 0 && (Math.Abs(j - y) + Math.Abs(l - x)) <= 3
                        && j < _board.Height && l < _board.Width)
                    {
                        robotsMap.SetValue(l, j, _board.GetFieldValue(l, j));
                    }
                }
            }
            _robotsMap.Add(robotsMap);
        }

        /// <summary>
        /// Calculates the direction of the current action.
        /// </summary>
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

        /// <summary>
        /// Triggers the GameOver event.
        /// </summary>
        /// <param name="end">Whether the game is over.</param>
        /// <param name="team">The team who won.</param>
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

        /// <summary>
        /// Updates the game time and the round, also ptentially triggers the NewRound event.
        /// </summary>
        /// <param name="team">The team which made the last move.</param>
        private void OnNewRound(Team team)
        {
            if (_nextPlayerFromTeam1 == 0 && _nextPlayerFromTeam2 == 0)
            {
                _round++;
            }

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

        /// <summary>
        /// Triggers the GameAdvanced event.
        /// </summary>
        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new GameEventArgs(false, 1, _round, _gameTime, _team1points, _team2points));
        }

        /// <summary>
        /// Triggers the UpdateFields event.
        /// </summary>
        /// <param name="robot">The robot who made the action.</param>
        /// <param name="direction">The direction of the action.</param>
        /// <param name="action">The type of the action.</param>
        /// <param name="direction">Whether the action was successful or not.</param>
        private void OnUpdateFields(Robot robot, Direction direction, Action action, bool canExecute)
        {
            UpdateFields?.Invoke(this, new ActionEventArgs(robot, direction, action, canExecute));
        }

        /// <summary>
        /// Switches to the next team.
        /// </summary>
        private void NextTeam()
        {
            if (_nextTeam1)
            {
                if (_nextPlayerFromTeam1 == _team1.Robots.Length - 1)
                {
                    _nextTeam1 = false;
                }

                if (_nextPlayerFromTeam1 < _team1.Robots.Length - 1)
                {
                    _nextPlayerFromTeam1++;
                }
                else
                {
                    _nextPlayerFromTeam1 = 0;
                }

            }
            else
            {
                if (_nextPlayerFromTeam2 == _team2.Robots.Length - 1)
                {
                    _nextTeam1 = true;
                }

                if (_nextPlayerFromTeam2 < _team2.Robots.Length - 1)
                {
                    _nextPlayerFromTeam2++;
                }
                else
                {
                    _nextPlayerFromTeam2 = 0;
                }
            }
        }

        /// <summary>
        /// Switches to the next robot.
        /// </summary>
        private Robot NextRobot()
        {
            if (_nextTeam1)
            {
                return _team1.GetRobot(_nextPlayerFromTeam1);
            }
            else
            {
                return _team2.GetRobot(_nextPlayerFromTeam2);
            }
        }

        #endregion

        #region Move

        /// <summary>
        /// The move action.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="dir">The direction of the move.</param>
        public void MoveRobot(Robot robot, Direction dir)
        {
            if (IsConnectedToRobots(robot))
            {
                MoveRobots(robot, dir);
                return;
            }
            if (dir == Direction.EAST)
            {
                if (CanMoveToDirection(robot, 1, 0))
                {
                    MoveToDirection(robot, dir);
                    robot.X++;
                    _board.SetValueNewField(robot);
                    OnUpdateFields(robot, Direction.EAST, Action.Move, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.EAST, Action.Move, false);
                }
            }
            else if (dir == Direction.WEST)
            {
                if (CanMoveToDirection(robot, -1, 0))
                {
                    MoveToDirection(robot, dir);
                    robot.X--;
                    _board.SetValueNewField(robot);
                    OnUpdateFields(robot, Direction.WEST, Action.Move, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.WEST, Action.Move, false);
                }
            }
            else if (dir == Direction.NORTH)
            {
                if (CanMoveToDirection(robot, 0, -1))
                {
                    MoveToDirection(robot, dir);
                    robot.Y--;
                    _board.SetValueNewField(robot);
                    OnUpdateFields(robot, Direction.NORTH, Action.Move, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.NORTH, Action.Move, false);
                }
            }
            else if (dir == Direction.SOUTH)
            {
                if (CanMoveToDirection(robot, 0, 1))
                {
                    MoveToDirection(robot, dir);
                    robot.Y++;
                    _board.SetValueNewField(robot);
                    OnUpdateFields(robot, Direction.SOUTH, Action.Move, true);
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

        }

        /// <summary>
        /// Checks whether the robot can move to a specific direction.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="a">The X relative coordinate.</param>
        /// <param name="b">The Y relative coordinate.</param>
        /// <returns>True, if the robot can move, else false.</returns>
        private bool CanMoveToDirection(Robot robot, int a, int b)
        {
            if (!IsOnBoard(robot.X + a, robot.Y + b)
                   || !((_board.GetFieldValue(robot.X + a, robot.Y + b) is Exit)
                   || (_board.GetFieldValue(robot.X + a, robot.Y + b) is Empty)
                   || ((_board.GetFieldValue(robot.X + a, robot.Y + b) is Cube) &&
                   robot.IsConnected(new XYcoordinates(robot.X + a, robot.Y + b)))))
            {
                return false;
            }

            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if (IsOnBoard(connections[i].X + a, connections[i].Y + b))
                {
                    if (_board.GetFieldValue(connections[i].X + a, connections[i].Y + b) is Empty)
                    {
                        continue;
                    }
                    else if ((_board.GetFieldValue(connections[i].X + a, connections[i].Y + b) is Robot)
                    && (connections[i].X + a == robot.X && connections[i].Y + b == robot.Y))
                    {
                        continue;
                    }
                    else if ((_board.GetFieldValue(connections[i].X + a, connections[i].Y + b) is Cube) &&
                    robot.IsConnected(new XYcoordinates(connections[i].X + a, connections[i].Y + b)))
                    {
                        continue;
                    }
                    else if (_board.GetFieldValue(connections[i].X + a, connections[i].Y + b) is Exit)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether the robot can move to a specific direction.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="dir">The direction of the move.</param>
        /// <returns>True, if the robot can move, else false.</returns>
        private void MoveToDirection(Robot robot, Direction dir)
        {
            if (IsOnEdge(robot.X, robot.Y))
            {
                _board.SetValueNewField(new Exit(robot.X, robot.Y));
            }
            else
            {
                _board.SetValueNewField(new Empty(robot.X, robot.Y));
            }

            List<XYcoordinates> connections = robot.AllConnections();
            for (int i = 0; i < connections.Count; i++)
            {
                if (IsOnBoard(connections[i].X, connections[i].Y))
                {
                    if (IsOnEdge(connections[i].X, connections[i].Y))
                    {
                        _board.SetValueNewField(new Exit(connections[i].X, connections[i].Y));
                    }
                    else
                    {
                        _board.SetValueNewField(new Empty(connections[i].X, connections[i].Y));
                    }
                }

            }

            if (dir == Direction.EAST)
            {
                robot.ToEast();
            }
            else if (dir == Direction.WEST)
            {
                robot.ToWest();
            }
            else if (dir == Direction.NORTH)
            {
                robot.ToNorth();
            }
            else if (dir == Direction.SOUTH)
            {
                robot.ToSouth();
            }

            List<XYcoordinates> connectionsNew = robot.AllConnections();
            for (int i = 0; i < connectionsNew.Count; i++)
            {
                if (IsOnBoard(connectionsNew[i].X, connectionsNew[i].Y))
                {
                    _board.SetValueNewField(new Cube(connectionsNew[i].X, connectionsNew[i].Y,
                        robot.getHealthAt(i), robot.getColorAt(i)));
                    Cube cube = (Cube)_board.GetFieldValue(connectionsNew[i].X, connectionsNew[i].Y);
                    cube.IsConnected = true;
                }
            }
        }

        /// <summary>
        /// Checks whether the robot is connected to other robots.
        /// </summary>
        /// <param name="robot">The robot we want to check its connection.</param>
        /// <returns>True, if it's connected, else false.</returns>
        private bool IsConnectedToRobots(Robot robot)
        {
            if (robot.ConnectedRobot == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks whether a coordinate is on the board.
        /// </summary>
        /// <param name="x">The X coordinate of the field.</param>
        /// <param name="y">The Y coordinate of the field.</param>
        /// <returns>True, if it's on the board, else false.</returns>
        public bool IsOnBoard(int x, int y)
        {
            if (x >= _board.Width || x < 0 || y >= _board.Height || y < 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks whether a coordinate is on the edge of the board.
        /// </summary>
        /// <param name="x">The X coordinate of the field.</param>
        /// <param name="y">The Y coordinate of the field.</param>
        /// <returns>True, if it's on the edge of the board, else false.</returns>
        private bool IsOnEdge(int x, int y)
        {
            if (x == 0 || y == 0 || x == _board.Width - 1 || y == _board.Height - 1)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region MoveTwoRobots

        /// <summary>
        /// The move action of two robots.
        /// </summary>
        /// <param name="r1">One of the robots we want to move with.</param>
        /// <param name="dir">The direction of the movement.</param>
        private void MoveRobots(Robot r1, Direction dir)
        {
            Robot? r2 = null;

            if (_team1.GetRobotByNum(r1.ConnectedRobot) != null)
            {
                r2 = _team1.GetRobotByNum(r1.ConnectedRobot);
            }
            if (_team2.GetRobotByNum(r1.ConnectedRobot) != null)
            {
                r2 = _team2.GetRobotByNum(r1.ConnectedRobot);
            }
            if (r2 != null)
            {
                if ((r2.WantsToMoveToDirection) != dir)
                {
                    r1.WantsToMoveToDirection = dir;
                    OnUpdateFields(r1, dir, Action.Move, false);
                }
                else
                {
                    if (dir == Direction.EAST)
                    {
                        if (CanMoveToDirectionRobots(r1, r2, 1, 0))
                        {
                            MoveToDirectionRobots(r1, r2, dir);
                            r1.X++;
                            r2.X++;
                            _board.SetValueNewField(r1);
                            _board.SetValueNewField(r2);
                            r2.WantsToMoveToDirection = null;
                            OnUpdateFields(r1, Direction.EAST, Action.Move, true);
                        }
                        else
                        {
                            r1.WantsToMoveToDirection = dir;
                            OnUpdateFields(r1, Direction.EAST, Action.Move, false);
                        }
                    }
                    else if (dir == Direction.WEST)
                    {
                        if (CanMoveToDirectionRobots(r1, r2, -1, 0))
                        {
                            MoveToDirectionRobots(r1, r2, dir);
                            r1.X--;
                            r2.X--;
                            _board.SetValueNewField(r1);
                            _board.SetValueNewField(r2);
                            r2.WantsToMoveToDirection = null;
                            OnUpdateFields(r1, Direction.WEST, Action.Move, true);
                        }
                        else
                        {
                            r1.WantsToMoveToDirection = dir;
                            OnUpdateFields(r1, Direction.WEST, Action.Move, false);
                        }
                    }
                    else if (dir == Direction.NORTH)
                    {
                        if (CanMoveToDirectionRobots(r1, r2, 0, -1))
                        {
                            MoveToDirectionRobots(r1, r2, dir);
                            r1.Y--;
                            r2.Y--;
                            _board.SetValueNewField(r1);
                            _board.SetValueNewField(r2);
                            r2.WantsToMoveToDirection = null;
                            OnUpdateFields(r1, Direction.NORTH, Action.Move, true);
                        }
                        else
                        {
                            r1.WantsToMoveToDirection = dir;
                            OnUpdateFields(r1, Direction.NORTH, Action.Move, false);
                        }
                    }
                    else if (dir == Direction.SOUTH)
                    {
                        if (CanMoveToDirectionRobots(r1, r2, 0, 1))
                        {
                            MoveToDirectionRobots(r1, r2, dir);
                            r1.Y++;
                            r2.Y++;
                            _board.SetValueNewField(r1);
                            _board.SetValueNewField(r2);
                            r2.WantsToMoveToDirection = null;
                            OnUpdateFields(r1, Direction.SOUTH, Action.Move, true);
                        }
                        else
                        {
                            r1.WantsToMoveToDirection = dir;
                            OnUpdateFields(r1, Direction.SOUTH, Action.Move, false);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Updates the fields according to the move action of two robots.
        /// </summary>
        /// <param name="robot">The first robot.</param>
        /// <param name="robot2">The second robot.</param>
        /// <param name="dir">The direction of the movement.</param>
        private void MoveToDirectionRobots(Robot robot, Robot robot2, Direction dir)
        {
            if (IsOnEdge(robot.X, robot.Y))
            {
                _board.SetValueNewField(new Exit(robot.X, robot.Y));
            }
            else
            {
                _board.SetValueNewField(new Empty(robot.X, robot.Y));
            }

            if (IsOnEdge(robot2.X, robot2.Y))
            {
                _board.SetValueNewField(new Exit(robot2.X, robot2.Y));
            }
            else
            {
                _board.SetValueNewField(new Empty(robot2.X, robot2.Y));
            }

            List<XYcoordinates> connections = robot.AllConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                if (IsOnBoard(connections[i].X, connections[i].Y))
                {
                    if (IsOnEdge(connections[i].X, connections[i].Y))
                    {
                        _board.SetValueNewField(new Exit(connections[i].X, connections[i].Y));
                    }
                    else
                    {
                        _board.SetValueNewField(new Empty(connections[i].X, connections[i].Y));
                    }
                }

            }

            if (dir == Direction.EAST)
            {
                robot.ToEast();
            }
            else if (dir == Direction.WEST)
            {
                robot.ToWest();
            }
            else if (dir == Direction.NORTH)
            {
                robot.ToNorth();
            }
            else if (dir == Direction.SOUTH)
            {
                robot.ToSouth();
            }

            List<XYcoordinates> connectionsNew = robot.AllConnections();
            for (int i = 0; i < connectionsNew.Count; i++)
            {
                if (IsOnBoard(connectionsNew[i].X, connectionsNew[i].Y))
                {
                    _board.SetValueNewField(new Cube(connectionsNew[i].X, connectionsNew[i].Y,
                        robot.getHealthAt(i), robot.getColorAt(i)));
                    Cube cube = (Cube)_board.GetFieldValue(connectionsNew[i].X, connectionsNew[i].Y);
                    cube.IsConnected = true;
                }
            }
        }

        /// <summary>
        /// Checks whether the robots can move to a specific direction.
        /// </summary>
        /// <param name="robot">The first robot we want to execute the action with.</param>
        /// <param name="robot2">The second robot we want to execute the action with.</param>
        /// <param name="a">The X relative coordinate.</param>
        /// <param name="b">The Y relative coordinate.</param>
        /// <returns>True, if the robots can move, else false.</returns>
        private bool CanMoveToDirectionRobots(Robot robot, Robot robot2, int a, int b)
        {
            if (!IsOnBoard(robot.X + a, robot.Y + b)
                   || !((_board.GetFieldValue(robot.X + a, robot.Y + b) is Exit)
                   || (_board.GetFieldValue(robot.X + a, robot.Y + b) is Empty)
                   || ((_board.GetFieldValue(robot.X + a, robot.Y + b) is Cube) &&
                   robot.IsConnected(new XYcoordinates(robot.X + a, robot.Y + b)))))
            {
                return false;
            }

            if (!IsOnBoard(robot2.X + a, robot2.Y + b)
                   || !((_board.GetFieldValue(robot2.X + a, robot2.Y + b) is Exit)
                   || (_board.GetFieldValue(robot2.X + a, robot2.Y + b) is Empty)
                   || ((_board.GetFieldValue(robot2.X + a, robot2.Y + b) is Cube) &&
                   robot2.IsConnected(new XYcoordinates(robot2.X + a, robot2.Y + b)))))
            {
                return false;
            }

            List<XYcoordinates> connections = robot.AllConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                if (IsOnBoard(connections[i].X + a, connections[i].Y + b))
                {
                    if (_board.GetFieldValue(connections[i].X + a, connections[i].Y + b) is Empty)
                    {
                        continue;
                    }
                    else if ((_board.GetFieldValue(connections[i].X + a, connections[i].Y + b) is Robot)
                    && ((connections[i].X + a == robot.X && connections[i].Y + b == robot.Y)
                        || connections[i].X + a == robot2.X && connections[i].Y + b == robot2.Y))
                    {
                        continue;
                    }
                    else if ((_board.GetFieldValue(connections[i].X + a, connections[i].Y + b) is Cube) &&
                    robot.IsConnected(new XYcoordinates(connections[i].X + a, connections[i].Y + b)))
                    {
                        continue;
                    }
                    else if (_board.GetFieldValue(connections[i].X + a, connections[i].Y + b) is Exit)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region Rotate

        /// <summary>
        /// The rotation action.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="angle">The angle of the action.</param>
        public void RotateRobot(Robot robot, Angle angle)
        {
            if (IsConnectedToRobots(robot))
            {
                OnUpdateFields(robot, robot.Direction, Action.Turn, false);
                return;
            }
            if ((angle == Angle.Clockwise && CanRotateClockwise(robot))
                || (angle == Angle.CounterClockwise && CanRotateCounterClockwise(robot)))
            {
                RotateAll(robot, angle);
                if (angle == Angle.Clockwise)
                {
                    OnUpdateFields(robot, Direction.EAST, Action.Turn, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.WEST, Action.Turn, true);
                }
            }
            else
            {
                OnUpdateFields(robot, Direction.WEST, Action.Turn, false);
            }
        }

        // <summary>
        /// The rotation action of the connected cubes of the robot.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="angle">The angle of the action.</param>
        private void RotateAll(Robot robot, Angle angle)
        {
            List<XYcoordinates> connections = robot.AllConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                if (IsOnBoard(connections[i].X, connections[i].Y))
                {
                    if (IsOnEdge(connections[i].X, connections[i].Y))
                    {
                        _board.SetValueNewField(new Exit(connections[i].X, connections[i].Y));
                    }
                    else
                    {
                        _board.SetValueNewField(new Empty(connections[i].X, connections[i].Y));
                    }
                }
            }

            if (angle == Angle.Clockwise)
            {
                robot.RotateClockwise();

                switch (robot.Direction)
                {
                    case Direction.EAST:
                        robot.Direction = Direction.SOUTH;
                        break;
                    case Direction.SOUTH:
                        robot.Direction = Direction.WEST;
                        break;
                    case Direction.WEST:
                        robot.Direction = Direction.NORTH;
                        break;
                    case Direction.NORTH:
                        robot.Direction = Direction.EAST;
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
                if (IsOnBoard(connectionsNew[i].X, connectionsNew[i].Y))
                {
                    _board.SetValueNewField(new Cube(connectionsNew[i].X, connectionsNew[i].Y,
                    robot.getHealthAt(i), robot.getColorAt(i)));
                    Cube cube = (Cube)_board.GetFieldValue(connectionsNew[i].X, connectionsNew[i].Y);
                    cube.IsConnected = true;
                }
            }
        }

        /// <summary>
        /// Checks whether the robot can rotate clockwise.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <returns>True, if the robot can rotate, else false.</returns>
        private bool CanRotateClockwise(Robot robot)
        {
            for (int i = 0; i < (robot.AllConnections()).Count(); i++)
            {
                int newX = robot.X + robot.Y - (robot.AllConnections())[i].Y;
                int newY = -robot.X + robot.Y + (robot.AllConnections())[i].X;

                if (IsOnBoard(newX, newY))
                {
                    if (_board.GetFieldValue(newX, newY) is Empty)
                    {
                        continue;
                    }
                    else if (_board.GetFieldValue(newX, newY) is Exit)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether the robot can rotate counter clockwise.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <returns>True, if the robot can rotate, else false.</returns>
        private bool CanRotateCounterClockwise(Robot robot)
        {
            for (int i = 0; i < (robot.AllConnections()).Count(); i++)
            {
                int newX = robot.X - robot.Y + (robot.AllConnections())[i].Y;
                int newY = robot.X + robot.Y - (robot.AllConnections())[i].X;

                if (IsOnBoard(newX, newY))
                {
                    if (_board.GetFieldValue(newX, newY) is Empty)
                    {
                        continue;
                    }
                    else if (_board.GetFieldValue(newX, newY) is Exit)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region ConnectRobot

        /// <summary>
        /// The connect to a cube action.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        public void ConnectRobot(Robot robot)
        {
            _actionDirection = robot.Direction;

            if (_actionDirection == null)
            {
                return;
            }

            if (_actionDirection == Direction.EAST)
            {
                if (ConnectDirection(robot, 1, 0))
                {
                    OnUpdateFields(robot, Direction.EAST, Action.ConnectRobot, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.EAST, Action.ConnectRobot, false);
                }
            }
            else if (_actionDirection == Direction.WEST)
            {
                if (ConnectDirection(robot, -1, 0))
                {
                    OnUpdateFields(robot, Direction.WEST, Action.ConnectRobot, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.WEST, Action.ConnectRobot, false);
                }
            }
            else if (_actionDirection == Direction.NORTH)
            {
                if (ConnectDirection(robot, 0, -1))
                {
                    OnUpdateFields(robot, Direction.NORTH, Action.ConnectRobot, true);
                }
                else
                {
                    OnUpdateFields(robot, Direction.NORTH, Action.ConnectRobot, false);
                }
            }
            else if (_actionDirection == Direction.SOUTH)
            {
                if (ConnectDirection(robot, 0, 1))
                {
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

        }

        /// <summary>
        /// Checks whether the robot can connect to a cube.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="a">The X relative coordinate.</param>
        /// <param name="b">The Y relative coordinate.</param>
        /// <returns>True, if the robot can connect to the cube, else false.</returns>
        private bool ConnectDirection(Robot robot, int a, int b)
        {
            int x = robot.X + a;
            int y = robot.Y + b;
            if (robot.IsConnected(new XYcoordinates(x, y)))
            {
                return false;
            }
            if (IsOnBoard(x, y) && _board.GetFieldValue(x, y) is Cube)
            {
                Cube cube = (Cube)_board.GetFieldValue(x, y);
                if (!cube.IsConnected)
                {
                    robot.AddConnection(new XYcoordinates(x, y));
                    robot.addHealthColor(cube.Health, cube.Color);
                    cube.IsConnected = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region DisconnectRobot

        /// <summary>
        /// The disconnect action.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        public void DisconnectRobot(Robot robot)
        {
            if (IsOnEdge(robot.X, robot.Y))
            {
                GetPoints(robot);
            }
            foreach (XYcoordinates cubeXY in Robot.AllConnections())
            {
                if (IsOnBoard(cubeXY.X, cubeXY.Y))
                {
                    Cube cube = (Cube)_board.GetFieldValue(cubeXY.X, cubeXY.Y);
                    cube.IsConnected = false;
                }
            }
            robot.ClearConnections();
            foreach (Robot r in _team1.Robots)
            {
                if ((robot.ConnectedRobot).Equals(r.RobotNumber))
                {
                    r.ConnectedRobot = -1;
                }
            }
            foreach (Robot r in _team2.Robots)
            {
                if ((robot.ConnectedRobot).Equals(r.RobotNumber))
                {
                    r.ConnectedRobot = -1;
                }
            }
            robot.ConnectedRobot = -1;
            OnUpdateFields(robot, Direction.WEST, Action.DisconnectRobot, true);
        }

        private void GetPoints(Robot robot)
        {
            bool canGetPoints = false;
            if (robot.X == 0)
            {
                if (CanAddPoints(robot, new XYcoordinates(3, 0)) ||
                CanAddPoints(robot, new XYcoordinates(3, 1)) ||
                CanAddPoints(robot, new XYcoordinates(3, 2)))
                {
                    canGetPoints = true;
                }
            }
            if (robot.X == _board.Width - 1)
            {
                if (CanAddPoints(robot, new XYcoordinates(-1, 0)) ||
                CanAddPoints(robot, new XYcoordinates(-1, 1)) ||
                CanAddPoints(robot, new XYcoordinates(-1, 2)))
                {
                    canGetPoints = true;
                }
            }
            if (robot.Y == 0)
            {
                if (CanAddPoints(robot, new XYcoordinates(0, 3)) ||
                CanAddPoints(robot, new XYcoordinates(1, 3)) ||
                CanAddPoints(robot, new XYcoordinates(2, 3)))
                {
                    canGetPoints = true;
                }
            }
            if (robot.Y == _board.Height - 1)
            {
                if (CanAddPoints(robot, new XYcoordinates(0, -1)) ||
                CanAddPoints(robot, new XYcoordinates(1, -1)) ||
                CanAddPoints(robot, new XYcoordinates(2, -1)))
                {
                    canGetPoints = true;
                }
            }
            if (canGetPoints)
            {
                if (robot.Player1)
                {
                    _team1.Points = _noticeBoard.TaskReward;
                }
                if (robot.Player1)
                {
                    _team2.Points = _noticeBoard.TaskReward;
                }
            }
        }

        private bool CanAddPoints(Robot robot, XYcoordinates robotRelPos)
        {
            bool canAdd = true;
            foreach (Field f in NoticeBoard.Fields)
            {
                if (f is Cube)
                {
                    int newX = f.X + (robot.X - robotRelPos.X);
                    int newY = f.Y + (robot.Y - robotRelPos.Y);
                    for (int i = 0; i < Robot.AllConnections().Count(); i++)
                    {
                        canAdd = false;
                        {
                            if (Robot.AllConnections()[i].X == newX && Robot.AllConnections()[i].Y == newY)
                            {
                                Cube cube = (Cube)f;
                                if (cube.Color == Robot.AllColor()[i])
                                {
                                    canAdd = true;
                                }
                            }
                        }
                        if (!canAdd)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        #endregion

        #region ConnectCubes

        /// <summary>
        /// The connect multiple cubes with another robot action.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="ownCube">The coordinates of the cube the robot is linked to.</param>
        /// <param name="wantsToConnect">The coordinates of the cube the robot wants to connect to.</param>
        public void ConnectCubes(Robot robot, XYcoordinates ownCube, XYcoordinates wantsToConnect)
        {
            ownCube.X = ownCube.X + robot.X;
            ownCube.Y = ownCube.Y + robot.Y;

            wantsToConnect.X = wantsToConnect.X + robot.X;
            wantsToConnect.Y = wantsToConnect.Y + robot.Y;

            if (_board.GetFieldValue(ownCube.X, ownCube.Y) is Cube &&
                _board.GetFieldValue(wantsToConnect.X, wantsToConnect.Y) is Cube &&
                robot.IsConnected(ownCube) && !robot.IsConnected(wantsToConnect) &&
                NextToEachOther(ownCube, wantsToConnect))
            {
                if (robot.RobotNumber < 4)
                {
                    foreach (Robot r in _team1.Robots)
                    {
                        if (r.WantsToConnectTo != null && r.OwnCube != null &&
                            (r.WantsToConnectTo).Equals(ownCube) && (r.OwnCube).Equals(wantsToConnect))
                        {
                            UnionRobotConnections(r, robot);
                            r.WantsToConnectTo = null;
                            r.OwnCube = null;
                            r.ConnectedRobot = robot.RobotNumber;
                            robot.ConnectedRobot = r.RobotNumber;
                            OnUpdateFields(robot, robot.Direction, Action.ConnectCubes, true);
                            return;
                        }
                    }
                }
                else
                {
                    foreach (Robot r in _team2.Robots)
                    {
                        if (r.WantsToConnectTo != null && r.OwnCube != null &&
                            (r.WantsToConnectTo).Equals(ownCube) && (r.OwnCube).Equals(wantsToConnect))
                        {
                            UnionRobotConnections(r, robot);
                            r.WantsToConnectTo = null;
                            r.OwnCube = null;
                            r.ConnectedRobot = robot.RobotNumber;
                            robot.ConnectedRobot = r.RobotNumber;
                            OnUpdateFields(robot, robot.Direction, Action.ConnectCubes, true);
                            return;
                        }
                    }
                }

                robot.WantsToConnectTo = wantsToConnect;
                robot.OwnCube = ownCube;
            }

        }

        /// <summary>
        /// Creates the union of the connections of the two robots.
        /// </summary>
        /// <param name="r1">The first robot we want to execute the action with.</param>
        /// <param name="r2">The second robot we want to execute the action with.</param>
        private void UnionRobotConnections(Robot r1, Robot r2)
        {
            for (int i = 0; i < r1.AllConnections().Count; i++)
            {
                if (!r2.IsConnected(r1.AllConnections()[i]))
                {
                    r2.AddConnection(r1.AllConnections()[i]);
                    r2.addHealthColor(r1.getHealthAt(i), r1.getColorAt(i));
                }
            }
            for (int i = 0; i < r2.AllConnections().Count; i++)
            {
                if (!r1.IsConnected(r2.AllConnections()[i]))
                {
                    r1.AddConnection(r2.AllConnections()[i]);
                    r1.addHealthColor(r2.getHealthAt(i), r2.getColorAt(i));
                }
            }
        }

        /// <summary>
        /// Checks whether the two cubes we want to connect are next to each other.
        /// </summary>
        /// <param name="ownCube">The coordinates of the first cube.</param>
        /// <param name="wantsToConnect">The coordinates of the second cube.</param>
        /// <returns>True, if the the cubes are next to each other, else false.</returns>
        private bool NextToEachOther(XYcoordinates ownCube, XYcoordinates wantsToConnect)
        {
            if ((ownCube.X + 1 == wantsToConnect.X && ownCube.Y == wantsToConnect.Y) ||
                (ownCube.X - 1 == wantsToConnect.X && ownCube.Y == wantsToConnect.Y) ||
                (ownCube.X == wantsToConnect.X && ownCube.Y + 1 == wantsToConnect.Y) ||
                (ownCube.X == wantsToConnect.X && ownCube.Y - 1 == wantsToConnect.Y))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region DisconnectCubes

        /// <summary>
        /// The disconnecting of more cubes action.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="ownCube_1">The coordinates of the first cube.</param>
        /// <param name="ownCube_2">The coordinates of the second cube.</param>
        public void DisconnectCubes(Robot robot, XYcoordinates ownCube_1, XYcoordinates ownCube_2)
        {
            ownCube_1.X = ownCube_1.X + robot.X;
            ownCube_1.Y = ownCube_1.Y + robot.Y;

            ownCube_2.X = ownCube_2.X + robot.X;
            ownCube_2.Y = ownCube_2.Y + robot.Y;

            if (IsConnectedToRobots(robot) && robot.IsConnected(ownCube_1) &&
                robot.IsConnected(ownCube_2) && NextToEachOther(ownCube_1, ownCube_2))
            {
                if (robot != null)
                {
                    if (_team1.GetRobotByNum(robot.ConnectedRobot) != null)
                    {
                        Robot r = _team1.GetRobotByNum(robot.ConnectedRobot)!;
                        SeparateRobotConnections(r, robot, ownCube_1, ownCube_2);

                    }
                    if (_team2.GetRobotByNum(robot.ConnectedRobot) != null)
                    {
                        Robot r = _team2.GetRobotByNum(robot.ConnectedRobot)!;
                        SeparateRobotConnections(r, robot, ownCube_1, ownCube_2);

                    }
                }

            }
        }

        /// <summary>
        /// The separation of the connections of the two robots.
        /// </summary>
        /// <param name="r1">The first robot we want to execute the separation with.</param>
        /// <param name="r2">The second robot we want to execute the separation with.</param>
        /// <param name="ownCube_1">The coordinates of the first cube.</param>
        /// <param name="ownCube_2">The coordinates of the second cube.</param>
        public void SeparateRobotConnections(Robot r1, Robot r2, XYcoordinates ownCube_1, XYcoordinates ownCube_2)
        {
            List<XYcoordinates> connections = new List<XYcoordinates>();
            List<int> healths = new List<int>();
            List<Color> colors = new List<Color>();

            for (int i = 0; i < r1.AllConnections().Count(); i++)
            {
                connections.Add(r1.AllConnections()[i]);
                healths.Add(r1.getHealthAt(i));
                colors.Add(r1.getColorAt(i));
            }

            r1.ClearConnections();
            r2.ClearConnections();

            r2.ConnectedRobot = -1;
            r1.ConnectedRobot = -1;

            if (ownCube_1.X == ownCube_2.X)
            {
                if (ownCube_1.Y > ownCube_2.Y)
                {
                    XYcoordinates temp = ownCube_1;
                    ownCube_1 = ownCube_2;
                    ownCube_2 = temp;
                }
                if (r1.Y < r2.Y)
                {
                    for (int i = 0; i < connections.Count; i++)
                    {
                        if (connections[i].Y <= ownCube_1.Y)
                        {
                            r1.AllConnections().Add(connections[i]);
                            r1.addHealthColor(healths[i], colors[i]);
                        }
                        else
                        {
                            r2.AllConnections().Add(connections[i]);
                            r2.addHealthColor(healths[i], colors[i]);
                        }
                    }
                }
                else if (r1.Y >= r2.Y)
                {
                    for (int i = 0; i < connections.Count; i++)
                    {
                        if (connections[i].Y <= ownCube_1.Y)
                        {
                            r2.AllConnections().Add(connections[i]);
                            r2.addHealthColor(healths[i], colors[i]);
                        }
                        else
                        {
                            r1.AllConnections().Add(connections[i]);
                            r1.addHealthColor(healths[i], colors[i]);
                        }
                    }
                }

            }
            if (ownCube_1.Y == ownCube_2.Y)
            {
                if (ownCube_1.X > ownCube_2.X)
                {
                    XYcoordinates temp = ownCube_1;
                    ownCube_1 = ownCube_2;
                    ownCube_2 = temp;
                }
                if (r1.X < r2.X)
                {
                    for (int i = 0; i < connections.Count; i++)
                    {
                        if (connections[i].X <= ownCube_1.X)
                        {
                            r1.AllConnections().Add(connections[i]);
                            r1.addHealthColor(healths[i], colors[i]);
                        }
                        else
                        {
                            r2.AllConnections().Add(connections[i]);
                            r2.addHealthColor(healths[i], colors[i]);
                        }
                    }
                }
                else if (r1.X >= r2.X)
                {
                    for (int i = 0; i < connections.Count; i++)
                    {
                        if (connections[i].X <= ownCube_1.X)
                        {
                            r2.AllConnections().Add(connections[i]);
                            r2.addHealthColor(healths[i], colors[i]);
                        }
                        else
                        {
                            r1.AllConnections().Add(connections[i]);
                            r1.addHealthColor(healths[i], colors[i]);
                        }
                    }
                }
            }
        }

        #endregion

        #region Clean

        /// <summary>
        /// The cleaning action.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        public void Clean(Robot robot)
        {
            if (robot.Direction == Direction.EAST && CleanDirection(robot, 1, 0))
            {
                OnUpdateFields(robot, robot.Direction, Action.Clean, true);
            }
            else if (robot.Direction == Direction.WEST && CleanDirection(robot, -1, 0))
            {
                OnUpdateFields(robot, robot.Direction, Action.Clean, true);
            }
            else if (robot.Direction == Direction.NORTH && CleanDirection(robot, 0, -1))
            {
                OnUpdateFields(robot, robot.Direction, Action.Clean, true);
            }
            else if (robot.Direction == Direction.SOUTH && CleanDirection(robot, 0, 1))
            {
                OnUpdateFields(robot, robot.Direction, Action.Clean, true);
            }
            else
            {
                OnUpdateFields(robot, robot.Direction, Action.Clean, false);
            }
        }

        /// <summary>
        /// Checks whether the robot can clean in a specific direction.
        /// </summary>
        /// <param name="robot">The robot we want to execute the action with.</param>
        /// <param name="a">The X relative coordinate of the field.</param>
        /// <param name="b">The Y relative coordinate of the field</param>
        /// <returns>True, if the robot can clean, else false.</returns>
        private bool CleanDirection(Robot robot, int a, int b)
        {
            if (_board.GetFieldValue(robot.X + a, robot.Y + b) is Obstacle)
            {
                Obstacle obs = (Obstacle)_board.GetFieldValue(robot.X + a, robot.Y + b);
                obs.DecreaseHealth();
                if (obs.Health == 0)
                {
                    _board.SetValueNewField(new Empty(robot.X + a, robot.Y + b));
                }
                else
                {
                    _board.SetValueNewField(obs);
                }
                return true;
            }
            else if (_board.GetFieldValue(robot.X + a, robot.Y + b) is Cube &&
                !robot.IsConnected(new XYcoordinates(robot.X + a, robot.Y + b)))
            {
                Cube cube = (Cube)_board.GetFieldValue(robot.X + a, robot.Y + b);
                if (!cube.IsConnected)
                {
                    cube.DecreaseHealth();
                    if (cube.Health == 0)
                    {
                        _board.SetValueNewField(new Empty(robot.X + a, robot.Y + b));
                    }
                    else
                    {
                        _board.SetValueNewField(cube);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (_board.GetFieldValue(robot.X + a, robot.Y + b) is Robot)
            {
                Robot cleanRobot = (Robot)_board.GetFieldValue(robot.X + a, robot.Y + b);
                cleanRobot.DecreaseHealth();
                if (cleanRobot.Health == 0)
                {
                    _board.SetValueNewField(new Empty(robot.X + a, robot.Y + b));
                    if (cleanRobot.Player1)
                    {
                        _team1.RemoveRobotFromTeam(cleanRobot);
                        if (_team1.IsEmptyTeam())
                        {
                            OnGameOver(true, _team2);
                        }
                        if (_nextPlayerFromTeam1 > _team1.Robots.Length - 1)
                        {
                            _nextPlayerFromTeam1 = 0;
                        }
                    }
                    else
                    {
                        _team2.RemoveRobotFromTeam(cleanRobot);
                        if (_team2.IsEmptyTeam())
                        {
                            OnGameOver(true, _team1);
                        }
                        if (_nextPlayerFromTeam2 > _team2.Robots.Length - 1)
                        {
                            _nextPlayerFromTeam2 = 0;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Wait

        public void Wait(Robot robot)
        {
            OnUpdateFields(robot, robot.Direction, Action.Wait, true);

        }

        #endregion

    }


    /// <summary>
    /// Robots GameEventArgs type.
    /// </summary>
    public class GameEventArgs : EventArgs
    {
        #region Fields

        private bool _isGameOver;
        private int _winnerTeam;
        private int _currentRound;
        private int _gameTime;
        private int _team1points;
        private int _team2points;

        #endregion

        #region Properties

        /// <summary>
        /// The query or setting of wheter the game is over.
        /// </summary>
        public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }

        /// <summary>
        /// The query or setting of the winner team.
        /// </summary>
        public int WinnerTeam { get { return _winnerTeam; } set { _winnerTeam = value; } }

        /// <summary>
        /// The query or setting of the current round.
        /// </summary>
        public int CurrentRound { get { return _currentRound; } set { _currentRound = value; } }

        /// <summary>
        /// The query or setting of the amount of the game time.
        /// </summary>
        public int GameTime { get { return _gameTime; } set { _gameTime = value; } }

        /// <summary>
        /// The first team's points.
        /// </summary>
        public int Team1Points { get { return _team1points; } set { _team1points = value; } }

        /// <summary>
        /// The second team's points.
        /// </summary>
        public int Team2Points { get { return _team2points; } set { _team2points = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the Game class.
        /// </summary>
        /// <param name="dataAccess">The data access for the Load and Save methods.</param>
        public GameEventArgs(bool isGameOver, int winnerTeam, int currentRound, int gameTime, int team1points, int team2points)
        {
            _isGameOver = isGameOver;
            _winnerTeam = winnerTeam;
            _currentRound = currentRound;
            _gameTime = gameTime;
            _team1points = team1points;
            _team2points = team2points;
        }

        #endregion

    }



}

