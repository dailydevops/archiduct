# NetEvolve.ArchiDuct

<img src="logo.jpg" alt="NetEvolve.ArchiDuct" title="In the same way that aqueducts elegantly and reliably directed the flow of water across the Roman Empire, *ArchiDuct* ensures the flow of clean, well-structured code in your .NET projects." width="250" align="left" />

***ArchiDuct*** (short version for `NetEvolve.ArchiDuct`) is a powerful .NET library designed to help developers enforce architectural rules and maintain a clean, structured codebase. By integrating seamlessly with popular testing frameworks such as **xUnit**, **NUnit** *(planned)*, and **MSTest** *(planned)*, ***ArchiDuct*** ensures that your project architecture stays in line with defined rules throughout its lifecycle.

Inspired by tools like [ArchUnitNET](https://github.com/TNG/ArchUnitNET) and [NetArchTest](https://github.com/BenMorris/NetArchTest), ***ArchiDuct*** takes the concept further by focusing on both flexibility and performance.

<br />
<br />

> **In the same way that aqueducts elegantly and reliably directed the flow of water across the Roman Empire, *ArchiDuct* ensures the flow of clean, well-structured code in your .NET projects.**

## Table of Contents

- [Introduction](#introduction)
- [Why ArchiDuct?](#why-archiduct)
- [Features](#features)
- [Installation](#installation)
- [Getting Started](#getting-started)
- [Examples](#examples)
- [Compatibility](#compatibility)
- [Performance](#performance)
- [Contributing](#contributing)
- [License](#license)

## Introduction

In the fast-paced world of modern software development, maintaining a well-defined and robust architecture is not just a best practice—it's a cornerstone of sustainable success. Whether you're building a sprawling enterprise application or crafting a small, nimble open-source project, a clean, well-organized codebase is essential to prevent technical debt, reduce complexity, and ensure future scalability. A strong architectural foundation minimizes the risk of bugs, accelerates development speed, and enhances the overall maintainability of the project.

**ArchiDuct** is a powerful tool designed to enforce architectural rules within .NET projects, acting as a safeguard against common design pitfalls such as dependency cycles, improper layering, or the violation of established architectural patterns. By seamlessly integrating these checks into your test suite, ***ArchiDuct*** provides a continuous monitoring mechanism that keeps your architecture aligned with best practices. As your project grows and evolves, ***ArchiDuct*** ensures that your architectural integrity remains intact, reducing the risk of costly refactors and enabling your team to focus on delivering value to your users.

## Why *ArchiDuct*? - Inspired by the Best, Designed for the Future

***ArchiDuct*** has been influenced by some of the pioneering tools in this field, such as [ArchUnitNET](https://github.com/TNG/ArchUnitNET) and [NetArchTest](https://github.com/BenMorris/NetArchTest). Both of these tools have set a high standard for architectural testing in the .NET ecosystem, and we have been inspired by their approach. However, ***ArchiDuct*** has undergone significant enhancements, particularly in terms of performance and feature set expansion, which has made it a more versatile and efficient solution for developers.

*Much like the aqueducts of ancient times — remarkable feats of engineering that provided a consistent and reliable flow of water across vast distances*.

***ArchiDuct*** aims to serve as the modern developer's indispensable tool for managing and enforcing project architecture. It combines the foundational principles established by its predecessors with innovative improvements, ensuring that your software projects are built on a solid architectural foundation, designed to stand the test of time. With ***ArchiDuct***, you gain not just a tool, but a partner in maintaining the architectural integrity of your codebase as it evolves and scales.

### Benefits of Using *ArchiDuct*

- **Flexibility:** Compatible with xUnit, NUnit *(planned)*, and MSTest *(planned)*, giving you the freedom to integrate with your preferred testing framework.
- **Performance:** Optimized for speed, ensuring that architectural tests do not become a bottleneck in your CI/CD pipeline.
- **Ease of Use:** Simple API designed for ease of use, even for developers who are new to architectural testing.
- **Maintainability:** Helps enforce best practices, making your codebase more maintainable and easier to navigate.
- **Extensibility:** Create custom rules to match your specific architectural requirements.
- **Controllability:** Define rules that align with your project's architecture and enforce them consistently.

## Features

### 1. **Rule-based Architecture Validation**
***ArchiDuct*** allows you to define rules that describe the desired architecture of your project. These rules can be applied to various aspects of your code, such as:

- Dependency management
- Layered architecture compliance
- Naming conventions
- Modular boundaries
- Inheritance hierarchies
- And man

### 2. **Integration with Popular Testing Frameworks**
***ArchiDuct*** is designed to integrate seamlessly with xUnit, NUnit *(planned)*, and MSTest *(planned)*. This means you can run your architectural tests alongside your unit and integration tests without any additional setup.

### 3. **Custom Rule Creation**
While ***ArchiDuct*** comes with a set of predefined rules, it also allows you to create custom rules that fit your specific needs. This feature makes ***ArchiDuct*** extremely versatile and adaptable to any project.

### 4. **Performance Optimization**
Architectural testing should not slow down your development process. ***ArchiDuct*** is optimized for performance, ensuring that your architectural tests run quickly even as your codebase grows.

### 5. **Detailed Reporting**
***ArchiDuct*** provides detailed reports of architectural violations, making it easy to identify and resolve issues. The reports are designed to be developer-friendly, highlighting the exact location and nature of the violation.

## Installation

### NuGet Package Manager

You can install ***ArchiDuct*** using the NuGet Package Manager in Visual Studio:

```bash
Install-Package NetEvolve.ArchiDuct
```

### .NET CLI

Alternatively, you can use the .NET CLI:

```bash
dotnet add package NetEvolve.ArchiDuct
```

### PackageReference

For projects that support `PackageReference`, add the following line to your project file:

```xml
<!-- Replace x.x.x with the latest version of NetEvolve.ArchiDuct -->
<PackageReference Include="NetEvolve.ArchiDuct" Version="x.x.x" />
```

## Getting Started

### 1. **Set Up Your Project**

After installing ***ArchiDuct***, you'll need to set up your project to start using it. Begin by adding the necessary using directives to your test files:

```csharp
using NetEvolve.ArchiDuct;
using NetEvolve.ArchiDuct.Rules;
```

### 2. **Define Your First Rule**

Create a simple rule to ensure that classes in a specific namespace should only depend on other classes within the same namespace:

```csharp
// TODO: Create a test example using xUnit
```

### 3. **Run Your Tests**

Run your tests as you normally would using your preferred testing framework. If any architectural rules are violated, ***ArchiDuct*** will provide detailed feedback on what went wrong.

## Examples

### Ensuring Layered Architecture

This example enforces that the domain layer should not have any dependencies on the application layer:

```csharp
// TODO: Create a test example using NUnit
```

### Enforcing Naming Conventions

You can also use ***ArchiDuct*** to enforce naming conventions within your project:

```csharp
// TODO: Create a test example using MSTest
```

## Compatibility

***ArchiDuct*** is compatible with the following testing frameworks:

- **xUnit**
- **NUnit** *(planned)*
- **MSTest** *(planned)*

This ensures that no matter what framework your project is built on, ***ArchiDuct*** can be integrated seamlessly.

## Performance

One of the key differentiators of ***ArchiDuct*** is its focus on performance. Architectural tests, by their nature, can be resource-intensive, especially in large codebases. ***ArchiDuct*** is optimized to run these tests quickly, allowing you to integrate architectural testing into your CI/CD pipeline without significant slowdowns.

## Contributing

Contributions are welcome! If you have an idea for a new feature or have found a bug, feel free to open an issue or submit a pull request. We encourage the community to help improve ***ArchiDuct*** and make it the best tool it can be.

### How to Contribute

1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Implement your changes and add tests where applicable.
4. Submit a pull request with a detailed description of your changes.

## License

***ArchiDuct*** is licensed under the [MIT License](LICENSE). This means you are free to use, modify, and distribute the library in your own projects.
