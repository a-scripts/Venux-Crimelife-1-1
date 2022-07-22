using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public class PolicePersonalDataModule
    {
        public string address { get; set; } // Wohnort
        public int phone { get; set; } // Handynummer
        public string membership { get; set; } // Fraktion
        public string info { get; set; } // Bemerkung

        public PolicePersonalDataModule(string address, int phone, string membership, string info)
        {
            this.address = address;
            this.phone = phone;
            this.membership = membership;
            this.info = info;
        }
    }
}
