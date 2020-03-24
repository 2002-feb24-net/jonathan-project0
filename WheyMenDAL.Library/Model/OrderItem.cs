using System;
using System.Collections.Generic;

namespace WheyMenDAL.Library.Model
{
    public partial class OrderItem
    {
        public int Oid { get; set; }
        public int Qty { get; set; }
        public int Id { get; set; }
        public int? Pid { get; set; }

        public virtual Order O { get; set; }
        public virtual Inventory P { get; set; }
    }
}
