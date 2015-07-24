using ADODataAccess.BusinessLogic;
using ADODataAccess.BusinessLogic.Contract;
using ADODataAccess.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODataAccess
{
    //this console application will act as the "head" of the application. If this needs to be made with a UI, the data access and business logic layers could be made into dlls and a new UI could use them.
    //application will not work without db set up. This is just a test.
    class Program
    {
        static private bool endProgram = false;
        static void Main(string[] args)
        {
            try
            {
                while (!endProgram)
                {
                    Console.WriteLine("Options:");
                    Console.WriteLine("Enter 1 to view a user's preferences:");
                    Console.WriteLine("Enter 2 exit:"); //the application should enable the user to exit and be returned to the command prompt.
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            viewPreferences();
                            break;
                        case "2":
                            endTheProgram();
                            break;
                        default:
                            Console.WriteLine("You have entered an invalid option");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("A Fatal Error has Occurred - Please Restart");
                Console.ReadKey();

            }

        }

#region View Preferences
        static void viewPreferences()
        {

            //Provide a login screen that accepts a username and password and
            //authenticates the information entered against values stored in our database. The
            //passwords stored in our database are encrypted.
            Console.WriteLine("Please enter a user name:");
            var userName = Console.ReadLine();

            //if showing the password in plaintext on the screen is a concern a function could be used to mask this. This is something most UI frameworks give you out of the box
            //could also have validation logic if there are certain password requirements
            Console.WriteLine("Please enter the password for the user:");
            var password = Console.ReadLine(); 

            
            IPreferenceBusinessLogic businessLogic = new StandardPreferenceBusinessLogic();
            handleBusinessLogicResponse(businessLogic.GetUserPreferences(userName, password));

            //This takes the user back to the main options --
            //The application should enable the user to be logged out and returned to the
            //login screen so another set of credentials can be entered and the process can be
            //repeated.
            Console.WriteLine("Press any key to return to main options");
            Console.ReadKey();
            Console.WriteLine("");
        }

        
        //We could make a separate interface and class with different implementations of how we want to handle the response. This would be like a strategy design pattern
        //For simplicity and since there is only one way to implement this right now, I just left the logic in here.
        
        static void handleBusinessLogicResponse(GetUserPreferencesResult response)
        {
            switch(response.ReturnCode)
            {
                case 0 :
                    if(response.Preferences.Any())
                    {
                        //Once a user is logged in, the application should output all the preferences
                        //associated for that user to the screen. There is no maximum number of preferences
                        //for a given user.
                        Console.WriteLine("Showing Preferences...");
                        foreach (var preference in response.Preferences)
                        {
                            Console.WriteLine(preference.Name + " : " + preference.Setting);
                        }
                    }
                    else
                    {
                        Console.WriteLine("User Has No Preferences...");
                    }
                    break;
                case 1 :
                    Console.WriteLine("Invalid user name and/or password...");
                    break;
                default :
                    Console.WriteLine("Unknown error occurred...");
                    break;
            }
        }
           
#endregion

        static void endTheProgram()
        {
            endProgram = true;
        }
    }
}
