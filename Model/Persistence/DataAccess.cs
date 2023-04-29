using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Persistence
{
    public interface IDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <returns>A beolvasott mezőértékek.</returns>

        Board LoadAsync(String path, int height, int width);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        Task SaveAsync(String path, Board table);
    }
}
