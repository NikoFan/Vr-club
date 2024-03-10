using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using VR_registration.Properties;

namespace VR_registration
{
    public class ProgramCashReader
    {
        public void clearCash()
        {
            Settings.Default["lastWindowName"] = "";
            Settings.Default.Save();
            Settings.Default["coordinatesLastWindow"] = "";
            Settings.Default.Save();
        }
        // Запись имени окна
        public void recordingLastWinsName(string windowName)
        {
            if (Settings.Default["lastWindowName"].ToString().Length != 0)
            {
                Console.WriteLine(Settings.Default["lastWindowName"].ToString());
                Console.WriteLine(Settings.Default["lastWindowName"].ToString().Length);
                if (windowName.Length != 0)
                {
                    Console.WriteLine("windowName: " + windowName + "|");
                    Console.WriteLine("windowName L: " + windowName.Length);
                    Settings.Default["lastWindowName"] += "_" + windowName;
                    Settings.Default.Save();
                    
                }
                return;

            }
            Settings.Default["lastWindowName"] = "";
            Settings.Default.Save();
            Settings.Default["lastWindowName"] += windowName;
            Settings.Default.Save();

        }

        public void createStartWindow()
        {
            Settings.Default["lastWindowName"] = "";
            Settings.Default.Save();
            Settings.Default["lastWindowName"] += "Главное меню";
            Settings.Default.Save();
        }

        private void rewrite(string windowName)
        {
            Settings.Default["lastWindowName"] += windowName;
            Settings.Default.Save();
        }

        // Возврат имени окна
        public string returnLastWindowsName()
        {
            

            string[] cashArray = Settings.Default["lastWindowName"].ToString().Split('_');
            string findingWindowsName = "";
            Console.WriteLine("------------------------------------------- " + cashArray.Length);
            if (cashArray.Length == 1)
            {
                Console.WriteLine("|||||||||||||||");
                Console.WriteLine(String.Join(" ", cashArray));
                Settings.Default["lastWindowName"] = "";
                Settings.Default.Save();
                return cashArray[cashArray.Length-1];

            }
            try
            {
                findingWindowsName = cashArray[
                Settings.Default["lastWindowName"].ToString().Split('_').Length - 2];
            } catch (Exception)
            {
                findingWindowsName = cashArray[
                Settings.Default["lastWindowName"].ToString().Split('_').Length - 1];
            }
            int iteration = 0;
            Settings.Default["lastWindowName"] = "";
            Settings.Default.Save();
            while (cashArray[iteration] != findingWindowsName)
            {
                Console.WriteLine("Element: " + cashArray[iteration]);
                recordingLastWinsName(cashArray[iteration]);
                iteration++;
            }
            
            Console.WriteLine("Backer: " + findingWindowsName);
            return findingWindowsName;
        }

        // Запись координат окна
        public void recordingLastActiveWindowCoordinates(string lastWindowCoordinates)
        {
            Settings.Default["coordinatesLastWindow"] =  lastWindowCoordinates;
            Settings.Default.Save();
        }

        // Возврат координат последнего окна
        public string returnLastRecordingCoordinates()
        {
            return Settings.Default["coordinatesLastWindow"].ToString();
        }
        // Сохранение ID активного пользвателя
        public void recordingCurrentCustomersIdInCash(int currentUserID)
        {
            Console.WriteLine(":::::"+Convert.ToString(currentUserID));
            Settings.Default["activeUserId"] = Convert.ToString(currentUserID);
            Settings.Default.Save();
            Console.WriteLine("-- "+Settings.Default["activeUserId"]);
        }
        // Возвращение активного ID пользователя
        public string returnActiveUserId()
        {   
            return Settings.Default["activeUserId"].ToString();
        }

        public void recordingInactiveWordInCash()
        {
            Settings.Default["activeUserId"] = "out";
            Settings.Default.Save();
        }
    }
}
