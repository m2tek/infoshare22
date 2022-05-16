namespace Infoshare22.Functions;
public static class Extensions
{
    public static string FromBase64(this string input)
    {
        return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(input));
    }
}
