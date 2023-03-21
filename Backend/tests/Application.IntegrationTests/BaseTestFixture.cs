using FluentAssertions;
using NUnit.Framework;

namespace Backend.Application.IntegrationTests;

using static Testing;

[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp()
    {
        RunAsDefaultUser();
        await ResetState();
    }
}
