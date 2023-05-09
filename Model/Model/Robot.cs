using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Robot : Field
    {
        #region Fields
        private Direction _direction;
        private List<XYcoordinates> _connected;
        private List<int> _healthConnected = null!;
        private List<Color> _colorConnected = null!;
        private Team? _team = null!;
        private int _robotNumber;
        private int _connectedRobot;
        private bool _player;
        public XYcoordinates? WantsToConnectTo = null;
        public XYcoordinates? OwnCube = null;
        private int _health;
        
        #endregion
        public int RobotNumber { get { return _robotNumber; } set { _robotNumber = value; } }
        public int ConnectedRobot { get { return _connectedRobot; } set { _connectedRobot = value; } }
        public int Health { get { return _health; } set { _health = value; } }
        public Robot(int x, int y, Direction direction, int robotNumber)
        {
            _X = x;
            _Y = y;
            _direction = direction;
            _connected = new List<XYcoordinates>();
            _healthConnected = new List<int>();
            _colorConnected = new List<Color>();
            _connectedRobot = -1;
            _health = 3;
            _robotNumber = robotNumber;
            if (_robotNumber<4)
            {
                _player = true;
            }
            else
            {
                _player= false;
            }
        }

       
        public Robot(int x, int y, Direction direction, List<XYcoordinates> connected) { 
            _X = x;
            _Y = y;
            _direction = direction;
            _connected = connected;
        }

        #region Properties
        public Direction Direction { set { _direction = value; } get { return _direction; } }
        public bool Player { set { _player = value; } get { return _player; } }
        public void AddConnection(XYcoordinates tuple)
        {
            _connected.Add(tuple);
        }

        public void DeleteConnection(XYcoordinates tuple)
        {
            for(int i=0; i<_connected.Count; i++)
            {
                if (_connected[i].Equals(tuple))
                {
                    _connected.RemoveAt(i);
                }
            }
        }

        public void DecreaseHealth()
        {
            if (_health <= 0)
            {
                throw new ArgumentOutOfRangeException("The health can't be less than 0.");
            }
            _health -= 1;
        }

        public bool IsConnected(XYcoordinates tuple)
        {
            for(int i=0; i< _connected.Count; i++)
            {
                if (_connected[i].Equals(tuple))
                {
                    return true;
                }
            }
            return false;
        }

        public void clearConnections()
        {
            _connected.Clear();
            _healthConnected.Clear();
            _colorConnected.Clear();
        }



        public List<XYcoordinates> AllConnections()
        {
            return _connected;
        }

        public void ToEast()
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                _connected[i].X++;
            }
        }

        public void ToWest()
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                _connected[i].X--;
            }
        }

        public void ToNorth()
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                _connected[i].Y--;
            }
        }

        public void ToSouth()
        {
            for (int i = 0; i < _connected.Count; i++)
            {
                _connected[i].Y++;
            }
        }

        public void RotateClockwise()
        {
            for (int i = 0; i < _connected.Count(); i++)
            {
                int temp = _connected[i].X;
                _connected[i].X = _X + _Y - _connected[i].Y;
                _connected[i].Y = -_X + _Y + temp;
            }
        }

        public void RotateCounterClockwise()
        {
            for (int i = 0; i < _connected.Count(); i++)
            {
                int temp = _connected[i].X;
                _connected[i].X = _X - _Y + _connected[i].Y;
                _connected[i].Y = _X + _Y - temp;
            }
        }

        //public int X { set { _x = value; } get { return _x; } }
        //public int Y { set { _y = value; } get { return _y; } }
        public Team Team { set { _team = value; } get { return _team!; } }
        public void SetXY(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException();
            }
            _X = x;
            _Y = y;
        }

        public void addHealthColor(int health, Color color)
        {
            _healthConnected.Add(health);
            _colorConnected.Add(color);
        }

        public List<int> AllHealth() { return _healthConnected; }
        public List<Color> AllColor() { return _colorConnected;}

        public int getHealthAt(int i) { return _healthConnected[i]; }
        public Color getColorAt(int i) { return _colorConnected[i];}

        #endregion

    }

    public class RobotEventArgs : EventArgs
    {
        private bool _canExecute;
        private int _cordX;
        private int _cordY;
        private Direction _direction;
        public RobotEventArgs(int cordX, int cordY) { _cordX = cordX; _cordY = cordY; }
        public RobotEventArgs(Direction direction) { _direction = direction; }
        public RobotEventArgs(bool canExecute) { _canExecute = canExecute; }
        public bool CanExecute { get { return _canExecute; } }
        public int CordX { get { return _cordX; } }
        public int CordY { get { return _cordY; } }
        public Direction direction { get { return _direction; } }
    }

    public class ActionEventArgs : EventArgs
    {
        public ActionEventArgs(Robot robot, Direction direction, Action action, bool canExecute)
        {
            _robot = robot;
            _direction = direction;
            _action = action;
            _canExecute = canExecute;
        }

        private Robot _robot;
        private Direction _direction;
        private Action _action;
        private bool _canExecute;

        public Robot Robot { get { return _robot; } set { _robot = value; } }

        public Direction Direction { get { return _direction; } set { _direction = value; } }

        public Action Action { get { return _action; } set { _action = value; } }

        public bool CanExecute { get { return _canExecute; } set { _canExecute = value; } }
    }

    public class XYcoordinates
    {
        public int X;
        public int Y;
        public XYcoordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

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
    }

    public enum Action
    {
        Wait, Clean, Move, Turn, DisconnectRobot, ConnectRobot, DisconnectCubes, ConnectCubes
    }
}


