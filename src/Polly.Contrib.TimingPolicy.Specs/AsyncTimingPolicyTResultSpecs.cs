using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Polly.Contrib.TimingPolicy.Specs
{
    public class AsyncTimingPolicyTResultSpecs
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
            AsyncTimingPolicy<int> policy = AsyncTimingPolicy<int>.Create(capturedElapsed);

            TimeSpan forcedDelay = TimeSpan.FromSeconds(1);
            await policy.ExecuteAsync(async () =>
            {
                await Task.Delay(forcedDelay);
                return default(int);
            });

            elapsed.Should().BeCloseTo(forcedDelay);
        }

    }
}
