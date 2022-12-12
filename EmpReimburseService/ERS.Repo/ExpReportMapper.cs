using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ERS.Model;

namespace ERS.Repo
{
    internal static class ExpReportMapper
    {
        internal static ExpenseReport DbToExpenseReport(SqlDataReader sdr)
        {
            ExpenseReport exp = new ExpenseReport();

            try
            {
                // create a for loop to check is each individual element is null or not.
                // if null, take appropriate action for the datatype
                // is non-null, map the data.
                
            }
            catch (System.Exception)
            {
                
                throw;
            }

            return exp;
        }
    }
}