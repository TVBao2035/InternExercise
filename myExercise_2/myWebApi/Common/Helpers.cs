using System.Text.RegularExpressions;
using myWebApi.Enity;

namespace myWebApi.Common
{
    public class Helpers
    {
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }


        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(0[0-9]{9})$").Success;
        }


        public static bool ValidateEmailAndPhone(string email, string phone)
        {
            if (
                    email.Trim().Length == 0
                    || !Helpers.IsValidEmail(email)
                    || phone.Trim().Length == 0
                    || !Helpers.IsPhoneNumber(phone)
                )
            {
                return false;
            }
            return true;
        }
    }
}
