using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Board
    {

        public Board(int width, int height)
        {
            if (height < 0 || width < 0)
                throw new ArgumentOutOfRangeException("One of the table sizes is less than 0.");
            _height = height;
            _width = width;



            _cubes = 0; // kockák kezdőértéke

            _fields = new Field[width, height];
            GenerateTable(width, height);
            //PrintTable(width, height);

        }

        #region Fields

        private int _height;
        private int _width;
        private Field[,] _fields;
        private int _cubes;

        #endregion

        #region Properties

        public Int32 Height { get { return _height; } }

        public Int32 Width { get { return _width; } }

        #endregion

        #region Public Methods

        public Field GetFieldValue(int x, int y)
        {
            if (x < 0 || x >= _fields.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fields.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fields[x, y];
        }

        public void SetValue(int x, int y, Field field)
        {
            if (x < 0 || x >= _fields.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fields.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            _fields[x, y] = field;
        }

        #endregion

        #region Private Methods

        private void PrintTable(int width, int height)
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\Mathe Arnold\\Desktop\\proba1.txt"))
            {

                for (int j = 0; j < height; j++)
                {
                    writer.WriteLine(" ");
                    for (int i = 0; i < width; i++)
                    {
                        if (_fields[i, j] is Empty)
                        {
                            writer.Write("  ");
                        }
                        else if (_fields[i, j] is Obstacle)
                        {
                            writer.Write("O ");
                        }
                        else if (_fields[i, j] is Exit)
                        {
                            writer.Write("E ");
                        }
                        else if (_fields[i, j] is Robot)
                        {
                            writer.Write("R ");
                        }
                        else if (_fields[i, j] is Cube)
                        {
                            writer.Write("C ");
                        }

                    }
                }
            }

        }

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
                } while ((x != 1 || y != 1) && !(GetFieldValue(x, y) is Empty));


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
                } while ((x != 1 || y != 1) && !(GetFieldValue(x, y) is Empty));


                Field field1 = new Cube(x, y, rnd.Next(1, 6), (Color)(_cubes % 8));
                this.SetValue(x, y, field1);
                _cubes++;
            }



        }

        #endregion



    }

    public struct RelDistance
    {
        int x;
        int y;
    }

    public enum Angle
    {
        Clockwise, CounterClockwise
        //_90, _180, _270, _360
    }
}


