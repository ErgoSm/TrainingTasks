using System.Text.RegularExpressions;
var number = Convert.ToInt32(Console.ReadLine());
var regex = new Regex(@"(\-|\d|\w)+");
for (int i = 0; i < number; i++)
{
    var attempts = Convert.ToInt32(Console.ReadLine());
    var dictionary = new Dictionary<string, string>(200000);
    string result = "YES";

    for (int j = 0; j < attempts; j++)
    {
        result = "NO";
        var str = Console.ReadLine().ToUpper();

        if (str.Length >= 2 && str.Length <= 24 && str[0] != 45 && dictionary.TryAdd(str, str))
        {
            if (regex.Match(str)?.Value == str)
                result = "YES";

            //if (dictionary[str].Overlaps(nonValidSet))
            //    result = "NO";
        }
        else
            result = "NO";

        Console.WriteLine(result);
    }

    Console.WriteLine();
}
