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
        #region Fields
        private int _indX;
        private int _indY;
        private Color _color;
        private string? _picture;
        private string? _text;
        private int _number;
        #endregion

        #region Properties
        public Color setColor { get { return _color; } set { _color = value; } }
        public int IndY
        {
            get { return _indY; }
            set
            {
                if (_indY != value)
                {
                    _indY = value;
                    OnPropertyChanged();
                }
            }
        }

        public int IndX
        {
            get { return _indX; }
            set
            {
                if (_indX != value)
                {
                    _indX = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string? Picture {
            get { return _picture; }
            set
            {
                if (_picture != value)
                {
                    _picture = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Number { get { return _number; } set { _number = value; } }
        public string? Text {
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

        public void SetText(Field field)
        {
            //_text = _number.ToString();
            if (field is Empty)
            {
                _text = " ";
            }
            else if (field is Obstacle)
            {
                _text = ("O");
            }
            else if (field is Cube)
            {
                _text = ("C");
            }
            else if (field is Exit)
            {
                _text = ("E");
            }
            else if (field is Robot)
            {
                _text = ("R");
            }
            OnPropertyChanged(nameof(Text));
        }

        public void SetPicture(Field field)
        {
            //_text = _number.ToString();
            if (field is Empty)
            {
                _picture = "empty.png";
            }
            else if (field is Obstacle)
            {
                _picture = "obstacle.png";
            }
            else if (field is Cube)
            {
                _picture = "cube.png";
            }
            else if (field is Exit)
            {
                _picture = ("empty.png");
            }
            else if (field is Robot)
            {
                Robot robot = (Robot)field;
                if (robot.Direction == Direction.EAST)
                {
                    _picture = "robot_right.png";
                }
                else if (robot.Direction == Direction.WEST)
                {
                    _picture = "robot_left.png";
                }
                else if (robot.Direction == Direction.SOUTH)
                {
                    _picture = "robot_front.png";
                }
                else if (robot.Direction == Direction.NORTH)
                {
                    _picture = "robot_back.png";
                }
            }
            OnPropertyChanged(nameof(Picture));
        }
        /// <summary>
        /// Mezőváltoztató parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand? ChooseActionFieldCommand { get; set; }

        #endregion
    }
}
