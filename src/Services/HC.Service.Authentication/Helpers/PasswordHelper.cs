using System.Text;

namespace HC.Service.Authentication.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Endcode Password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static (bool, string) EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return (true, encodedData);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Decode Password
        /// </summary>
        /// <param name="encodedData"></param>
        /// <returns></returns>
        public static string DecodeFrom64(string encodedData)
        {
            UTF8Encoding encoder = new();
            Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new(decoded_char);
            return result;
        }
    }
}
