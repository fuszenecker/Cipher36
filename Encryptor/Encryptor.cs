using System.Text;

internal static class Encryptor
{
    private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static int GetIndex(char ch) => alphabet.IndexOf(ch);
    private static char GetChar(int idx) => alphabet[(idx  + alphabet.Length) % alphabet.Length];

    public static string Encrypt(string password, string text, bool autokey, bool decrypt, bool verbose)
    {
        string result = string.Empty;
        int index = 0;
        password = password.ToUpper();
        text = text.ToUpper();

        foreach (var character in text)
        {
            // C = K - P
            char passwordChar = index < password.Length ? password[index] :
                (autokey, decrypt) switch
                {
                    (false, _)      => password[index % password.Length],
                    (true, false)   => text[index - password.Length],
                    (true, true)    => result[index - password.Length],
                };

            if (verbose)
            {
                Console.Write(passwordChar);
            }

            result += GetChar(GetIndex(passwordChar) - GetIndex(character));
            index++;
        }

        if (verbose)
        {
            Console.WriteLine("\n" + text);
        }

        return result;
    }
}