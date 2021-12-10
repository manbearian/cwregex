using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace cwregex;

public enum Direction
{
    Horizontal = 0, VerticalA = 1, VerticalB = 2
}

// hexagonal puzzle, 7 hexes on a side
public class Puzzle
{
    readonly Regex[,] clues = new Regex[3, 13];
    
    char?[] values = new char?[(2 * (7 + 8 + 9 + 10 + 11 + 12)) + 13];

    readonly int[] rowStart = new int[13];
    readonly int[] lengths = new int[13];

    public Puzzle()
    {
        // horizontal
        clues[0, 0] = new Regex(@".*H.*H.*");
        clues[0, 1] = new Regex(@"(DI|NS|TH|OM)*");
        clues[0, 2] = new Regex(@"F.*[AO].*[AO].*");
        clues[0, 3] = new Regex(@"(O|RHHH|MM)*");
        clues[0, 4] = new Regex(@".*");
        clues[0, 5] = new Regex(@"C*MC(CCC|MM)*");
        clues[0, 6] = new Regex(@"[^C]*[^R]*III.*");
        clues[0, 7] = new Regex(@"(...?)\1*");
        clues[0, 8] = new Regex(@"([^X]|XCC)*");
        clues[0, 9] = new Regex(@"(RR|HHH)*.?");
        clues[0, 10] = new Regex(@"N.*X.X.X.*E");
        clues[0, 11] = new Regex(@"R*D*M*");
        clues[0, 12] = new Regex(@".(C|HH)*");

        // vertical A
        clues[1, 0] = new Regex(@".*G.*V.*H.*");
        clues[1, 1] = new Regex(@"[CR]*");
        clues[1, 2] = new Regex(@".*XEXM*");
        clues[1, 3] = new Regex(@".*DD.*CCM.*");
        clues[1, 4] = new Regex(@".*XHCR.*X.*");
        clues[1, 5] = new Regex(@".*(.)(.)(.)(.)\4\3\2\1.*");
        clues[1, 6] = new Regex(@".*(IN|SE|HI)");
        clues[1, 7] = new Regex(@"[^C]*MMM[^C]*");
        clues[1, 8] = new Regex(@".*(.)C\1X\1.*");
        clues[1, 9] = new Regex(@"[CEIMU]*OH[AEMOR]*");
        clues[1, 10] = new Regex(@"(RX|[^R])*");
        clues[1, 11] = new Regex(@"[^M]*M[^M]*");
        clues[1, 12] = new Regex(@"(S|MM|HHH)*");


        // vertical B
        clues[2, 0] = new Regex(@"(ND|ET|IN)[^X]*");
        clues[2, 1] = new Regex(@"[CHMNOR]*I[CHMNOR]*");
        clues[2, 2] = new Regex(@"P|(..)\1.*");
        clues[2, 3] = new Regex(@"(E|CR|MN)*");
        clues[2, 4] = new Regex(@"([^MC]|MM|CC)*");
        clues[2, 5] = new Regex(@"[AM]*CM(RC)*R?");
        clues[2, 6] = new Regex(@".*");
        clues[2, 7] = new Regex(@".*PRR.*DDC.*");
        clues[2, 8] = new Regex(@"(HHX|[^HX])*");
        clues[2, 9] = new Regex(@"([^EMC]|EM)*");
        clues[2, 10] = new Regex(@".*OXR.*");
        clues[2, 11] = new Regex(@".*LR.*RL.*");
        clues[2, 12] = new Regex(@".*SE.*UE.*");

        for (int i = 0; i < lengths.Length; ++i)
        {
            lengths[i] = (i <= 6) ? 7 + i : 13 - (i - 6);
        }

        rowStart[0] = 0;
        for (int i = 1; i < rowStart.Length; ++i)
        {
            rowStart[i] = rowStart[i - 1] + lengths[i - 1];
        }
    }

    public int MaxIndex { get => values.Length; }

    public void Set(int index, char? c)
    {
        values[index] = c;
    }

    public void Set(Direction direction, int index, string s)
    {
        if (index < 0 || index >= 13)
        {
            throw new Exception("illegal index");
        }

        int len = lengths[index];
        if (s.Length != len)
            throw new Exception($"invlalid string '{s}' should have length '{len}'");


        switch (direction)
        {
            case Direction.Horizontal:
                for (int i = 0, startIndex = rowStart[index]; i < len; ++i, ++startIndex)
                {
                    values[startIndex] = s[i];
                }
                break;
            case Direction.VerticalA:
            case Direction.VerticalB:
                throw new NotImplementedException();
            default:
                throw new Exception("bad direction");
        }
    }

    public override string ToString()
    {
        StringBuilder s = new();
        for (int i = 0; i < 13; ++i)
        {
            int len = lengths[i];
            int prefix = 13 - len;
            s.Append(' ', prefix);
            var guess = Get(Direction.Horizontal, i);
            if (guess == null)
                guess = new string('⍰', len);
            s.Append(guess.SelectMany(c => new char[] { '.', c }).ToArray());
            s.AppendLine(".");
        }
        return s.ToString();
    }

    public string GetClue(Direction direction, int index) => clues[(int)direction, index].ToString();

    public char? Get(int index) => values[index];

    public string? Get(Direction direction, int index)
    {
        if (index < 0 || index >= 13)
        {
            throw new Exception("illegal index");
        }

        int len = lengths[index];
        char[] guess = new char[len];

        // map full diagnal (starts from top/bottom hex)
        int low_lookup(int i) =>
            i < 6 ? lengths[i + 1] : lengths[12 - i];

        // map partial diagnal (starts from left/right hex)
        int hi_lookup(int i) =>
            i < (12 - index) ? lengths[i + (index - 5)] : lengths[18 - i - index];

        switch (direction)
        {
            case Direction.Horizontal:
                int start = rowStart[index];
                for (int i = 0; i < len; ++i)
                {
                    var value = values[start + i];
                    if (value == null)
                        return null;
                    guess[i] = value.Value;
                }
                break;

            case Direction.VerticalA:
                if (index >= 0 && index < 7)
                {
                    int s = rowStart[12] + index;
                    for (int i = 0; i < len; s -= low_lookup(i++))
                    {
                        var value = values[s];
                        if (value == null)
                            return null;
                        guess[i] = value.Value;
                    }
                }
                else
                {
                    int s = rowStart[12 - (index - 6)] + lengths[index - 7];
                    for (int i = 0; i < len; s -= hi_lookup(i++))
                    {
                        var value = values[s];
                        if (value == null)
                            return null;
                        guess[i] = value.Value;
                    }
                }
                break;

            case Direction.VerticalB:
                if (index >= 0 && index < 7)
                {
                    int s = rowStart[0] + index;
                    for (int i = 0; i < len; s += low_lookup(i++) - 1)
                    {
                        var value = values[s];
                        if (value == null)
                            return null;
                        guess[i] = value.Value;
                    }
                }
                else
                {
                    int s = rowStart[index - 6] + lengths[index - 7];
                    for (int i = 0; i < len; s += hi_lookup(i++) - 1)
                    {
                        var value = values[s];
                        if (value == null)
                            return null;
                        guess[i] = value.Value;
                    }
                }
                break;

            default:
                throw new Exception("bad direction");
        }

        return new string(guess);
    }

    public bool Validate(Direction direction, int index)
    {
        var guess = Get(direction, index);
        if (guess == null)
            return false;

        var match = clues[(int)direction, index].Match(guess);
        return match.Success && match.Length == guess.Length;
    }
}
