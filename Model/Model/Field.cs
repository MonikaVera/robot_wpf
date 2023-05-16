

namespace Model.Model
{
    /// <summary>
    /// Robots Field abstract type.
    /// </summary>
    public abstract class Field
    {
        #region Fields

        protected int _x; // x coordinate of the field
        protected int _y; // y coordinate of the field

        #endregion

        #region Properties

        /// <summary>
        /// Query or setting of the X coordinate of the field.
        /// </summary>
        public int X { set { _x = value; } get { return _x; } }

        /// <summary>
        /// Query or setting of the Y coordinate of the field.
        /// </summary>
        public int Y { set { _y = value; } get { return _y; } }

        #endregion

    }

    /// <summary>
    /// Robots Empty type.
    /// </summary>
    public class Empty : Field
    {
        /// <summary>
        /// Instantiation of the Empty class.
        /// </summary>
        /// <param name="x">The X coordinate of the field</param>
        /// <param name="y">The y coordinate of the field</param>
        public Empty(int x, int y) { _x = x; _y = y; }
    }

    /// <summary>
    /// Robots Exit type.
    /// </summary>
    public class Exit : Field
    {
        /// <summary>
        /// Instantiation of the Exit class.
        /// </summary>
        /// <param name="x">The X coordinate of the field</param>
        /// <param name="y">The y coordinate of the field</param>
        public Exit(int x, int y) { _x = x; _y = y; }
    }

    /// <summary>
    /// Robots Obstacle type.
    /// </summary>
    public class Obstacle : Field
    {
        #region Fields

        private int _health; // the obstacle's health

        #endregion

        #region Properties

        /// <summary>
        /// Query or setting of the health of the field.
        /// </summary>
        public int Health { get { return _health; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the Obstacle class.
        /// </summary>
        /// <param name="x">The X coordinate of the field</param>
        /// <param name="y">The y coordinate of the field</param>
        /// <param name="health">The health of the field</param>
        public Obstacle(int x, int y, int health) { _x = x; _y = y; _health = health; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Decreases the obstacle's health by one.
        /// </summary>
        public void DecreaseHealth() {
            if (_health <= 0)
            {
                throw new ArgumentOutOfRangeException("The health can't be less than 0.");
            }
            _health -= 1;
        }

        #endregion
    }

    /// <summary>
    /// Robots Cube type.
    /// </summary>
    public class Cube : Field
    {
        #region Fields

        private Color _color; //color of the cube
        private int _health; //health of the cube

        #endregion

        #region Properties

        /// <summary>
        /// Query of the health of the field.
        /// </summary>
        public int Health { get { return _health; } }

        /// <summary>
        /// Query of the color of the field.
        /// </summary>
        public Color Color { get { return _color; } }

        /// <summary>
        /// Query of the color of the field.
        /// </summary>
        public Color CubeColor { get { return _color; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the Cube class.
        /// </summary>
        /// <param name="x">The X coordinate of the field</param>
        /// <param name="y">The y coordinate of the field</param>
        /// <param name="health">The health of the field</param>
        ///  <param name="color">The color of the field</param>
        public Cube(int x, int y, int health, Color color)
        {
            _x = x;
            _y = y;
            _health = health;
            _color = color;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Decreases the cube's health by one.
        /// </summary>
        public void DecreaseHealth()
        {
            if (_health <= 0)
            {
                throw new ArgumentOutOfRangeException("The health can't be less than 0.");
            }
            _health -= 1;
        }

        #endregion
    }

    /// <summary>
    /// Robots None type.
    /// </summary>
    public class None : Field
    {

    }

    /// <summary>
    /// Robots Color type.
    /// </summary>
    public enum Color
    {
        RED, GREEN, BLUE, YELLOW, PURPLE, ORANGE, PINK, GRAY
    }

    /// <summary>
    /// Robots Direction type.
    /// </summary>
    public enum Direction
    {
        NORTH, SOUTH, WEST, EAST
    }
}
