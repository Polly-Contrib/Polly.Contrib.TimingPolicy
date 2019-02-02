namespace Polly.Contrib.TimingPolicy 
{
    /// <summary>
    /// Defines properties common to synchronous and asynchronous TimingPolicy policies.
    /// </summary>
    public interface ITimingPolicy : IsPolicy
    {
    }

    /// <summary>
    /// Defines properties common to generic, synchronous and asynchronous TimingPolicy policies.
    /// </summary>
    public interface ITimingPolicy<TResult> : ITimingPolicy
    {
    }
}
