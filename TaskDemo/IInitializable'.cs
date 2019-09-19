namespace TaskDemo
{
    public interface IInitializable<in T>
    {
        void Initialize(T context);
    }
}
