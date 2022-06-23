using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Internship_Task;

public class EchoBot
{
    public EchoBot() => Console.WriteLine("Console EchoBot is online. I will repeat any message you send me!\nSay \"quit\" to end.");

    private string input = string.Empty;

    private const string outputBaseString = "reply:";

    public void Run()
    {
        while (input != "quit")
        {
            input = Console.ReadLine();

            if (IsExpression())
                Print(GetParsedExpression());
            else
                Console.WriteLine($"{outputBaseString} {input}");
        }
    }

    private bool IsExpression()
    {
        // it only matches math expression of type x+y, x-y, x/y, x*y, where x and y are integers
        Regex regex = new(@"^[0-9]+[-+*/][0-9]+$", RegexOptions.Compiled);

        return regex.IsMatch(input);
    }

    // The reason why it returns ReadOnlyCollection is, collection must be immutable. There is no need to change it.
    private ReadOnlyCollection<string> GetParsedExpression()
    {
        var expressionMembers = Regex.Split(input, @"([-+*/])").ToList();

        var firstNumber = int.Parse(expressionMembers[0]);

        // used double for cases when selected "/" (division) as a arithmetic operator.
        // Because int / int returns integer, NOT a real number (вещественное число).
        var secondNumber = double.Parse(expressionMembers[2]);

        var result = expressionMembers[1] switch
        {
            "+" => firstNumber + secondNumber,
            "-" => firstNumber - secondNumber,
            "*" => firstNumber * secondNumber,
            "/" => firstNumber / secondNumber,
            _ => throw new Exception("Invalid arithmetic operator")
        };

        expressionMembers.Add(result.ToString());
        ReadOnlyCollection<string> readOnlyMembers = new(expressionMembers);

        return readOnlyMembers;
    }

    private void Print(ReadOnlyCollection<string> expressionMembers)
    {
        Console.WriteLine($"{outputBaseString} Первое число -> {expressionMembers[0]}\n" +
                          $"{outputBaseString} Второе число -> {expressionMembers[2]}\n" +
                          $"{outputBaseString} Арифметический оператор -> {expressionMembers[1]}\n" +
                          $"Результат = {expressionMembers[3]}");
    }
}