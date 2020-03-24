using System;
using System.Text.RegularExpressions;

namespace WheyMenIOValidation.Library
{
    public static class CustomerInfoValidation 
    {
        /// <summary>
        /// Checks name does not contain numbers, other non punctuation special characters
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidateName(string name)
        {
            if (Regex.IsMatch(name, @"^\p{L}+[' -]*\p{L}*$", RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks email is in format [identifier]@[domain]
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email)
        {
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static bool ValidateUsername(string username)
        {
            if (username.Length > 2) return true;
            return false;
        }
        public static bool ValidatePwd(string pwd)
        {
            if (pwd.Length > 2) return true;
            return false;
        }
    }

}
