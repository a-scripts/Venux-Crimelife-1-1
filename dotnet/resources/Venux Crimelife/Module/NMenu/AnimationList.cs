using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public class AnimationList
    {
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        [JsonProperty("selectable")]
        public string Selectable
        {
            get;
            set;
        }

        public AnimationList(string name, string selectable)
        {
            this.Name = name;
            this.Selectable = selectable;
        }
    }
}
