using System;
using System.ComponentModel.DataAnnotations;

namespace StockBacktesting.Models
{
    public class Connection
    {
        [Key]
        public int ConnectionId { get; set; }
        public string ConnStr { get; set; }
        public string Remark { get; set; }
    }

}

