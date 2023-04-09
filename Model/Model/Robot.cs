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
        private List<MyTuple> _connected;
        private Team? _team;
        //private int _x;
        //private int _y;
        #endregion

        #region Properties
        public Robot(int x, int y, Direction direction) { _X = x; _Y = y; _direction = direction; }
        public Robot(int x, int y, Direction direction, List<MyTuple> connected) { 
            _X = x;
            _Y = y;
            _direction = direction;
            _connected = connected;
        }
        public Direction Direction { set { _direction = value; } get { return _direction; } }
        public void AddConnection(MyTuple tuple)
        {
            _connected.Add(tuple);
        }

        public void DeleteConnection(MyTuple tuple)
        {
            for(int i=0; i<_connected.Count; i++)
            {
                if (_connected[i].Equals(tuple))
                {
                    _connected.RemoveAt(i);
                }
            }
        }

        public bool IsConnected(MyTuple tuple)
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



        public List<MyTuple> AllConnections()
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

        //public int X { set { _x = value; } get { return _x; } }
        //public int Y { set { _y = value; } get { return _y; } }
        public Team Team { set { _team = value; } get { return _team; } }
        public void SetXY(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException();
            }
            _X = x;
            _Y = y;
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

    public class MyTuple
    {
        public int X;
        public int Y;
        public MyTuple(int x, int y)
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
                MyTuple p = (MyTuple)obj;
                return (X == p.X) && (Y == p.Y);
            }
        }
    }
}


