using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Contrib.TimingPolicy 
{
    /// <summary>
    /// A policy to publish execution timing of delegates executed through the policy.
    /// </summary>
    /// <typeparam name="TResult">The type of return values this policy will handle.</typeparam>
    public class AsyncTimingPolicy<TResult> : AsyncPolicy<TResult>
    {
        private readonly Func<TimeSpan, Context, Task> timingPublisher;

        /// <summary>
        /// Creates a new instance of a <see cref="AsyncTimingPolicy{TResult}"/>
        /// </summary>
        /// <param name="timingPublisher">An asynchronous delegate to publish the execution timing information.</param>
        /// <returns>An instance of a <see cref="AsyncTimingPolicy{TResult}"/></returns>
        public static AsyncTimingPolicy<TResult> Create(Func<TimeSpan, Context, Task> timingPublisher)
        {
            return new AsyncTimingPolicy<TResult>(timingPublisher);
        }

        private AsyncTimingPolicy(Func<TimeSpan, Context, Task> timingPublisher)
            : base(ExceptionPredicates.None, ResultPredicates<TResult>.None)
        {
            this.timingPublisher = timingPublisher ?? throw new ArgumentNullException(nameof(timingPublisher));
        }

        /// <inheritdoc/>
        protected sealed override Task<TResult> ImplementationAsync(
            Func<Context, CancellationToken, Task<TResult>> action,
            Context context,
            CancellationToken cancellationToken,
            bool continueOnCapturedContext)
        {
           return AsyncTimingPolicyEngine.ImplementationAsync(action, context, cancellationToken, continueOnCapturedContext, timingPublisher);
        }
   
    }
}