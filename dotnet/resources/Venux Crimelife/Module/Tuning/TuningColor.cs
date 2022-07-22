﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public class TuningColor
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int ColorId { get; set; }

        public TuningColor(string Label, string Name, int ColorId, int Price)
        {
            this.Label = Label;
            this.Name = Name;
            this.ColorId = ColorId;
            this.Price = Price;
        }
    }
}
