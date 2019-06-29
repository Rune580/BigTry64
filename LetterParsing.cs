using System;
using System.Collections.Generic;
using System.Text;

namespace BigTry64
{
    public static class LetterParsing
    {
        public static int[] ParseString(char character)
        {
            switch (character)
            {

                case 'Q':
                    return new int[] { 0, 0 };
                case 'W':
                    return new int[] { 1, 0 };
                case 'E':
                    return new int[] { 2, 0 };
                case 'R':
                    return new int[] { 3, 0 };
                case 'T':
                    return new int[] { 4, 0 };
                case 'Y':
                    return new int[] { 5, 0 };
                case 'U':
                    return new int[] { 6, 0 };
                case 'I':
                    return new int[] { 7, 0 };
                case 'O':
                    return new int[] { 8, 0 };
                case 'A':
                    return new int[] { 0, 1 };
                case 'S':
                    return new int[] { 1, 1 };
                case 'D':
                    return new int[] { 2, 1 };
                case 'F':
                    return new int[] { 3, 1 };
                case 'G':
                    return new int[] { 4, 1 };
                case 'H':
                    return new int[] { 5, 1 };
                case 'J':
                    return new int[] { 6, 1 };
                case 'K':
                    return new int[] { 7, 1 };
                case 'L':
                    return new int[] { 8, 1 };
                case 'Z':
                    return new int[] { 0, 2 };
                case 'X':
                    return new int[] { 1, 2 };
                case 'C':
                    return new int[] { 2, 2 };
                case 'V':
                    return new int[] { 3, 2 };
                case 'B':
                    return new int[] { 4, 2 };
                case 'N':
                    return new int[] { 5, 2 };
                case 'M':
                    return new int[] { 6, 2 };
                case ',':
                    return new int[] { 7, 2 };
                case '.':
                    return new int[] { 8, 2 };
                case '1':
                    return new int[] { 0, 3 };
                case '2':
                    return new int[] { 1, 3 };
                case '3':
                    return new int[] { 2, 3 };
                case '4':
                    return new int[] { 3, 3 };
                case '5':
                    return new int[] { 4, 3 };
                case '6':
                    return new int[] { 5, 3 };
                case '7':
                    return new int[] { 6, 3 };
                case '8':
                    return new int[] { 7, 3 };
                case '9':
                    return new int[] { 8, 3 };
                default:
                    return new int[]{0,0};
            }
        }
    }
}
