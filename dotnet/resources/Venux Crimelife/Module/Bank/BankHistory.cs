using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public class BankHistory
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public BankHistory() { }
    }
}
