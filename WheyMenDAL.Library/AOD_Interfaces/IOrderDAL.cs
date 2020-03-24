using System;
using System.Collections.Generic;
using System.Text;

using WheyMen.DAL;
using WheyMen.Model;

namespace WheyMenDAL.Library.OLD
{
    public interface IOrderDAL
    {
        /// <summary>
        /// Calls getorders, list of orders searched for given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>-1 if order does not exist, order id otherwise</returns>
        int ValidateOrder(int id);
        /// <summary>
        /// Runs sp_create_ord and assisgns cid, lid params
        /// </summary>
        /// <param name="cid"> customer id</param>
        /// <param name="lid"> location id</param>
        /// <returns>id of created order</returns>
        int CreateOrder(int cid, int lid);
        /// <summary>
        /// Adds order item to database 
        /// </summary>
        /// <param name="oid">order id</param>
        /// <param name="pid">product id</param>
        /// <param name="qty">quantity </param>
        void AddOrderItem(int oid, int pid, int qty);

        /// <summary>
        /// Retrieves all orders from db by given search parameter and id
        /// </summary>
        /// <param name="search_param">location/order/customer id/name</param>
        /// <param name="mode">1: orders for a location, 2: orders for a given customer, 3: specific order, default: all orders
        /// </param>
        /// <returns></returns>
        List<OrderModel> GetOrders(string search_param, int mode = 0);
        /// <summary>
        /// Retrieves each item for a given order
        /// </summary>
        /// <param name="oid">id of order to retrieve items for</param>
        /// <param name="order_model">Local representation of order, returned order items added to its item list</param>
        /// <returns></returns>
        void GetOrderItems(int oid,OrderModel order_model);
        
    }
}
