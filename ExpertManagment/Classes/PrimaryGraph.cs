using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
public class PrimaryGraph
{
    public PrimaryGraph(string verticle_name, string verticle_id, double weight, bool is_used)
    {
        ID = verticle_id; Name = verticle_name; Weight = weight;
    }

    public string _id;
    public string ID { get { return _id; } set { _id = value; } }

    public string _name;
    public string Name { get { return _name; } set { _name = value; } }

    public double _weight;
    public double Weight { get { return _weight; } set { _weight = value; } }

    public class Verticle
    {
        [JsonProperty("verticle_id")]
        public string verticle_id { get; set; }

        [JsonProperty("verticle")]
        public string verticle { get; set; }

        [JsonProperty("connections")]
        public List<Connections> connections { get; set; }
    }
    public class Connections
    {
        [JsonProperty("connectedTo")]
        public string connectedTo { get; set; }

        [JsonProperty("strength")]
        public double strength { get; set; }
    }
}

