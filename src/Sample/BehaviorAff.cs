using Proto;

namespace Sample;

public record BehaviorT<T>(IDictionary<T, Receive> Behaviors)
    where T : struct
{
    public T Current { get; private set; } = default;
    public Task ReceiveAsync(IContext context)
    {
        var behavior = Behaviors[Current];
        return behavior(context);
    }

    public void Become(T next)
    {
        Current = next;
    }
}
