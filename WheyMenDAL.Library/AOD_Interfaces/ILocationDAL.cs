﻿using System;
using System.Collections.Generic;
using System.Text;

using WheyMen.Model;

namespace WheyMenDAL.Library.OLD
{
    public interface ILocationDAL
    {
        /// <summary>
        /// Updates inventory quantity in database
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="qty">quantity to decrease by</param>
        void UpdateInventory(int id, int qty);
        /// <summary>
        /// Retrieves inventory of a location
        /// </summary>
        /// <param name="id">location id or name</param>
        /// <returns>list of items</returns>
        List<InventoryModel> GetInventory(int id);
        /// <summary>
        /// Returns all locations
        /// </summary>
        /// <returns></returns>
        List<LocationModel> GetList();
    }
}
