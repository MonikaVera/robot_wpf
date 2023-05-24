namespace Model.Model
{
    /// <summary>
    /// Robots NoticeBoard type.
    /// </summary>
    public class NoticeBoard
    {

        #region Fields

        private string _taskName = ""; // the name of the task
        private int _deadline; // the deadline of the task
        private int _points; // points for the task
        private Field[,] _fields = new Field[3, 3]; // the specification of the task
        private readonly Random _rand = new Random(); // a random number

        #endregion

        #region Properties

        /// <summary>
        /// Query or setting of the task's deadline.
        /// </summary>
        public int Deadline { get { return _deadline; } set { _deadline = value; } }

        /// <summary>
        /// Query or setting of the task's name.
        /// </summary>
        public string TaskName { get { return _taskName; } set { _taskName = value; } }

        /// <summary>
        /// Query or setting of the task's points.
        /// </summary>
        public int TaskReward { get { return _points; } set { _points = value; } }

        /// <summary>
        /// Query or setting of the task's specification.
        /// </summary>
        public Field[,] Fields { get { return _fields; } set { _fields = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the NoticeBoard class.
        /// </summary>
        public NoticeBoard() { GenerateTasks(3, 3); }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generating a new task.
        /// </summary>
        /// <param name="width">The width of the notice board's specification</param>
        /// <param name="height">The height of the notice board's specification</param>
        public void GenerateTasks(int width, int height)
        {

            for (int i = 0; i < width; i++) // minden mezőt üresnek inicializál
                for (int j = 0; j < height; j++)
                {
                    Field field = new Empty(i, j);
                    _fields[i, j] = field;
                }

            int cubeNr = _rand.Next(2, 5);
            _points = cubeNr;

            int generCubeNr = 0;

            //color
            int col = _rand.Next(0, 8);
            Field field2 = new Cube(1, 1, 1, (Color)(col % 8));
            _fields[1, 1] = field2;
            generCubeNr++;
            if (cubeNr == 5)
            {
                generCubeNr++;
                col = _rand.Next(0, 8);
                field2 = new Cube(1, 0, 1, (Color)(col % 8));
                _fields[1, 0] = field2;

                _deadline = 40;
                _taskName = "Hard";
            }
            else if (cubeNr == 2)
            {
                _deadline = 21;
                _taskName = "Easy";
            }
            else if (cubeNr == 3)
            {
                _deadline = 27;
                _taskName = "Medium";
            }
            else if (cubeNr == 4)
            {
                _deadline = 32;
                _taskName = "Medium";
            }

            while (generCubeNr < cubeNr)
            {
                int x = (_rand.Next(0, 6)) % 3;
                int y = (_rand.Next(0, 6)) % 3;

                if (_fields[x, y] is Empty)
                    if ((x > 0 && _fields[x - 1, y] is Cube)
                        || (x < 2 && _fields[x + 1, y] is Cube)
                        || (y > 0 && _fields[x, y - 1] is Cube)
                        || (y < 0 && _fields[x, y + 1] is Cube))
                    {
                        col = _rand.Next(0, 8);
                        field2 = new Cube(x, y, 1, (Color)(col % 8));
                        _fields[x, y] = field2;
                        generCubeNr++;
                    }

            }

        }

        #endregion

    }
}
