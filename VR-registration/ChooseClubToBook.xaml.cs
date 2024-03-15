using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VR_registration.Properties;

namespace VR_registration
{
    /// <summary>
    /// Логика взаимодействия для ChooseClubToBook.xaml
    /// </summary>
    public partial class ChooseClubToBook : Window
    {
        public ProgramCashReader programCashReader = new ProgramCashReader();
        public RegistrationWorkFromDB registrtationWorkFromDB = new RegistrationWorkFromDB();
        public AuthorizationWorkFromDB authorizationWorkFromDB = new AuthorizationWorkFromDB();
        public ConnectDataBase connectDataBaseClass = new ConnectDataBase();
        public SwitchWindows switchWins = new SwitchWindows();

        public const string computerFoldersPATH = @"picture\";


        private Dictionary<string, string> buttonsNamesDict = new Dictionary<string, string>();
        public string choosenClub;
        public ChooseClubToBook()
        {
            InitializeComponent();
            compliteFullInformationAboutClubs();
            programCashReader.recordingLastWinsName(this.Title.ToString());
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
        // Заполнение списка клубов
        public void compliteFullInformationAboutClubs()
        {
            Dictionary<string, Dictionary<string, string>> returnInformationAboutClubs =
                connectDataBaseClass.returnClubsInformation();
            
            int clubInformationButtonIndex = 0;
            
            foreach (var dictionaryElement in returnInformationAboutClubs)
            {
                Console.WriteLine(dictionaryElement.Key);
                
                Border clubCard = createClubCardToChoose();
                Grid informationGrid = createUniverseGrid();
                informationGrid.Children.Add(CLUBNAMEtextblockReturner("VR Zone"));
                informationGrid.Children.Add(setCardBackGround());
                string clubInformationButtonName = "buttonName_" + Convert.ToString(clubInformationButtonIndex);
                buttonsNamesDict[clubInformationButtonName] = dictionaryElement.Value["Address"];
                clubInformationButtonIndex++;
                foreach (var invaildDioctionaryElement in dictionaryElement.Value)
                {
                    Console.WriteLine(invaildDioctionaryElement.Key + " : " + invaildDioctionaryElement.Value);
                    switch (invaildDioctionaryElement.Key)
                    {
                        case ("Address"):
                            informationGrid.Children.Add(
                                ADDRESStextboxreturner(invaildDioctionaryElement.Value));
                            break;
                        case ("Mail"):
                            informationGrid.Children.Add(
                                MAILtextblockReturner(invaildDioctionaryElement.Value));
                            break;

                        case ("Phone"):
                            informationGrid.Children.Add(
                                PHONEtextblockReturner(invaildDioctionaryElement.Value));
                            break;
                    }

                }
                informationGrid.Children.Add(createInformationCardBorder(clubInformationButtonName));
                clubCard.Child = informationGrid;
                cardsShower.Children.Add(clubCard);
            }
            Border saleAgitCard = createClubCardToChoose(50);
            saleAgitCard.Child = createSaleAgitation();
            cardsShower.Children.Add(saleAgitCard);

        }

        // Реклама
        private TextBlock createSaleAgitation()
        {
            TextBlock saleAgitation = new TextBlock
            {
                Text = "Успей использовать промокод 'РИСК'",
                FontSize = 25,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Black)
            };
            return saleAgitation;
        }
        // Определение выбранного клуба
        private void goClubBooking(string informationBUtonName)
        {
            Console.WriteLine("go club booking");
            Settings.Default["choosenClubInfo"] = buttonsNamesDict[informationBUtonName];
            Settings.Default.Save();
            // отправка названия окна на запись
            //programCashReader.recordingLastWinsName(this.Title.ToString());
            ClubBookingClass clubBookingClass = new ClubBookingClass()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };

            this.Visibility = Visibility.Hidden;
            clubBookingClass.Show();
        }

        //-------------------------------------OTHER-----------------------------------------
        // Сетка карточки
        private Grid createUniverseGrid()
        {
            Grid cardInformatioGrid = new Grid();
            return cardInformatioGrid;
        }
        // Подложка
        private Image setCardBackGround()
        {
            Image cardBG = new Image();
            cardBG.Source = new BitmapImage(
                new Uri($"{computerFoldersPATH}cardbackGround.png"))
            {
                DecodePixelHeight = 400,
                DecodePixelWidth = 1100
            };
            return cardBG;
        }
        //--------------------------------------BORDER----------------------------------------
        // Карточка лоя кнопки
        private Border createInformationCardBorder(string buttonName)
        {
            Border buttonContentDocker = new Border
            {
                Width = 150,
                Height = 70,
                Background = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(150, 250, 0, 0),
                CornerRadius = new CornerRadius(20)
            };
            Grid buttonBorderGrid = createUniverseGrid();
            buttonBorderGrid.Children.Add(createContentToBorder());
            buttonBorderGrid.Children.Add(createInformationCardButton(buttonName));
            buttonContentDocker.Child = buttonBorderGrid;
            return buttonContentDocker;
        }


        // Карточки клуба для выбора
        private Border createClubCardToChoose(int HEIGHT = 400)
        {
            Border clubCardBorder = new Border();
            clubCardBorder.BorderBrush = new SolidColorBrush(Colors.MediumPurple);
            clubCardBorder.BorderThickness = new Thickness(3);
            clubCardBorder.Height = HEIGHT;
            return clubCardBorder;
        }
        // ---------------------------------------BUTTON--------------------------------------
        // Кнопка для подробной информации
        private Button createInformationCardButton(string nameOfButton)
        {
            Console.WriteLine("name of button: " + nameOfButton);
            Button clubInformationButton = new Button
            {
                Name = nameOfButton,
                Opacity = 0,
                Cursor = Cursors.Hand
            };
            
            clubInformationButton.Click += (sender, args) =>
            {
                goClubBooking(clubInformationButton.Name.ToString());
            };
            

            return clubInformationButton;
        }
        // ----------------------------------------TEXT BLOCK------------------------------------
        // Адрес клуба
        private TextBlock ADDRESStextboxreturner(string clubADDRESS)
        {
            TextBlock addressToVrClubsCard = new TextBlock
            {
                Text = "Адрес: " + clubADDRESS,
                FontSize = 30,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(100, 150, 0, 0)
            };
            return addressToVrClubsCard;
        }
        
        // Текст для кнопки
        private TextBlock createContentToBorder()
        {
            TextBlock content = new TextBlock
            {
                Text = "Подробнее",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 25,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,

            };
            return content;
        }

        // Почта пользователя
        private TextBlock MAILtextblockReturner(string clubMAIL)
        {
            TextBlock addressToVrClubsCard = new TextBlock
            {
                Text = "Почта: " + clubMAIL,
                FontSize = 25,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(100, 200, 0, 0)
            };
            return addressToVrClubsCard;
        }


        private TextBlock PHONEtextblockReturner(string clubPHONE)
        {
            TextBlock addressToVrClubsCard = new TextBlock
            {
                Text = "Телефон: " + clubPHONE,
                FontSize = 25,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(100, 250, 0, 0)
            };
            return addressToVrClubsCard;
        }

        // Имя клуба на карточке
        private TextBlock CLUBNAMEtextblockReturner(string clubNameToTitle)
        {
            
            TextBlock clubNameTextBlock = new TextBlock()
            {
                Text = clubNameToTitle,
                FontSize = 40,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(100, 50 , 0, 0)

            };
            return clubNameTextBlock;
        }

        

        public void goUserAcc(object sender, RoutedEventArgs e)
        {
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

        // Формирование координат
        private string makeCoordinates()
        {
            return this.Left.ToString() + "_" + this.Top.ToString();
        }

        // возвращение к прошлому окну
        public void goBack(object sender, RoutedEventArgs e)
        {
            string lastWindowName = programCashReader.returnLastWindowsName();
            Console.WriteLine("последнее окно " + lastWindowName);
            this.Visibility = Visibility.Hidden;
            programCashReader.recordingLastActiveWindowCoordinates(makeCoordinates());
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
