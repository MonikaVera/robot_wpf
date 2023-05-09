using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private int _health;
        public int Health { get { return _health; } }
        public Obstacle(int x, int y, int health) { _X = x; _Y = y; _health = health; }
        public void DecreaseHealth() {
            if (_health <= 0)
            {
                throw new ArgumentOutOfRangeException("The health can't be less than 0.");
            }
            _health -= 1;
        }
    }

    public class Cube : Field
    {
        private Color _color;
        private int _health;
        public Color CubeColor { get { return _color; } }
        public Cube(int x, int y, int health, Color color)
        {
            _X = x;
            _Y = y;
            _health = health;
            _color = color;
        }
        public int Health { get { return _health; } }
        public Color Color { get { return _color; } }

        public void DecreaseHealth()
        {
            if (_health <= 0)
            {
                throw new ArgumentOutOfRangeException("The health can't be less than 0.");
            }
            _health -= 1;
        }
    }

    public class None : Field
    {

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
