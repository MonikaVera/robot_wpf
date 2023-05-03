using Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Persistence
{
    public class MyDataAccess : IDataAccess
    {
        public Board LoadAsync(String path, int height, int width) {

            if (path == null)
                throw new ArgumentNullException("path");

            Board table = new Board(width, height);

            try
            {
                // Read file line by line
                using (StreamReader file = new StreamReader(path))
                {
                    if (file == null)
                        throw new ArgumentNullException("file");

                    string ln;
                    for (int j = 0; j < height; j++)
                        for (int i = 0; i < width; i++)
                        {
                            ln = file.ReadLine();
                            if(ln == null ) continue;
                            //empty
                            if ("empty".Equals(ln))
                                table.SetValue(i, j, new Empty(i, j));
                            //exit
                            else if ("exit".Equals(ln))
                                table.SetValue(i, j, new Exit(i, j));
                            else if ("water".Equals(ln))
                                table.SetValue(i, j, new Obstacle(i, j, 1000));
                            //obstacle
                              else if ("obstacle1".Equals(ln))
                                  table.SetValue(i, j, new Obstacle(i, j, 1));
                              else if ("obstacle2".Equals(ln))
                                  table.SetValue(i, j, new Obstacle(i, j, 2));
                              else if ("obstacle3".Equals(ln))
                                  table.SetValue(i, j, new Obstacle(i, j, 3));
                              else if ("obstacle4".Equals(ln))
                                  table.SetValue(i, j, new Obstacle(i, j, 4));
                              else if ("obstacle5".Equals(ln))
                                  table.SetValue(i, j, new Obstacle(i, j, 5));
                            //cube red
                            else if ("red".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 1, Color.RED));
                        /*    else if ("red2".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 2, Color.RED));
                            else if ("red3".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 3, Color.RED));
                            else if ("red4".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 4, Color.RED));
                            else if ("red5".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 5, Color.RED)); */
                            //cub yellow
                            else if ("yellow".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 1, Color.YELLOW));
                         /*   else if ("yellow2".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 2, Color.YELLOW));
                            else if ("yellow3".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 3, Color.YELLOW));
                            else if ("yellow4".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 4, Color.YELLOW));
                            else if ("yellow5".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 5, Color.YELLOW));*/
                            //cube pink
                            else if ("pink".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 1, Color.PINK));
                        /*    else if ("pink2".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 2, Color.PINK));
                            else if ("pink3".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 3, Color.PINK));
                            else if ("pink4".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 4, Color.PINK));
                            else if ("pink5".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 5, Color.PINK));*/
                            //cube purple
                            else if ("purple".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 1, Color.PURPLE));
                         /*   else if ("purple2".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 2, Color.PURPLE));
                            else if ("purple3".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 3, Color.PURPLE));
                            else if ("purple4".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 4, Color.PURPLE));
                            else if ("purple5".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 5, Color.PURPLE)); */
                            //cube blue
                            else if ("blue".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 1, Color.BLUE));
                         /*   else if ("blue2".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 2, Color.BLUE));
                            else if ("blue3".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 3, Color.BLUE));
                            else if ("blue4".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 4, Color.BLUE));
                            else if ("blue5".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 5, Color.BLUE));*/
                            //cube brown
                            else if ("brown".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 1, Color.ORANGE));
                         /*   else if ("brown2".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 2, Color.ORANGE));
                            else if ("brown3".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 3, Color.ORANGE));
                            else if ("brown4".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 4, Color.ORANGE));
                            else if ("brown5".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 5, Color.ORANGE));*/
                            //CUBE gray
                            else if ("gray".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 1, Color.GRAY));
                         /*   else if ("gray2".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 2, Color.GRAY));
                            else if ("gray3".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 3, Color.GRAY));
                            else if ("gray4".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 4, Color.GRAY));
                            else if ("gray5".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 5, Color.GRAY));*/
                            //cube green
                            else if ("green".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 1, Color.GREEN));
                        /*    else if ("green2".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 2, Color.GREEN));
                            else if ("green3".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 3, Color.GREEN));
                            else if ("green4".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 4, Color.GREEN));
                            else if ("green5".Equals(ln))
                                table.SetValue(i, j, new Cube(i, j, 5, Color.GREEN));*/
                            //robot_right
                           
                            //robot_back
                            else if ("robot_back".Equals(ln))
                                table.SetValue(i, j, new Robot(i, j, Direction.EAST, 0));
                            else if ("b_robot_right".Equals(ln))
                                table.SetValue(i, j, new Robot(i, j, Direction.EAST, 1));
                            //robot_lefy
                            else if ("robot_left".Equals(ln))
                                table.SetValue(i, j, new Robot(i, j, Direction.WEST, 0));
                            else if ("b_robot_left".Equals(ln))
                                table.SetValue(i, j, new Robot(i, j, Direction.WEST, 1));
                            //robot_front
                            else if ("robot_front".Equals(ln))
                                table.SetValue(i, j, new Robot(i, j, Direction.SOUTH, 0));
                            else if ("b_robot_front".Equals(ln))
                                table.SetValue(i, j, new Robot(i, j, Direction.SOUTH, 1));
                            //robot_back
                            else if ("robot_back".Equals(ln))
                                table.SetValue(i, j, new Robot(i, j, Direction.NORTH, 0));
                            else if ("b_robot_back".Equals(ln))
                                table.SetValue(i, j, new Robot(i, j, Direction.NORTH, 1));
                        }

                    file.Close();
                }
            }
            catch // ha bármi hiba történt
            {
                throw new DataException("Error occurred during reading.");
            }
            return table; 
        }
        public async Task SaveAsync(String path, Board table) {
            if (path == null)
                throw new ArgumentNullException("path");
            if (table == null)
                throw new ArgumentNullException("table");

            try
            {
                // kiírjuk a tartalmat a megadott fájlba
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
                                    await Task.Run(() => writer.WriteLine("water"));
                                else
                                    await Task.Run(() => writer.WriteLine("obstacle" + obs.Health));
                            }
                            else if (table.GetFieldValue(i, j) is Cube)
                            {
                                Cube cube = (Cube)table.GetFieldValue(i, j);

                                if (cube.CubeColor == Color.RED)
                                    await Task.Run(() => writer.WriteLine("red"));// +cube.Health));
                                else if (cube.CubeColor == Color.YELLOW)
                                    await Task.Run(() => writer.WriteLine("yellow"));// + cube.Health));
                                else if (cube.CubeColor == Color.PINK)
                                    await Task.Run(() => writer.WriteLine("pink"));// + cube.Health));
                                else if (cube.CubeColor == Color.PURPLE)
                                    await Task.Run(() => writer.WriteLine("purple"));// + cube.Health));
                                else if (cube.CubeColor == Color.BLUE)
                                    await Task.Run(() => writer.WriteLine("blue"));// + cube.Health));
                                else if (cube.CubeColor == Color.ORANGE)
                                    await Task.Run(() => writer.WriteLine("brown"));// + cube.Health));
                                else if (cube.CubeColor == Color.GRAY)
                                    await Task.Run(() => writer.WriteLine("gray"));// + cube.Health));
                                else //if (cube.CubeColor == Color.GREEN)
                                    await Task.Run(() => writer.WriteLine("green"));// + cube.Health));
                                //writer.WriteLine("cube");
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
                                    if(robot.Player==true)
                                        await Task.Run(() => writer.WriteLine("robot_right"));
                                    else
                                        await Task.Run(() => writer.WriteLine("b_robot_right"));
                                }
                                else if (robot.Direction == Direction.WEST)
                                {
                                    if (robot.Player == true)
                                        await Task.Run(() => writer.WriteLine("robot_left"));
                                    else
                                        await Task.Run(() => writer.WriteLine("b_robot_left"));
                                }
                                else if (robot.Direction == Direction.SOUTH)
                                {
                                    if (robot.Player == true)
                                        await Task.Run(() => writer.WriteLine("robot_front"));
                                    else
                                        await Task.Run(() => writer.WriteLine("b_robot_front"));
                                }
                                else if (robot.Direction == Direction.NORTH)
                                {
                                    if (robot.Player == true)
                                        await Task.Run(() => writer.WriteLine("robot_back"));
                                    else
                                        await Task.Run(() => writer.WriteLine("b_robot_back"));
                                }
                            }
                        }
                    }
                    await Task.Run(() => writer.WriteLine("stop"));

                }
            }
            catch // ha bármi hiba történt
            {
                throw new DataException("Error occurred during writing.");
            }
        }
    }
}

