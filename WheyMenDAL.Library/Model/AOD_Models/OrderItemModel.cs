using System;
using System.Collections.Generic;
using System.Text;

namespace WheyMen.Model
{
    public class OrderItemModel
    {
        public int OID { get; set; }
        public int PID { get; set; }
        public int Qty { get; set; }
        public string ProdName { get; set; }
    }
}
