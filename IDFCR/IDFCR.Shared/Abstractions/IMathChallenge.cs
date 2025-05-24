namespace IDFCR.Shared.Abstractions;

public interface IMathChallenge
{
    int FirstElement { get; }
    int SecondElement { get; }
    int Answer { get; }
    Sign Sign { get; }
    void SetChallenge();
    bool Solve(int answer);
}
