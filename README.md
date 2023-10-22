<h1 align="center">
  <img src="genet_icon.png" alt="GeNet Logo" width="128" height="128"><br>
  GeNet - Source Generators for .NET
</h1>


![Version](https://img.shields.io/badge/version-0.0.1-blue.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)
![Build Status](https://github.com/GregDomzalski/GeNet/workflows/build/badge.svg)
![GitHub Stars](https://img.shields.io/github/stars/GregDomzalski/GeNet.svg)

GeNet (Source **Gen**erators for .**NET**) is a collection of source generators that simplify common development tasks in .NET applications. These generators help automate code generation, improve performance, and enhance developer productivity.

## Generators

| Generator                                                   | Description                           |
|-------------------------------------------------------------|---------------------------------------|
| [Disposable](./GeNet.Disposable/GeNet.Disposable.readme.md) | Automatically generate disposable pattern implementations for your classes. This helps ensure that resources are properly managed when your objects are no longer needed. |
| [Equals](./GeNet.Equals/GeNet.Equals.readme.md)             | Generate efficient and correct `Equals`, `GetHashCode`, and `IEquatable` implementations for your types based on their properties and fields. |

## Getting Started

To use GeNet in your .NET project, follow these steps:

1. Clone the GeNet repository or install it via NuGet.
2. Reference the specific generator(s) you need in your project.
3. Configure the generator(s) according to your project requirements.
4. Build and run your project to generate code automatically.

For detailed instructions on how to use each generator, please refer to their respective README files linked above.

## Contribution

We welcome contributions from the community. If you have an idea for a new generator or want to improve existing ones, please open an issue or submit a pull request. We appreciate your help in making GeNet better!

## License

GeNet is released under the [MIT License](LICENSE.md).

---

Feel free to reach out if you have any questions or need further assistance with GeNet.
