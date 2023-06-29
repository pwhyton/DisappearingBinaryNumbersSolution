class Program
{
    static void Main()
    {
        var p = new Program();
        p.Run();
        Console.ReadLine();
    }

    void Run()
    {
        const string TwentyEightWithLeadingZeros = "0000011100";
        const string TwentyEight = "011100";
        const string Fourteen = "111";
        const string SevenEightFiveFive = "1111010101111";        

        Dictionary<string, int> ExpectedResults = new Dictionary<string, int>
        {
            {TwentyEight, 7},
            {TwentyEightWithLeadingZeros, 7 },
            {Fourteen, 5},
            {SevenEightFiveFive, 22},
            {new String(Enumerable.Range(0,400000).SelectMany(x => "1").ToArray()), 799999 }
        };
        Dictionary<string, int> ActualResults = new Dictionary<string, int>();

        var solution = new Solution();
        foreach (var binaryString in ExpectedResults.Keys)
        {
            ActualResults.Add(binaryString, solution.solution(binaryString));
        }

        if (ActualResults.All(r => ExpectedResults[r.Key] == r.Value))
        {
            Console.WriteLine("All the results are as expected");
        }
        else
        {
            Console.WriteLine("Results are not as expected");
        }
    }

}

class Solution
{
    public int solution(string s)
    {
        var dissappearingBinaryStringArray = new DisappearingBinaryStringArray(s);
        return dissappearingBinaryStringArray.HowLongToMakeMeDisappear();
    }
}

class DisappearingBinaryStringArray
{
    private const int MinLength = 1;
    private const int MaxLength = 1000000;   

    private int _numberOfOperations = 0;
    private char[] _binaryStringArray;
    private long _currentCharIndex;
    private long _arrayMaxLength;
    private bool _isGone;
    
    public DisappearingBinaryStringArray(string initialString)
    {
        if (!IsInRange(initialString))
        {
            throw new ArgumentOutOfRangeException($"String must be between {MinLength} and {MaxLength} characters inclusive");
        }

        if (!IsValid(initialString))
        {
            throw new ArgumentException($"The string {initialString} contains invalid characters for this operation");
        }
        _binaryStringArray = RemoveLeadingZeros(initialString).ToCharArray().Reverse().ToArray();
        _arrayMaxLength = _binaryStringArray.Length - 1;
    }

    public int HowLongToMakeMeDisappear()
    {
        while (!_isGone)       
        {
            var currentChar = _binaryStringArray[_currentCharIndex];
            if(currentChar == '0')
            {
                DivideByTwo();
            }
            else
            {
                SubtractOne();
            }
        }

        return _numberOfOperations;
    }
    private string RemoveLeadingZeros(string binaryNumber)
    {
        var trimmedNumber = binaryNumber;
        while (trimmedNumber.StartsWith("0"))
        {
            trimmedNumber = trimmedNumber.Substring(1, trimmedNumber.Length - 1);
        }

        return trimmedNumber;
    }

    private void DivideByTwo()
    {
        //just remove the zero, so one operation
        _numberOfOperations++;
        _currentCharIndex++;
    }

    private void SubtractOne()
    {
        // two operations, change 1 to zero and then remove it to divide by two
        // unless it's the last character in which case just change the 1 to zero
        _numberOfOperations++;
        if (_currentCharIndex == _arrayMaxLength)
        {
            _isGone = true;
            return;
        }

        DivideByTwo();
    }

    private bool IsInRange(string s)
    {
        return s.Length >= MinLength && s.Length <= MaxLength;
    }

    private bool IsValid(string s)
    {
        var characters = s.ToCharArray();
        return characters.All(c => c == '1' || c == '0');
    }

}