using System.Runtime.CompilerServices;

namespace TUnitExamples.TUnitRunner.Data;

public record SubtractCase(int A, int B, int Expected);

public class MethodDataSources
{
    // Returning T is OK if it's a value type
    // static methods are generally AOT compatible
    public static IEnumerable<(int, int, int)> SubtractCases()
    {
        yield return (5, 3, 2);
        yield return (10, 7, 3);
        yield return (0, 0, 0);
    }

    // For reference types, method data source has to return Func<T>
    // Func<T> ensures that each test gets a fresh object
    public static IEnumerable<Func<SubtractCase>> SubtractCasesAsRecords()
    {
        yield return () => new SubtractCase(5, 3, 2);
        yield return () => new SubtractCase(10, 7, 3);
        yield return () => new SubtractCase(0, 0, 0);
    }

    // Async data source with cancellation support, return T when it's a value type
    public static async IAsyncEnumerable<(int, int, int)> AsyncSubtractCases(
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        // Simulate async data loading
        await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);

        yield return (5, 3, 2);
        yield return (10, 7, 3);
        yield return (0, 0, 0);
    }

    // Async data source with cancellation support, return Func<T> when it's a reference type
    public static async IAsyncEnumerable<Func<SubtractCase>> AsyncSubtractCasesAsRecords(
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        // Simulate async data loading
        await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);

        yield return () => new SubtractCase(5, 3, 2);
        yield return () => new SubtractCase(10, 7, 3);
        yield return () => new SubtractCase(0, 0, 0);
    }

    // To add a custom display name, category or to skip some of the test cases, wrap the data in TestDataRow
    public static IEnumerable<TestDataRow<(int A, int B, int Expected)>> SubtractTestDataRows()
    {
        yield return new(
            (5, 3, 2),
            DisplayName: "$a minus $b equals $expected",
            Categories: ["SimpleSubtraction"]
        );
        yield return new((10, 7, 3), DisplayName: "$arg1 minus $arg2 equals $arg3");
        yield return new((0, 0, 0), Skip: "Some reason");
    }

    // To add a custom display name, category or to skip some of the test cases, wrap the data in TestDataRow
    public static IEnumerable<TestDataRow<Func<SubtractCase>>> SubtractTestDataRowsWithRecords()
    {
        yield return new(
            () => new(5, 3, 2),
            DisplayName: "Subtract with case: $subtractCase",
            Categories: ["SimpleSubtraction"]
        );
        yield return new(() => new(10, 7, 3), DisplayName: "Subtract with case: $arg1");
        yield return new(() => new(0, 0, 0), Skip: "Some reason");
    }

    public static IEnumerable<int> MatrixNumbers1()
    {
        yield return 1;
        yield return 2;
        yield return 3;
    }

    public static IEnumerable<int> MatrixNumbers2()
    {
        yield return -1;
        yield return 0;
        yield return 1;
    }
}
