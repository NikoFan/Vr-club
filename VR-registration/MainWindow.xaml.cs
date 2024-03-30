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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;


namespace VR_registration
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool StopThread = false;
        private int bannersPhotoIndex = 1;
        public string computerFoldersPATH = Convert.ToString(String.Join("\\", Environment.CurrentDirectory.ToString().Split('\\').Take(
            Environment.CurrentDirectory.ToString().Split('\\').Length-2))) + "/Picture/";

        public MainWindow()
        {
            InitializeComponent();
            ProgramCashReader programCashReader = new ProgramCashReader();
            programCashReader.clearCash();
            programCashReader.recordingLastWinsName(this.Title.ToString());
            programCashReader = null;
            // создание и запуск потока по смене фотографий
            Thread pictureChengerThread = new Thread(pictureChanger);
            pictureChengerThread.Start();
            

        }

        
        // Метод для передвижения окна нажав на него ЛКМ
        public void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
                Console.WriteLine(Head.GetType());
            }
            catch (Exception)
            {
                Console.WriteLine("nothing");
            }
        }

        
        // метод потока для смены фотографий
        public void pictureChanger()
        {
            while (!StopThread)
            {
                
                Console.WriteLine("start another thread " + bannersPhotoIndex);
                Console.WriteLine(Environment.CurrentDirectory);
                Console.WriteLine($@"{computerFoldersPATH}poster_{bannersPhotoIndex}.jpg");
                Posters.Dispatcher.BeginInvoke(new Action(() => Posters.ImageSource = new BitmapImage(
                new Uri(
                       $@"{computerFoldersPATH}poster_{bannersPhotoIndex}.jpg", UriKind.RelativeOrAbsolute))
                {
                    DecodePixelWidth = 600,
                    DecodePixelHeight = 400
                }));
                bannersPhotoIndex += 1;
                if (bannersPhotoIndex == 4)
                {
                    bannersPhotoIndex = 1;
                }
                
                Thread.Sleep(2000);
                
            }
        }

        public void goUserAcc(object sender, RoutedEventArgs e)
        {
            StopThread = true;
            if (new ProgramCashReader().returnActiveUserId() == "out" || new ProgramCashReader().returnActiveUserId() == "")
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
            
            StopThread = true;
            UserAccountWindow userAccountWindow = new UserAccountWindow()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            Head.Children.Clear();
            Body.Children.Clear();
            userAccountWindow.Show();
        }

        public void goUserRegistrationWindow()
        {
            StopThread = true;
            UserRegistration userRegistrationWindow = new UserRegistration()
            {
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            Head.Children.Clear();
            Body.Children.Clear();
            userRegistrationWindow.Show();
        }


        // Формирование координат
        private string makeCoordinates()
        {
            return this.Left.ToString() + "_" + this.Top.ToString(); ;
        }

        // возвращение к прошлому окну
        public void goBack(object sender, RoutedEventArgs e)
        {
            StopThread = true;
            string lastWindowName = new ProgramCashReader().returnLastWindowsName().Replace("]", "");
            Console.WriteLine("последнее окно " + lastWindowName);
            this.Visibility = Visibility.Hidden;
            new ProgramCashReader().recordingLastActiveWindowCoordinates(makeCoordinates());
            new SwitchWindows().Windows_X_Names[lastWindowName]();
            Head.Children.Clear();
            Body.Children.Clear();
        }
        public void goVK(object sender, RoutedEventArgs e)
        {
            StopThread = true;
            Console.WriteLine("go VK");
            ProcessStartInfo linkWay = new ProcessStartInfo("https://vk.com/");
            Process.Start(linkWay);
        }
        public void goYouTube(object sender, RoutedEventArgs e)
        {
            StopThread = true;
            Console.WriteLine("go YouTube");
            ProcessStartInfo linkWay = new ProcessStartInfo("https://www.youtube.com/");
            Process.Start(linkWay);
        }
        public void goTelegram(object sender, RoutedEventArgs e)
        {
            StopThread = true;
            Console.WriteLine("go Telegram");
            ProcessStartInfo linkWay = new ProcessStartInfo("https://t.me/+TjGTywz1xPRjNzg6");
            Process.Start(linkWay);
            
        }

        // Бронирование времени
        public void bookPlace(object sender, RoutedEventArgs e)
        {
            
            StopThread = true;
            ChooseClubToBook chooseClubToBook = new ChooseClubToBook()
            {
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            Head.Children.Clear();
            Body.Children.Clear();
            chooseClubToBook.Show();
            
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
                StopThread = true;
                new ProgramCashReader().clearCash();
                Environment.Exit(0);
            }

        }
    }
}
