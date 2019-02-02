using System;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace Polly.Contrib.TimingPolicy.Specs
{
    public class TimingPolicySpecs
    {
        [Fact]
        public void PolicyCapturesTimingOfExecutedDelegate()
        {
            TimeSpan elapsed = TimeSpan.MinValue;
            Action<TimeSpan, Context> capturedElapsed = (t, _) => elapsed = t;
            TimingPolicy policy = TimingPolicy.Create(capturedElapsed);

            TimeSpan forcedDelay = TimeSpan.FromSeconds(1);
            policy.Execute(() => Thread.Sleep(forcedDelay));

            elapsed.Should().BeCloseTo(forcedDelay);
        }

    }
}
