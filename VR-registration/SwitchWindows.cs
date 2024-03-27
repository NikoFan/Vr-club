using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VR_registration
{
    public class SwitchWindows : Window
    {
        /*
         Класс для смены окон
        */
        public struct DataSet
        {
            public string[] coordinates;
        }
        public Dictionary<string, System.Action> Windows_X_Names = new Dictionary<string, System.Action>();


        public void switchOn_MainWindow()
        {
            DataSet dataSetStruct = new DataSet();
            dataSetStruct.coordinates = new ProgramCashReader().returnLastRecordingCoordinates().Split('_');
            MainWindow mainMenuWindow = new MainWindow()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Convert.ToDouble(dataSetStruct.coordinates[0]),
                Top = Convert.ToDouble(dataSetStruct.coordinates[1])
            };
            mainMenuWindow.Show();
        }
        public void switchOn_UserRegistrationWindow()
        {
            DataSet dataSetStruct = new DataSet();
            dataSetStruct.coordinates = new ProgramCashReader().returnLastRecordingCoordinates().Split('_');
            UserRegistration userRegistrationWindow = new UserRegistration()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Convert.ToDouble(dataSetStruct.coordinates[0]),
                Top = Convert.ToDouble(dataSetStruct.coordinates[1])
            };
            this.Visibility = Visibility.Hidden;
            userRegistrationWindow.Show();
        }

        public void switchOn_UserAccountWindow()
        {
            DataSet dataSetStruct = new DataSet();
            dataSetStruct.coordinates = new ProgramCashReader().returnLastRecordingCoordinates().Split('_');
            UserAccountWindow UserAccountWindow = new UserAccountWindow()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Convert.ToDouble(dataSetStruct.coordinates[0]),
                Top = Convert.ToDouble(dataSetStruct.coordinates[1])
            };
            this.Visibility = Visibility.Hidden;
            UserAccountWindow.Show();
        }

        public void switchOn_ChooseClubToBook()
        {
            DataSet dataSetStruct = new DataSet();
            dataSetStruct.coordinates = new ProgramCashReader().returnLastRecordingCoordinates().Split('_');
            ChooseClubToBook chooseClubToBook = new ChooseClubToBook()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Convert.ToDouble(dataSetStruct.coordinates[0]),
                Top = Convert.ToDouble(dataSetStruct.coordinates[1])
            };
            this.Visibility = Visibility.Hidden;
            chooseClubToBook.Show();
        }

        public void switchOn_ClubBookingClass()
        {
            DataSet dataSetStruct = new DataSet();
            dataSetStruct.coordinates = new ProgramCashReader().returnLastRecordingCoordinates().Split('_');
            ClubBookingClass CLubBookingClass = new ClubBookingClass()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Convert.ToDouble(dataSetStruct.coordinates[0]),
                Top = Convert.ToDouble(dataSetStruct.coordinates[1])
            };
            this.Visibility = Visibility.Hidden;
            CLubBookingClass.Show();
        }

        public void switchOn_CalendarWindow()
        {
            DataSet dataSetStruct = new DataSet();
            dataSetStruct.coordinates = new ProgramCashReader().returnLastRecordingCoordinates().Split('_');
            Calendar calendarWindowClass = new Calendar()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Convert.ToDouble(dataSetStruct.coordinates[0]),
                Top = Convert.ToDouble(dataSetStruct.coordinates[1])
            };
            this.Visibility = Visibility.Hidden;
            calendarWindowClass.Show();
        }

        public void switchOn_OrderClubClass()
        {
            DataSet dataSetStruct = new DataSet();
            dataSetStruct.coordinates = new ProgramCashReader().returnLastRecordingCoordinates().Split('_');
            OrderClubClass orderClubClass = new OrderClubClass()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Convert.ToDouble(dataSetStruct.coordinates[0]),
                Top = Convert.ToDouble(dataSetStruct.coordinates[1])
            };
            this.Visibility = Visibility.Hidden;
            orderClubClass.Show();
        }

        // Конструктор класса
        public SwitchWindows()
        {
            Windows_X_Names["Главное меню"] = 
                switchOn_MainWindow;
            Windows_X_Names["Регистрация пользователя"] =
                switchOn_UserRegistrationWindow;
            Windows_X_Names["Аккаунт пользователя"] =
                switchOn_UserAccountWindow;
            Windows_X_Names["Выбор клуба"] =
                switchOn_ChooseClubToBook;
            Windows_X_Names["Бронь клуба"] =
                switchOn_ClubBookingClass;
            Windows_X_Names["Расписание клуба"] =
                switchOn_CalendarWindow;
            Windows_X_Names["Оформление Заказа"] =
                switchOn_OrderClubClass;
        }
    }
}
