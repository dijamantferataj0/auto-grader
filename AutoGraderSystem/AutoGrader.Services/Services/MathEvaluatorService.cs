using AutoGrader.Services.Interfaces;

namespace AutoGrader.Services.Services;

public class MathEvaluatorService : IMathEvaluatorService
{
    public double Evaluate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException("Expression cannot be empty");

        expression = expression.Replace(" ", "");

        var tokens = Tokenize(expression);
        var rpn = ConvertToRPN(tokens);
        return EvaluateRPN(rpn);
    }

    public bool TryEvaluate(string expression, out double result)
    {
        try
        {
            result = Evaluate(expression);
            return true;
        }
        catch
        {
            result = 0;
            return false;
        }
    }

    private List<string> Tokenize(string expression)
    {
        var tokens = new List<string>();
        var currentNumber = "";

        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];

            if (char.IsDigit(c) || c == '.')
            {
                currentNumber += c;
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                if (!string.IsNullOrEmpty(currentNumber))
                {
                    tokens.Add(currentNumber);
                    currentNumber = "";
                }
                tokens.Add(c.ToString());
            }
            else if (c != ' ')
            {
                throw new ArgumentException($"Invalid character in expression: {c}");
            }
        }

        if (!string.IsNullOrEmpty(currentNumber))
        {
            tokens.Add(currentNumber);
        }

        return tokens;
    }

    private List<string> ConvertToRPN(List<string> tokens)
    {
        var output = new List<string>();
        var operators = new Stack<string>();

        foreach (var token in tokens)
        {
            if (double.TryParse(token, out _))
            {
                output.Add(token);
            }
            else if (IsOperator(token))
            {
                while (operators.Count > 0 &&
                       IsOperator(operators.Peek()) &&
                       GetPrecedence(operators.Peek()) >= GetPrecedence(token))
                {
                    output.Add(operators.Pop());
                }
                operators.Push(token);
            }
        }

        while (operators.Count > 0)
        {
            output.Add(operators.Pop());
        }

        return output;
    }

    private double EvaluateRPN(List<string> rpn)
    {
        var stack = new Stack<double>();

        foreach (var token in rpn)
        {
            if (double.TryParse(token, out double number))
            {
                stack.Push(number);
            }
            else if (IsOperator(token))
            {
                if (stack.Count < 2)
                    throw new ArgumentException("Invalid expression");

                double b = stack.Pop();
                double a = stack.Pop();

                double result = token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => b == 0 ? throw new DivideByZeroException() : a / b,
                    _ => throw new ArgumentException($"Unknown operator: {token}")
                };

                stack.Push(result);
            }
        }

        if (stack.Count != 1)
            throw new ArgumentException("Invalid expression");

        return stack.Pop();
    }

    private bool IsOperator(string token)
    {
        return token == "+" || token == "-" || token == "*" || token == "/";
    }

    private int GetPrecedence(string op)
    {
        return op switch
        {
            "+" => 1,
            "-" => 1,
            "*" => 2,
            "/" => 2,
            _ => 0
        };
    }
}
