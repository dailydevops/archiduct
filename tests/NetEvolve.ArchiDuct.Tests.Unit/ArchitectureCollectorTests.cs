namespace NetEvolve.ArchiDuct.Tests.Unit;

using System;
using System.IO;
using System.Reflection;
using NetEvolve.FluentValue;

public class ArchitectureCollectorTests
{
    [Test]
    public async Task Create_WhenCalled_ShouldReturnNewInstance()
    {
        // Arrange
        var result = ArchitectureCollector.Create();

        // Act

        // Assert
        _ = await Assert.That(result).IsNotNull();
    }

    [Test]
    public void AddAssembly_WhenAssemblyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        Assembly assembly = null!;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => collector.AddAssembly(assembly));
    }

    [Test]
    public void AddAssembly_WhenAssemblyIsDuplicate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var collector = ArchitectureCollector.Create().AddAssembly(assembly);

        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(() => collector.AddAssembly(assembly));
    }

    [Test]
    public void AddDirectory_WhenDirectoryNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => collector.AddDirectory(null!));
    }

    [Test]
    public void AddDirectory_WhenDirectoryPathIsEmptyOrWhiteSpace_ShouldThrowArgumentException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();

        // Act & Assert
        using (Assert.Multiple())
        {
            _ = Assert.Throws<ArgumentException>(() => collector.AddDirectory(""));
            _ = Assert.Throws<ArgumentException>(() => collector.AddDirectory("\t"));
        }
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Minor Code Smell",
        "S1075:URIs should not be hardcoded",
        Justification = "Required for testing."
    )]
    public void AddDirectory_WhenDirectoryDoesNotExist_ShouldThrowDirectoryNotFoundException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();

        // Act & Assert
        _ = Assert.Throws<DirectoryNotFoundException>(() => collector.AddDirectory(@"c:\NonExistingDirectory"));
    }

    [Test]
    public void FilterNamespace_WhenConstraintIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        IConstraint constraint = null!;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => collector.FilterNamespace(constraint));
    }

    [Test]
    public void FilterNamespace_WhenNoAssemblies_ShouldThrowArgumentException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        var constraint = Value.Null;

        // Act & Assert
        _ = Assert.Throws<ArgumentException>(() => collector.FilterNamespace(constraint));
    }

    [Test]
    public void FilterNamespace_WhenConstraintIsAlreadyRegistered_ShouldThrowArgumentException()
    {
        // Arrange
        var constraint = Value.Null;
        var collector = ArchitectureCollector
            .Create()
            .AddAssembly(Assembly.GetExecutingAssembly())
            .FilterNamespace(constraint);

        // Act & Assert
        _ = Assert.Throws<ArgumentException>(() => collector.FilterNamespace(constraint));
    }
}
