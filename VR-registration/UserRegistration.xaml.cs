using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace VR_registration
{
    /// <summary>
    /// Логика взаимодействия для UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : Window
    {
        public bool seePassword = false;
        private Dictionary<int, string> mask = new Dictionary<int, string>()
        {
            {1, "#"}, {2, "#(#"}, {3, "#(##"}, {4, "#(###)"}, {5, "#(###)#"},
            {6, "#(###)##"}, {7, "#(###)###"}, {8, "#(###)###-#"}, {9, "#(###)###-##"}, 
            {10, "#(###)###-###"}, {11, "#(###)###-####"}
        };
        private bool ThreadEx = false;
        
        // ПУть до папки на ПК
        public const string computerFoldersPATH = @"C:\Users\Олег\Desktop\Хакатон\VR-registration\VR-registration\images\";
        // Путь до папки на Ноуте
        // public const string computerFoldersPATH = @"C:\Users\Олег\Desktop\Хакатон\VR-registration\VR-registration\images\";


        public ControlInputData controlInputData = new ControlInputData();
        public ConnectDataBase connect = new ConnectDataBase();
        public SwitchWindows switchWins = new SwitchWindows();
        public ProgramCashReader programCashReader = new ProgramCashReader();
        public RegistrationWorkFromDB registrtationWorkFromDB = new RegistrationWorkFromDB();
        public AuthorizationWorkFromDB authorizationWorkFromDB = new AuthorizationWorkFromDB();
        
        



        public UserRegistration()
        {
            InitializeComponent();
            createStartView();
            programCashReader.recordingLastWinsName(this.Title.ToString());
            // Потоки маски телефона
            Thread registrationPhoneMask = new Thread(createRegistrationPhoneInputMask);
            registrationPhoneMask.Start();
            Thread authorizationPhoneMask = new Thread(createAuthorizationPhoneInputMask);
            authorizationPhoneMask.Start();

        }
        // Очистка поля ввода
        public void cleanTextBoxR(object sender, RoutedEventArgs e)
        {
            REG_inputPhone.Text = "";
        }
        public void cleanTextBoxA(object sender, RoutedEventArgs e)
        {
            AUTH_inputPhone.Text = "";
        }

        private string putMask(TextBox takenTextBox, string lastPhoneInputVersion, string newPhoneInputVersion)
        {
            lastPhoneInputVersion = newPhoneInputVersion;
            var inputVersionWithReplace = newPhoneInputVersion.Replace("(", "")
                .Replace("-", "")
                .Replace(")", "");
            int inputLength = inputVersionWithReplace.Length;
            Console.WriteLine("Length: " + inputLength);
            var readyPhoneNumber = Convert.ToInt64(inputVersionWithReplace);
            this.Dispatcher.BeginInvoke(new Action(() => takenTextBox.Text = (readyPhoneNumber).ToString(mask[inputLength])));
            this.Dispatcher.BeginInvoke(new Action(() => takenTextBox.CaretIndex = takenTextBox.Text.Length));
            return lastPhoneInputVersion;
        }

        // маска при регистрации номера телефона
        private void createRegistrationPhoneInputMask()
        {
            
            var newPhoneInputVersion = "";
            var lastPhoneInputVersion = "";
            while (!ThreadEx)
            {
                this.Dispatcher.BeginInvoke(new Action(() => newPhoneInputVersion = REG_inputPhone.Text.ToString()));
                if (newPhoneInputVersion.Length > 0 && newPhoneInputVersion != lastPhoneInputVersion)
                {
                    lastPhoneInputVersion = putMask(REG_inputPhone, lastPhoneInputVersion, newPhoneInputVersion);
                    

                }
                Thread.Sleep(1);


            }
        }
        // маска при авторизации номера телефона
        private void createAuthorizationPhoneInputMask()
        {
            var newPhoneInputVersion = "";
            var lastPhoneInputVersion = "";
            while (!ThreadEx)
            {
                this.Dispatcher.BeginInvoke(new Action(() => newPhoneInputVersion = AUTH_inputPhone.Text.ToString()));
                if (newPhoneInputVersion.Length > 0 && newPhoneInputVersion != lastPhoneInputVersion)
                {
                    lastPhoneInputVersion = putMask(AUTH_inputPhone, lastPhoneInputVersion, newPhoneInputVersion);

                }
                Thread.Sleep(1);


            }
        }

        // Перемеещние окна на ЛКМ
        public void MoveWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DragMove();
            } catch (Exception)
            {
                Console.WriteLine("nothing");
            }
            
        }

        // Удаление маски телефона
        public string deletePhoneMask(string phoneMask)
        {
            return phoneMask.Replace("(", "")
                .Replace(")", "")
                .Replace("-", ""); ;
        }
        private void changePasswordValue()
        {
            /*
            Если пароль не видно -> назначаем основным паролем для чтения тот, что скрыт;
            Если пароль видно, назначаем на обработку тот пароль, что видно;
            */
            if (!seePassword)
            {
                ShowPassword_reg.Text = REG_inputPassword.Password.ToString();
                return;
            }
            REG_inputPassword.Password = ShowPassword_reg.Text.ToString();
        }
        private bool checkCorrectRegistrationInfo()
        {
            changePasswordValue();
            ShowPassword_reg.Text = REG_inputPassword.Password.ToString();
            controlInputData.controlTelephoneNumbers(deletePhoneMask(REG_inputPhone.Text.ToString()));
            string[] informationArray = new string[] 
            { 
                ShowPassword_reg.Text.ToString(), REG_inputEmail.Text.ToString(),
                REG_inputName.Text.ToString(), deletePhoneMask(REG_inputPhone.Text.ToString())
            };
            if (controlInputData.checkIncorrectSymbolsInTextInputs(informationArray)
                &&
                controlInputData.controlTelephoneNumbers(deletePhoneMask(REG_inputPhone.Text.ToString()))
                &&
                controlInputData.IsValidEmail(REG_inputEmail.Text.ToString()))
                
            {
                return true;
            }
            dumbMessageBox($"Проверьте корректность данных\n" +
                $"Проверьте чтобы не осталось пустых полей!\n" +
                $"Проверьте чтобы в введенных данных отсутствовали символы: ', -, ;");
            return false;
        }
        // Регистрация пользователя на сервере
        public void registrationUser(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Регистрация пользователя");

            if (checkCorrectRegistrationInfo())
            {
                string readyToRegistrationPassword = seePassword == false ?
                REG_inputPassword.Password.ToString() :
                ShowPassword_reg.Text.ToString();
                Dictionary<string, string> allRegistrationDataFromTextBoxes = new Dictionary<string, string>()
                {
                {"Name", REG_inputName.Text.ToString()},
                {"Password", readyToRegistrationPassword},
                {"Email", REG_inputEmail.Text.ToString()},
                {"Phone", deletePhoneMask(REG_inputPhone.Text.ToString())},
                {"Status", "U"},

                };
                // Регистрация пользователя
                if (registrtationWorkFromDB.registarationUserInUserTableFromDataBase(allRegistrationDataFromTextBoxes))
                {
                    ThreadEx = true;
                    displayMessageBox("Пользователь зарегистрирован");
                    MainWindow mainWindow = new MainWindow();
                    goUserAccountWindow();
                }
                else
                {
                    dumbMessageBox("Данные совпадают с другим аккаунтом!\n<b>Регистрация отменена!</b>\nИзмените номер телефона или Почтовый адрес!");
                }
            }
            ThreadEx = true;
        }

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

        // click авторизации пользователя
        public void authorizationUser(object sender, RoutedEventArgs e)
        {


            displayMessageBox("пользователь авторизован");
        }

        // запись координат нынешнего окна для появления нового окна в тех же координатах
        private string makeCoordinates()
        {
            return this.Left.ToString() + "_" + this.Top.ToString();
        }


        // возвращение к прошлому окну
        public void onLastWindow(object sender, RoutedEventArgs e)
        {

            string lastWindowName = programCashReader.returnLastWindowsName();
            this.Visibility = Visibility.Hidden;
            programCashReader.recordingLastActiveWindowCoordinates(makeCoordinates());
            switchWins.Windows_X_Names[lastWindowName]();
        }

        private void displayMessageBox(string messageBoxText)
        {
            MessageBox.Show(messageBoxText);
        }

        // увеличение колонки регистрации при создании окна
        private void createStartView()
        {
            Console.WriteLine("create start view");
            LeftShadow.Visibility = Visibility.Hidden;
            authColumn.Width = new GridLength(400);
            regColumn.Width = new GridLength(700);
            seePassBtn_reg.Visibility = Visibility.Visible;
            seePassBtn_Auth.Visibility = Visibility.Hidden;
            RightShadow.Visibility = Visibility.Visible;
        }

        // увеличение колонки регистрации при наведении мыши
        private void resizeRegColumn(object sender, RoutedEventArgs e)
        {
            LeftShadow.Visibility = Visibility.Hidden;
            authColumn.Width = new GridLength(400);
            regColumn.Width = new GridLength(700);
            seePassBtn_reg.Visibility = Visibility.Visible;
            seePassBtn_Auth.Visibility = Visibility.Hidden;
            RightShadow.Visibility = Visibility.Visible;
        }

        // увеличение колонки авторизации при наведении мыши
        private void resizeAuthColumn(object sender, RoutedEventArgs e)
        {
            RightShadow.Visibility = Visibility.Hidden;
            regColumn.Width = new GridLength(400);
            authColumn.Width = new GridLength(700);
            seePassBtn_reg.Visibility = Visibility.Hidden;
            seePassBtn_Auth.Visibility = Visibility.Visible;
            LeftShadow.Visibility = Visibility.Visible;
        }

        // очистка подсказки для пользователя
        private void ClearTextBox(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).Visibility = Visibility.Hidden;
        }

        
        
        // проявление подсказки при пустом поле
        private void FillTextBox(object sender, RoutedEventArgs e)
        {
            
            switch (((FrameworkElement)sender).Name.ToString())
            {
                case ("REG_inputName"):
                    // используем теонаоную операцию для выставления значения видимым или нет
                    Reg_inputName_textBlock.Visibility =
                        (REG_inputName.IsSelectionActive || REG_inputName.Text.ToString().Trim() != "")
                        ? Visibility.Hidden : Visibility.Visible;

                    break;
                case ("REG_inputPassword"):
                    Console.WriteLine(REG_inputPassword.IsSelectionActive);
                    Reg_inputPassword_textBlock.Visibility =
                        (REG_inputPassword.IsSelectionActive || REG_inputPassword.Password.ToString().Trim() != "")
                        ? Visibility.Hidden : Visibility.Visible;

                    break;
                case ("REG_inputEmail"):
                    Reg_inputEmail_textBlock.Visibility =
                        (REG_inputEmail.IsSelectionActive || REG_inputEmail.Text.ToString().Trim() != "")
                        ? Visibility.Hidden : Visibility.Visible;
                    
                    break;
                case ("REG_inputPhone"):
                    Reg_inputPhone_textBlock.Visibility =
                        (REG_inputPhone.IsSelectionActive || REG_inputPhone.Text.ToString().Trim() != "")
                        ? Visibility.Hidden : Visibility.Visible;

                    break;

                case ("AUTH_inputPhone"):
                    // используем теонаоную операцию для выставления значения видимым или нет
                    Auth_inputPhone_textBlock.Visibility =
                        (AUTH_inputPhone.IsSelectionActive || AUTH_inputPhone.Text.ToString().Trim() != "")
                        ? Visibility.Hidden : Visibility.Visible;

                    
                    break;
                case ("AUTH_inputPassword"):
                    
                    Auth_inputPassword_textBlock.Visibility =
                        (AUTH_inputPassword.IsSelectionActive || AUTH_inputPassword.Password.ToString().Trim() != "")
                        ? Visibility.Hidden : Visibility.Visible;
                    break;
                case ("ShowPassword_reg"):
                    Reg_inputPassword_textBlock.Visibility =
                         (ShowPassword_reg.IsSelectionActive || REG_inputPassword.Password.ToString().Trim() != "")
                         ? Visibility.Hidden : Visibility.Visible;
                    break;
                case ("ShowPassword_Auth"):
                    Auth_inputPassword_textBlock.Visibility =
                         (ShowPassword_Auth.IsSelectionActive || AUTH_inputPassword.Password.ToString().Trim() != "")
                         ? Visibility.Hidden : Visibility.Visible;
                    break;
                default:
                    break;
            }

        }

        public void ShowPass(object sender, RoutedEventArgs e)
        {
            if (!seePassword)
            {
                Console.WriteLine("show");
                ShowPassword_reg.Text = REG_inputPassword.Password.ToString();
                ShowPassword_Auth.Text = AUTH_inputPassword.Password.ToString();
                ShowPassword_reg.Visibility = Visibility.Visible;
                ShowPassword_Auth.Visibility = Visibility.Visible;
                Eye.ImageSource = new BitmapImage(
                new Uri(
                       $@"{computerFoldersPATH}Eye open.png"))
                {
                    DecodePixelHeight = 40,
                    DecodePixelWidth = 40
                };
                EyeAuth.ImageSource = new BitmapImage(
                new Uri(
                       $@"{computerFoldersPATH}Eye open.png"))
                {
                    DecodePixelHeight = 40,
                    DecodePixelWidth = 40
                };
                seePassword = true;
            }
            else
            {
                
                ShowPassword_reg.Visibility = Visibility.Hidden;
                ShowPassword_Auth.Visibility = Visibility.Hidden;
                EyeAuth.ImageSource = new BitmapImage(
                new Uri(
                       $@"{computerFoldersPATH}Eye close.png"));
                Eye.ImageSource = new BitmapImage(
                new Uri(
                       $@"{computerFoldersPATH}Eye close.png"));
                REG_inputPassword.Password = ShowPassword_reg.Text.ToString();
                AUTH_inputPassword.Password = ShowPassword_Auth.Text.ToString();
                seePassword = false;
            }
        }
        public bool dumbMessageBox(string message)
        {
            // Создание messagebox для защиты от случайных действий

            if (MessageBox.Show(message, "Служба поддержки", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
