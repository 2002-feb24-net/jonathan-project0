using System;
using System.Collections.Generic;
using System.Text;

namespace WheyMenIOValidation.Library
{
    public static class BusinessValidation
    {
        /// <summary>
        /// Called by order created DAL,
        /// </summary>
        /// <param name="request_qty"></param>
        /// <param name="available_qty"></param>
        /// <returns></returns>
        public static bool ValidateQuantity(int requested_qty, int available_qty)
        {
            if(requested_qty>0 && (requested_qty<.5*available_qty 
                || (requested_qty<100 && requested_qty<available_qty)))
            {
                return true;
            }
            return false;
        }
    }
}
