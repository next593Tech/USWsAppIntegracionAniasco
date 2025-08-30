using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
    public static class DbContextExtensions
    {
        public static MultiResultSetReader MultiResultSetSqlQuery(this DbContext context, string query, params SqlParameter[] parameters)
        {
            return new MultiResultSetReader(context, query, parameters);
        }
    }

}
