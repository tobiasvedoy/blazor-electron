using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class DataAccess : IDataAccess
    {
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sql, parameters);
                return rows.ToList();
            }
        }

        public async Task<int> SaveData<T>(string sql, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                int rowsAffected = await connection.ExecuteAsync(sql, parameters);
                return rowsAffected;
            }
        }

        public async Task<int> ExecuteProcedure<T>(string procedure, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                int rowsAffected = await connection.ExecuteAsync("EXEC "+procedure, parameters);
                return rowsAffected;
            }
        }

        public async Task<List<List<string>>> LoadData2DList(string sql, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync(sql);
                List<List<string>> strings2D = new List<List<string>>();
                
                List<string> colNames = new List<string>();
                foreach (KeyValuePair<string, object> val in rows.First())
                {
                    colNames.Add(val.Key);
                }

                strings2D.Add(colNames);

                foreach(var row in rows)
                {
                    List<string> strings = new List<string>();
                    foreach (KeyValuePair<string, object> value in row)
                    {
                        if(value.Value == null)
                        {
                            strings.Add("");
                        }
                        else
                        {
                            strings.Add(value.Value.ToString());
                        }
                    }
                    strings2D.Add(strings);
                }
                

                return strings2D;
            }
        }
    }
}
