using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Threading;
using VR_registration.Properties;
using System.Runtime.CompilerServices;

namespace VR_registration
{
    /// <summary>
    /// Логика взаимодействия для OrderClubClass.xaml
    /// </summary>
    public partial class OrderClubClass : Window
    {
        bool threadStop = false;
        private Dictionary<int, int> countDaysInMonthsArray = new Dictionary<int, int>()
        {
            {1,31 }, {2,28 }, {3,31 }, {4,30 }, {5,31 }, {6, 30},
            {7,31 }, {8,31 }, {9,30 }, {10,31 }, {11,30 }, {12,31}
        };
        private int yearNumber = 0;
        private int monthNumber = 0;
        private int daysInMonth = 0;
        string costToPay = "";
        public OrderClubClass()
        {
            InitializeComponent();
            ProgramCashReader programCashReader = new ProgramCashReader();
            programCashReader.recordingLastWinsName(this.Title.ToString());
            programCashReader = null;
            new Thread(CalculateCostOfOrder).Start();
            takeCorrectDateInformation();
            TakenDate.Text = $"Дата: {Settings.Default["Date"].ToString()}";
            
        }

        // Рассчет стоимости заказа
        private void CalculateCostOfOrder()
        {
            
            this.Dispatcher.BeginInvoke(new Action(() => costToPay = $"{Convert.ToString(Convert.ToInt32(TextBoxValue.Text.ToString()) * 2300)}руб/ч"));
            this.Dispatcher.BeginInvoke(new Action(() => CostOfOrder.Text = $"Цена:\n{costToPay}"));
                
            
        }


        public void MoveWindow(object sender, MouseButtonEventArgs e)
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


        // Переход на Аккаунт
        public void goUserAccountWindow()
        {
            Console.WriteLine("go user account");
            UserAccountWindow userAccountWindow = new UserAccountWindow()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            userAccountWindow.Show();
        }

        // Переход в окно регистрации
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

        // Возвращение к прошлому окну
        public void goBack(object sender, RoutedEventArgs e)
        {
            ProgramCashReader programCashReader = new ProgramCashReader();
            string lastWindowName = programCashReader.returnLastWindowsName();
            
            Console.WriteLine("последнее окно " + lastWindowName);
            this.Visibility = Visibility.Hidden;
            programCashReader.recordingLastActiveWindowCoordinates(makeCoordinates());
            programCashReader = null;
            SwitchWindows switchWins = new SwitchWindows();
            switchWins.Windows_X_Names[lastWindowName]();
            switchWins = null;
        }
        public void goVK(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("go VK");
            ProcessStartInfo linkWay = new ProcessStartInfo("https://vk.com/");
            Process.Start(linkWay);
        }
        public void goYouTube(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("go YouTube");
            ProcessStartInfo linkWay = new ProcessStartInfo("https://www.youtube.com/");
            Process.Start(linkWay);
        }
        public void goTelegram(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("go Telegram");
            ProcessStartInfo linkWay = new ProcessStartInfo("https://t.me/+TjGTywz1xPRjNzg6");
            Process.Start(linkWay);

        }

        // Увеличить количество
        public void DoUpper(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(TextBoxValue.Text.ToString()) + 1 > 10)
                dumbMessageBox("Количество не может превышать 10");
            else
                TextBoxValue.Text = Convert.ToString(Convert.ToInt32(TextBoxValue.Text.ToString()) + 1);
            new Thread(CalculateCostOfOrder).Start();
        }

        // Уменьшить количество
        public void DoLower(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(TextBoxValue.Text.ToString()) - 1 < 1)
                dumbMessageBox("Количество не может быть меньше 1");
            else
                TextBoxValue.Text = Convert.ToString(Convert.ToInt32(TextBoxValue.Text.ToString()) - 1);
            new Thread(CalculateCostOfOrder).Start();
        }

        public bool dumbMessageBox(string message)
        {
            if (MessageBox.Show(message, "Проверка", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;

        }

        // прекращаем работу программы с ошибкой 0
        public void closeApp(object sender, RoutedEventArgs e)
        {
            if (dumbMessageBox("Вы уверены что хотите закрыть программу?"))
            {
                ProgramCashReader programCashReader = new ProgramCashReader();
                threadStop = true;
                programCashReader.clearCash();
                programCashReader = null;
                Environment.Exit(0);
            }

        }

        // Покрасить кнопку при наведении
        public void DoSeen(object sender, RoutedEventArgs e)
        {
            Border buttonBorder = sender as Border;
            buttonBorder.Background = new SolidColorBrush(Colors.DarkGreen);
        }

        // Расписание клуба
        public void timingClub(object sender, RoutedEventArgs e)
        {

            Console.WriteLine("Расписание клуба");
            threadStop = true;
            Calendar calendarClassOpener = new Calendar()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            calendarClassOpener.Show();
        }

        public void DoHide(object sender, RoutedEventArgs e)
        {
            Border buttonBorder = sender as Border;
            buttonBorder.Background = new SolidColorBrush(Colors.Black);
        }

        // Проверка Кеша
        public void goUserAcc(object sender, RoutedEventArgs e)
        {
            threadStop = true;
            ProgramCashReader programCashReader = new ProgramCashReader();
            Console.WriteLine(":: " + programCashReader.returnActiveUserId());
            if (programCashReader.returnActiveUserId() == "out" || programCashReader.returnActiveUserId() == "")
            {
                programCashReader = null;
                goUserRegistrationWindow();
                return;
            }
            programCashReader = null;
            goUserAccountWindow();
            return;
        }


        // Формирование координат
        private string makeCoordinates()
        {
            string coord = this.Left.ToString() + "_" + this.Top.ToString();
            return coord;
        }

        // Получаем дату сегодня
        private void takeCorrectDateInformation()
        {
            string[] splitDate = Convert.ToString(DateTime.Now).Split(' ');
            yearNumber = Convert.ToInt32(splitDate[0].Split('.')[2]);
            monthNumber = Convert.ToInt32(splitDate[0].Split('.')[1]);
            daysInMonth = countDaysInMonthsArray[Convert.ToInt32(monthNumber)];
        }

        
        public void Payment(object sender, RoutedEventArgs e)
        {
            string Cost = costToPay;
            string Count = TextBoxValue.Text.ToString();
            if (Settings.Default["Date"].ToString() == "Не выбрана")
            {
                dumbMessageBox("У вас не выбрана дата");
                return;
            }
            string BookingDate = Settings.Default["Date"].ToString();
            int correctNowDateDay = Convert.ToInt32(DateTime.Today.ToString().Split('.')[0]);
            int correctNowDateMonth = Convert.ToInt32(DateTime.Today.ToString().Split('.')[1]);
            string NowDate = $"{yearNumber}-{correctNowDateMonth}-{correctNowDateDay}";
            string Address = Settings.Default["choosenClubInfo"].ToString();
            string ClubID = new ConnectDataBase().takeClubId(Address);
            Console.WriteLine(ClubID);
            string UserID = Settings.Default["activeUserId"].ToString();
            List<string> payInformation = new List<string>()
            {
                Count, NowDate, BookingDate, Cost, ClubID, Address, UserID
            };

            new ConnectDataBase().pay(payInformation);
            dumbMessageBox("Оплата прошла!");

            MainWindow mainWindow = new MainWindow()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            mainWindow.Show();
        }
    }
}
