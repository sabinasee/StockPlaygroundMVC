using System;
using System.Collections.Generic;

#nullable disable

namespace StockPlaygroundMVC
{
    public partial class UserWatchlist
    {
        public long? UserId { get; set; }
        public long WatchlistId { get; set; }
        public string WatchItems { get; set; }
    }
}
