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
        public bool checkIncorrectSymbolsInTextInputs(string inputText)
        {
            List<string> incorrectSymbolsArr = new List<string>()
            {
                "'", "-", ";"
            };
            if (inputText.Length == 0)
            {
                return false;
            }
            foreach (var symbol in inputText)
            {
                if (incorrectSymbolsArr.Contains(symbol.ToString()))
                {
                    return false;
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
            if (outPlusPhoneNumberController.IsMatch(telephoneNumberData))
            {
                return outPlusPhoneNumberController.IsMatch(telephoneNumberData);
            }
            return plusPhoneNumberController.IsMatch(telephoneNumberData);
        }

        // Проверка почты
        public string IsValidEmail(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return "false";

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
                return "false";
            }
            catch (ArgumentException)
            {
                return "false";
            }

            try
            {
                return $"{Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))}";
            }
            catch (RegexMatchTimeoutException)
            {
                return "false";
            }
        }
    }
}
