using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODataAccess.Models;

namespace ADODataAccess.DataAccess
{
    interface IDataAccessor
    {
        Boolean IsUserAuthenticated(string userName, string password);
        IList<UserPreference> GetUserPreferences(string userName);

    }
}
