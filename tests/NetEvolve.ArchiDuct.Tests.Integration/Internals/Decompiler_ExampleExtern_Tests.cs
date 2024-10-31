namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using System.Runtime.InteropServices;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleExtern_Tests(ExternClassTypeProvider provider)
    : TestCaseBase<ExternClassTypeProvider>(provider) { }

public sealed class ExternClassTypeProvider() : TypeProviderBase(typeof(ExternTest)) { }

internal static class ExternTest
{
    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int MessageBox(IntPtr h, string m, string c, int type);
}
