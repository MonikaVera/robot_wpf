using Model.Model;
using System.Data;

namespace Model.Persistence
{
    /// <summary>
    /// Robots RobotDataAccess interface.
    /// </summary>
    public class RobotDataAccess : IDataAccess
    {
        /// <summary>
        /// The loading of the table.
        /// </summary>
        /// <param name="path">The path to the file we want to load from.</param>
        /// <param name="height">The height of the table we want to load.</param>
        /// <param name="width">The width of the table we want to load.</param>
        /// <returns>The table.</returns>
        public Board Load(String path, int height, int width) {

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
              
            Board table = new Board(width, height);

            try
            {
                // read file line by line
                using (StreamReader file = new StreamReader(path))
                {
                    if (file == null)
                    {
                        throw new ArgumentNullException("file");
                    }
                       
                    string ln;

                    for (int j = 0; j < height; j++)
                        for (int i = 0; i < width; i++)
                        {
                            ln = file.ReadLine()!;

                            if(ln == null ) continue;

                            if ("empty".Equals(ln)) //empty
                            {
                                table.SetValue(i, j, new Empty(i, j));
                            }
                            else if ("exit".Equals(ln)) //exit
                            {
                                table.SetValue(i, j, new Exit(i, j));
                            }
                            else if ("water".Equals(ln)) //edge
                            {
                                table.SetValue(i, j, new Obstacle(i, j, 1000));
                            }
                            else if ("obstacle1".Equals(ln)) //obstacle
                            {
                                table.SetValue(i, j, new Obstacle(i, j, 1));
                            }
                            else if ("obstacle2".Equals(ln))
                            {
                                table.SetValue(i, j, new Obstacle(i, j, 2));
                            }
                            else if ("obstacle3".Equals(ln))
                            {
                                table.SetValue(i, j, new Obstacle(i, j, 3));
                            }
                            else if ("obstacle4".Equals(ln))
                            {
                                table.SetValue(i, j, new Obstacle(i, j, 4));
                            }
                            else if ("obstacle5".Equals(ln))
                            {
                                table.SetValue(i, j, new Obstacle(i, j, 5));
                            }
                            else if ("red".Equals(ln))  //cube red
                            {
                                table.SetValue(i, j, new Cube(i, j, 1, Color.RED));
                            }
                            else if ("yellow".Equals(ln))  //cube yellow
                            {
                                table.SetValue(i, j, new Cube(i, j, 1, Color.YELLOW));
                            }
                            else if ("pink".Equals(ln))   //cube pink
                            {
                                table.SetValue(i, j, new Cube(i, j, 1, Color.PINK));
                            }
                            else if ("purple".Equals(ln))  //cube purple
                            {
                                table.SetValue(i, j, new Cube(i, j, 1, Color.PURPLE));
                            }
                            else if ("blue".Equals(ln))  //cube blue
                            {
                                table.SetValue(i, j, new Cube(i, j, 1, Color.BLUE));
                            }
                            else if ("brown".Equals(ln))  //cube brown
                            {
                                table.SetValue(i, j, new Cube(i, j, 1, Color.ORANGE));
                            }
                            else if ("gray".Equals(ln))  //cube gray
                            {
                                table.SetValue(i, j, new Cube(i, j, 1, Color.GRAY));
                            }
                            else if ("green".Equals(ln)) //cube green
                            {
                                table.SetValue(i, j, new Cube(i, j, 1, Color.GREEN));
                            }
                            else if ("robot_right".Equals(ln))  //robot right direction
                            {
                                table.SetValue(i, j, new Robot(i, j, Direction.EAST, 0));
                            } 
                            else if ("b_robot_right".Equals(ln))
                            {
                                table.SetValue(i, j, new Robot(i, j, Direction.EAST, 4));
                            }
                            else if ("robot_left".Equals(ln))  //robot left direction
                            {
                                table.SetValue(i, j, new Robot(i, j, Direction.WEST, 0));
                            }
                            else if ("b_robot_left".Equals(ln))
                            {
                                table.SetValue(i, j, new Robot(i, j, Direction.WEST, 4));
                            }
                            else if ("robot_front".Equals(ln))  //robot front direction
                            {
                                table.SetValue(i, j, new Robot(i, j, Direction.SOUTH, 0));
                            }
                            else if ("b_robot_front".Equals(ln))
                            {
                                table.SetValue(i, j, new Robot(i, j, Direction.SOUTH, 4));
                            }
                            else if ("robot_back".Equals(ln))  //robot back direction
                            {
                                table.SetValue(i, j, new Robot(i, j, Direction.NORTH, 0));
                            }
                            else if ("b_robot_back".Equals(ln))
                            {
                                table.SetValue(i, j, new Robot(i, j, Direction.NORTH, 4));
                            }
                               
                        }

                    file.Close();
                }
            }
            catch // throws exception if the loading was unsuccesful
            {
                throw new DataException("Error occurred during reading.");
            }
            return table; 
        }

        /// <summary>
        /// The saving of the table.
        /// </summary>
        /// <param name="path">The path to the file we want to save to.</param>
        /// <param name="table">The table we want to save.</param>
        public async Task SaveAsync(String path, Board table) {
            if (path == null)
            {
                throw new ArgumentNullException("invalid path");
            }
               
            if (table == null)
            {
                throw new ArgumentNullException("invalid table");
            }
               
            try
            {
                // write the table fields to a file
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int j = 0; j < table.Height; j++)
                    {
                        for (int i = 0; i < table.Width; i++)
                        {
                            if (table.GetFieldValue(i,j) is Empty)
                            {
                                await Task.Run(() => writer.WriteLine("empty"));
                            }
                            else if (table.GetFieldValue(i, j) is Obstacle)
                            {
                                Obstacle obs = (Obstacle)table.GetFieldValue(i, j);

                                if (obs.Health > 500)
                                {
                                    await Task.Run(() => writer.WriteLine("water"));
                                }
                                else
                                {
                                    await Task.Run(() => writer.WriteLine("obstacle" + obs.Health));
                                }
                                  
                            }
                            else if (table.GetFieldValue(i, j) is Cube)
                            {
                                Cube cube = (Cube)table.GetFieldValue(i, j);

                                if (cube.CubeColor == Color.RED)
                                {
                                    await Task.Run(() => writer.WriteLine("red"));
                                } 
                                else if (cube.CubeColor == Color.YELLOW)
                                {
                                    await Task.Run(() => writer.WriteLine("yellow"));
                                } 
                                else if (cube.CubeColor == Color.PINK)
                                {
                                    await Task.Run(() => writer.WriteLine("pink"));
                                } 
                                else if (cube.CubeColor == Color.PURPLE)
                                {
                                    await Task.Run(() => writer.WriteLine("purple"));
                                }
                                else if (cube.CubeColor == Color.BLUE)
                                {
                                    await Task.Run(() => writer.WriteLine("blue"));
                                }
                                else if (cube.CubeColor == Color.ORANGE)
                                {
                                    await Task.Run(() => writer.WriteLine("brown"));
                                }
                                else if (cube.CubeColor == Color.GRAY)
                                {
                                    await Task.Run(() => writer.WriteLine("gray"));
                                }
                                else
                                {
                                    await Task.Run(() => writer.WriteLine("green"));
                                }
                            }
                            else if (table.GetFieldValue(i, j) is Exit)
                            {
                                await Task.Run(() => writer.WriteLine(("exit")));
                            }
                            else if (table.GetFieldValue(i, j) is Robot)
                            {
                                Robot robot = (Robot)table.GetFieldValue(i, j);

                                if (robot.Direction == Direction.EAST)
                                {
                                    if (robot.Player1 == true)
                                    {
                                        await Task.Run(() => writer.WriteLine("robot_right"));
                                    }
                                    else
                                    {
                                        await Task.Run(() => writer.WriteLine("b_robot_right"));
                                    }
                                }
                                else if (robot.Direction == Direction.WEST)
                                {
                                    if (robot.Player1 == true)
                                    {
                                        await Task.Run(() => writer.WriteLine("robot_left"));
                                    }
                                    else
                                    {
                                        await Task.Run(() => writer.WriteLine("b_robot_left"));
                                    }
                                }
                                else if (robot.Direction == Direction.SOUTH)
                                {
                                    if (robot.Player1 == true)
                                    {
                                        await Task.Run(() => writer.WriteLine("robot_front"));
                                    }
                                    else
                                    {
                                        await Task.Run(() => writer.WriteLine("b_robot_front"));
                                    }
                                }
                                else if (robot.Direction == Direction.NORTH)
                                {
                                    if (robot.Player1 == true)
                                    {
                                        await Task.Run(() => writer.WriteLine("robot_back"));
                                    }
                                    else
                                    {
                                        await Task.Run(() => writer.WriteLine("b_robot_back"));
                                    }   
                                }
                            }
                        }
                    }

                    await Task.Run(() => writer.WriteLine("stop"));

                }
            }
            catch // throws exception if the save was unsuccessful
            {
                throw new DataException("Error occurred during writing.");
            }
        }
    }
}

