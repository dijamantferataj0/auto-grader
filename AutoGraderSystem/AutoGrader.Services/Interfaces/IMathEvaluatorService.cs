namespace AutoGrader.Services.Interfaces;

public interface IMathEvaluatorService
{
    double Evaluate(string expression);
    bool TryEvaluate(string expression, out double result);
}
