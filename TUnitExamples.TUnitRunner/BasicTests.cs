using TUnitExamples.Common;

namespace TUnitExamples.TUnitRunner;

public class BasicTests
{
    [Test]
    public async Task Add_ReturnsSum()
    {
        var calculator = new Calculator();

        var result = calculator.Add(1, 2);

        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    [Skip("Adding two numbers is too hard")]
    public async Task Add_ReturnsSum_Skipped()
    {
        var calculator = new Calculator();

        var result = calculator.Add(1, 2);

        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    [NotInParallel("SyncTest")]
    public async Task Add_ReturnsSum_Sync1()
    {
        var calculator = new Calculator();

        var result = calculator.Add(1, 2);

        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    [NotInParallel("SyncTest")]
    public async Task Add_ReturnsSum_Sync2()
    {
        var calculator = new Calculator();

        var result = calculator.Add(1, 2);

        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    public async Task Divide_ByZero_ThrowsException()
    {
        var calculator = new Calculator();

        // Asserts can return values, very useful for null checking and casting.
        var maybeException = await Assert
            .That(() => calculator.Divide(1, 0))
            .Throws<Exception>()
            .WithMessage("Attempted to divide by zero.");

        var exception = await Assert.That(maybeException).IsNotNull();

        var divideByZeroException = await Assert.That(exception).IsTypeOf<DivideByZeroException>();

        // you can assert on object properties with .Member()
        // you can chain assertions on the same object with .And and .Or
        await Assert
            .That(divideByZeroException)
            .IsNotNull()
            .And.Member(
                e => e.Message,
                message => message.IsEqualTo("Attempted to divide by zero.")
            )
            .And.Member(e => e.Source, source => source.IsEqualTo("TUnitExamples.Common"));

        // does not stop at the first failure, it collects all failures inside the block and reports them together.
        using (Assert.Multiple())
        {
            await Assert.That(divideByZeroException).IsNotNull();
            await Assert
                .That(divideByZeroException.Message)
                .IsEqualTo("Attempted to divide by zero.");
            await Assert.That(divideByZeroException.Source).IsEqualTo("TUnitExamples.Common");
        }
    }
}
