namespace NetEvolve.ArchiDuct.Models;

/// <summary>
/// Defines the described object type.
/// </summary>
public enum ModelKind
{
    // Modules

    /// <summary>
    /// Object that describes an assembly.
    /// </summary>
    Assembly,

    /// <summary>
    /// Object that describes a reference, typically to another assembly. Like a NuGet package or a project reference.
    /// </summary>
    Reference,

    /// <summary>
    /// Object that describes a namespace.
    /// </summary>
    Namespace,

    /// <summary>
    /// Object that describes an attribute, typically applied to a type or member.
    /// </summary>
    Attribute,

    // Types

    /// <summary>
    /// Object that describes a class.
    /// </summary>
    Class,

    /// <summary>
    /// Object that describes a struct.
    /// </summary>
    Struct,

    /// <summary>
    /// Object that describes an interface.
    /// </summary>
    Interface,

    /// <summary>
    /// Object that describes a delegate.
    /// </summary>
    Delegate,

    /// <summary>
    /// Object that describes an enum.
    /// </summary>
    Enum,

    // Members

    /// <summary>
    /// Object that describes an event.
    /// </summary>
    Event,

    /// <summary>
    /// Object that describes a constructor.
    /// </summary>
    Constructor,

    /// <summary>
    /// Object that describes a destructor.
    /// </summary>
    Destructor,

    /// <summary>
    /// Object that describes a deconstructor.
    /// </summary>
    Deconstructor,

    /// <summary>
    /// Object that describes a static constructor.
    /// </summary>
    StaticConstructor,

    /// <summary>
    /// Object that describes an enum.
    /// </summary>
    Method,

    /// <summary>
    /// Object that describes an explicit interface implementation of a method.
    /// </summary>
    ExplicitMethod,

    /// <summary>
    /// Object that describes an explicit interface implementation of an event.
    /// </summary>
    ExplicitEvent,

    /// <summary>
    /// Object that describes an explicit interface implementation of a property.
    /// </summary>
    ExplicitProperty,

    /// <summary>
    /// Object that describes an extension method.
    /// </summary>
    ExtensionMethod,

    /// <summary>
    /// Object that describes an enum member.
    /// </summary>
    EnumMember,

    /// <summary>
    /// Object that describes an indexer.
    /// </summary>
    Indexer,

    /// <summary>
    /// Object that describes an field.
    /// </summary>
    Field,

    /// <summary>
    /// Object that describes an operator.
    /// </summary>
    Operator,

    /// <summary>
    /// Object that describes a property.
    /// </summary>
    Property,

    // Decorators

    /// <summary>
    /// Object that describes a method parameter.
    /// </summary>
    Parameter,

    /// <summary>
    /// Object that describes a generic type parameter.
    /// </summary>
    TypeParameter,
}
