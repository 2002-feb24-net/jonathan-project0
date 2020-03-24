using System;
using System.Collections.Generic;

using WheyMen.Model;

namespace WheyMenDAL.Library.OLD
{
    public interface ICustomerDAL
    {
        /// <summary>
        /// Adds customer to databse
        /// </summary>
        /// <param name="fn">First name</param>
        /// <param name="ln">Last name</param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="pwd">password</param>
        void AddCust(string fn, string ln, string username, string email, string pwd);
       
        /// <summary>
        /// Searches customers by given parameter/search mode
        /// </summary>
        /// <param name="mode">Search mode: 1 - By name, 2 - By username</param>
        /// <param name="search_param">nameusername to search by</param>
        /// <returns></returns>
        CustomerModel SearchCust(int mode = 0,params string[] search_param);
        //Retrieves actual pwd of customer returns id of matching username
       /// <summary>
       /// Verifies that username exists, assigns id if it does
       /// </summary>
       /// <param name="username"></param>
       /// <param name="id">id holder for invoker</param>
       /// <returns>actual password of customer</returns>
        string VerifyCustomer(string username,out int id);
        //returns -1 if customer name or id does not exist
        //returns the matching ID other wise
        /// <summary>
        /// Verifies if customer with given name or id exists
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>id corresponding to customer matching search param or -1 if not exists</returns>
        int ValidateCustomer(int id = -1, params string[] name);
        /// <summary>
        /// Retrieves list of customers from db
        /// </summary>
        /// <returns></returns>
        List<CustomerModel> GetList();
    }
}
