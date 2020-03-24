using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

using WheyMen.DAL;
using WheyMenDAL.Library;
using WheyMenIOValidation.Library;

namespace Manager
{
    class LocationMgr
    {
        static IConfiguration _iconfiguration;
        static ILocationDAL DAL;
        public LocationMgr(IConfiguration config)
        {
            _iconfiguration = config;
            var specific_DAL = new LocationDAL(config);
            DAL = specific_DAL;
        }
       
        public void Update(int id,int qty)
        {
            DAL.UpdateInventory(id,qty);    
        }
        public void PrintInventory(int id)
        {
            var listLocationModel = DAL.GetInventory(id);
            Console.WriteLine($"----------\nInventory for Store ID {id}");
            listLocationModel.ForEach(item =>
            {
                Console.WriteLine($"Name:{item.P.Name,20} ID:{item.Id,5} Quantity:{item.Qty,8} Price:{item.P.Price,10}");
            });
            Console.WriteLine("----------");
        }
        public void PrintLocations()
        {
            var listLocationModel = DAL.GetList();
            Console.WriteLine("----------\nAvailable Locations");
            listLocationModel.ForEach(item =>
            {
                Console.WriteLine($"Name:{item.Name} ID:{item.Id}");
            });
            Console.WriteLine("----------");
        }
        //returns positive error code or 0 if valid request
        //-1: insufficient qty
        //-2: nonexistant product
        //@param id: store id
        //@param input:product name string or string of product id
        //@param qty: requested quantity of product
        public int ValidateProd(int store_id,string input, int qty,out Decimal price)
        {
            input = input.Trim();
            int id = -1;
            try
            {
                id = int.Parse(input);
            }
            catch(Exception)
            {
                input = input.ToLower();
            }
            //Find requested product
            var listInventory = DAL.GetInventory(store_id);
            var prod = listInventory.Find(inv => inv.Id == id);
            if (prod == null) prod = listInventory.Find(inv => inv.P.Name.ToLower() == input);
            price = -1;
            if(prod==null)
            {
                Console.WriteLine("Item not found");
                return -2;
            }
            else if(!BusinessValidation.ValidateQuantity(qty,prod.Qty))
            {
                Console.WriteLine("Insufficient stock/Excessive quantity");
                return -1;
            }
            else
            {
                price = prod.P.Price;
                return prod.Id;
            }
            
        }
        //checks if string is int or name, searches locations for matching id or name
        public int ValidateLoc(string input)
        {
            var listLocModel = DAL.GetList();
            int id = -1;
            input = input.Trim();
            //Assume input is either a 4 digit store id or store name
            try
            {
                id=int.Parse(input);
            }
            catch(Exception)
            {
                input = input.ToLower();
            }
            foreach(var loc in listLocModel)
            {
                if (id > 0 && loc.Id==id)
                {
                   return loc.Id;
                }
                else if(loc.Name.ToLower()==input)
                {
                    return loc.Id;
                }
            }
            Console.WriteLine("Location does not exist");
            return -1;

        }
        public void DisplayOptions()
        {
            Console.WriteLine("\n\n");
            while (true)
            {
                Console.WriteLine("1: Display Order History");
                Console.Write("Enter the number for your option: ");
                int choice = Wrappers.ReadInt();
                switch (choice)
                {
                    case 1:
                        return;

                    default:
                        Console.WriteLine("Please enter a valid option.");
                        break;
                }
            }
        }
    }
}