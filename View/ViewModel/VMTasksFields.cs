using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.ViewModel
{
    public class VMTasksFields : ViewModelBase
    {
        private string? _text;
        private String? _image = null;
        private Color _color;
        private int _number;

        /// <summary>
        /// Játékos lekérdezése, vagy beállítása.
        /// </summary>
        public Color setColor { get { return _color; } set { _color = value; } }

        /// <summary>
        /// Oszlop lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Sor lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y { get; set; }
        public int Number { get { return _number; } set { _number = value; } }
        public string? Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SetText(Field field)
        {
            //_text = _number.ToString();
            if (field is Empty)
            {
                _text = " ";
            }
            else if (field is Cube)
            {
                //_image = "cube.jpg";
                _text = ("C");
            }

            OnPropertyChanged(nameof(Text));
        }

        public void SetImage(Field field)
        {
            //_text = _number.ToString();
            if (field is Empty)
            {
                _image = "empty.png";
            }
            else if (field is Obstacle)
            {
                _image = "obstacle.png";
            }
            else if (field is Cube)
            {
                _image = "cube.png";
            }
            else if (field is Exit)
            {
                _image = ("empty.png");
            }
            OnPropertyChanged(nameof(Image));
        }

        /// <summary>
        /// Mezőváltoztató parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand? FieldChangeCommand { get; set; }
    }
}