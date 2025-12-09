namespace NetEvolve.ArchiDuct.Framework.Commands;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Testing.Platform.CommandLine;
using Microsoft.Testing.Platform.Extensions;
using Microsoft.Testing.Platform.Extensions.CommandLine;
using NetEvolve.ArchiDuct;

internal sealed class DisableLogoCommandProvider : ICommandLineOptionsProvider
{
    public const string DisableLogo = "disable-logo";

    public string Uid => ArchiDuctFramework.Uid;

    public string Version => ArchiDuctFramework.Version;

    public string DisplayName => ArchiDuctFramework.DisplayName;

    public string Description => ArchiDuctFramework.Description;

    public IReadOnlyCollection<CommandLineOption> GetCommandLineOptions() =>
        [
            new CommandLineOption(
                DisableLogo,
                "Disables the ArchiDuct logo when starting a test session",
                ArgumentArity.Zero,
                false
            ),
        ];

    public Task<bool> IsEnabledAsync() => Task.FromResult(true);

    public Task<ValidationResult> ValidateCommandLineOptionsAsync(ICommandLineOptions commandLineOptions) =>
        Task.FromResult(ValidationResult.Valid());

    public Task<ValidationResult> ValidateOptionArgumentsAsync(CommandLineOption commandOption, string[] arguments) =>
        Task.FromResult(ValidationResult.Valid());
}
