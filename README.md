# Polly.Contrib.TimingPolicy

This repo contains a custom [Polly](https://github.com/App-vNext/Polly) policy to capture execution timings of delegates executed through the policy.

For more background on Polly see the [main Polly repo](https://github.com/App-vNext/Polly).

### Usage

#### Asynchronous executions

Define an `Func<TimeSpan, Context, Task> timingPublisher` to capture the execution duration as a `TimeSpan`.  The `Context` input parameter allows filtering based on Polly's in-built [execution metadata](https://github.com/App-vNext/Polly/wiki/Keys-And-Context-Data) or  context you pass in to the execution.


Configure the policy:

    var timingPolicy = AsyncTimingPolicy.Create(timingPublisher);

Execute through the policy:

    timingPolicy.ExecuteAsync(/* whatever */);

The policy can also be combined with other policies in a PolicyWrap - see below.

#### Synchronous executions

Define an `Action<TimeSpan, Context> timingPublisher` to capture the `TimeSpan` information.

Configure the policy:

    var timingPolicy = TimingPolicy.Create(timingPublisher);

Execute through the policy:

    timingPolicy.Execute(/* whatever */);

The policy can also be combined with other policies in a PolicyWrap.

### Using TimingPolicy in PolicyWrap

The `TimingPolicy` can be used in any position in a [`PolicyWrap`](https://github.com/App-vNext/Polly/wiki/PolicyWrap).  

+ A TimingPolicy used **outermost** (configured first) in a PolicyWrap will capture overall execution-time of the PolicyWrap, including all retries or waits-for-bulkhead-execution-slot that the polices in the PolicyWrap may introduce.
+ A TimingPolicy used **innermost** (configured last) in a PolicyWrap will capture execution-timing solely for the underlying executed delegate.

### `Action` versus event pattern versus `IObservable<>`

`Action` and `Func` were intentionally chosen to keep the code for the related blog post as simple as possible.  _You may wish to adapt the code to an event pattern or `IObservable<>` for production use_.

### Blog post example

The policy is an example for the blog [Custom policies Part II: Authoring a non-reactive custom policy](LINK WHEN PUBLISHED).

The policy in this repo differs in small ways from the blog post, as this repo offers TimingPolicy in all four combinations:

+ `TimingPolicy` (synchronous non-generic)
+ `TimingPolicy<TResult>` (synchronous generic)
+ `AsyncTimingPolicy` (asynchronous non-generic)
+ `AsyncTimingPolicy<TResult>` (asynchronous generic)

## Interested in developing your own custom policies?

See our blog series:

+ [Part I: Introducing custom Polly policies and the Polly.Contrib](http://www.thepollyproject.org/2019/02/13/introducing-custom-polly-policies-and-polly-contrib-custom-policies-part-i/)
+ [Part II: Authoring a non-reactive custom policy](http://www.thepollyproject.org/2019/02/13/authoring-a-proactive-polly-policy-custom-policies-part-ii/) (a policy which acts on all executions)
+ [Part III: Authoring a reactive custom policy](http://www.thepollyproject.org/2019/02/13/authoring-a-reactive-polly-policy-custom-policies-part-iii-2/) (a policy which react to faults)
+ [Part IV: Custom policies for all execution types](http://www.thepollyproject.org/2019/02/13/custom-policies-for-all-execution-types-custom-policies-part-iv/): sync and async, generic and non-generic

And see the templates for developing custom policies: [Polly.Contrib.CustomPolicyTemplates](https://github.com/Polly-Contrib/Polly.Contrib.CustomPolicyTemplates).
