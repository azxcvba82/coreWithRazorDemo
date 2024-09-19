namespace WebApplicationDemo.Util
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static bool ContainsSpecialCharacter(string input)
        {
            char[] specialCharacters = { '#', '$', '@', '!', '%', '&' };

            foreach (char c in specialCharacters)
            {
                if (input.Contains(c))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
