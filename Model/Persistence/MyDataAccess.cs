using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Persistence
{
    public class MyDataAccess : IDataAccess
    {
        Board board; //ez nem kell ide, csak azért van, hogy ne legyen error
        public async Task<Board> LoadAsync(String path) {/*code*/  return board; }
        public async Task SaveAsync(String path, Board table) {; }
    }
}
