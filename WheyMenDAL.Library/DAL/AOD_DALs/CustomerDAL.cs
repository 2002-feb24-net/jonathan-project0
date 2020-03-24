using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using WheyMenDAL.Library;
using WheyMen.Model;

namespace WheyMen.DAL.AOD
{
    public class CustomerDAL
    {
        private string _connectionString;
        
    
        public CustomerDAL(IConfiguration iconfiguration)
        {
            _connectionString = iconfiguration.GetConnectionString("Default");
        }

        public void AddCust(string fn, string ln, string username, string email, string pwd)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_CUST_ADD", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FIRST_NAME", SqlDbType.VarChar).Value = fn;
                    cmd.Parameters.Add("@LAST_NAME", SqlDbType.VarChar).Value = ln;
                    cmd.Parameters.Add("@USER_NAME", SqlDbType.VarChar).Value = username;
                    cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@PWD", SqlDbType.NVarChar).Value = pwd;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
               
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Searches customers by either name or username
        //modes:
        //  1: Search by name
        //  2: username
        //  default: email
        public CustomerModel SearchCust(int mode=0,params string[] search_param)
        {
            var CustList = GetList();
            foreach(var cust in CustList)
            {
                if (mode == 0)
                {
                    if ((cust.Name == search_param[0] && cust.LastName == search_param[1]))
                    {
                        return cust;
                    }
                }
                else if(mode ==1)
                {
                    if(cust.Username==search_param[0] || cust.Email == search_param[0])
                    {
                        return cust;
                    }
                }
                else
                {
                    if(cust.Email==search_param[0])
                    {
                        return cust;
                    }
                }
            }
            return null;
            
        }

        //assigns id of verified customer -1 if does not exist/invalid etc.
        //returns actual pwd of customer
        public string VerifyCustomer(string username,out int id)
        {
            var cust = SearchCust(1,username);
            if(cust==null)
            {
                id = -1;
            }
            else
            {
                id = cust.ID;
            }
            
            return cust.Pwd;
        }
        //returns -1 if customer name or id does not exist
        //returns the matching ID other wise
        public int ValidateCustomer(int id = -1, params string[] name)
        {
            var listCustomers = GetList();
            foreach (var cust in listCustomers)
            {
                if (id > 0 && cust.ID == id)
                {
                    return id;
                }
                else if (id<0 && cust.Name == name[0] && cust.LastName == name[1])
                {
                    return cust.ID;
                }
            }
            return -1;
        }

        public List<CustomerModel> GetList()
        {
            var listCustomerModel = new List<CustomerModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_CUSTOMER_GET_LIST", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listCustomerModel.Add(new CustomerModel
                        {
                            ID = Convert.ToInt32(rdr[0]),
                            Name = rdr[1].ToString(),
                            Email = rdr[2].ToString(),
                            Username = rdr[3].ToString(),
                            Pwd = rdr[4].ToString(),
                            LastName = rdr[5].ToString()

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listCustomerModel;
        }
    }
}