using Microsoft.Extensions.Hosting;
using Xunit;
using Boost.Proto.Actor.Hosting.Cluster;
using Boost.Proto.Actor.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Proto.Cluster;
using System.Threading;

namespace Sample.Tests;

public class PreludeSpec
{
    [Fact]
    public async Task AddSuccessAsync()
    {
        var host = Host.CreateDefaultBuilder()
                       .ConfigureServices(services =>
                       {

                       })
                       .UseProtoActorCluster((option, sp) =>
                       {
                           option.Name = "test";
                           option.ClusterKinds.Add(new(
                           "Poc",
                           sp.GetService<IPropsFactory<PocActor>>().Create()));
                       })
                       .Build();

        await host.StartAsync();

        var cluster = host.Services.GetService<Cluster>();

        using var cts = new CancellationTokenSource();

        var ret = await cluster.RequestAsync<string>("1", "Poc", "Hello", cts.Token);

        await host.StopAsync();
    }
}
