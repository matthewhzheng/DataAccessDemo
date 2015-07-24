using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ADODataAccess.Models;
using System.Data;
using ADODataAccess.Mappers;
using System.Security.Cryptography;

namespace ADODataAccess.DataAccess
{
    public class CheetahDataAccessor : IDataAccessor
    {

        public bool IsUserAuthenticated(string userName, string password)
        {
            var hashedPassword = hashPassword(password);

            //A stored procedure is one way to get data out of a database. The logic for the query lives in SQL server which could be an architectural design preference
            var connection = GetConnection();
            Func<Boolean> call = () =>
            {

                using (SqlCommand validateUser = new SqlCommand("dbo.sprocGetUserNamePassword", connection))
                {
                    validateUser.CommandType = CommandType.StoredProcedure;
                    validateUser.Parameters.AddWithValue("@userName", userName);
                    validateUser.Parameters.AddWithValue("@password", hashedPassword);

                    //stored procedure returns a user if the username and password match,so it will have 1 row if match.
                    using (var reader = validateUser.ExecuteReader())
                    {
                        var table = new DataTable();
                        table.Load(reader);
                        return (table.Rows.Count == 1);
                    }
                }

            };

            return establishConnection<Boolean>(call, connection);
        }

        private string hashPassword(string clearText)
        {
            //assumes db stores pw in md5
            //creates a hash of the password... there is no salt since the db has no place to store it. Hashing the password in general is a more secure and faster solution than encrypting
            //MD5 also is not a good hash to use but we are limited by the size of the password column.
            using (MD5 md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(clearText));


                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            }
        }

        IList<Models.UserPreference> IDataAccessor.GetUserPreferences(string userName)
        {
            //A parameterized query is one way to get data out of a database. The benefit here is that it protects you from SQL injection when compared to a dynamic query built through string concats
            var connection = GetConnection();

            Func<IList<UserPreference>> call = () =>
            {
                using (SqlCommand getPreferences = new SqlCommand("select p.pid, p.pname, p.psetting " +
                                                                "from dbo.user_preferences up " +
                                                                "inner join dbo.preference p " +
                                                                "on (p.pid = up.pid) " +
                                                                "inner join dbo.users u " +
                                                                "on (u.id = up.id) " +
                                                                "where u.username = @userName", connection))
                {
                    getPreferences.Parameters.AddWithValue("@userName", userName);

                    using (var reader = getPreferences.ExecuteReader())
                    {
                        var table = new DataTable();
                        table.Load(reader);

                        var mapper = new PreferencesMapper();
                        return mapper.Map(table);

                    }
                }

            };

            return establishConnection<IList<UserPreference>>(call, connection);
        }

        //a common function to return the sql connection with the same query string any time it's needed
        private SqlConnection GetConnection()
        {
            //these attributes could be stored and read from a config file also. It would be good to encrypt the passwords in production and allow access for only those who need it.
            return new SqlConnection("user id=sa;" +
                                       "password=cheetah;server=.\\SQLEXPRESS;" +
                                       "database=cheetah; ");
        }

        private TReturn establishConnection<TReturn>(Func<TReturn> call, SqlConnection connection)
        {
            //wraps the call in the connection setup and tear down process
            //callers can use a lambda function
            using (connection)
            {
                try
                {
                    connection.Open();
                    return call();
                }
                catch (Exception e)
                {
                    //if we had a logging component we could log here
                    //throw error up stack, this will keep the stack trace
                    //Console.WriteLine(e.ToString());
                    throw;
                }
            }
        }




    }
}
