using Model.Model;

namespace Model.Persistence
{
    /// <summary>
    /// Robots IDataAccess interface.
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// The loading of the table.
        /// </summary>
        /// <param name="path">The path to the file we want to load from.</param>
        /// <param name="height">The height of the table we want to load.</param>
        /// <param name="width">The width of the table we want to load.</param>
        /// <returns>The table.</returns>
        Board LoadAsync(String path, int height, int width);

        /// <summary>
        /// The saving of the table.
        /// </summary>
        /// <param name="path">The path to the file we want to save to.</param>
        /// <param name="table">The table we want to save.</param>
        Task SaveAsync(String path, Board table);
    }
}
