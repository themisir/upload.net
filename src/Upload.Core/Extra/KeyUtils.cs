namespace Upload.Core.Extra;

public static class KeyUtils
{
    public static string NormalizeKey(string key)
    {
        if (key.Length == 0) return string.Empty;

        key = key.Replace('\\', '/');

        if (key[0] == '/')
        {
            return key[^1] == '/' ? key[1..^1] : key[1..];
        }
        if (key[^1] == '/')
        {
            return key[..^1];
        }

        return key;
    }
    
    public static void MushBeSafeKey(string key)
    {
        if (key.Contains(".."))
        {
            throw new ApplicationException("Key should not contain '..'");
        }
    }

    public static string GetUniqueKey(string name)
    {
        // todo: fix me
        return name;
    }
}