using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using static VR_registration.ConnectDataBase;

namespace VR_registration
{
    public class ConnectDataBase
    {
        /*
         Класс для работы с данными
        */

        public struct DatabaseInformationStruct
        {
            public string userPhoneNumberFromDB;
            public string userNameFromDB;
            public string userPasswordFromDB;
            public string userMailFromDB;
            public string clubAddress;
            public string clubMail;
            public string clubPhone;
            public string clubGame;
            public string clubTarif;
            public string clubTiming;
            public string gameName;
            public string gameRating;
            public string gameCooper;
            public string gamePhotoName;
            
        }

        // Корректный id пользователя при регистрации
        public int activeUsersId = 1;
        
        
        
        public string ConnectionString = "data source=0.tcp.eu.ngrok.io, 16847;" +
            "Database=VR_club;" +
            "User Id=sa;" +
            "Password=7784865Oleg#;" +
            "TrustServerCertificate=True;";

        // получение id пользователя для регистрации
        public void takeTruthId()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                    Use VR_club
                    SELECT [Id Пользователя]
                    FROM Пользователь;
                    ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string count = "" + dr["Id Пользователя"];
                        activeUsersId = Convert.ToInt32("" + dr["Id Пользователя"]) + 1;
                    }
                    dr.Close();
                }
                conn.Close();
            }
        }

        // метод для проверки телефонных номеров на совпадение
        public bool checkPhoneNumber(string userPhoneNumberFromRegistrationWindow)
        {
            DatabaseInformationStruct databaseUserTableColumn = new DatabaseInformationStruct();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                    Use VR_club
                    Select Телефон
                    From Пользователь
                    ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        databaseUserTableColumn.userPhoneNumberFromDB = "" + dr["Телефон"];
                        if ((databaseUserTableColumn.userPhoneNumberFromDB.Trim().Substring(1, databaseUserTableColumn.userPhoneNumberFromDB.Length - 2) == userPhoneNumberFromRegistrationWindow.Trim().Substring(1, userPhoneNumberFromRegistrationWindow.Length - 2))
                            || databaseUserTableColumn.userPhoneNumberFromDB.Trim().Substring(2, databaseUserTableColumn.userPhoneNumberFromDB.Length - 3) == userPhoneNumberFromRegistrationWindow.Trim().Substring(2, userPhoneNumberFromRegistrationWindow.Length - 3))
                        {
                            return false;
                        }
                    }
                    dr.Close();
                }
                conn.Close();
            }
            return true;
        }

        // Запрос к БД по клубу
        public List<string> takeInformationFromTableAboutGames(string gameFindID)
        {
            DatabaseInformationStruct databaseGameListTableColumn = new DatabaseInformationStruct();
            Console.WriteLine(gameFindID);
            List<string> gameInformationArray = new List<string>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                    Use VR_club
                    Select *
                    From [Список игр]
                    Where [ID Списка игр] = {gameFindID}
                    ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        /*databaseGameListTableColumn.gameName = ("" + dr["Игра"]).Trim();
                        databaseGameListTableColumn.gameRating = ("" + dr["Рейтинг"]).Trim();
                        databaseGameListTableColumn.gameCooper = ("" + dr["Совместная"]).Trim();
                        databaseGameListTableColumn.gamePhotoName = ("" + dr["Фото"]).Trim();

                        Console.WriteLine($"{databaseGameListTableColumn.gameName};\n{databaseGameListTableColumn.gameRating};\n" +
                            $"{databaseGameListTableColumn.gameCooper};\n{databaseGameListTableColumn.gamePhotoName};");*/
                        gameInformationArray.Add(("" + dr["Игра"]).Trim());
                        gameInformationArray.Add(("" + dr["Рейтинг"]).Trim());
                        gameInformationArray.Add(("" + dr["Совместная"]).Trim());
                        gameInformationArray.Add(("" + dr["Фото"]).Trim());
                    }
                    dr.Close();
                }
                conn.Close();
                
            }
            databaseGameListTableColumn.gameName = null;
            databaseGameListTableColumn.gameRating = null;
            databaseGameListTableColumn.gameCooper = null;
            databaseGameListTableColumn.gamePhotoName = null;
            return gameInformationArray;
        }
        public string takeClubPrice(string priceType)
        {
            DatabaseInformationStruct databaseTarifTableColumn = new DatabaseInformationStruct();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                    Use VR_club
                    Select *
                    From Тариф
                    Where Название = N'{priceType}'
                    ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        databaseTarifTableColumn.clubTarif = ((string)dr["Цена"]).Trim();
                        break;
                    }
                    dr.Close();
                }
                conn.Close();
            }
            return databaseTarifTableColumn.clubTarif;
        }

        // Полная информация про клубы
        public Dictionary<string, string> returnInformationAboutClub(string necessaryClubName)
        {
            Dictionary<string, string> returnInformationDict = new Dictionary<string, string>()
            {
                {"Адрес", ""},
                {"Почта", "" },
                {"Телефон", ""},
                {"Список игр", ""},
                {"Тариф",""},
                {"Расписание",""}
            };
            DatabaseInformationStruct databaseClubTableColumn = new DatabaseInformationStruct();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                    Use VR_club
                    Select Адрес, Почта, Телефон, [ID Списка игр], Название, [ID расписания]
                    From Клуб
                    Where Адрес = N'{necessaryClubName}'
                    ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (returnInformationDict["Адрес"] == "")
                        {
                            returnInformationDict["Адрес"] = ("" + dr["Адрес"]).Trim();
                            returnInformationDict["Почта"] = ("" + dr["Почта"]).Trim();
                            returnInformationDict["Телефон"] = ("" + dr["Телефон"]).Trim();
                            

                            
                            returnInformationDict["Тариф"] = ("" + dr["Название"]).Trim();
                            returnInformationDict["Расписание"] = ("" + dr["ID расписания"]).Trim();
                            databaseClubTableColumn.clubGame = null;
                        }
                        databaseClubTableColumn.clubGame = ("" + dr["ID Списка игр"]).Trim();
                        // Запрашиваем полную информацию по игре
                        returnInformationDict["Список игр"] += String.Join("-",
                            takeInformationFromTableAboutGames(databaseClubTableColumn.clubGame)) + "|";


                    }
                    dr.Close();
                }
                conn.Close();
            }

            return returnInformationDict;
        }

        // проверка почт на совпадение
        public bool checkMails(string userMailFromRegistrationWindow)
        {
            ControlInputData controlInputData = new ControlInputData();
            if (!controlInputData.IsValidEmail(userMailFromRegistrationWindow))
            {
                return false;
            }
            controlInputData = null;
            DatabaseInformationStruct databaseUserTableColumn = new DatabaseInformationStruct();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                    Use VR_club
                    Select Почта
                    From Пользователь
                    ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        databaseUserTableColumn.userMailFromDB = "" + dr["Почта"];
                        if (databaseUserTableColumn.userMailFromDB.Trim() == userMailFromRegistrationWindow.Trim())
                        {
                            return false;
                        }
                    }
                    dr.Close();
                }
                conn.Close();
            }
            databaseUserTableColumn.userMailFromDB = null;
            return true;
        }

        
        
        // Имя для аккаунта пользователя
        public string getNameOfUserToCompleteInformation()
        {
            DatabaseInformationStruct databaseUserTableColumn = new DatabaseInformationStruct();
            ProgramCashReader programCashReader = new ProgramCashReader();
            Console.WriteLine(programCashReader.returnActiveUserId());
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                        Use VR_club
                        Select ФИО
                        From Пользователь
                        Where [Id пользователя] = {programCashReader.returnActiveUserId()}
                        ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        databaseUserTableColumn.userNameFromDB = (string)dr["ФИО"];

                    }
                    dr.Close();
                }
                conn.Close();
            }
            programCashReader = null;
            return databaseUserTableColumn.userNameFromDB.Trim();
        }

        // Почта для аккаунта пользователя
        public string getMailOfUserToCompleteInformation()
        {
            DatabaseInformationStruct databaseUserTableColumn = new DatabaseInformationStruct();
            ProgramCashReader programCashReader = new ProgramCashReader();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                        Use VR_club
                        Select Почта
                        From Пользователь
                        Where [Id пользователя] = {programCashReader.returnActiveUserId()}
                        ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        return ("" + dr["Почта"]).Trim();

                    }
                    dr.Close();
                }
                conn.Close();
            }
            programCashReader = null;
            return databaseUserTableColumn.userMailFromDB.Trim();
        }

        // Номер телефона для аккаунта пользователя
        public string getPhoneOfUserToCompleteInformation()
        {
            ProgramCashReader programCashReader = new ProgramCashReader();
            DatabaseInformationStruct databaseUserTableColumn = new DatabaseInformationStruct();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                        Use VR_club
                        Select Телефон
                        From Пользователь
                        Where [Id пользователя] = {programCashReader.returnActiveUserId()}
                        ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        databaseUserTableColumn.userPhoneNumberFromDB = "" + dr["Телефон"];

                    }
                    dr.Close();
                }
                conn.Close();
            }
            programCashReader = null;
            return databaseUserTableColumn.userPhoneNumberFromDB.Trim();
        }

        // Возврат клубов для карточек
        public Dictionary<string, Dictionary<string, string>> returnClubsInformation()
        {
            Dictionary<string, Dictionary<string, string>> clubFullInformation =
                new Dictionary<string, Dictionary<string, string>>();
            List<string> clubsName = new List<string>();
            DatabaseInformationStruct clubAttributes = new DatabaseInformationStruct();
            int clubListIndex = 1;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"
                        Use VR_club
                        Select Адрес, Телефон, Почта
                        From Клуб
                        ";

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Dictionary<string, string> clubInfo = new Dictionary<string, string>();                        
                        if (!clubsName.Contains("" + dr["Адрес"].ToString().Trim()))
                        {
                            clubAttributes.clubPhone = "" + dr["Телефон"].ToString().Trim();
                            clubAttributes.clubMail = "" + dr["Почта"].ToString().Trim();
                            clubAttributes.clubAddress = "" + dr["Адрес"].ToString().Trim();
                            clubsName.Add(clubAttributes.clubAddress);
                            clubInfo["Address"] = clubAttributes.clubAddress;
                            clubInfo["Mail"] = clubAttributes.clubMail;
                            clubInfo["Phone"] = clubAttributes.clubPhone;
                            
                            // Добавление в основной словарь
                            clubFullInformation[$"club№{clubListIndex}"] = clubInfo;
                            clubAttributes.clubPhone = null;
                            clubAttributes.clubMail = null;
                            clubAttributes.clubAddress = null;
                            clubListIndex++;

                        }


                    }
                    dr.Close();
                }
                conn.Close();
            }
            return clubFullInformation;
        }
        

    }
}
