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
        private int _teamNumber;
        public int TeamNumber { get { return _teamNumber; } set { _teamNumber = value; } }

        public Team(Robot[] robots, int number, int teamNumber) { 
            _robots = robots; 
            _numberOfRobots = number;
            _teamNumber= teamNumber;
        }
        public Robot[] Robots { get { return _robots; } }

        public Robot? GetRobotByNum(int num)
        {
            foreach(Robot r in _robots)
            {
                if(r.RobotNumber==num)
                {
                    return r;
                }
            }
            return null;
        }

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
