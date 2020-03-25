using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using WheyMen.DAL;
using WheyMenDAL.Library.Model;
using WheyMenDAL.Library;

namespace Manager
{
    class OrderMgr
    {
        static IOrderDAL DAL;
        static IConfiguration _iconfiguration;
        static LocationMgr _locMgr;
        static CustomerMgr _custMgr;
        
        public OrderMgr(IConfiguration config,LocationMgr lm)
        {
            _locMgr = lm;
            _iconfiguration = config;
            var specifc_DAL = new OrderDAL(_iconfiguration);
            DAL = specifc_DAL;
        }
        public void SetCm(CustomerMgr cm)
        {
            _custMgr = cm;
        }
        //returns id of created order
        public int Create(int cid,int lid)
        {
            return DAL.CreateOrder(cid, lid);
        }
        public Decimal AddOrderItem(int oid, int pid, int qty)
        {
            return DAL.AddOrderItem(oid, pid, qty);
        }
        //Confirm order existence
        public int ValidateOrder(string input)
        {
            try
            {
                return DAL.ValidateOrder(int.Parse(input.Trim()));
            }
            catch(FormatException)
            {
                return -1;
            }
        }

        //Attempts to parse input as int
        //Validates input either as an id or name with the respective manager according to the mode
        public bool ValidateSearchParam(int mode, string input,out int id)
        {
            id = -1;
            switch (mode)
            {
                
                case 4:
                case -1:
                    return true;
                case 1:
                    if ((id=_locMgr.ValidateLoc(input))>0) return true;
                    return false;
                case 2:
                    if ((id=_custMgr.ValidateCustomer(input)) > 0) return true;
                    return false;
                case 3:
                    if ((id=ValidateOrder(input)) > 0) return true;
                    return false;
            }
            return false;
        }

        public string GetSearchParam(int mode)
        {
            if (mode < 0 || mode>3) return "";
            else if(mode==1)
            {
                Console.WriteLine("Enter location name or ID");
            }
            else if(mode==2)
            {
                Console.WriteLine("Enter customer name or ID");
            }
            else if(mode==3)
            {
                Console.WriteLine("Enter order ID");
            }
            return Console.ReadLine().Trim();
        }

        public void PrintOrderItems(Order order)
        {
            Console.WriteLine("Order contents:");
            if (order != null)
            {
                foreach (var ord in order.OrderItem)
                {
                    Console.WriteLine($"\tProduct Name:{ord.P.P.Name} Quantity:{ord.Qty}");
                }
            }
            else
            {
                Console.WriteLine("Order empty");
            }

        }

        public void PrintOrder(Order ord)
        {
            Console.WriteLine($"Order ID:{ord.Id,6} Customer:{ord.Cust.Name,5} {ord.Cust.LastName} Location:{ord.Loc.Name,9} Price: {ord.Total,5}, Order Received:{ord.Timestamp,15}");
            PrintOrderItems(ord);
        }
        void GetOrderParam(List<Order> ordList)
        {
            Console.WriteLine("1: Earliest\n2: Latest\n3: Cheapest\n4: Most Expensive\n5: Default - ID");
            Console.WriteLine("Choose an option to sort by or enter to sort by order ID");
            try
            {
                PrintSortedOrders(int.Parse(Console.ReadLine().Trim()),ordList);
            }
            catch
            {
                return;
            }
        }

        public void PrintSortedOrders(int order_by,List<Order> orderList)
        {
            IEnumerable<Order> sortedOrdersList = null; 
            switch(order_by)
            {
                case 1:
                    sortedOrdersList = orderList.OrderBy(o => o.Timestamp);
                    //search_param = "earliest";
                    break;
                case 2:
                    //search_param = "latest";
                    sortedOrdersList = orderList.OrderByDescending(o => o.Timestamp);
                    break;
                case 3:
                    sortedOrdersList = orderList.OrderBy(o => o.Total);
                    //search_param = "cheapest";
                    break;
                case 4:
                    sortedOrdersList = orderList.OrderByDescending(o => o.Total);
                    break;
            }
            foreach(var ords in sortedOrdersList)
            {
                PrintOrder(ords);
            }

        }
     
        //@mode: method to order by, coded as follows
        //1: search by loc
        //2: search by cust
        //3: search by specific order id
        //default: print all orders
        //@search_param: 
        public void PrintOrders(int mode)
        {
            string search_param = GetSearchParam(mode);
            if (ValidateSearchParam(mode, search_param,out int id)) //checks if search param is a valid customer, or order location id/name
            {
                var ordList = DAL.GetOrders(id.ToString(), mode);
                GetOrderParam(ordList);
            }
            else
            {
                Console.WriteLine("Name or ID not valid");
            }
        }
       
        public void DisplayOptions()
        {
            Console.WriteLine("1: Search orders by location\n2: Search orders by customer\n3: Get order details\n4: Print all orders");
            Console.WriteLine("Enter the number for your option:");
            while(true)
            {
                int choice = Wrappers.ReadInt();
                switch (choice)
                {
                    case 1:
                        PrintOrders(1);
                        return;
                    case 2:
                        PrintOrders(2);
                        return;
                    case 3:
                        PrintOrders(3);
                        return;
                    default:
                        PrintOrders(0);
                        return;

                }
            }
        }
    }
}