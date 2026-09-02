using TUnitExamples.Common;
using TUnitExamples.TUnitRunner.Data;

namespace TUnitExamples.TUnitRunner;

public class DataDrivenTests
{
    public IEnumerable<(int, int, int)> InstanceSubtractCases => [(5, 3, 2), (10, 7, 3), (0, 0, 0)];

    public static IEnumerable<(int, int, int)> SubtractCases()
    {
        yield return (5, 3, 2);
        yield return (10, 7, 3);
        yield return (0, 0, 0);
    }

    [Test]
    [Arguments(1, 2, 3, DisplayName = "One plus two equals three", Categories = ["SimpleAddition"])]
    [Arguments(5, -3, 2)]
    [Arguments(0, 0, 0, Skip = "Because reasons")]
    public async Task Add_WithArguments(int a, int b, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Add(a, b);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [MethodDataSource(nameof(SubtractCases))]
    public async Task Subtract_WithMethodDataSource(int a, int b, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(a, b);

        await Assert.That(result).IsEqualTo(expected);
    }

    // When deferring enumeration, the individual test cases won't show up in the test explorer,
    // instead they will be enumerated at runtime.
    [Test]
    [MethodDataSource(nameof(SubtractCases), DeferEnumeration = true)]
    public async Task Subtract_WithMethodDataSourceDeferred(int a, int b, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(a, b);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [InstanceMethodDataSource(nameof(InstanceSubtractCases))]
    public async Task Subtract_WithInstanceMethodDataSource(int a, int b, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(a, b);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [MethodDataSource<MethodDataSources>(nameof(MethodDataSources.SubtractCases))]
    public async Task Subtract_WithMethodDataSourceFromOtherClass(int a, int b, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(a, b);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [MethodDataSource<MethodDataSources>(nameof(MethodDataSources.SubtractCasesAsRecords))]
    public async Task Subtract_WithMethodDataSourceAsRecordsFromOtherClass(
        SubtractCase subtractCase
    )
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(subtractCase.A, subtractCase.B);

        await Assert.That(result).IsEqualTo(subtractCase.Expected);
    }

    [Test]
    [MethodDataSource<MethodDataSources>(nameof(MethodDataSources.AsyncSubtractCases))]
    public async Task Subtract_WithAsyncMethodDataSourceFromOtherClass(int a, int b, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(a, b);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [MethodDataSource<MethodDataSources>(nameof(MethodDataSources.AsyncSubtractCasesAsRecords))]
    public async Task Subtract_WithAsyncMethodDataSourceAsRecordsFromOtherClass(
        SubtractCase subtractCase
    )
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(subtractCase.A, subtractCase.B);

        await Assert.That(result).IsEqualTo(subtractCase.Expected);
    }

    [Test]
    [MethodDataSource<MethodDataSources>(nameof(MethodDataSources.SubtractTestDataRows))]
    public async Task Subtract_WithMethodDataSourceTestDataRows(int a, int b, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(a, b);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [MethodDataSource<MethodDataSources>(nameof(MethodDataSources.SubtractTestDataRowsWithRecords))]
    public async Task Subtract_WithMethodDataSourceTestDataRowsWithRecords(
        SubtractCase subtractCase
    )
    {
        var calculator = new Calculator();

        var result = calculator.Subtract(subtractCase.A, subtractCase.B);

        await Assert.That(result).IsEqualTo(subtractCase.Expected);
    }

    [Test]
    [MatrixDataSource]
    public async Task Multiply_AllCombinations([Matrix(1, 2, 3)] int a, [Matrix(0, 1, -1)] int b)
    {
        var calculator = new Calculator();

        var result = calculator.Multiply(a, b);

        await Assert.That(result).IsEqualTo(a * b);
    }

    // You can exclude specific combinations using the MatrixExclusion attribute
    [Test]
    [MatrixDataSource]
    [MatrixExclusion(1, -1)]
    public async Task Multiply_AllCombinationsWithRanges(
        [MatrixRange<int>(1, 3)] int a,
        [MatrixRange<int>(-1, 1)] int b
    )
    {
        var calculator = new Calculator();

        var result = calculator.Multiply(a, b);

        await Assert.That(result).IsEqualTo(a * b);
    }

    [Test]
    [MatrixDataSource]
    public async Task Multiply_AllCombinationsWithMethods(
        [MatrixMethod<MethodDataSources>(nameof(MethodDataSources.MatrixNumbers1))] int a,
        [MatrixMethod<MethodDataSources>(nameof(MethodDataSources.MatrixNumbers2))] int b
    )
    {
        var calculator = new Calculator();

        var result = calculator.Multiply(a, b);

        await Assert.That(result).IsEqualTo(a * b);
    }

    [Test]
    [CombinedDataSources]
    public async Task Multiply_AllCombinationsWithArgumentsAndMethods(
        [Arguments(1, 2, 3)] int a,
        [MethodDataSource<MethodDataSources>(nameof(MethodDataSources.MatrixNumbers2))] int b
    )
    {
        var calculator = new Calculator();

        var result = calculator.Multiply(a, b);

        await Assert.That(result).IsEqualTo(a * b);
    }

    [Test]
    [AdditionDataGenerator]
    public async Task Add_WithCustomDataGenerator(int a, int b, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Add(a, b);

        await Assert.That(result).IsEqualTo(expected);
    }
}

// Arguments can also be applied at the class level to parameterize the constructor
[Arguments(10)]
[Arguments(100)]
public class ClassLevelArgumentTests(int divisor)
{
    [Test]
    [Arguments(100)]
    [Arguments(50)]
    public async Task Divide_WithClassAndMethodArguments(int dividend)
    {
        var calculator = new Calculator();

        var result = calculator.Divide(dividend, divisor);

        await Assert.That(result).IsGreaterThan(0);
    }
}
