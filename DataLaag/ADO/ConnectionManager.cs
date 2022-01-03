using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLaag.ADO
{
    public static class ConnectionManager
    {
        #region Properties
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["EscapeFromTheWoodsDb"].ConnectionString;
        #endregion

        #region Methods
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new(_connectionString);
            return connection;
        }
        #endregion
    }
}
