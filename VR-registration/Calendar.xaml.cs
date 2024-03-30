using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography.X509Certificates;
using VR_registration.Properties;

namespace VR_registration
{
    /// <summary>
    /// Логика взаимодействия для Calendar.xaml
    /// </summary>
    public partial class Calendar : Window 
    {

        private string calendarBlocksName = "calendarBlock_number_";
        public string computerFoldersPATH = Convert.ToString(String.Join("\\", Environment.CurrentDirectory.ToString().Split('\\').Take(
            Environment.CurrentDirectory.ToString().Split('\\').Length - 2))) + "/Picture/";
        private Dictionary<int, int> countDaysInMonthsArray = new Dictionary<int, int>()
        {
            {1,31 }, {2,28 }, {3,31 }, {4,30 }, {5,31 }, {6, 30},
            {7,31 }, {8,31 }, {9,30 }, {10,31 }, {11,30 }, {12,31}
        };

        private List<int> leftSideOffset = new List<int>()
        {
            0, 0, 0, 0, 310, 625, 945
        };

        private List<int> topSideOffset = new List<int>()
        {
            0, 105, 210, 315, 420, 525
        };

        private List<int> rightSideOffset = new List<int>()
        {
            945, 633, 320, 5, 0, 0, 0
        };

        private List<string> backGroundColors = new List<string>()
        {
            "#E5434B", "#DD6E74", "#731216", "#28B61B"
        };
        private Dictionary<string, int> weekDaysCounter = new Dictionary<string, int>()
        {
            {"monday", 1},{ "tuesday", 2},{ "wednesday", 3},{"thursday",  4},
            { "friday", 5},{ "saturday", 6},{"sunday", 7}
        };
        private string isDateBlocked = "block";
        private int yearNumber = 0;
        private int monthNumber = 0;
        private int daysInMonth = 0;
        int correctNowDateDay = Convert.ToInt32(DateTime.Today.ToString().Split('.')[0]);
        int correctNowDateMonth = Convert.ToInt32(DateTime.Today.ToString().Split('.')[1]);
        private bool threadStoper = false;
        private string dateUnderMouse = "??";

        public Calendar()
        {
            InitializeComponent();

            takeCorrectDateInformation();
            DisplayCalendar();
            ProgramCashReader programCashReader = new ProgramCashReader();
            programCashReader.recordingLastWinsName(this.Title.ToString());
            programCashReader = null;
            Thread dateSetterThread = new Thread(setDateUnderMouseInTextBlock);
            dateSetterThread.Start();


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

        public bool dumbMessageBox(string message)
        {
            // Создание messagebox для защиты от случайных действий

            var userReactionToMessageBoxResult = MessageBox.Show(message, "Проверка", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (userReactionToMessageBoxResult == MessageBoxResult.Yes)
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
                programCashReader = null;
                threadStoper = true;
                Environment.Exit(0);
            }

        }

        

        // Формирование координат
        private string makeCoordinates()
        {
            string coord = this.Left.ToString() + "_" + this.Top.ToString();
            return coord;
        }

        // возвращение к прошлому окну
        public void goBack(object sender, RoutedEventArgs e)
        {
            threadStoper = true;
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

        public void goUserAcc(object sender, RoutedEventArgs e)
        {
            threadStoper = true;
            ProgramCashReader programCashReader = new ProgramCashReader();
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

        public void goUserAccountWindow()
        {
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
            UserRegistration userRegistrationWindow = new UserRegistration()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            userRegistrationWindow.Show();
        }

        //------------------------CREATE CALENDAR--------------------
        // Изменение месяца
        public void monthMinus(object sender, RoutedEventArgs e)
        {
            if (monthNumber - 1 < 1)
            {
                monthNumber = 12;
                yearNumber--;
            }
            else
            {
                monthNumber--;
            }
            if (monthNumber != 2)
            {
                daysInMonth = countDaysInMonthsArray[monthNumber];
            }
            else
            {
                if (yearNumber % 4 == 0)
                {
                    daysInMonth = countDaysInMonthsArray[monthNumber] + 1;
                }
                else
                {
                    daysInMonth = countDaysInMonthsArray[monthNumber];
                }
            }
            dateUnderMouse = "??";
            DisplayCalendar();
        }
        // Изменение месяца
        public void monthPlus(object sender, RoutedEventArgs e)
        {
            if (monthNumber + 1 > 12)
            {
                monthNumber = 1;
                yearNumber++;
            }
            else
            {
                monthNumber++;
            }
            if (monthNumber != 2)
            {
                daysInMonth = countDaysInMonthsArray[monthNumber];
            }
            else
            {
                if (yearNumber % 4 == 0)
                {
                    daysInMonth = countDaysInMonthsArray[monthNumber] + 1;
                }
                else
                {
                    daysInMonth = countDaysInMonthsArray[monthNumber];
                }
            }
            dateUnderMouse = "??";
            DisplayCalendar();
        }
        // Дата
        private TextBlock createDateNumber(int number)
        {
            TextBlock dateNumber = new TextBlock()
            {
                Width = 150,
                Height = 100,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = new SolidColorBrush(Colors.Black),
                Text = Convert.ToString(number),
                TextAlignment = TextAlignment.Center,
                Padding = new Thickness(0, 0, 0, 0),
                FontSize = 30,
                FontWeight = FontWeights.Bold
            };
            dateNumber.MouseEnter += (sender, args) =>
            {
                dateUnderMouse = dateNumber.Text.ToString();
            };
            return dateNumber;
        }
        // Изменение фона при наведении
        private void changeCalendarBlockColorWithMouseEnter(Border nameOfChoosenBlock, string isDate)
        {
            
            BitmapImage img
                = new BitmapImage(
                new Uri($@"{computerFoldersPATH}bg-{isDate}-enter_1.png"));
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;
            nameOfChoosenBlock.Background = image;
        }
        // Изменение фона на прошлый
        private void blocksColorBack(Border nameOfChoosenBlock, string isDate)
        {
            BitmapImage img
                = new BitmapImage(
                new Uri($@"{computerFoldersPATH}bg-{isDate}-leave_1.png"));
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;
            nameOfChoosenBlock.Background = image;
        }
        // Блок календаря
        private Border createCalendarBlocks(List<int> blocksOffsetParams,string nameOfBlock, string isDate)
        {

            BitmapImage img
                = new BitmapImage(
                new Uri($@"{computerFoldersPATH}bg-{isDateBlocked}-leave_1.png"));
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;

            Border calendarBlock = new Border()
            {
                Name = calendarBlocksName + nameOfBlock,
                Width = 150,
                Height = 100,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(
                    leftSideOffset[blocksOffsetParams[0]],
                    topSideOffset[blocksOffsetParams[1]],
                    rightSideOffset[blocksOffsetParams[0]], 0),
                BorderBrush = new SolidColorBrush(Colors.MediumPurple),
                BorderThickness = new Thickness(2),
                Background = image,
                CornerRadius = new CornerRadius(15),

            };

            calendarBlock.MouseEnter += (sender, args) =>
            {
                changeCalendarBlockColorWithMouseEnter(calendarBlock, isDate);
            };
            calendarBlock.MouseLeave += (sender, args) =>
            {
                blocksColorBack(calendarBlock, isDate);
            };
            calendarBlock.MouseDown += (sender, args) =>
            {
                Console.WriteLine(isDate);
                if (isDate != "block")
                    bookDateInClub(dateUnderMouse);
                else
                    dumbMessageBox("Дата недоступна к бронированию!");
            };
            return calendarBlock;
        }

        private void bookDateInClub(string choosenDate)
        {
            Console.WriteLine("Выбрана дата");
            if (dumbMessageBox($"Вы хотите забронировать дату\n{dateUnderMouse}.{monthNumber}.{yearNumber}\nВ клубе:\n{Settings.Default["choosenClubInfo"]}"))
            {
                if (new ProgramCashReader().returnActiveUserId() == "out" || new ProgramCashReader().returnActiveUserId() == "")
                {
                    dumbMessageBox("Авторизуйтесь в аккаунте или создайте новый для оплаты!");
                    goUserRegistrationWindow();
                    return;
                }
                Settings.Default["Date"] = $"{yearNumber}-{monthNumber}-{dateUnderMouse}";
                Settings.Default.Save();
                OrderClubClass orderClubClass = new OrderClubClass()
                {
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Left = Left,
                    Top = Top
                };

                this.Visibility = Visibility.Hidden;
                orderClubClass.Show();
            }
        }


        // Получаем дату сегодня
        private void takeCorrectDateInformation()
        {
            string[] splitDate = Convert.ToString(DateTime.Now).Split(' ');
            yearNumber = Convert.ToInt32(splitDate[0].Split('.')[2]);
            monthNumber = Convert.ToInt32(splitDate[0].Split('.')[1]);
            daysInMonth = countDaysInMonthsArray[Convert.ToInt32(monthNumber)];
        }
        // Замена даты на актуальную
        private void setDateUnderMouseInTextBlock()
        {
            string oldActiveDate = dateUnderMouse;
            int oldMonthNumber = monthNumber;
            activeDateUnderUsersMouse.Dispatcher.BeginInvoke(new Action(() => activeDateUnderUsersMouse.Text = $"{dateUnderMouse}/{monthNumber}/{yearNumber}"));
            while (!threadStoper)
            {
                if (oldActiveDate != dateUnderMouse || monthNumber != oldMonthNumber)
                {
                    activeDateUnderUsersMouse.Dispatcher.BeginInvoke(new Action(() => activeDateUnderUsersMouse.Text = $"{dateUnderMouse}/{monthNumber}/{yearNumber}"));
                    oldActiveDate = dateUnderMouse;
                }
                Thread.Sleep(100);
            }
        }

        // Создание календаря
        private void DisplayCalendar()
        {
            /*
            insert into Запись
            Values(1, 5, '2024-03-02', '2024-04-02', N'2300 р/час', 1, N'Комсомольский проспект, 27с5', 1)
             */

            // Очистака сетки от прошлых блоков
            calendarGrid1.Children.Clear();

            Console.WriteLine(yearNumber);
            Console.WriteLine(monthNumber);
            Console.WriteLine(daysInMonth);
            

            DateTime now = new DateTime(yearNumber, monthNumber, daysInMonth);
            Console.WriteLine($"NOW: {now}");
            DateTime startOfMonth = new DateTime(yearNumber, monthNumber, 1);
            int dayOfTheWeek = weekDaysCounter[startOfMonth.DayOfWeek.ToString().Trim().ToLower()];
            Console.WriteLine("day of week: " + startOfMonth.DayOfWeek.ToString());
            // Create usercontrol
            int leftRightArgs = 0;
            int TopArg = 0;
            int numberOfDate = 1;
            Console.WriteLine($"ДЕНЬ НЕДЕЛИ: {dayOfTheWeek}");
            for (int iterations = 0; iterations < 42; iterations++)
            {
                List<int> calendarBlocksCreateArguments = new List<int>();
                if (iterations + 1 >= dayOfTheWeek && iterations <= daysInMonth + dayOfTheWeek - 2)
                {
                    Console.WriteLine(numberOfDate + " " + correctNowDateDay + " " + monthNumber + " " + correctNowDateMonth);
                    isDateBlocked = "open";
                    if (numberOfDate <= correctNowDateDay || 
                        monthNumber < correctNowDateMonth)
                    {
                        isDateBlocked = "block";
                        if (monthNumber > correctNowDateMonth)
                        {
                            isDateBlocked = "open";
                        }
                        
                    }
                    calendarBlocksCreateArguments.Add(leftRightArgs);
                    calendarBlocksCreateArguments.Add(TopArg);
                    string nameToCalendrBlock = $"{numberOfDate}_{monthNumber}_{yearNumber}";
                    Border calendarBlockElement = createCalendarBlocks(calendarBlocksCreateArguments,
                        nameToCalendrBlock, isDateBlocked);
                    calendarBlockElement.Child = createDateNumber(numberOfDate);
                    calendarGrid1.Children.Add(calendarBlockElement);
                    numberOfDate++;
                }
                
                switch((iterations + 1) / 7)
                {
                    case (0):
                        TopArg = 0;
                        break;
                    case (1):
                        TopArg = 1;
                        break;
                    case (2):
                        TopArg = 2;
                        break;
                    case (3):
                        TopArg = 3;
                        break;
                    case (4):
                        TopArg = 4;
                        break;
                    case (5):
                        TopArg = 5;
                        break;
                }
                leftRightArgs++;
                if ((leftRightArgs) == 7)
                {
                    leftRightArgs = 0;
                }
                



            }


        }
    }
}
