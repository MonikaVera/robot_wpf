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
        private int _number;

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
                _image = "empty.jpg";
            }
            else if (field is Cube)
            {
                Cube cube = (Cube)field;

                if (cube.CubeColor == Color.RED)
                    _image = "red.jpg";
                else if (cube.CubeColor == Color.YELLOW)
                    _image = "yellow.jpg";
                else if (cube.CubeColor == Color.PINK)
                    _image = "pink.jpg";
                else if (cube.CubeColor == Color.PURPLE)
                    _image = "purple.jpg";
                else if (cube.CubeColor == Color.BLUE)
                    _image = "blue.jpg";
                else if (cube.CubeColor == Color.ORANGE)
                    _image = "brown.jpg";
                else if (cube.CubeColor == Color.GRAY)
                    _image = "gray.jpg";
                else if (cube.CubeColor == Color.GREEN)
                    _image = "green.jpg";
               // _image = "cube.jpg";
            }
            OnPropertyChanged(nameof(Image));
        }

        /// <summary>
        /// Mezőváltoztató parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand? FieldChangeCommand { get; set; }
    }
}