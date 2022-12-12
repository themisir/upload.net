namespace Upload.Core.Extra;

public static class KeyUtils
{
    public static void MushBeSafeKey(string key)
    {
        if (key.Contains(".."))
        {
            throw new ApplicationException("Key should not contain '..'");
        }
    }

    public static string GetUniqueKey(string name)
    {
        return (Guid.NewGuid() + Path.GetExtension(name)).ToLowerInvariant();
    }
}