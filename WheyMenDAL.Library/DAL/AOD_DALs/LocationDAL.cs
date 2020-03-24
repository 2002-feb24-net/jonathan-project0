using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using WheyMen.Model;
using WheyMenDAL.Library;

namespace WheyMen.DAL.AOD
{
    public class LocationDAL
    {
        private string _connectionString;

        public LocationDAL(IConfiguration iconfiguration)
        {
            _connectionString = iconfiguration.GetConnectionString("Default");
        }
        public void UpdateInventory(int id,int qty)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_LOC_UPDATE_INVENTORY", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PROD_ID", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@REQ_QTY", SqlDbType.Int).Value = qty;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<InventoryModel> GetInventory(int id)
        {
            var listInventoryModel = new List<InventoryModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_LOC_GET_INVENTORY", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listInventoryModel.Add(new InventoryModel
                        {
                            ID = Convert.ToInt32(rdr[0]),
                            Store_ID = Convert.ToInt32(rdr[1]),
                            Name = rdr[2].ToString(),
                            Qty = Convert.ToInt32(rdr[3]),
                            Price = Convert.ToInt32(rdr[4])
                        }) ;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listInventoryModel;
        }
       
        public List<LocationModel> GetList()
        {
          
            var listLocationModel = new List<LocationModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_LOC_GET_LIST", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listLocationModel.Add(new LocationModel
                        {
                            ID = Convert.ToInt32(rdr[0]),
                            Name = rdr[1].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listLocationModel;
        }
    }
}