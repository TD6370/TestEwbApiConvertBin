using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestClientWebApi.Model
{
  
    public class ConvFile
    {
        [JsonProperty("Path")]
        public string Path { set; get; }
        [JsonProperty("TypeF")]
        public string TypeF { set; get; }
    }

    public struct TradeRecord
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("account")]
        public int account { get; set; }
        [JsonProperty("volume")]
        public double volume { get; set; }
        [JsonProperty("comment")]
        public string comment { get; set; }
    }

}
