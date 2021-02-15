using System;
using System.Collections.Generic;

#nullable disable

namespace StockPlaygroundMVC
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int UserRole { get; set; }
        public int? UserWatchlistId { get; set; }
    }
}
