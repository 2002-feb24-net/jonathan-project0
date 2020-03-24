using System;
using System.Collections.Generic;
using System.Text;

namespace WheyMen.Model
{
    public class OrderModel
    {
        public int ID { get; set; }
        public int CustID { get; set; }
        public int LocID { get; set; }
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
        public Decimal Total { get; set; }
        public string TimeStamp { get; set; }
        public string CustName { get; set; }
        public string LocName { get; set; }

    }
}
