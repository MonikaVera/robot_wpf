using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Model;

namespace View.ViewModel
{
    public class ViewModelField : ViewModelBase
    {
        private int indX;
        private int indY;
        private Color color;
        private string? picture;
        public Color setColor { get { return color; }  set { color = value; } }
        public int IndY { get { return indY; } set { indY = value; } }
        public int IndX { get { return indX; } set { indX = value; } }
        public string Picture { get { return picture; } set { picture = value; } }
        /// <summary>
        /// Mezőváltoztató parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand? FieldChangeCommand { get; set; }
    }
}
