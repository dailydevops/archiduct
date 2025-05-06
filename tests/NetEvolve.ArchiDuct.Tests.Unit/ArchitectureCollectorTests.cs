namespace NetEvolve.ArchiDuct.Tests.Unit;

using System;
using System.Diagnostics.CodeAnalysis;
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
    public void Collect_WhenNoSourcesRegistered_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(() => collector.Collect());
    }

    [Test]
    public async Task CollectAsync_WhenNoSourcesRegistered_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();

        // Act & Assert
        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () => await collector.CollectAsync());
    }

    [Test]
    public async Task CollectAsync_WhenCancellationIsRequested_ShouldThrowOperationCanceledException()
    {
        var collector = ArchitectureCollector.Create();
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();
        // Act & Assert
        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await collector.CollectAsync(cancellationTokenSource.Token)
        );
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
    public void AddAssemblyType_WhenAssemblyIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        Type type = null!;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => collector.AddAssembly(type));
    }

    [Test]
    public void AddAssemblyType_WhenAssemblyIsDuplicate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var collector = ArchitectureCollector.Create().AddAssembly(assembly);
        var type = typeof(ArchitectureCollectorTests);

        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(() => collector.AddAssembly(type));
    }

    [Test]
    public void AddAssemblyGeneric_WhenAssemblyIsDuplicate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var collector = ArchitectureCollector.Create().AddAssembly(assembly);

        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(() => collector.AddAssembly<ArchitectureCollectorTests>());
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
    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Required for testing.")]
    public void AddDirectory_WhenDirectoryDoesNotExist_ShouldThrowDirectoryNotFoundException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();

        // Act & Assert
        _ = Assert.Throws<DirectoryNotFoundException>(() => collector.AddDirectory(@"c:\NonExistingDirectory"));
    }

    [Test]
    public void AddDirectory_WhenDirectoryIsEmpty_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var collector = ArchitectureCollector.Create();
        var directoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _ = Directory.CreateDirectory(directoryPath);
        // Act & Assert
        _ = Assert.Throws<InvalidOperationException>(() => collector.AddDirectory(directoryPath));
        Directory.Delete(directoryPath);
    }

    [Test]
    public void AddDirectory_WheenDirectoryIsAlreadyRegistered_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var collector = ArchitectureCollector.Create().AddDirectory(directoryPath);
        // Act & Assert
        _ = Assert.Throws<AggregateException>(() => collector.AddDirectory(directoryPath));
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

    [Test]
    public async Task GetTypeFullName_WhenTypeIsNotNested_ShouldReturnValidValue()
    {
        // Arrange
        var type = typeof(GetTypeFullNameTestClass);

        // Act
        var result = ArchitectureCollector.GetTypeFullName(type);

        // Assert
        _ = await Assert.That(result).IsEqualTo($"NetEvolve.ArchiDuct.Tests.Unit.{nameof(GetTypeFullNameTestClass)}");
    }

    [Test]
    public async Task GetTypeFullName_WhenTypeIsNestedOneLevel_ShouldReturnValidValue()
    {
        // Arrange
        var type = typeof(GetTypeFullNameTestClass.NestedClass1);
        // Act
        var result = ArchitectureCollector.GetTypeFullName(type);
        // Assert
        _ = await Assert.That(result).IsEqualTo($"NetEvolve.ArchiDuct.Tests.Unit.{nameof(GetTypeFullNameTestClass)}");
    }

    [Test]
    public async Task GetTypeFullName_WhenTypeIsNestedTwoLevels_ShouldReturnValidValue()
    {
        // Arrange
        var type = typeof(GetTypeFullNameTestClass.NestedClass1.NestedClass2);
        // Act
        var result = ArchitectureCollector.GetTypeFullName(type);
        // Assert
        _ = await Assert.That(result).IsEqualTo($"NetEvolve.ArchiDuct.Tests.Unit.{nameof(GetTypeFullNameTestClass)}");
    }
}

#pragma warning disable CA1812 // CA1812: Avoid uninstantiated internal classes
internal sealed partial class GetTypeFullNameTestClass
{
    internal sealed partial class NestedClass1
    {
        internal sealed partial class NestedClass2;
    }
}
#pragma warning restore CA1812 // CA1812: Avoid uninstantiated internal classes
