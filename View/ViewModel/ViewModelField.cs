﻿using System;
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
        
        public string? Picture { get { return _picture; } set { _picture = value; } }
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
        }
        /// <summary>
        /// Mezőváltoztató parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand? FieldChangeCommand { get; set; }

        #endregion
    }
}
