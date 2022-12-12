namespace Upload.Core.Service;

public sealed class StorageManagerOptions
{
    public Dictionary<string, Func<IServiceProvider, IStorageProvider>> ProviderFactories { get; } = new();

    public void AddProvider(string name, Func<IServiceProvider, IStorageProvider> factory)
    {
        if (!ProviderFactories.TryAdd(name, factory))
        {
            throw new ApplicationException($"A storage backend with name '{name}' is already configured");
        }
    }
}