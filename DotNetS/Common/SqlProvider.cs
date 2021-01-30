using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DotNetS.Common
{
    public class SqlProvider
    {
        public static SqlCommand CreateSQLCommand(object vm, string tableName, string exClude = "", bool isCreate = true, SqlConnection connection = null, string primKey = null)
        {
            int Id = 0;
            Dictionary<string, object> columnValueDictionary = new Dictionary<string, object>();
            foreach (var prop in vm.GetType().GetProperties().Where(x => x.Name != exClude))
            {
                string name = prop.Name;
                var value = prop.GetValue(vm, null);
                columnValueDictionary.Add(name, value);
                if (name.ToLower() == primKey)
                {
                    Id = Int32.Parse(value.ToString());
                }
            }

            string sql = "";

            if (isCreate)
            {
                var columns = columnValueDictionary.Select(x => x.Key).ToArray();
                var parameters = columnValueDictionary.Select(x => "@" + x.Key).ToArray();
                sql = "INSERT INTO " + tableName + " (";
                sql += string.Join(", ", columns);
                sql += ") VALUES(";
                sql += string.Join(", ", parameters);
                sql += ")";
            }
            else
            {
                var parameters = columnValueDictionary.Select(x => string.Format(" {0} = @{0}", x.Key));
                sql = "UPDATE " + tableName + " SET ";
                sql += string.Join(", ", parameters);
                sql += " WHERE " + primKey + " = " + Id;
            }

            SqlCommand cmd = new SqlCommand(sql);
            foreach (var dic in columnValueDictionary)
            {
                cmd.Parameters.AddWithValue("@" + dic.Key, dic.Value);
            }
            if (connection != null)
            {
                cmd.Connection = connection;
            }
            return cmd;
        }
    }
}