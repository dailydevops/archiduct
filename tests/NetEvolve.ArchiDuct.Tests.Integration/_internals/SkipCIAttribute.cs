namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

/// <summary>
/// <see cref="SkipCIAttribute"/> is used to skip tests in CI environments.
/// </summary>
[SuppressMessage(
    "Major Code Smell",
    "S3993:Custom attributes should be marked with \"System.AttributeUsageAttribute\"",
    Justification = "By design."
)]
public sealed class SkipCIAttribute : SkipAttribute
{
    public SkipCIAttribute()
        : base("Disabled in CI for now.") { }

    public override Task<bool> ShouldSkip(TestRegisteredContext context) =>
        Task.FromResult(
            Environment.GetEnvironmentVariable("CI") is string ci
                && ci.Equals("true", StringComparison.OrdinalIgnoreCase)
        );
}
