using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public string? Picture
        {
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
        public int Number { get { return IndY; } }
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
            if (field is None)
            {
                _picture = "";
            }
            else if (field is Empty)
            {
                _picture = "empty.jpg";
            }
            else if (field is Obstacle)
            {
                if (((Obstacle)field).Health > 500)
                {
                    _picture = "water.jpg";
                }
                else if (((Obstacle)field).Health == 1)
                {
                    _picture = "cart1.jpg";
                }
                else if (((Obstacle)field).Health == 2)
                {
                    _picture = "cart2.jpg";
                }
                else if (((Obstacle)field).Health == 3)
                {
                    _picture = "cart3.jpg";
                }
                else if (((Obstacle)field).Health == 4)
                {
                    _picture = "cart4.jpg";
                }
                else if (((Obstacle)field).Health == 5)
                {
                    _picture = "cart5.jpg";
                }

            }
            else if (field is Cube)
            {
                Cube cube = (Cube)field;

                if (cube.CubeColor == Color.RED)
                {
                    if (cube.IsConnected)
                        _picture = "red_conected.jpg";
                    else
                        _picture = "red.jpg";
                }
                else if (cube.CubeColor == Color.YELLOW)
                {
                    if (cube.IsConnected)
                        _picture = "yellow_conected.jpg";
                    else
                        _picture = "yellow.jpg";
                }
                else if (cube.CubeColor == Color.PINK)
                {
                    if (cube.IsConnected)
                        _picture = "pink_conected.jpg";
                    else
                        _picture = "pink.jpg";
                }
                else if (cube.CubeColor == Color.PURPLE)
                {
                    if (cube.IsConnected)
                        _picture = "purple_conected.jpg";
                    else
                        _picture = "purple.jpg";
                }
                else if (cube.CubeColor == Color.BLUE)
                {
                    if (cube.IsConnected)
                        _picture = "blue_conected.jpg";
                    else
                        _picture = "blue.jpg";
                }
                else if (cube.CubeColor == Color.ORANGE)
                {
                    if (cube.IsConnected)
                        _picture = "brown_conected.jpg";
                    else
                        _picture = "brown.jpg";
                }
                else if (cube.CubeColor == Color.GRAY)
                {
                    if (cube.IsConnected)
                        _picture = "gray_conected.jpg";
                    else
                        _picture = "gray.jpg";
                }
                else //if (cube.CubeColor == Color.GREEN)
                {
                    if (cube.IsConnected)
                        _picture = "green_conected.jpg";
                    else
                        _picture = "green.jpg";
                }

                //_picture = "cube.png";
            }
            else if (field is Exit)
            {
                _picture = ("exit.jpg");
            }
            else if (field is Robot)
            {
                Robot robot = (Robot)field;
                if (robot.Direction == Direction.EAST)
                {
                    if (robot.Player1 == true)
                    {
                        if (robot.AllConnections().Any())
                            _picture = "robot_right_cone.png";
                        else
                            _picture = "robot_right.jpg";
                    }
                    else
                    {
                        if (robot.AllConnections().Any())
                            _picture = "blue_robot_right_conect.png";
                        else
                            _picture = "blue_robot_right.jpg";
                    }

                }
                else if (robot.Direction == Direction.WEST)
                {
                    if (robot.Player1 == true)
                    {
                        if (robot.AllConnections().Any())
                            _picture = "robot_left_cone.png";
                        else
                            _picture = "robot_left.jpg";

                    }

                    else
                    {
                        if (robot.AllConnections().Any())
                            _picture = "blue_robot_left_conect.png";
                        else
                            _picture = "blue_robot_left.jpg";
                    }

                }
                else if (robot.Direction == Direction.SOUTH)
                {
                    if (robot.Player1 == true)
                    {
                        if (robot.AllConnections().Any())
                            _picture = "robot_front_cone.png";
                        else
                            _picture = "robot_front.jpg";
                    }

                    else
                    {
                        if (robot.AllConnections().Any())
                            _picture = "blue_robot_front_conect.png";
                        else
                            _picture = "blue_fobot_front.jpg";
                    }

                }
                else if (robot.Direction == Direction.NORTH)
                {
                    if (robot.Player1 == true)
                    {
                        if (robot.AllConnections().Any())
                            _picture = "robot_back_conect.png";
                        else
                            _picture = "robot_back.jpg";
                    }

                    else
                    {
                        if (robot.AllConnections().Any())
                            _picture = "blue_robot_back_conect.png";
                        else
                            _picture = "blue_robot_back.jpg";
                    }

                }
            }
            OnPropertyChanged(nameof(Picture));
        }
        /// <summary>
        /// Mezőváltoztató parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand? ChooseActionFieldCommand { get; set; }
        public Thickness BorderThickness { get; internal set; }
        public System.Windows.Media.SolidColorBrush? BorderBrush { get; internal set; }
        public System.Windows.Controls.Border? Borders { get; internal set; }
        #endregion
    }
}
