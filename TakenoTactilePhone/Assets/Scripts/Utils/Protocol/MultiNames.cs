using System.Collections.Generic;
using System.Text;

public class MultiNames
{
    public static string ToMessage(List<string> names)
    {
        if (names.Count == 0) return "";
        StringBuilder result = new StringBuilder(names[0]);
        for (int i = 1; i < names.Count; i++)
        {
            result.Append(names[i]).Append(',');
        }
        return result.ToString();
    }

    public static List<string> ToNames(string message)
    {
        return new List<string>(message.Split(','));
    }
}