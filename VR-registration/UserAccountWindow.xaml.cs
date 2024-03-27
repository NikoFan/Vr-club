using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VR_registration
{
    /// <summary>
    /// Логика взаимодействия для UserAccountWindow.xaml
    /// </summary>
    public partial class UserAccountWindow : Window
    {

        public string computerFoldersPATH = Convert.ToString(String.Join("\\", Environment.CurrentDirectory.ToString().Split('\\').Take(
            Environment.CurrentDirectory.ToString().Split('\\').Length - 2))) + "/Picture/";
        public UserAccountWindow()
        {
            InitializeComponent();
            
            new ProgramCashReader().recordingLastWinsName(this.Title.ToString());
            completeUserInformationOnPage();

            
        }

        // Метод для передвижения окна нажав на него ЛКМ
        public void MoveWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {
                Console.WriteLine("nothing");
            }

        }

        // Пользовательская информация на страничке
        public void completeUserInformationOnPage()
        {
            ConnectDataBase connectDataBaseClass = new ConnectDataBase();
            if (connectDataBaseClass.getNameOfUserToCompleteInformation() != "ERROR")
            {
                CustomerName.Text = connectDataBaseClass.getNameOfUserToCompleteInformation();
                CustomerMailAddress.Text = connectDataBaseClass.getMailOfUserToCompleteInformation();
                CustomerPhoneNumber.Text = Convert.ToInt64(connectDataBaseClass.getPhoneOfUserToCompleteInformation()).ToString("#(###)###-####");
            } else
            {
                // При отсутствии записи - разлогинить пользователя
                dumbMessageBox("На устройстве отсутствует активный аккаунт!\nСоздайте аккаунт или авторизуйтесь!");
                
                new ProgramCashReader().recordingInactiveWordInCash();
                
                changeLastWindowsNameInTxtFile();
                goUserRegistrationWindow();
            }
            
        }

        public void goUserAcc(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("go user account");
            // отправка названия окна на запись
            UserRegistration UserAccount = new UserRegistration()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            UserAccount.Show();
        }



        // Маска координат
        private string makeCoordinates()
        {
            return this.Left.ToString() + "_" + this.Top.ToString();
        }


        // Возврат к старому окну
        public void goBack(object sender, RoutedEventArgs e)
        {
            
            
            string lastWindowName = new ProgramCashReader().returnLastWindowsName();
            Console.WriteLine("последнее окно " + lastWindowName);
            this.Visibility = Visibility.Hidden;
            new ProgramCashReader().recordingLastActiveWindowCoordinates(makeCoordinates());
            new SwitchWindows().Windows_X_Names[lastWindowName]();
            
        }

        public void goVK(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("go VK");
            ProcessStartInfo sInfo = new ProcessStartInfo("https://vk.com/");
            Process.Start(sInfo);
        }
        public void goYouTube(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("go YouTube");
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.youtube.com/");
            Process.Start(sInfo);
        }
        public void goTelegram(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("go Telegram");
            ProcessStartInfo sInfo = new ProcessStartInfo("https://t.me/+TjGTywz1xPRjNzg6");
            Process.Start(sInfo);
        }
        public void goUserRegistrationWindow()
        {
            Console.WriteLine("go user registration");
            
            UserRegistration userRegistrationWindow = new UserRegistration()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            userRegistrationWindow.Show();
        }
        public void logOutFromAccount(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Log out from account");
            new ProgramCashReader().recordingInactiveWordInCash();
            changeLastWindowsNameInTxtFile();
            goUserRegistrationWindow();
        }

        public void changeLastWindowsNameInTxtFile()
        {
            new ProgramCashReader().recordingLastWinsName("Главное меню");
            // запись координат окна на момент смены
            new ProgramCashReader().recordingLastActiveWindowCoordinates(makeCoordinates());
            
        }

        // Прекращение работы программы
        public void closeApp(object sender, RoutedEventArgs e)
        {
            if (dumbMessageBox("Вы уверены что хотите закрыть программу?"))
            {
                new ProgramCashReader().clearCash();
                Environment.Exit(0);
            }

        }

        public bool dumbMessageBox(string message)
        {
            var result = MessageBox.Show(message, "Служба поддержки", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;

        }
    }
}
