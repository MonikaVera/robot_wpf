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
        Task<Board> LoadAsync(String path);
        Task SaveAsync(String path, Board table);
    }
}
