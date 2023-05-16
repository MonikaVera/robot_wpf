using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    /// <summary>
    /// Robots Team type.
    /// </summary>
    public class Team
    {
        #region Fields

        private int _points; //points the team currently has
        private Robot[] _robots; //robots the team currently has
        private int _numberOfRobots; //number of robots the team currently has
        private int _teamNumber; // the number of the team

        #endregion

        #region Properties

        /// <summary>
        /// Query of the team's number.
        /// </summary>
        public int TeamNumber { get { return _teamNumber; } set { _teamNumber = value; } }

        /// <summary>
        /// Query of the robots inside the team.
        /// </summary>
        public Robot[] Robots { get { return _robots; } }

        /// <summary>
        /// Query or setting of the points of the team.
        /// </summary>
        public int Points { get { return _points; } set { _points += value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiation of the Team class.
        /// </summary>
        /// <param name="robots">Array of robots which will be part of the team.</param>
        /// <param name="number">Number of robots the team will have.</param>
        /// <param name="teamNumber">Identifier of the team.</param>
        public Team(Robot[] robots, int number, int teamNumber)
        {
            _robots = robots;
            _numberOfRobots = number;
            _teamNumber = teamNumber;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Query of a robot.
        /// </summary>
        /// <param name="num">The identifier of the robot.</param>
        /// <returns>The robot, if exists else null.</returns>
        
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

        /// <summary>
        /// Query of a robot.
        /// </summary>
        /// <param name="index">The index of the robot.</param>
        /// <returns>The robot, if exists else throws exception.</returns>
        public Robot GetRobot(int index)
        {
            if (index >= _numberOfRobots)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "The robot index is out of range.");
            }
            return _robots[index];
        }

        /// <summary>
        /// Query of a robot is inside the team.
        /// </summary>
        /// <param name="robot">Robot we want to check</param>
        /// <returns>True, if the robot is inside the team, else false.</returns>
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

        /// <summary>
        /// Removes robot from the team.
        /// </summary>
        /// <param name="robot">Robot we want to remove</param>
        public void RemoveRobotFromTeam(Robot robot)
        {
            _robots = _robots.Where(val => val != robot).ToArray();
        }

        /// <summary>
        /// Query of whether the team is empty.
        /// </summary>
        /// <returns>True, if the team is empty, else false.</returns>
        public bool IsEmptyTeam()
        {
            return !_robots.Any();
        }

        #endregion
    }
}
