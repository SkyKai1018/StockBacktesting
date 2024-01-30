using System;
using System.ComponentModel.DataAnnotations;

namespace StockBacktesting.Models
{
    public class Connection
    {
        //public Connection(int connectionId,string connStr,string remark)
        //{
        //    ConnectionId = connectionId;
        //    ConnStr = connStr;
        //    Remark = remark;
        //}

        [Key]
        public int ConnectionId { get; set; }
        public string ConnStr { get; set; }
        public string Remark { get; set; }
    }

}

