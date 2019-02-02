using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Polly.Contrib.TimingPolicy.Specs
{
    public class AsyncTimingPolicySpecs
    {
        [Fact]
        public async Task PolicyCapturesTimingOfExecutedDelegate()
        {
            TimeSpan elapsed = TimeSpan.MinValue;
            Func<TimeSpan, Context, Task> capturedElapsed = (t, _) =>
            {
                elapsed = t;
                return Task.CompletedTask;
            };
            AsyncTimingPolicy policy = AsyncTimingPolicy.Create(capturedElapsed);

            TimeSpan forcedDelay = TimeSpan.FromSeconds(1);
            await policy.ExecuteAsync(() => Task.Delay(forcedDelay));

            elapsed.Should().BeCloseTo(forcedDelay);
        }

    }
}
