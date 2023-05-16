

namespace Model.Persistence
{
    /// <summary>
    /// Robots RobotDataException type.
    /// </summary>
    public class RobotDataException : Exception
    {
        /// <summary>
        /// Instantiation of the RobotDataException class.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public RobotDataException(String message) : base(message) { }
    }
}
