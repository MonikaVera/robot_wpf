using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Persistence
{
    public class RobotDataException : Exception
    {
        /// <summary>
        /// Kivétel példányosítása.
        /// </summary>
        public RobotDataException(String message) : base(message) { }
    }
}
