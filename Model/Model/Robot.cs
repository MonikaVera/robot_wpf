using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Robot
    {
        private Direction direction;
        private List<Direction> connected;
        private Team? team;
        private int _X;
        private int _Y;
        public Robot(int x, int y, Direction _direction) { _X = x; _Y = y; direction = _direction; }
        Direction Direction { set { direction = value; } get { return direction; } }
        public int X { set { _X = value; } get { return _X; } }
        public int Y { set { _Y = value; } get { return _Y; } }
        public Team Team { set { team = value; } get { return team; } }
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
