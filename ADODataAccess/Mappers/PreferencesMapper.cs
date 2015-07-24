using ADODataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODataAccess.Mappers
{
    class PreferencesMapper
    {
        public IList<UserPreference> Map(DataTable table)
        {
            try
            {
                var userPreferences = new List<UserPreference>();

                foreach (DataRow row in table.Rows)
                {
                    var userPreference = new UserPreference();
                    userPreference.ID = (int)row["pid"];
                    //I decided to trim here because the table was storing in nchar instead of varchar. 
                    //This could cause problems if you expect to have leading/trailing spaces. 
                    userPreference.Name = row["pname"].ToString().Trim();
                    userPreference.Setting = row["psetting"].ToString().Trim();

                    userPreferences.Add(userPreference);
                }
                return userPreferences;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

        }
    }
}
