using System.Runtime.CompilerServices;

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
        if (!IsSafeKey(key))
        {
            throw new ApplicationException("Provided object key is not considered safe");
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSafeKey(string key)
    {
        return key.Contains("..");
    }
}