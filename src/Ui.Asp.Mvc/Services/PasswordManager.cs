using System.Security.Cryptography;
using System.Text;

namespace Ui.Asp.Mvc.Services;

public static class PasswordManager
{
    public static string CreatePassword()
    {
        return GenerateNewPassword();
    }

    private static string GenerateNewPassword()
    {
        Random rng = new Random();
        int passwordLength = rng.Next(8, 16);
        char[] password = new char[passwordLength];
        char[] specialChars = ['!', '@', '#', '$', '%', '&', '*', '_', '+', '?'];
        
        password[0] = Convert.ToChar(rng.Next(65, 91));
        password[1] = Convert.ToChar(rng.Next(48, 58));
        password[2] = specialChars[rng.Next(specialChars.Length)];

        for (int i = 3; i < password.Length; i++)
        {
            password[i] = rng.Next(4) switch
            {
                0 => Convert.ToChar(rng.Next(65, 91)),
                1 => Convert.ToChar(rng.Next(48, 58)),
                2 => specialChars[rng.Next(specialChars.Length)],
                _ => Convert.ToChar(rng.Next(97, 123))
            };
        }
        
        return new string(password.OrderBy(c => rng.Next()).ToArray());
    }
}
