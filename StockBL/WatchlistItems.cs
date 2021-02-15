using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockBL
{
    public class WatchlistItems
    {
        [JsonProperty("1. symbol")]
        public string Symbol { get; set; }
        [JsonProperty("2. name")]
        public string Name { get; set; }
        [JsonProperty("4. region")]
        public string Region { get; set; }
        [JsonProperty("5. marketOpen")]
        public string MarketOpen { get; set; }
        [JsonProperty("6. marketClose")]
        public string MarketClose { get; set; }
        public string StockPrice { get; set; }
    }
}
