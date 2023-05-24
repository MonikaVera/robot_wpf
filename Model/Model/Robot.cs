namespace Model.Model
{
    /// <summary>
    /// Robots Robot type.
    /// </summary>
    public class Robot : Field
    {
        #region Fields

        private Direction _direction; // direction of the robot
        private List<XYcoordinates> _connected; // the list of cube coordinates the robot is connected to
        private List<int> _healthConnected = null!; // the health of the connected cubes
        private List<Color> _colorConnected = null!; // the color of the connected cubes
        private Team? _team = null!; // the team the robot is part of
        private int _robotNumber; // the number of the robot
        private int _connectedRobot; // the number of the connected robot 
        private bool _player1; // whether the robot is in team1
        private XYcoordinates? _wantsToConnectTo = null; // the coordinate of the cube the robot wants to connect to
        private XYcoordinates? _ownCube = null; // the own cube of the robot
        private Direction? _wantsToMoveToDirection; // the direction the robot wants to move to 
        private int _health; // the health of the robot

        #endregion

        #region Properties

        /// <summary>
        /// Query or setting of the robot's number.
        /// </summary>
        public int RobotNumber { get { return _robotNumber; } set { _robotNumber = value; } }

        /// <summary>
        /// Query or setting of the direction of the robot.
        /// </summary>
        public Direction Direction { set { _direction = value; } get { return _direction; } }

        /// <summary>
        /// Query or setting of the connected robot's number.
        /// </summary>
        public int ConnectedRobot { get { return _connectedRobot; } set { _connectedRobot = value; } }

        /// <summary>
        /// Query or setting of the health of the robot.
        /// </summary>
        public int Health { get { return _health; } set { _health = value; } }

        /// <summary>
        /// Query or setting of the coordinates the robot wants to connect.
        /// </summary>
        public XYcoordinates? WantsToConnectTo { get { return _wantsToConnectTo; } set { _wantsToConnectTo = value; } }

        /// <summary>
        /// Query or setting of the cube's coordinates the robot owns.
        /// </summary>
        public XYcoordinates? OwnCube { get { return _ownCube; } set { _ownCube = value; } }

        /// <summary>
        /// Query or setting of the direction of the robot movement.
        /// </summary>
        public Direction? WantsToMoveToDirection { get { return _wantsToMoveToDirection; } set { _wantsToMoveToDirection = value; } }

        /// <summary>
        /// Query or setting of whether the robot is in Team1.
        /// </summary>
        public bool Player1 { set { _player1 = value; } get { return _player1; } }

        /// <summary>
        /// Query or setting of the robot's team.
        /// </summary>
        public Team Team { set { _team = value; } get { return _team!; } }

        /// <summary>
        /// Query of the connected cubes' health.
        /// </summary>
        public List<int> AllHealth() { return _healthConnected; }

        /// <summary>
        /// Query of the connected cubes' color.
        /// </summary>
        public List<Color> AllColor() { return _colorConnected; }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the Robot class.
        /// </summary>
        /// <param name="x">The X coordinate of the robot</param>
        /// <param name="y">The y coordinate of the robot</param>
        /// <param name="direction">The direction of the robot.</param>
        /// <param name="robotNumber">The number of the robot.</param>
        public Robot(int x, int y, Direction direction, int robotNumber)
        {
            _x = x;
            _y = y;
            _direction = direction;
            _connected = new List<XYcoordinates>();
            _healthConnected = new List<int>();
            _colorConnected = new List<Color>();
            _connectedRobot = -1;
            _health = 3;
            _robotNumber = robotNumber;
            if (_robotNumber < 4)
            {
                _player1 = true;
            }
            else
            {
                _player1 = false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds new cube connection to the robot.
        /// </summary>
        /// <param name="tuple">The X and Y coordinates of the cube the robot want to connect.</param>
        public void AddConnection(XYcoordinates tuple)
        {
            _connected.Add(tuple);
        }

        /// <summary>
        /// Delets a cube connection.
        /// </summary>
        /// <param name="tuple">The X and Y coordinates of the cube the robot wants to delete.</param>
        public void DeleteConnection(XYcoordinates tuple)
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                if (_connected[i].Equals(tuple))
                {
                    _connected.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Decreases the health of the robot by one.
        /// </summary>
        public void DecreaseHealth()
        {
            if (_health <= 0)
            {
                throw new ArgumentOutOfRangeException("The health can't be less than 0.");
            }
            _health -= 1;
        }

        /// <summary>
        /// Checks if the robot is connected to a cube.
        /// </summary>
        /// <param name="tuple">The X and Y coordinates of the cube the robot wants to check.</param>
        /// <returns>True, if is connected to the cube, else false.</returns>
        public bool IsConnected(XYcoordinates tuple)
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                if (_connected[i].Equals(tuple))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clears all connections of the robot.
        /// </summary>
        public void ClearConnections()
        {
            _connected.Clear();
            _healthConnected.Clear();
            _colorConnected.Clear();
        }

        /// <summary>
        /// Query of all connections of the robot.
        /// </summary>
        public List<XYcoordinates> AllConnections()
        {
            return _connected;
        }

        /// <summary>
        /// Updates the X coordinates of the connected cubes if the robot moves east.
        /// </summary>
        public void ToEast()
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                _connected[i].X++;
            }
        }

        /// <summary>
        /// Updates the X coordinates of the connected cubes if the robot moves west.
        /// </summary>
        public void ToWest()
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                _connected[i].X--;
            }
        }

        /// <summary>
        /// Updates the Y coordinates of the connected cubes if the robot moves north.
        /// </summary>
        public void ToNorth()
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                _connected[i].Y--;
            }
        }

        /// <summary>
        /// Updates the Y coordinates of the connected cubes if the robot moves south.
        /// </summary>
        public void ToSouth()
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                _connected[i].Y++;
            }
        }

        /// <summary>
        /// Updates the X and Y coordinates of the connected cubes if the robot rotates clockwise.
        /// </summary>
        public void RotateClockwise()
        {
            for (int i = 0; i < _connected.Count(); i++)
            {
                int temp = _connected[i].X;
                _connected[i].X = _x + _y - _connected[i].Y;
                _connected[i].Y = -_x + _y + temp;
            }
        }

        /// <summary>
        /// Updates the X and Y coordinates of the connected cubes if the robot rotates counter clockwise.
        /// </summary>
        public void RotateCounterClockwise()
        {
            for (int i = 0; i < _connected.Count(); i++)
            {
                int temp = _connected[i].X;
                _connected[i].X = _x - _y + _connected[i].Y;
                _connected[i].Y = _x + _y - temp;
            }
        }

        /// <summary>
        /// Setting of the robot's X and Y coordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the robot</param>
        /// <param name="y">The y coordinate of the robot</param>
        public void SetXY(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException();
            }
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Adds the of the robot's X and Y coordinates.
        /// </summary>
        /// <param name="health">The health of the cube.</param>
        /// <param name="color">The color  of the cube.</param>
        public void addHealthColor(int health, Color color)
        {
            _healthConnected.Add(health);
            _colorConnected.Add(color);
        }

        /// <summary>
        /// Query the of the cube's health.
        /// </summary>
        /// <param name="i">The index of cube we want to get its health.</param>
        public int getHealthAt(int i) { return _healthConnected[i]; }

        /// <summary>
        /// Query the of the cube's color.
        /// </summary>
        /// <param name="i">The index of cube we want to get its color.</param>
        public Color getColorAt(int i) { return _colorConnected[i]; }

        #endregion

    }

    /// <summary>
    /// Robots RobotEventArgs type.
    /// </summary>
    public class RobotEventArgs : EventArgs
    {
        #region Fields 

        private bool _canExecute; // whether the robot can execute the action
        private int _cordX; // the x coordinate of the robot
        private int _cordY; // the y coordinate of the robot
        private Direction _direction; // the direction of the robot

        #endregion

        #region Properties

        /// <summary>
        /// Query of the whether the robot can execute the action.
        /// </summary>
        public bool CanExecute { get { return _canExecute; } }

        /// <summary>
        /// Query of the X coordinate of the robot.
        /// </summary>
        public int CordX { get { return _cordX; } }

        /// <summary>
        /// Query of the Y coordinate of the robot.
        /// </summary>
        public int CordY { get { return _cordY; } }

        /// <summary>
        /// Query of the direction of the robot.
        /// </summary>
        public Direction Direction { get { return _direction; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiation of the RobotEventArgs class.
        /// </summary>
        /// <param name="x">The X coordinate of the robot</param>
        /// <param name="y">The y coordinate of the robot</param>
        public RobotEventArgs(int cordX, int cordY) { _cordX = cordX; _cordY = cordY; }

        /// <summary>
        /// Instatntiation of the RobotEventArgs class.
        /// </summary>
        /// <param name="direction">The direction of the robot</param>
        public RobotEventArgs(Direction direction) { _direction = direction; }

        /// <summary>
        /// Instatntiation of the RobotEventArgs class.
        /// </summary>
        /// <param name="canExecute">Whether the robot can execute the action</param>
        public RobotEventArgs(bool canExecute) { _canExecute = canExecute; }

        #endregion

    }

    /// <summary>
    /// Robots ActionEventArgs type.
    /// </summary>
    public class ActionEventArgs : EventArgs
    {
        #region Fields

        private Robot _robot; // the robot who completes the action
        private Direction _direction; // the direction of the action
        private Action _action; // the type of the action
        private bool _canExecute; // whether the robot can execute the action

        #endregion

        #region Properties

        /// <summary>
        /// Query or setting of the robot who executes the action.
        /// </summary>
        public Robot Robot { get { return _robot; } set { _robot = value; } }

        /// <summary>
        /// Query or setting of the action's direction.
        /// </summary>
        public Direction Direction { get { return _direction; } set { _direction = value; } }

        /// <summary>
        /// Query or setting of the action's type.
        /// </summary>
        public Action Action { get { return _action; } set { _action = value; } }

        /// <summary>
        /// Query or setting of whether the robot can execute the action.
        /// </summary>
        public bool CanExecute { get { return _canExecute; } set { _canExecute = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the ActionEventArgs class.
        /// </summary>
        /// <param name="robot">Robot who completes the action.</param>
        /// <param name="direction">Direction of the action.</param>
        /// <param name="action">Type of the action.</param>
        /// <param name="canExecute">Whether the robot can execute the action.</param>
        public ActionEventArgs(Robot robot, Direction direction, Action action, bool canExecute)
        {
            _robot = robot;
            _direction = direction;
            _action = action;
            _canExecute = canExecute;
        }

        #endregion

    }

    /// <summary>
    /// Robots XYcoordinates type.
    /// </summary>
    public class XYcoordinates
    {
        #region Fields

        private int _x;
        private int _y;

        #endregion

        #region Properties

        /// <summary>
        /// Query or setting of the X coordinate.
        /// </summary>
        public int X { get { return _x; } set { _x = value; } }

        /// <summary>
        /// Query or setting of the Y coordinate.
        /// </summary>
        public int Y { get { return _y; } set { _y = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the XYcoordinates class.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public XYcoordinates(int x, int y)
        {
            _x = x;
            _y = y;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Override of Equals.
        /// </summary>
        /// <param name="obj">Object we want to check.</param>
        /// <returns>True, if is equal else false.</returns>
        public override bool Equals(Object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                XYcoordinates p = (XYcoordinates)obj;
                return (X == p.X) && (Y == p.Y);
            }
        }

        #endregion
    }

    /// <summary>
    /// Robots Action type.
    /// </summary>
    public enum Action
    {
        Wait, Clean, Move, Turn, DisconnectRobot, ConnectRobot, DisconnectCubes, ConnectCubes
    }
}
