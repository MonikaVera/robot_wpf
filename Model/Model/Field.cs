using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Model.Model
{
    public abstract class Field
    {
        protected int _X;
        protected int _Y;
        public int X { set { _X = value; } get { return _X; } }
        public int Y { set { _Y = value; } get { return _Y; } }

    }

    public class Empty : Field
    {
        public Empty(int x, int y) { _X = x; _Y = y; }
    }

    public class Exit : Field
    {
        public Exit(int x, int y) { _X = x; _Y = y; }
    }

    public class Obstacle : Field
    {
        private int health;
        public int Health { get { return health; } }
        public Obstacle(int x, int y, int _health) { _X = x; _Y = y; health = _health; }
        public void DecreaseHealth() {
            health -= 1;
        }
    }

    public class Cube : Field
    {
        private Color color;
        private int health;
        private List<Direction> connected;
        public Color CubeColor { get { return color; } }
        public Cube(int x, int y, int _health, Color _color)
        {
            _X = x;
            _Y = y;
            health = _health;
            color = _color;
        }
        public int Health { get { return health; } }
        public Color Color { get { return color; } }

        public void DecreaseHealth()
        {
            health -= 1;
        }
    }

    public enum Color
    {
        RED, GREEN, BLUE, YELLOW, PURPLE, ORANGE, PINK, GRAY
    }

    public enum Direction
    {
        NORTH, SOUTH, WEST, EAST
    }
}
