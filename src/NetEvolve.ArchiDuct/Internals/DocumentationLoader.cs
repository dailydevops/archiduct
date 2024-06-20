namespace NetEvolve.ArchiDuct.Internals;

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.Metadata;

internal static class DocumentationLoader
{
    private static readonly Lazy<XmlDocumentationProvider?> _mscorlibDocumentation =
        new Lazy<XmlDocumentationProvider?>(LoadMscorlibDocumentation);
    private static readonly ConcurrentDictionary<PEFile, XmlDocumentationProvider?> _cache = new();

    private static XmlDocumentationProvider? LoadMscorlibDocumentation()
    {
        var xmlDocFile =
            FindXmlDocumentation("mscorlib.dll", TargetRuntime.Net_4_0)
            ?? FindXmlDocumentation("mscorlib.dll", TargetRuntime.Net_2_0)
            ?? FindXmlDocumentation("mscorlib.dll", TargetRuntime.Unknown);

        if (xmlDocFile != null)
        {
            return new XmlDocumentationProvider(xmlDocFile);
        }

        return null;
    }

    public static XmlDocumentationProvider? MscorlibDocumentation => _mscorlibDocumentation.Value;

    public static XmlDocumentationProvider? LoadDocumentation(PEFile module)
    {
        ArgumentNullException.ThrowIfNull(module);

        return _cache.GetOrAdd(
            module,
            m =>
            {
                var documentation =
                    LookupLocalizedXmlDoc(m.FileName)
                    ?? FindXmlDocumentation(Path.GetFileName(m.FileName), m.GetRuntime());

                return documentation != null ? new XmlDocumentationProvider(documentation) : null;
            }
        );
    }

    private static readonly string _referencePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
        @"Reference Assemblies\Microsoft\\Framework"
    );
    private static readonly string _frameworkPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Windows),
        @"Microsoft.NET\Framework"
    );

#pragma warning disable CA1802 // Use literals where appropriate
    private static readonly string _unixPath = "/usr/share/dotnet/shared/";
    private static readonly string _mono45Path = "/usr/local/lib/mono/4.5/";
    private static readonly string _mono40Path = "/usr/local/lib/mono/4.0/";
    private static readonly string _mono35Path = "/usr/local/lib/mono/3.5/";
    private static readonly string _mono20Path = "/usr/local/lib/mono/2.0/";
#pragma warning restore CA1802 // Use literals where appropriate


    private static string? FindXmlDocumentation(string assemblyFileName, TargetRuntime runtime) =>
        runtime switch
        {
            TargetRuntime.Net_1_0
                => LookupLocalizedXmlDoc(_frameworkPath, "v1.0.3705", assemblyFileName),
            TargetRuntime.Net_1_1
                => LookupLocalizedXmlDoc(_frameworkPath, "v1.1.4322", assemblyFileName),
            TargetRuntime.Net_2_0
                => LookupLocalizedXmlDoc(_frameworkPath, "v2.0.50727", assemblyFileName)
                    ?? LookupLocalizedXmlDoc(_referencePath, "v3.5", assemblyFileName)
                    ?? LookupLocalizedXmlDoc(_referencePath, "v3.0", assemblyFileName)
                    ?? LookupLocalizedXmlDoc(
                        _referencePath,
                        @".NETFramework\v3.5\Profile\Client",
                        assemblyFileName
                    ),
            _
                => LookupLocalizedXmlDoc(_referencePath, @".NETFramework\v4.0", assemblyFileName)
                    ?? LookupLocalizedXmlDoc(_frameworkPath, "v4.0.30319", assemblyFileName)
                    ?? LookupLocalizedXmlDoc(_unixPath, assemblyFileName)
                    ?? LookupLocalizedXmlDoc(_mono45Path, assemblyFileName)
                    ?? LookupLocalizedXmlDoc(_mono40Path, assemblyFileName)
                    ?? LookupLocalizedXmlDoc(_mono35Path, assemblyFileName)
                    ?? LookupLocalizedXmlDoc(_mono20Path, assemblyFileName),
        };

    internal static string? LookupLocalizedXmlDoc(params string[] pathSegments) =>
        LookupLocalizedXmlDoc(Path.Combine(pathSegments));

    [SuppressMessage("ApiDesign", "RS0030:Do not use banned APIs", Justification = "<Pending>")]
    internal static string? LookupLocalizedXmlDoc(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return null;
        }

        var directory = Path.GetDirectoryName(fileName);
        if (directory is null || !Directory.Exists(directory))
        {
            Console.WriteLine($"Directory {directory} does not exist.");
            return null;
        }

        var xmlFileName = Path.ChangeExtension(fileName, ".xml");

        if (File.Exists(xmlFileName))
        {
            return xmlFileName;
        }

        var currentCulture = Thread.CurrentThread.CurrentUICulture;
        var localizedXmlDocFile = GetLocalizedName(xmlFileName, currentCulture.Name);

        if (File.Exists(localizedXmlDocFile))
        {
            return localizedXmlDocFile;
        }

        localizedXmlDocFile = GetLocalizedName(
            xmlFileName,
            currentCulture.TwoLetterISOLanguageName
        );

        if (File.Exists(localizedXmlDocFile))
        {
            return localizedXmlDocFile;
        }

        if (currentCulture.TwoLetterISOLanguageName != "en")
        {
            localizedXmlDocFile = GetLocalizedName(xmlFileName, "en");
            if (File.Exists(localizedXmlDocFile))
            {
                return localizedXmlDocFile;
            }
        }

        Console.WriteLine($"Could not find XML documentation file for {fileName}.");

        return null;
    }

    private static string GetLocalizedName(string fileName, string language) =>
        Path.Combine(Path.GetDirectoryName(fileName)!, language, Path.GetFileName(fileName));
}
