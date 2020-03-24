using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;

using WheyMenDAL.Library;
using WheyMenDAL.Library.Model;
using WheyMen.DAL;
using WheyMenIOValidation.Library;

namespace Manager
{
    class CustomerMgr
    {
        static int current_id=-1;


        static OrderMgr _ordManager;
        static LocationMgr _locManager;
        static ICustomerDAL DAL;
        public CustomerMgr(IConfiguration config, LocationMgr lm,OrderMgr om)
        {
            CustomerDAL specifc_DAL = new CustomerDAL();
            DAL = specifc_DAL;
            _ordManager = om;
            _locManager = lm;
        }

        static void PrintCust(Customer cust)
        {
            Console.WriteLine($"First Name:{cust.Name}, Last Name:{cust.LastName}, Email: {cust.Email} ");
        }
        static void PrintCustomers()
        {

            var listcustomerModel = DAL.GetList();
            listcustomerModel.ForEach(item =>
            {
                PrintCust(item);
            });
        }

        public string EnterPass()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            return pass;
        }

        public void VerifyUser()
        {
            string pass = "", input = "";
            bool verified = false;
            do
            {
                Console.WriteLine("Enter username or email:");
                input = Console.ReadLine().Trim();
                Console.WriteLine("Enter password:");
                pass = EnterPass();
                if (pass != DAL.VerifyCustomer(input,out current_id))
                {
                    Console.WriteLine("Invalid username/pwd combo");
                }
                else
                {
                    verified = true;
                    Console.WriteLine($"\nLogging in as {input} with id {current_id}");
                }

            } while (!verified);
        }
        public void Login()
        {
            Console.WriteLine("1: Login\n2: Add user");
            int input = Wrappers.ReadInt();
            if (input == 1)
            {
                while(true)
                {
                    try
                    {
                        VerifyUser();
                        return;
                    }
                    catch(NullReferenceException)
                    {
                        Console.WriteLine("Username/email not found");
                    }
                }
            }
            else
            {
                bool user_added = false;
                while(!user_added)
                {
                    user_added = AddCust();
                }
                
            }
        }

        public int ValidateCustomer(string input)
        {
            try
            {
                int id = int.Parse(input.Trim());
                return DAL.ValidateCustomer(id);
            }
            catch(FormatException)
            {
                return DAL.ValidateCustomer(name: input.Trim().Split(" "));
            }
        }
       
        //Prints available locations Prompts for store id
        //Returns -1 if entered id doesnt exist;
        static int GetStoreID()
        {
            _locManager.PrintLocations();
            Console.WriteLine("Enter the name or ID of the location you would like to order from:");
            string input = Console.ReadLine();
            return _locManager.ValidateLoc(input);
        }
        //Prompts for order item, returns false if order is complete, breaking containing while loop
        static bool GetOrderItem(int store_id,int oid)
        {
            //Determine which product and qty
            Console.WriteLine("Enter the name or ID of the product you would like to purchase or 'q' to quit:");
            string input = Console.ReadLine().Trim().ToLower(); 
            if (input == "q") return false;
            
            Console.WriteLine("Enter the quantity or 'q' to quit:");
            int quantity = Wrappers.ReadInt();
            if (quantity == -1) return false;

            Decimal p_price = 0;
            //Validate product quantity, update and place order
            int prod_id = _locManager.ValidateProd(store_id, input, quantity, out p_price);
            if (prod_id >= 0)
            {
                _locManager.Update(prod_id, quantity);
                _ordManager.AddOrderItem(oid,prod_id, quantity);
            }
            else
            {
                Console.WriteLine("Invalid Product or Quantity");
            }
            
            //Prompt for continue
            Console.WriteLine("Would you like to add another item (y/n)?");
            string cont = Console.ReadLine().Trim().ToLower();
            if(cont == "y")
            {
                return true;
            }
            return false;
        }
        static void AddOrder()
        {
            var customerDAL = new CustomerDAL();
            int store_id = GetStoreID();

            if (store_id==-1) return;
            _locManager.PrintInventory(store_id);

            int oid = _ordManager.Create(current_id, store_id);
            Decimal total = 0,price=0;
            while(true)
            {
                if(GetOrderItem(store_id, oid))
                {
                    total += price;
                }
                else
                {
                    break;
                }
            }
        }

        public bool ValidateCustomerInfo(string fn, string ln, string uname, string email, string pwd)
        {
            if(!CustomerInfoValidation.ValidateName(fn)|| !CustomerInfoValidation.ValidateName(ln))
            {
                Console.WriteLine("Invalid name.");
                return false;
            }
            if (!CustomerInfoValidation.ValidateUsername(uname)||!DAL.CheckUnique(1,uname))
            {
                Console.WriteLine("Invalid username.");
                return false;
            }
            if (!CustomerInfoValidation.ValidateEmail(email)||!DAL.CheckUnique(2,uname))
            {
                Console.WriteLine("Invalid email.");
                return false;
            }
            if (!CustomerInfoValidation.ValidatePwd(pwd))
            {
                Console.WriteLine("Invalid password.");
                return false;
            }
            return true;
        }

        public bool AddCust()
        {
            string first_name = "", last_name = "", user_name = "", email = "", pwd = "", ver_pwd = " ";
            Console.WriteLine("Adding user, enter empty string at any time to quit:");
            //Short circuit to quit if user enters empty string
            if (Wrappers.ReadString("Enter first name", out first_name) ||
                Wrappers.ReadString("Enter last name", out last_name) ||
                 Wrappers.ReadString("Enter username", out user_name) ||
                  Wrappers.ReadString("Enter email", out email) ||
                   Wrappers.ReadString("Enter password", out pwd) ||
                    Wrappers.ReadString("Verify password", out ver_pwd))
            {
                Console.WriteLine("Quiting");
                return false;
            }
            else
            {
                do
                {
                    pwd = EnterPass();
                    ver_pwd = EnterPass();
                } while (pwd != ver_pwd);
                if (ValidateCustomerInfo(first_name, last_name, user_name, email, pwd))
                {
                    current_id=DAL.AddCust(first_name, last_name, user_name, email, pwd);
                    return true;
                }
                return false;
               
            }
        }

        public void SearchCust()
        {
            string fn = "", ln = "";
            Wrappers.ReadString("Enter first name",out fn);
            Wrappers.ReadString("Enter last name",out ln);
            var cust = DAL.SearchCust(0,fn,ln);
            if(cust==null)
            {
                Console.WriteLine($"Cust with name {fn} {ln} not found");
            }
            else
            {
                Console.WriteLine("Found: ");
                PrintCust(cust);
            }
        }
        public void DisplayOptions()
        {
            Console.WriteLine("\n\n");
            while (true)
            {
                Console.WriteLine("1: Place Order\n2: Add New Customer\n3: Search Customers");
                Console.WriteLine("Enter the number for your option");
                int choice = Wrappers.ReadInt();
                switch (choice)
                {
                    case 1:
                        AddOrder();
                        return;
                    case 2:
                        AddCust();
                        return;
                    case 3:
                        SearchCust();
                        return;
                    default:
                        Console.WriteLine("Please enter a valid option");
                        break;
                }
            }
            
        }
    }
  
}