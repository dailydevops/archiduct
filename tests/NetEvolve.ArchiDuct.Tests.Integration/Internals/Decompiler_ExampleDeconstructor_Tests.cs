namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleDeconstructor_Tests(DeconstructorTypeProvider provider)
    : TestCaseBase<DeconstructorTypeProvider>(provider) { }

public sealed class DeconstructorTypeProvider() : TypeProviderBase(typeof(DeconstructorExample)) { }

public class DeconstructorExample
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DeconstructorExample(string fname, string lname, string cityName, string stateName)
    {
        FirstName = fname;
        LastName = lname;
    }

    public void Deconstruct(out string fullName) =>
        fullName = string.Concat(FirstName, " ", LastName);

    public void Deconstruct(out string fname, out string lname)
    {
        fname = FirstName;
        lname = LastName;
    }
}
