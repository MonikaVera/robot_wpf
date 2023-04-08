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
        private List<Direction> _connected;
        private Team? _team;
        private int _x;
        private int _y;
        #endregion

        #region Properties
        public Robot(int x, int y, Direction direction) { _x = x; _y = y; _direction = direction; }
        public Direction Direction { set { _direction = value; } get { return _direction; } }
        public void addConnection(Direction dir)
        {
            _connected.Add(dir);
        }

        public void deleteConnection(Direction dir)
        {
            for(int i=0; i<_connected.Count; i++)
            {
                if (_connected[i] == dir)
                {
                    _connected.RemoveAt(i);
                }
            }
        }

        public bool isConnected(Direction dir)
        {
            for(int i=0; i< _connected.Count; i++)
            {
                if (_connected[i]==dir)
                {
                    return true;
                }
            }
            return false;
        }
        public int X { set { _x = value; } get { return _x; } }
        public int Y { set { _y = value; } get { return _y; } }
        public Team Team { set { _team = value; } get { return _team; } }
        public void SetXY(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException();
            }
            _x = x;
            _y = y;
        }

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
}


