namespace Upload.Core.Service;

public sealed class StorageManagerOptions
{
    public Dictionary<string, IStorageProvider> Providers { get; } = new();

    public void AddProvider(string name, IStorageProvider provider)
    {
        if (!Providers.TryAdd(name, provider))
        {
            throw new ApplicationException($"A storage backend with name '{name}' is already configured");
        }
    }
}