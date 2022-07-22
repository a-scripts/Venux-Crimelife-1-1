using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Venux
{
    public class GarageVehicle
    {
        public int Id
        {
            get;
            set;
        }

        public int OwnerID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }



        [JsonProperty(PropertyName = "Notice")]
        public string Notice { get; set; }



        public string Plate
        {
            get;
            set;
        } = "";

        public List<int> Keys { get; set; } = new List<int>();

        public string Garage
        {
            get;
            set;
        }
        public int Parked
        {
            get;
            set;
        }

        public int FactionId
        {
            get;
            set;
        }

        public long mileage { get; set; }

        public double fuel { get; set; }

        public GarageVehicle() { }
    }
}
