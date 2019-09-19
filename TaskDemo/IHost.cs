namespace TaskDemo
{
    public interface IHost
    {
        bool CanHandle(HostArguments args);

        void Handle(HostArguments args);

        string Description { get; }
    }
}