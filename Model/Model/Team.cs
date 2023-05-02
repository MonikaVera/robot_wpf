using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Team
    {
        private int _points;
        private Robot[] _robots;
        private int _numberOfRobots;

        public Team(Robot[] robots, int number) { 
            _robots = robots; 
            _numberOfRobots = number;
        }
        public Robot[] Robots { get { return _robots; } }

        public Robot GetRobot(int index)
        {
            if (index >= _numberOfRobots)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "The robot index is out of range.");
            }
            return _robots[index];
        }

        public int NewPoints { get { return _points; } set { _points += value; } }

        public bool IsInThisTeam(Robot robot)
        {
            foreach(Robot robot2 in _robots) 
            { 
                if(robot.X==robot2.X && robot.Y==robot2.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
