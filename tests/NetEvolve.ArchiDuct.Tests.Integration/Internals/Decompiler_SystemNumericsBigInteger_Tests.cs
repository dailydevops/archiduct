namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System.Numerics;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<BigInteger>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemNumericsBigInteger_Tests(GenericTypeProvider<BigInteger> provider)
    : TypesTestCaseGenericBase<BigInteger>(provider) { }
