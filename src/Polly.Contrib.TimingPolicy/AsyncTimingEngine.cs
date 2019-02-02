using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Contrib.TimingPolicy 
{
    internal static class AsyncTimingPolicyEngine
    {
        internal static async Task<TResult> ImplementationAsync<TResult>(
            Func<Context, CancellationToken, Task<TResult>> action,
            Context context,
            CancellationToken cancellationToken,
            bool continueOnCapturedContext,
            Func<TimeSpan, Context, Task> timingPublisher
            )
        {
            long before = Stopwatch.GetTimestamp();
            try
            {
                return await action(context, cancellationToken).ConfigureAwait(continueOnCapturedContext);
            }
            finally
            {
                TimeSpan elapsed = TimingPolicyEngine.TimeSpanBetweenStopwatchTimestamps(before, Stopwatch.GetTimestamp());
                await timingPublisher(elapsed, context).ConfigureAwait(continueOnCapturedContext);
            }
        }
    }
}
