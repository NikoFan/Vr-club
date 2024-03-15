using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR_registration.Properties;

namespace VR_registration
{
    public class RegistrationWorkFromDB : ConnectDataBase
    {
        // Регистрация пользователя
        public bool registarationUserInUserTableFromDataBase(Dictionary<string, string> userInformationFromRegistrationWindow)
        {
            Console.WriteLine("Активный id: " + activeUsersId);

            takeTruthId();
            if (!checkPhoneNumber(userInformationFromRegistrationWindow["Phone"])
                || !checkMails(userInformationFromRegistrationWindow["Email"]))
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                connection.Open();
                SqlCommand commandToAddInformationFromTable = new SqlCommand($@"
                Use VR_club
                INSERT INTO Пользователь
                VALUES({activeUsersId}, N'{userInformationFromRegistrationWindow["Name"]}',
                N'{userInformationFromRegistrationWindow["Email"]}', N'{userInformationFromRegistrationWindow["Phone"]}',
                N'{userInformationFromRegistrationWindow["Password"]}', '{userInformationFromRegistrationWindow["Status"]}')");
                commandToAddInformationFromTable.Connection = connection;
                commandToAddInformationFromTable.ExecuteNonQuery();
            }
            
            programCashReader.recordingCurrentCustomersIdInCash(activeUsersId);
            return true;

        }
    }
}
