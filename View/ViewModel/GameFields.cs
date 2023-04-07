using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.ViewModel
{
    /// <summary>
    /// PlayerMode játékmező típusa.
    /// </summary>
    public class GameFields : ViewModelBase
    {

        #region Fields

        private String _text = String.Empty;
        private String? _image = null;
        private Int32 _x;
        private Int32 _y;
        private Int32 _number;
        private Boolean _isLocked;
        private DelegateCommand? _stepCommand;

        #endregion

        #region Properties, Gettes, Setters

        /// <summary>
        /// Felirat lekérdezése
        /// </summary>
        public String Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Kép relatív elérési útvonal lekérdezése
        /// </summary>
        public String? Image
        {
            get { return _image; }
        }

        /// <summary>
        /// Zárolás lekérdezése
        /// </summary>
        public Boolean IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Kép és szöveg beállítása
        /// </summary>
        public void TextAndImage(Field field)
        {

            if (field is Empty) // ha üres a mező
            {
                _image = null;
                _text = String.Empty;
                OnPropertyChanged(nameof(Text));
                OnPropertyChanged(nameof(Image));
            }
            else if (field is Obstacle) // ha akadály a mező
            {
                _image = "obs.jpg";
                _text = "O";
                OnPropertyChanged(nameof(Text));
                OnPropertyChanged(nameof(Image));
            }
            else if (field is Cube) // ha kocka a mező
            {
                _image = "cube.jpg";
                _text = "K";
                OnPropertyChanged(nameof(Text));
                OnPropertyChanged(nameof(Image));
            }
            else if (field is Exit) // ha exit a mező
            {
               // _image = "exit.jpg";
                _text = "E";
                OnPropertyChanged(nameof(Text));
                OnPropertyChanged(nameof(Image));
            }
           /* else if (field is Robot) // ha játékos a mező
            {
                _image = "robot.jpg";
                _text = "R";
                OnPropertyChanged(nameof(Text));
                OnPropertyChanged(nameof(Image));
            }*/

        }

        /// <summary>
        /// Függőleges koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Vízszintes koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Sorszám lekérdezése.
        /// </summary>
        public Int32 Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Lépés parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand? StepCommand
        {
            get { return _stepCommand; }
            set
            {
                if (_stepCommand != value)
                {
                    _stepCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

    }
}
