using ADODataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODataAccess.BusinessLogic.Contract
{
    class GetUserPreferencesResult
    {
        public IList<UserPreference> Preferences { get; set; }
        public int ReturnCode { get; set; } //this could be an enumeration as well for better readability but we will just assume that 1 means can't authenticate
    }
}
