using System;
using System.Threading;

namespace Polly.Contrib.TimingPolicy
{
    /// <summary>
    /// A policy to publish execution timing of delegates executed through the policy.
    /// </summary>
    /// <typeparam name="TResult">The type of return values this policy will handle.</typeparam>
    public class TimingPolicy<TResult> : Policy<TResult>
    {
        private readonly Action<TimeSpan, Context> timingPublisher;

        /// <summary>
        /// Creates a new instance of a <see cref="TimingPolicy{TResult}"/>
        /// </summary>
        /// <param name="timingPublisher">An asynchronous delegate to publish the execution timing information.</param>
        /// <returns>An instance of a <see cref="TimingPolicy{TResult}"/></returns>
        public static TimingPolicy<TResult> Create(Action<TimeSpan, Context> timingPublisher)
        {
            return new TimingPolicy<TResult>(timingPublisher);
        }

        private TimingPolicy(Action<TimeSpan, Context> timingPublisher)
            : base(ExceptionPredicates.None, ResultPredicates<TResult>.None)
        {
            this.timingPublisher = timingPublisher ?? throw new ArgumentNullException(nameof(timingPublisher));
        }

        /// <inheritdoc/>
        protected sealed override TResult Implementation(
            Func<Context, CancellationToken, TResult> action,
            Context context,
            CancellationToken cancellationToken)
        {
            return TimingPolicyEngine.Implementation(action, context, cancellationToken, timingPublisher);
        }
    }
}