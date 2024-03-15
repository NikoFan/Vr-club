using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VR_registration
{
    public class AuthorizationWorkFromDB : ConnectDataBase
    {
        public bool returnAuthorizationUserId(Dictionary<string, string> authInformationDict)
        {
            Console.WriteLine("----");
            Console.WriteLine(authInformationDict["Phone"]);
            Console.WriteLine(authInformationDict["Password"]);
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                    Use VR_club
                    SELECT [Id Пользователя]
                    FROM Пользователь
                    Where Телефон = {authInformationDict["Phone"]} and Пароль = N'{authInformationDict["Password"]}';
                    ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int Id = Convert.ToInt32(dr["Id Пользователя"]);
                        programCashReader.recordingCurrentCustomersIdInCash(Id);
                        return true;
                    }
                    dr.Close();
                }
                conn.Close();
            }
            return false;
        }
    }
}
