using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public class TuningPart
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Index { get; set; }
        public int PartId { get; set; }

        public TuningPart(string Label, string Name, int Index, int PartId, int Price)
        {
            this.Label = Label;
            this.Name = Name;
            this.Index = Index;
            this.Price = Price;
            this.PartId = PartId;
        }
    }
}
