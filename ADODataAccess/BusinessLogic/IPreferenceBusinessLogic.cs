using ADODataAccess.BusinessLogic.Contract;
using ADODataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODataAccess.BusinessLogic
{
    interface IPreferenceBusinessLogic
    {
        GetUserPreferencesResult GetUserPreferences(string userName, string password);
    }
}
