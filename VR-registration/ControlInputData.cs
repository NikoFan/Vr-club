using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace VR_registration
{
    // Проверка вводимых данных
    public class ControlInputData
    {
        // Проверка sqli
        public bool checkIncorrectSymbolsInTextInputs(string[] arrayOfInputInformation)
        {
            // Массив запрещенных символов
            List<string> incorrectSymbolsArr = new List<string>()
            {
                "'", "-", ";"
            };
            // Цикл для перебора данных
            foreach (string stringData in arrayOfInputInformation)
            {
                if (stringData.Length == 0)
                {
                    return false;
                }
                foreach (var symbol in stringData)
                {
                    if (incorrectSymbolsArr.Contains(symbol.ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Проверка номера телефона
        public bool controlTelephoneNumbers(string telephoneNumberData)
        {
            Regex outPlusPhoneNumberController = new Regex(
                "[8][0-9]{10}");
            Regex plusPhoneNumberController = new Regex(
                "[+7][0-9]{10}");
            Console.WriteLine("Results 1: " + outPlusPhoneNumberController.IsMatch(telephoneNumberData));
            Console.WriteLine("Results 2: " + plusPhoneNumberController.IsMatch(telephoneNumberData));
            if (outPlusPhoneNumberController.IsMatch(telephoneNumberData))
            {
                
                return outPlusPhoneNumberController.IsMatch(telephoneNumberData);
            }
            return plusPhoneNumberController.IsMatch(telephoneNumberData);
        }

        // Проверка почты
        public bool IsValidEmail(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
