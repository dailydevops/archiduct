﻿namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<NullableProvider>(Shared = SharedType.PerClass)]
public class Decompiler_SystemNullable_Tests(NullableProvider provider) : TestCaseBase<NullableProvider>(provider) { }

public sealed class NullableProvider() : TypeProviderBase(typeof(Nullable)) { }
