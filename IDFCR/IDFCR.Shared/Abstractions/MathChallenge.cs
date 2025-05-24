namespace IDFCR.Shared.Abstractions;

internal sealed class MathChallenge(Random random) : IMathChallenge
{
    public int FirstElement { get; private set; }
    public int SecondElement { get; private set; }
    public int Answer { get; private set; }
    public Sign Sign { get; private set; }

    public void SetChallenge()
    {
        FirstElement = random.Next(1, 50);
        SecondElement = random.Next(1, 50);
        Sign = (Sign)random.Next(1, 4); // 1 to 3 inclusive

        Answer = Sign switch
        {
            Sign.Addition => FirstElement + SecondElement,
            Sign.Subtraction => FirstElement - SecondElement,
            Sign.Multiplication => FirstElement * SecondElement,
            _ => throw new InvalidOperationException("Unholy maths detected.")
        };
    }

    public override string ToString() => Sign switch
    {
        Sign.Addition => $"What is the sum of {FirstElement} and {SecondElement}?",
        Sign.Subtraction => $"What is the difference between {FirstElement} and {SecondElement}?",
        Sign.Multiplication => $"What is the product of {FirstElement} and {SecondElement}?",
        _ => "Unknown operation"
    };

    public bool Solve(int answer)
    {
        return answer == Answer;
    }
}
