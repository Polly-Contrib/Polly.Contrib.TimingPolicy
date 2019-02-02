using System;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace Polly.Contrib.TimingPolicy.Specs
{
    public class TimingPolicyTResultSpecs
    {
        [Fact]
        public void PolicyCapturesTimingOfExecutedDelegate()
        {
            TimeSpan elapsed = TimeSpan.MinValue;
            Action<TimeSpan, Context> capturedElapsed = (t, _) => elapsed = t;
            TimingPolicy<int> policy = TimingPolicy<int>.Create(capturedElapsed);

            TimeSpan forcedDelay = TimeSpan.FromSeconds(1);
            policy.Execute(() =>
            {
                Thread.Sleep(forcedDelay);
                return default(int);
            });

            elapsed.Should().BeCloseTo(forcedDelay);
        }
    }
}
