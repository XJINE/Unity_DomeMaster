public interface IInitializable
{
    bool IsInitialized { get; }

    bool Initialize();
}