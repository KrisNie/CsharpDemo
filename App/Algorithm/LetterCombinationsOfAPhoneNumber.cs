using System.Collections.Generic;

namespace App.Algorithm;

// https://leetcode-cn.com/problems/letter-combinations-of-a-phone-number/
public class LetterCombinationsOfAPhoneNumber
{
    public IList<string> LetterCombinations(string digits)
    {
        var letters = new List<string>();
        if (string.IsNullOrWhiteSpace(digits)) return letters;
        var dictionary = new Dictionary<char, string>
        {
            { '2', "abc" }, { '3', "def" }, { '4', "ghi" }, { '5', "jkl" }, { '6', "mno" },
            { '7', "pqrs" }, { '8', "tuv" }, { '9', "wxyz" }
        };

        // backtracking
        BackTrack(digits, dictionary, 0, letters, "");

        return letters;
    }

    private void BackTrack(
        string digits,
        Dictionary<char, string> dictionary,
        int index,
        List<string> letters,
        string result)
    {
        if (index > digits.Length - 1)
        {
            letters.Add(result);
            return;
        }

        foreach (var item in dictionary[digits[index]])
            BackTrack(digits, dictionary, index + 1, letters, result + item);
    }
}