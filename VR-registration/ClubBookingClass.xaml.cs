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
using VR_registration.Properties;

namespace VR_registration
{
    /// <summary>
    /// Логика взаимодействия для ClubBookingClass.xaml
    /// </summary>
    public partial class ClubBookingClass : Window
    {
        public ProgramCashReader programCashReader = new ProgramCashReader();
        public RegistrationWorkFromDB registrtationWorkFromDB = new RegistrationWorkFromDB();
        public AuthorizationWorkFromDB authorizationWorkFromDB = new AuthorizationWorkFromDB();
        public ConnectDataBase connectDataBaseClass = new ConnectDataBase();
        public SwitchWindows switchWins = new SwitchWindows();
        public ChooseClubToBook chooseClubToBook = new ChooseClubToBook();
        public Dictionary<string, string> clubFullInformation;
        public Dictionary<int, Dictionary<string, string>> gameInformationArray = new Dictionary<int, Dictionary<string, string>>();
        private int gameIndex = 0;
        private int GAMECOUNT = 0;
        private int setGameInfIndex = 0;
        private string PATH = @"C:\\Users\\Олег\\Desktop\\Хакатон\\VR-registration\\VR-registration\\images\\";
        public ClubBookingClass()
        {
            InitializeComponent();
            completeInformationAboutClub();
            programCashReader.recordingLastWinsName(this.Title.ToString());
        }

        // Метод для передвижения окна нажав на него ЛКМ
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

        private void completeInformationAboutClub()
        {
            Console.WriteLine("Заполение информации");
            Console.WriteLine("Клуб: " + Settings.Default["choosenClubInfo"].ToString());
            clubFullInformation =
                connectDataBaseClass.returnInformationAboutClub(Settings.Default["choosenClubInfo"].ToString()); ;
            setTextInTextBlock(clubFullInformation);
            completeInformationAboutGames();
        }
        private void completeInformationAboutGames()
        {
            

            
            foreach(var dictItem in clubFullInformation)
            {
                Console.WriteLine(dictItem.Key + ": " + dictItem.Value);
                if (dictItem.Key == "Список игр")
                {
                    
                    string[] array = dictItem.Value.ToString().Split('|');
                    for(int i = 0; i < array.Count() - 1; i++)
                    {
                        
                        Console.WriteLine($"index: {gameIndex}");
                        string[] info = array[i].Split('-');
                        gameInformationArray[gameIndex] = distribusionGameInformation(info);
                        gameIndex++;
                    }
                    GAMECOUNT = gameIndex - 1;
                    
                }
                Console.WriteLine("-");
            }
            Console.WriteLine("------------");

            foreach(var dictItem in gameInformationArray)
            {
                Console.WriteLine(dictItem.Key);
                foreach(var info in dictItem.Value)
                {
                    Console.WriteLine(info.Key);
                    Console.WriteLine(info.Value);
                }
                Console.WriteLine("-----");
            }
            setGameInfoIntoBlocks();

        }

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

        // Установка текста
        private void setTextInTextBlock(Dictionary<string, string> textParams)
        {
            ClubName.Text = "VR_ZONE";
            ClubAddress.Text = "Адрес: " + textParams["Адрес"];
            ClubMail.Text = "Почта: " + textParams["Почта"];
            ClubPhone.Text = "Тел. " + textParams["Телефон"];
            ClubPrice.Text += " " + connectDataBaseClass.takeClubPrice(textParams["Тариф"]);
            
        }

        private void setGameInfoIntoBlocks()
        {
            Console.WriteLine(":: " + gameInformationArray[setGameInfIndex]["Name"]);

            GameName.Text = gameInformationArray[setGameInfIndex]["Name"];
            GameRate.Text = gameInformationArray[setGameInfIndex]["Rating"] + "/10";

            GameCoop.Text =
                (gameInformationArray[setGameInfIndex]["Cooperative"].Trim() == "1") ? "Есть" : "Отсутствует";
            setMainGamePicture(gameInformationArray[setGameInfIndex]["Photo"]);
            if (setGameInfIndex - 1 >= 0)
            {
                setBackGroundGamePicture(gameInformationArray[setGameInfIndex-1]["Photo"]);
            }
            else
            {
                Console.WriteLine("GAMECOUNT: " + GAMECOUNT);
                Console.WriteLine(gameInformationArray[GAMECOUNT]["Photo"]);
                setBackGroundGamePicture(gameInformationArray[GAMECOUNT]["Photo"]);
            }
            


        }
        // Основная фотка
        private void setMainGamePicture(string pictureName)
        {
            gamePicture.Source = new BitmapImage(
                new Uri(PATH + pictureName));
        }

        // Фоновая фотка
        private void setBackGroundGamePicture(string pictureName)
        {
            rightGamePic.Source = new BitmapImage(
                new Uri(PATH + pictureName));
        }

        public void bookingClub(object  sender, RoutedEventArgs e)
        {
            Console.WriteLine("Бронирование клуба");
        }

        public void timingClub(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Расписание клуба");
            
            // отправка названия окна на запись
            // programCashReader.recordingLastWinsName(this.Title.ToString());
            Calendar calendarClassOpener = new Calendar()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            calendarClassOpener.Show();
        }

        public void goUserAcc(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(":: " + programCashReader.returnActiveUserId());
            if (programCashReader.returnActiveUserId() == "out")
            {
                goUserRegistrationWindow();
                return;
            }
            goUserAccountWindow();
            return;
        }
        public void goUserAccountWindow()
        {
            Console.WriteLine("go user account");
            // отправка названия окна на запись
            // programCashReader.recordingLastWinsName(this.Title.ToString());
            
            UserAccountWindow userAccountWindow = new UserAccountWindow()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            userAccountWindow.Show();
        }

        public void goUserRegistrationWindow()
        {
            Console.WriteLine("go user registration");
            // отправка названия окна на запись
            // programCashReader.recordingLastWinsName(this.Title.ToString());
            
            UserRegistration userRegistrationWindow = new UserRegistration()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            userRegistrationWindow.Show();
        }

        // Формирование координат
        private string makeCoordinates()
        {
            string coord = this.Left.ToString() + "_" + this.Top.ToString();
            return coord;
        }

        public void swapToRightSide(object sender, RoutedEventArgs e)
        {
            setGameInfIndex++;
            completeInformationAboutGames();
        }

        public void swapToLeftSide(object sender, RoutedEventArgs e)
        {
            setGameInfIndex--;
            completeInformationAboutGames();
        }

        // возвращение к прошлому окну
        public void goBack(object sender, RoutedEventArgs e)
        {
            // извлечение имени предыдущего окна
            string lastWindowName = programCashReader.returnLastWindowsName();
            Console.WriteLine("последнее окно " + lastWindowName);
            // перевод окна в невидимый режим
            this.Visibility = Visibility.Hidden;
            // запись нынешнего окна как предыдущего, для дальнейшей смены

            // programCashReader.recordingLastWinsName(this.Title.ToString());
            // запись координат окна на момент смены
            programCashReader.recordingLastActiveWindowCoordinates(makeCoordinates());
            // смена на предыдущее окно
            switchWins.Windows_X_Names[lastWindowName]();
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
                programCashReader.clearCash();
                Environment.Exit(0);
            }

        }
    }
}
