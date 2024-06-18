using System.Text;

namespace Pekspro.BuildInformationGenerator;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendAndQouteAndFormatLiteral(this StringBuilder sb, string str)
    {
        // This is slower:
        // string escapedString = Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(str, true);
        // return builder.Append(escapedString);

        sb.Append('\"');

        sb.AppendAndFormatLiteral(str);

        sb.Append('\"');

        return sb;
    }

    public static void AppendAndFormatLiteral(this StringBuilder sb, string str)
    {
        if (!ShouldEscape(str))
        {
            sb.Append(str);
        }
        else
        {
            foreach (char c in str)
            {
                if (ShouldEscape(c))
                {
                    sb.AppendAndFormatLiteral(c);
                }
                else
                {
                    sb.Append(c);
                }
            }
        }
    }

    public static StringBuilder AppendAndFormatLiteral(this StringBuilder sb, char c)
    {
        // And here is some overoptimzed code:

        if (c == '\"')
        {
            sb.Append('\\').Append('\"');
        }
        else if (c == '\\')
        {
            sb.Append('\\').Append('\\');
        }
        else if (c == '\n')
        {
            sb.Append('\\').Append('n');
        }
        else if (c == '\t')
        {
            sb.Append('\\').Append('t');
        }
        else if (c == '\r')
        {
            sb.Append('\\').Append('r');
        }
        // It's significantly faster skips these rarely used characters:
        // else if (c == '\v')
        // {
        //     sb.Append('\\').Append('v');
        // }
        // else if (c == '\a')
        // {
        //     sb.Append('\\').Append('a');
        // }
        else
        {
            sb.Append($"\\u{(int)c:x4}");
        }

        return sb;
    }

    public static bool ShouldEscape(string str)
    {
        foreach (var ch in str)
        {
            if (ShouldEscape(ch))
            {
                return true;
            }
        }

        return false;
    }

    public static bool ShouldEscape(char c)
    {
        /*
             32 Space
             33 !
             34 "
             35 #
            ...
             90 Z
             91 [
             92 \
             93 ]
            125 }
            126 ~
            127 DEL
        */
        return c < 32 || c > 126 || c == '\"' || c == '\\';
    }

    /*
    static SearchValues<char> NoneEscapeCharacters = SearchValues.Create(
        " !#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~");
    */
}

