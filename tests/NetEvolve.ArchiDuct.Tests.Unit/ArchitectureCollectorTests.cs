namespace NetEvolve.ArchiDuct.Tests.Unit;

using System;
using System.IO;
using System.Reflection;
using NetEvolve.FluentValue;
using Xunit;

public class ArchitectureCollectorTests
{
    [Fact]
    public void Create_WhenCalled_ShouldReturnNewInstance()
    {
        // Arrange
        var result = ArchitectureCollector.Create();

        // Act

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void AddAssembly_WhenAssemblyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        Assembly assembly = null!;

        // Act

        // Assert
        _ = Assert.Throws<ArgumentNullException>(() => collector.AddAssembly(assembly));
    }

    [Fact]
    public void AddAssembly_WhenAssemblyIsDuplicate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var collector = ArchitectureCollector.Create().AddAssembly(assembly);

        // Act

        // Assert
        _ = Assert.Throws<InvalidOperationException>(() => collector.AddAssembly(assembly));
    }

    [Fact]
    public void AddDirectory_WhenDirectoryNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();

        // Act

        // Assert
        _ = Assert.Throws<ArgumentNullException>(() => collector.AddDirectory(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("\t")]
    public void AddDirectory_WhenDirectoryPathIsEmptyOrWhiteSpace_ShouldThrowArgumentException(
        string? directoryPath
    )
    {
        // Arrange
        var collector = ArchitectureCollector.Create();

        // Act

        // Assert
        _ = Assert.Throws<ArgumentException>(() => collector.AddDirectory(directoryPath));
    }

    [Fact]
    public void AddDirectory_WhenDirectoryDoesNotExist_ShouldThrowDirectoryNotFoundException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();

        // Act

        // Assert
        _ = Assert.Throws<DirectoryNotFoundException>(
            () => collector.AddDirectory(@"c:\NonExistingDirectory")
        );
    }

    [Fact]
    public void FilterNamespace_WhenConstraintIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        IConstraint constraint = null!;

        // Act

        // Assert
        _ = Assert.Throws<ArgumentNullException>(() => collector.FilterNamespace(constraint));
    }

    [Fact]
    public void FilterNamespace_WhenNoAssemblies_ShouldThrowArgumentException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        var constraint = Value.Null;

        // Act

        // Assert
        _ = Assert.Throws<ArgumentException>(() => collector.FilterNamespace(constraint));
    }

    [Fact]
    public void FilterNamespace_WhenConstraintIsAlreadyRegistered_ShouldThrowArgumentException()
    {
        // Arrange
        var constraint = Value.Null;
        var collector = ArchitectureCollector
            .Create()
            .AddAssembly(Assembly.GetExecutingAssembly())
            .FilterNamespace(constraint);

        // Assert
        _ = Assert.Throws<ArgumentException>(() => collector.FilterNamespace(constraint));
    }
}
