using ADODataAccess.BusinessLogic.Contract;
using ADODataAccess.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODataAccess.BusinessLogic
{
    class StandardPreferenceBusinessLogic : IPreferenceBusinessLogic
    {

        //lazy loaded property. Allows for property based DI if needed.
        IDataAccessor _dataAccessor;
        public IDataAccessor DataAccessor
        {
            get
            {
                return new CheetahDataAccessor();
            }
            set
            {
                _dataAccessor = value;
            }
        }

        public GetUserPreferencesResult GetUserPreferences(string userName, string password)
        {
            var result = new GetUserPreferencesResult();
            if (DataAccessor.IsUserAuthenticated(userName, password))
            {
                 result.Preferences = DataAccessor.GetUserPreferences(userName);
                 result.ReturnCode = 0;
                
            }
            else
            {
                //bad password or user name
                result.ReturnCode = 1; 
            }

            return result;
            
        }
    }
}
