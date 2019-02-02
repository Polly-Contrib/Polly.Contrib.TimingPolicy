using System;
using System.Diagnostics;
using System.Threading;

namespace Polly.Contrib.TimingPolicy
{
    internal static class TimingPolicyEngine
    {
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        internal static TimeSpan TimeSpanBetweenStopwatchTimestamps(long before, long after) => new TimeSpan((long)(TimestampToTicks * (after - before)));

        internal static TResult Implementation<TResult>(
            Func<Context, CancellationToken, TResult> action,
            Context context,
            CancellationToken cancellationToken,
            Action<TimeSpan, Context> timingPublisher
            )
        {
            long before = Stopwatch.GetTimestamp();
            try
            {
                return action(context, cancellationToken);
            }
            finally
            {
                TimeSpan elapsed = TimeSpanBetweenStopwatchTimestamps(before, Stopwatch.GetTimestamp());
                timingPublisher(elapsed, context);
            }
        }
    }
}
