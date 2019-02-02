using System;
using System.Threading;

namespace Polly.Contrib.TimingPolicy
{
    /// <summary>
    /// A policy to publish execution timing of delegates executed through the policy.
    /// </summary>
    public class TimingPolicy : Policy
    {
        private readonly Action<TimeSpan, Context> timingPublisher;

        /// <summary>
        /// Creates a new instance of a <see cref="TimingPolicy"/>
        /// </summary>
        /// <param name="timingPublisher">An asynchronous delegate to publish the execution timing information.</param>
        /// <returns>An instance of a <see cref="TimingPolicy"/></returns>
        public static TimingPolicy Create(Action<TimeSpan, Context> timingPublisher)
        {
            return new TimingPolicy(timingPublisher);
        }

        private TimingPolicy(Action<TimeSpan, Context> timingPublisher)
            : base(ExceptionPredicates.None)
        {
            this.timingPublisher = timingPublisher ?? throw new ArgumentNullException(nameof(timingPublisher));
        }

        /// <inheritdoc/>
        protected sealed override TResult Implementation<TResult>(
            Func<Context, CancellationToken, TResult> action,
            Context context,
            CancellationToken cancellationToken)
        {
            return TimingPolicyEngine.Implementation(action, context, cancellationToken, timingPublisher);
        }
    }
}