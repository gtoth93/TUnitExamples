# TUnitExamples

A set of examples showcasing TUnit.

## What is TUnit

[TUnit](https://tunit.dev/) is a new testing framework for .NET, built on top of Microsoft Testing
Platform (MTP). It focuses on speed through parallelization and code generation, and an ergonomic
API, taking inspiration from NUnit and xUnit. It generally tries to be batteries-included.

## Adding a TUnit test project

There are two ways to install TUnit.you either install the templates with

- With templates
  - Install templates: `dotnet new install TUnit.Templates`
  - Create project: `dotnet new tunit --name <ProjectName>` or create the project with IDE
- Manually
  - Create console app project: `dotnet new console` or create the project with IDE
  - Add TUnit NuGet package: `dotnet add package TUnit` or `Install-Package TUnit -ProjectName` or
    add the package through the IDE
  - Delete Program.cs

Some global usings are automatically added to the project, like test attributes and assertions. It
also includes two extensions to MTP: Microsoft.Testing.Extensions.CodeCoverage and
Microsoft.Testing.Extensions.TrxReport. Do not add Microsoft.NET.Test.Sdk to the project, this
package is not compatible with MTP, only VSTest. Do not add coverlet to TUnit projects, they are not
compatible.

## Running tests

- For a single project:
  - `dotnet run --project <ProjectName>`
  - or
  - change the working directory to the project directory, then `dotnet run`
  - or
  - select the project in the IDE and run it
  - or
  - select the project in the test explorer window and run tests
- For the entire solution:
  - `dotnet test`
  - or
  - `dotnet test --test-modules **/bin/Debug/net10.0/<ProjectName>.dll` (if you want to filter out
    some of the test modules, supports globbing)
  - or
  - `dotnet test --treenode-filter /<Assembly>/<Namespace>/<Class name>/<Test name>` (if you want to
    filter out some tests, supports globbing, for all tests use `/*/*/*/*`)
  - or
  - select all tests in the test explorer window and run them

Additional flags:
- Code coverage: `--coverage`
- Trx reports: `--report-trx`

If you have [mise](https://mise.jdx.dev/) installed, then run `mise tasks` to list available tasks.
Run `mise run <task-name>` to run the task.

## Writing tests

Examples can be found in the TUnitExamples.TUnitRunner project.

### Things to keep in mind

- TUnit tests run in parallel by default, but there are mechanisms to control this behavior.
- All tests have separate test class instances to avoid accidentally sharing data across tests.
- To share data across tests, use static fields/properties, `[ClassDataSource<T>]`
- TUnit assertions have to be awaited, if this is annoying, use other assertion libraries.
- TestSession actually means executable
- Avoid circular dependencies in classes with `[ClassDataSource<T>]` properties

### Writing a simple test (BasicTests.cs)

Writing a simple test is easy: create a class in the TUnit project, then create a method with a
`[Test]` attribute, and that's it. The method can be sync or async. If you use TUnit assertions,
then all tests will be async.

### Test Data (DataDrivenTests.cs, DependencyInjectionTests.cs)

There are many ways to provide data to tests:

| Scenario                            | Approach                             |
|-------------------------------------|--------------------------------------|
| Fixed inline values                 | `[Arguments(...)]`                   |
| Data from a method                  | `[MethodDataSource]`                 |
| Shared object with lifecycle        | `[ClassDataSource<T>]`               |
| Reusable data rows                  | `[TestDataRow<T>]`                   |
| All parameter combinations          | `[MatrixDataSource]`                 |
| Multiple sources on one method      | Combined attributes                  |
| Hierarchical injection              | Nested properties                    |
| Custom generic attributes           | `[GenerateGenericTest(typeof(...))]` |
| Huge data set (reduce IDE overhead) | `DeferEnumeration = true`            |

### Test Context

All tests have a TestContext object available to them. It can be accessed with TestContext.Current,
or it can be added to `[Before(Test)]/[BeforeEvery(Test)]/[After(Test)]/[AfterEvery(Test)]` hooks as
a parameter.

Some useful properties include:

- `TestContext.Current.Metadata`: information about the test, like DisplayName
- `TestContext.Current.Execution.Result`: contains the result of the test, only available after
  execution of the test
- `TestContext.Current.Output`: can be used to write output and attach artifacts like logs or
  screenshots
- `TestContext.Current.Isolation`: can be used to create unique names and prefixes for tests
- `TestContext.Parameters`: provides access to key-value parameters passed at runtime via
  `--test-parameter` command-line option

### Controlling Tests

There are many ways to control test execution

- `[Skip]`: skips the test
- `[DependsOn]`: makes a test depend on another test. This is only needed in systems where stateless
  tests are not possible, extremely challenging, or too slow.
- `[NotInParallel(key)]`: prevents the test from running in parallel with other tests of the same
  key
- `[ParallelGroup(key)]`: runs tests with the same key in parallel, does not run any other tests
  while the group runs, in other words it batches tests with the same key together
- `[ParallelLimiter<T>]`: limits the number of tests sharing the same limiter type can run
  concurrently.
- `[Culture("en-US")]`: sets the culture for the test
- `[Timeout(milliseconds)]`: cancels the test after the specified number of milliseconds
- `[Retry(num)]`: retries the test 'num' times if it fails
- `[Repeat(num)]`: repeats the test 'num' times after the original run

### Assertions

TUnit provides its own assertion library. Its use is entirely optional, but it is mostly intuitive
(aside from having to await assertions). There is also a FluentAssertions-style API available through
the `TUnit.Assertions.Should` package.

The main syntax is:
```csharp
await Assert.That(actualValue).IsEqualTo(expectedValue);
await value.Should().BeEqualTo(expectedValue)
```
TUnit assertions have to be awaited, there is an analyzer that will warn you if you forget.

