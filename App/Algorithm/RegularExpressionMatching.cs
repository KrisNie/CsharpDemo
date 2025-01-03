namespace App.Algorithm;

// https: //leetcode-cn.com/problems/regular-expression-matching/
public class RegularExpressionMatching
{
    public bool IsMatch(string s, string p)
    {
        if (string.IsNullOrWhiteSpace(p)) return string.IsNullOrWhiteSpace(s);

        var firstMatch = !string.IsNullOrWhiteSpace(s) && (p[0] == s[0] || p[0] == '.');

        if (p.Length >= 2 && p[1] == '*')
            return (firstMatch && IsMatch(s.Substring(1), p)) || IsMatch(s, p.Substring(2));
        return firstMatch && IsMatch(s.Substring(1), p.Substring(1));
    }
}