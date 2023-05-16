

namespace Model.Model
{
    /// <summary>
    /// Robots Board type.
    /// </summary>
    public class Board
    {

        #region Fields

        private int _height; // height of the table
        private int _width; // width of the table
        private Field[,] _fields; // the fields of the table
        private int _cubes; // the number of cubes on the field

        #endregion

        #region Properties

        /// <summary>
        /// Query of the height of the table.
        /// </summary>
        public Int32 Height { get { return _height; } }

        /// <summary>
        /// Query of the width of the table.
        /// </summary>
        public Int32 Width { get { return _width; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the Board class.
        /// </summary>
        /// <param name="width">Width of the table.</param>
        /// <param name="height">Height of the table.</param>
        public Board(int width, int height)
        {
            if (height < 0 || width < 0)
                throw new ArgumentOutOfRangeException("One of the table sizes is less than 0.");

            _height = height;
            _width = width;

            _cubes = 0; // kockák kezdőértéke

            _fields = new Field[width, height];
            GenerateTable(width, height); // pálya generálása

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Query of the field's value.
        /// </summary>
        /// <param name="x">The X coordinate of the field. </param>
        /// <param name="y">The Y coordinate of the field. </param>
        /// <returns> The specified field, if the coordinates are valid, else throws exception. </returns>
        public Field GetFieldValue(int x, int y)
        {
            if (x < 0 || x >= _fields.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fields.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fields[x, y];
        }

        /// <summary>
        /// Setting of the field's value.
        /// </summary>
        /// <param name="x">The X coordinate of the field. </param>
        /// <param name="y">The Y coordinate of the field. </param>
        public void SetValue(int x, int y, Field field)
        {
            if (x < 0 || x >= _fields.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fields.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            _fields[x, y] = field;
        }

        /// <summary>
        /// Setting of the field's value.
        /// </summary>
        /// <param name="field">The new value of the field. </param>
        public void SetValueNewField(Field field)
        {
            if (field.X < 0 || field.X >= _fields.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(field.X), "The X coordinate is out of range.");
            if (field.Y < 0 || field.Y >= _fields.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(field.Y), "The Y coordinate is out of range.");

            _fields[field.X, field.Y] = field;
        }

        /// <summary>
        /// Generating new cubes.
        /// </summary>
        /// <param name="number">The number of the new cubes to be generated.</param>
        public void GenerateNewCubes(int number)
        {
            Random rnd = new Random();
            for (int i = 0; i < number; i++)
            {
                int x;
                int y;

                do
                {
                    x = rnd.Next(1, _width - 1);
                    y = rnd.Next(1, _height - 1);
                } while (!(GetFieldValue(x, y) is Empty));


                Field field1 = new Cube(x, y, rnd.Next(1, 6), (Color)(_cubes % 8));
                this.SetValue(x, y, field1);
                _cubes++;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generating the new table.
        /// </summary>
        /// <param name="width">Width of the table.</param>
        /// <param name="height">Height of the table.</param>
        private void GenerateTable(int width, int height)
        {
            for (int i = 1; i < width - 1; i++) // minden mezőt üresnek inicializál
                for (int j = 1; j < height - 1; j++)
                {
                    Field field = new Empty(i, j);
                    this.SetValue(i, j, field);
                }

            //a pálya szélére akadályok, kijáratok kerülnek
            for (int i = 0; i < width; i++)
            {
                Field field1 = new Obstacle(i, 0, 1000);
                Field field2 = new Obstacle(i, height - 1, 1000);
                this.SetValue(i, 0, field1);
                this.SetValue(i, height - 1, field2);
            }

            for (int i = 0; i < height; i++)
            {
                Field field1 = new Obstacle(0, i, 1000);
                Field field2 = new Obstacle(width - 1, i, 1000);
                this.SetValue(0, i, field1);
                this.SetValue(width - 1, i, field2);
            }

            int numberOfExits = Math.Max(width / 5, 1);

            Random rnd = new Random();
            for (int i = 0; i < numberOfExits; i++)
            {
                int index = rnd.Next(0, width);
                Field field1 = new Exit(index, 0);
                this.SetValue(index, 0, field1);
            }

            for (int i = 0; i < numberOfExits; i++)
            {
                int index = rnd.Next(0, width);
                Field field1 = new Exit(index, height - 1);
                this.SetValue(index, height - 1, field1);
            }
            numberOfExits = Math.Max(height / 5, 1);

            for (int i = 0; i < numberOfExits; i++)
            {
                int index = rnd.Next(0, height);
                Field field1 = new Exit(0, index);
                this.SetValue(0, index, field1);
            }

            for (int i = 0; i < numberOfExits; i++)
            {
                int index = rnd.Next(0, height);
                Field field1 = new Exit(width - 1, index);
                this.SetValue(width - 1, index, field1);
            }

            //akadályok, kockák generálása

            int numberOfObstacles = (height - 2) * (width - 2) / 8;
            int numberOfCubes = (height - 2) * (width - 2) / 4;

            for (int i = 0; i < numberOfObstacles; i++)
            {
                int x;
                int y;

                do
                {
                    x = rnd.Next(1, width - 1);
                    y = rnd.Next(1, height - 1);
                } while (!(GetFieldValue(x, y) is Empty));


                Field field1 = new Obstacle(x, y, rnd.Next(1, 6));
                this.SetValue(x, y, field1);
            }


            for (int i = 0; i < numberOfCubes; i++)
            {
                int x;
                int y;

                do
                {
                    x = rnd.Next(1, width - 1);
                    y = rnd.Next(1, height - 1);
                } while (!(GetFieldValue(x, y) is Empty));


                Field field1 = new Cube(x, y, rnd.Next(1, 6), (Color)(_cubes % 8));
                this.SetValue(x, y, field1);
                _cubes++;
            }



        }

        #endregion

    }

    /// <summary>
    /// Robots Angle type.
    /// </summary>
    public enum Angle
    {
        Clockwise, CounterClockwise
    }
}


