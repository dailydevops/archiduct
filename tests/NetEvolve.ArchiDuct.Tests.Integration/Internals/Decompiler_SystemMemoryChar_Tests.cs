﻿namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.Memory<char>>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemMemoryChar_Tests(GenericTypeProvider<System.Memory<char>> provider)
    : TypesTestCaseGenericBase<System.Memory<char>>(provider) { }
