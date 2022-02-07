using System.Text;

namespace server.Utils.Game;

public static class MultiNames
{
    public static string ToMessage<T>(List<T> names)
    {
        if (names.Count == 0) return "";
        StringBuilder result = new(names[0]?.ToString());
        for (int i = 1; i < names.Count; i++)
        {
            result.Append(',').Append(names[i]);
        }
        return result.ToString();
    }

    public static List<string> ToNames(string message)
    {
        return new List<string>(message.Split(','));
    }
}