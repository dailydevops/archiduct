namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using System.Runtime.InteropServices;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleExtern_Tests(ExternClassTypeProvider provider)
    : TestCaseBase<ExternClassTypeProvider>(provider) { }

public sealed class ExternClassTypeProvider() : TypeProviderBase(typeof(ExternTest)) { }

internal static class ExternTest
{
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int MessageBox(IntPtr h, string m, string c, int type);
#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
}
