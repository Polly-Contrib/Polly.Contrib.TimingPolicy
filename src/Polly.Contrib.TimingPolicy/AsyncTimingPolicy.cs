using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Contrib.TimingPolicy 
{
    /// <summary>
    /// A policy to publish execution timing of delegates executed through the policy.
    /// </summary>
    public class AsyncTimingPolicy : AsyncPolicy
    {
        private readonly Func<TimeSpan, Context, Task> timingPublisher;

        /// <summary>
        /// Creates a new instance of a <see cref="AsyncTimingPolicy"/>
        /// </summary>
        /// <param name="timingPublisher">An asynchronous delegate to publish the execution timing information.</param>
        /// <returns>An instance of a <see cref="AsyncTimingPolicy"/></returns>
        public static AsyncTimingPolicy Create(Func<TimeSpan, Context, Task> timingPublisher)
        {
            return new AsyncTimingPolicy(timingPublisher);
        }

        private AsyncTimingPolicy(Func<TimeSpan, Context, Task> timingPublisher)
            : base(ExceptionPredicates.None)
        {
            this.timingPublisher = timingPublisher ?? throw new ArgumentNullException(nameof(timingPublisher));
        }

        /// <inheritdoc/>
        protected sealed override Task<TResult> ImplementationAsync<TResult>(
            Func<Context, CancellationToken, Task<TResult>> action,
            Context context,
            CancellationToken cancellationToken,
            bool continueOnCapturedContext)
        {
            return AsyncTimingPolicyEngine.ImplementationAsync(action, context, cancellationToken, continueOnCapturedContext, timingPublisher);
        }
    }
}