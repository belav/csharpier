using CSharpier.Core;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
public class SharedFuncTests
{
    [Test]
    public static async Task TwoAwaitsOneRunAsync()
    {
        var result = 0;
        await Task.WhenAll(
            Task.Run(async () =>
            {
                result = await SharedFunc<int>.GetOrAddAsync(
                    "1",
                    async () =>
                    {
                        await Task.Delay(1000);
                        return 1;
                    },
                    CancellationToken.None
                );
            }),
            Task.Run(async () =>
            {
                await Task.Delay(100);
                result = await SharedFunc<int>.GetOrAddAsync(
                    "1",
                    async () =>
                    {
                        await Task.Delay(1000);
                        return 2;
                    },
                    CancellationToken.None
                );
            })
        );

        result.Should().Be(1);
    }
}
