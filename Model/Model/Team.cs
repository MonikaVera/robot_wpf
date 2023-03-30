using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Team
    {
        private int points;
        private Robot[] robots;

        public Team(Robot[] _robots) { robots = _robots; }
        public Robot[] Robots { get { return robots; } }
        public int NewPoints { get { return points; } set { points += value; } }
    }
}
