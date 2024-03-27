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

namespace VR_registration
{
    /// <summary>
    /// Логика взаимодействия для ClubBookingClass.xaml
    /// </summary>
    public partial class ClubBookingClass : Window
    {

        public Dictionary<string, string> clubFullInformation;
        public Dictionary<int, Dictionary<string, string>> gameInformationArray = new Dictionary<int, Dictionary<string, string>>();
        private int gameIndex = 0;
        private int GAMECOUNT = 0;
        private int setGameInfIndex = 0;
        public string computerFoldersPATH = Convert.ToString(String.Join("\\", Environment.CurrentDirectory.ToString().Split('\\').Take(
            Environment.CurrentDirectory.ToString().Split('\\').Length - 2))) + "/Picture/";
        
        public ClubBookingClass()
        {
            InitializeComponent();
            completeInformationAboutClub();
            ProgramCashReader programCashReader = new ProgramCashReader();
            programCashReader.recordingLastWinsName(this.Title.ToString());
            programCashReader = null;
            
        }

        // -------------------------------Дейсвтия----------------------


        // Передвижение окна
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


        // Забронировать клуб
        public void bookingClub(object sender, RoutedEventArgs e)
        {
            
            Console.WriteLine("Бронирование клуба");
            OrderClubClass orderClubClass = new OrderClubClass()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            this.Visibility = Visibility.Hidden;
            orderClubClass.Show();
            
        }

        // Расписание клуба
        public void timingClub(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Расписание клуба");

            Calendar calendarClassOpener = new Calendar()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            calendarClassOpener.Show();
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

        public void SwapRight(object sender, RoutedEventArgs e)
        {
            setGameInfIndex++;
            completeInformationAboutGames();
        }

        public void SwapLeft(object sender, RoutedEventArgs e)
        {
            setGameInfIndex--;
            completeInformationAboutGames();
        }

        // Открыть Список игр
        public void OpenGameInformation(object sender, RoutedEventArgs e)
        {
            GameInformationSheild.Visibility = Visibility.Hidden;
            completeInformationAboutGames();
        }

        // Покрасить кнопку при наведении
        public void DoSeen(object sender, RoutedEventArgs e)
        {
            Border buttonBorder = sender as Border;
            buttonBorder.Background = new SolidColorBrush(Colors.DarkSeaGreen);
        }

        public void DoHide(object sender, RoutedEventArgs e)
        {
            Border buttonBorder = sender as Border;
            buttonBorder.Background = new SolidColorBrush(Colors.Black);
        }

        // Возвращение к прошлому окну
        public void goBack(object sender, RoutedEventArgs e)
        {
            ProgramCashReader programCashReader = new ProgramCashReader();
            string lastWindowName = programCashReader.returnLastWindowsName();
            Console.WriteLine("последнее окно " + lastWindowName);
            this.Visibility = Visibility.Hidden;
            programCashReader.recordingLastActiveWindowCoordinates(makeCoordinates());
            SwitchWindows switchWins = new SwitchWindows();
            switchWins.Windows_X_Names[lastWindowName]();
            switchWins = null;
            programCashReader = null;
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
                programCashReader.clearCash();
                Environment.Exit(0);
            }

        }

        // --------------------Логика-------------------------

        // Заполнение информации про клуб
        private void completeInformationAboutClub()
        {
            ConnectDataBase connectDataBaseClass = new ConnectDataBase();
            clubFullInformation =
                connectDataBaseClass.returnInformationAboutClub(Settings.Default["choosenClubInfo"].ToString());
            connectDataBaseClass = null;
            setTextInTextBlock(clubFullInformation);
            
        }

        // Заполнение информации про игры
        private void completeInformationAboutGames()
        {
            string[] array = clubFullInformation["Список игр"].ToString().Split('|');
            for (int i = 0; i < array.Count() - 1; i++)
            {
                // Помещаем инфу про игры в словарь
                string[] info = array[i].Split('-');
                gameInformationArray[gameIndex] = distribusionGameInformation(info);
                gameIndex++;
            }
            GAMECOUNT = gameIndex - 1;

            setGameInfoIntoBlocks();

        }

        // Хранение информации про игры в словаре
        private Dictionary<string, string> distribusionGameInformation(string[] allInfo)
        {
            Dictionary<string, string> distribution = new Dictionary<string, string>()
            {
                {"Name", ""},
                {"Rating", ""},
                {"Cooperative", ""},
                {"Photo", ""}
            };

            List<string> kategory = new List<string>()
            {
                "Name", "Rating", "Cooperative", "Photo"
            };
            int index = 0;
            foreach(var infoElement in allInfo)
            {
                distribution[kategory[index]] = infoElement;
                index++;
            }

            return distribution;
        }

        // Проверка Кеша
        public void goUserAcc(object sender, RoutedEventArgs e)
        {
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

        // -----------------------------Интерфейс--------------------------------

        // Установка текста
        private void setTextInTextBlock(Dictionary<string, string> textParams)
        {
            clubAddressTextBox.Text = "Адрес: " + textParams["Адрес"];
            clubMailTextBox.Text = "Почта: " + textParams["Почта"];
            clubPhoneTextBox.Text = "Тел. " + textParams["Телефон"];
            ConnectDataBase connectDataBaseClass = new ConnectDataBase();
            Cost.Text = connectDataBaseClass.takeClubPrice(textParams["Тариф"].Trim());
            connectDataBaseClass = null;
            
        }
        // Установка списка игр
        private void setGameInfoIntoBlocks()
        {
            if (setGameInfIndex < 0)
                setGameInfIndex = GAMECOUNT;
            Console.WriteLine(":: " + gameInformationArray[setGameInfIndex]["Name"]);

            GameName.Text = gameInformationArray[setGameInfIndex]["Name"];
            GameRating.Text = "Рейтинг: " + gameInformationArray[setGameInfIndex]["Rating"] + "/10";

            GameCoop.Text =
                (gameInformationArray[setGameInfIndex]["Cooperative"].Trim() == "1") ? "Совместная: Да" : "Совместная: Нет";
            setMainGamePicture(gameInformationArray[setGameInfIndex]["Photo"]);
            if (!(setGameInfIndex - 1 >= 0))
            {
                Console.WriteLine("GAMECOUNT: " + GAMECOUNT);
                Console.WriteLine(gameInformationArray[GAMECOUNT]["Photo"]);
            }
            
        }

        // Основная фотка
        private void setMainGamePicture(string pictureName)
        {
            FrontGamePicture.Source = new BitmapImage(
                new Uri(computerFoldersPATH + pictureName));
        }

        
        
        

        

        

        
    }
}
