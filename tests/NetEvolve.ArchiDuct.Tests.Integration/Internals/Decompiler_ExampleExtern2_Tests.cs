namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using System.Runtime.InteropServices;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<Extern2ClassTypeProvider>(Shared = SharedType.PerClass)]
public class Decompiler_ExampleExtern2_Tests(Extern2ClassTypeProvider provider)
    : TestCaseBase<Extern2ClassTypeProvider>(provider) { }

public sealed class Extern2ClassTypeProvider() : TypeProviderBase(typeof(ExternTest2)) { }

internal static partial class ExternTest2
{
    [LibraryImport("User32.dll", StringMarshalling = StringMarshalling.Utf16)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static partial int MessageBox(IntPtr h, string m, string c, int type);
}
