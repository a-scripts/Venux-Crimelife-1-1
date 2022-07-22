using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public class AktenModule
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public AktenModule(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }
    }
}
