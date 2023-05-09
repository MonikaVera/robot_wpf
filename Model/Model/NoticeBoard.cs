using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class NoticeBoard
    {/*
             private ExtraTask eTask;
             public ExtraTask ETask;
             private List<ExtraTask> extraTasks;
             public NoticeBoard(int a) { eTask.GenerateTasks(a, a); }
             public void addExtraTask(ExtraTask extraTask) {
                 extraTask.GenerateTasks(3, 3);
                 extraTasks.Add(extraTask); 
             }
           
            public static bool sameF(Field out[,])
            {
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        if (eTask[i, j] is out[i, j])
                                return false;
                return true;
            }
            public bool isExtraTaskFinished(int time, Field outField[,]) {
                if(time != 0)
                    return false;
                if( sameF(outField) == false )
                    return false;
                 eTask.GenerateTasks(3, 3);
                 //add eTask.points
                 return true; 
             }

    }


           public class ExtraTask
           {*/


        private string _taskName = "";
        private int _deadline;

        public int Deadline { get { return _deadline; } set { _deadline = value; } }
        public string TaskName { get { return _taskName; } set { _taskName = value; } }

        public NoticeBoard() { GenerateTasks(3, 3); }

        private int _points;
        private Field[,] _fields = new Field[3, 3];
        public int TaskReward { get { return _points; } set { _points = value; } }
        public Field[,] Fields { get { return _fields; } set { _fields = value; } }

        private readonly Random rand = new Random();

        public void GenerateTasks(int width, int height)
        {

            for (int i = 0; i < width; i++) // minden mezőt üresnek inicializál
                for (int j = 0; j < height; j++)
                {
                    Field field = new Empty(i, j);
                    _fields[i, j] = field;
                }

            int cubeNr = rand.Next(3, 6);
            _points = cubeNr;

            int generCubeNr = 0;

            //color
            int col = rand.Next(0, 8);
            Field field2 = new Cube(1, 1, 1, (Color)(col % 8));
            _fields[1, 1] = field2;
            generCubeNr++;
            if(cubeNr > 4)
            {
                generCubeNr++;
                col = rand.Next(0, 8);
                field2 = new Cube(1, 0, 1, (Color)(col % 8));
                _fields[1, 0] = field2;

                _deadline = 1;
                _taskName = "Hard";
            }
            if(cubeNr == 3)
            {
                _deadline = 1;
                _taskName = "Easy";
            }
            if (cubeNr == 4)
            {
                _deadline = 1;
                _taskName = "Hard";
            }

            while (generCubeNr < cubeNr)
            {
                int x = ( rand.Next(0, 6) ) % 3;
                int y = ( rand.Next(0, 6) ) % 3;

                if (_fields[x, y] is Empty)
                    if ((x > 0 && _fields[x - 1, y] is Cube)
                        || (x < 2 && _fields[x + 1, y] is Cube)
                        || (y > 0 && _fields[x, y - 1] is Cube)
                        || (y < 0 && _fields[x, y + 1] is Cube))
                    {
                        col = rand.Next(0, 8);
                        field2 = new Cube(x, y, 1, (Color)(col % 8));
                        _fields[x, y] = field2;
                        generCubeNr++;
                    }

            }

            //test
          /*  using (StreamWriter writer = new StreamWriter("C:\\Softtech\\test.txt"))
            {
                for (int i = 0; i < width; i++)
                {
                    writer.Write("\n");
                    for (int j = 0; j < height; j++)
                        if(fields[i, j] is Empty)
                            writer.Write("  ");
                        else
                            writer.Write("C ");
                }

            }*/

        }


    }
}
