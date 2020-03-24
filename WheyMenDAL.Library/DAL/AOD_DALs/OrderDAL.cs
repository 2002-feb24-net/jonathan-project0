using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using WheyMenDAL.Library;
using WheyMenDAL.Library.Model;

namespace WheyMen.DAL.AOD
{
    public class OrderDAL 
    {
        private string _connectionString;

        public OrderDAL(IConfiguration iconfiguration)
        {
            _connectionString = iconfiguration.GetConnectionString("Default");
        }
        
        public void AddOrderItem(int oid, int pid, int qty)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_ORD_ADD_ITEM", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OID", SqlDbType.Int).Value = oid;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = pid;
                    cmd.Parameters.Add("@QTY", SqlDbType.Int).Value = qty;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
}

        public int ValidateOrder(int id)
        {
            var ordList = GetOrders("");
            foreach(var order in ordList)
            {
                if(id>0&&order.Id == id)
                {
                    return id;
                }
                
            }
            return -1;
        }

        public int CreateOrder(int cid, int lid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREATE_ORD", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CID", SqlDbType.Int).Value = cid;
                    cmd.Parameters.Add("@LID", SqlDbType.Int).Value = lid;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    return Convert.ToInt32(rdr[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public void GetOrderItems(int oid,Order order_model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_GET_ORD_ITEMS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OID", SqlDbType.Int).Value = oid;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                      
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Searches orders by given param, param is checked against Order columns according to mode
        //Mode Codes:
        //  1: Get orders by location
        //  2: By customer
        //  3: Get details of 1 specific order
        public List<Order> GetOrders(string search_param,int mode=0)
        {
            var OrdersList = new List<Order>();
            string sp = "";
            string param = "";
            bool add_param = true;
            switch(mode)
            {
                case 1:
                    sp = "SP_GET_LOC_ORDS";
                    param = "@LID";
                    break;
                case 2:
                    sp = "SP_GET_CUST_ORDS";
                    param = "@CID";
                    break;
                case 3:
                    sp = "SP_GET_ORD_DETS";
                    param = "@OID";
                    break;
                case 4:
                    sp = "SP_SORT_ORDERS";
                    param = "@ORDER_BY";
                    break;
                default:
                    sp = "SP_GET_ORD_LIST";
                    add_param = false;
                    break;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (add_param)
                    {
                        if (mode == 4) cmd.Parameters.Add(param, SqlDbType.NVarChar).Value = search_param;
                        else cmd.Parameters.Add(param, SqlDbType.Int).Value = search_param;
                    }
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var order = new Order();
                        
                        GetOrderItems(order.Id, order);
                        OrdersList.Add(order);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OrdersList;
        }
        
 
    }
}
