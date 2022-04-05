using Proto;
using Proto.Mailbox;
using static Sample.PocState;

namespace Sample;

public enum PocState
{
    Send,
    Sending,
    Sent
}

public record PocActor : IActor
{
    public PocActor() => Behavior = new(new Dictionary<PocState, Receive>
    {
        [Send] = async ctx =>
        {
            ctx.Forward(ctx.Self);
            Behavior.Become(Sending);
        },
        [Sending] = async ctx =>
        {
            ctx.Forward(ctx.Self);
            Behavior.Become(Sent);
        },
        [Sent] = async ctx =>
        {
            ctx.Respond("Sent");
        },
    });

    public Task ReceiveAsync(IContext context)
    {
        switch (context.Message)
        {
            case SystemMessage:
                return Task.CompletedTask;
        }

        return Behavior.ReceiveAsync(context);
    }

    private BehaviorT<PocState> Behavior { get; }
}
