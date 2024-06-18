using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Reflection;
using System;
using System.Text;
using Xunit;

namespace Pekspro.BuildInformationGenerator.Tests;

public class StringBuilderExtensionsTests
{
    [Theory]
    [InlineData("Hello, World!", "\"Hello, World!\"")]
    [InlineData("Hello\nWorld", "\"Hello\\nWorld\"")]
    [InlineData("Hello\tWorld", "\"Hello\\tWorld\"")]
    [InlineData("Hello\rWorld", "\"Hello\\rWorld\"")]
    [InlineData("Hello\\World", "\"Hello\\\\World\"")]
    [InlineData("Hello\"World\"", "\"Hello\\\"World\\\"\"")]
    public void AppendAndQouteAndFormatLiteral_ShouldAppendQuotedAndFormattedString(string input, string expectedOutput)
    {
        // Arrange
        StringBuilder sb = new StringBuilder();

        // Act
        sb.AppendAndQouteAndFormatLiteral(input);

        // Assert
        Assert.Equal(expectedOutput, sb.ToString());
    }

    [Theory]
    [InlineData("Hello, World!", "Hello, World!")]
    [InlineData("Hello\nWorld", "Hello\\nWorld")]
    [InlineData("Hello\tWorld", "Hello\\tWorld")]
    [InlineData("Hello\rWorld", "Hello\\rWorld")]
    [InlineData("Hello\\World", "Hello\\\\World")]
    [InlineData("Hello\"World\"", "Hello\\\"World\\\"")]
    public void AppendAndQouteAndFormatLiteral_ShouldAppendFormattedString(string input, string expectedOutput)
    {
        // Arrange
        StringBuilder sb = new StringBuilder();

        // Act
        sb.AppendAndFormatLiteral(input);

        // Assert
        Assert.Equal(expectedOutput, sb.ToString());
    }

    [Theory]
    [InlineData("Hello, World!", false)]
    [InlineData("Hello\nWorld", true)]
    [InlineData("Hello\tWorld", true)]
    [InlineData("Hello\rWorld", true)]
    [InlineData("Hello\\World", true)]
    [InlineData("Hello\"World\"", true)]
    public void ShouldEscape_ShouldReturnCorrectValueString(string input, bool expectedOutput)
    {
        // Act
        bool result = StringBuilderExtensions.ShouldEscape(input);

        // Assert
        Assert.Equal(expectedOutput, result);
    }

    [Theory]
    [InlineData('H', false)]
    [InlineData('\n', true)]
    [InlineData('\t', true)]
    [InlineData('\r', true)]
    [InlineData('\\', true)]
    [InlineData('\"', true)]
    public void ShouldEscape_ShouldReturnCorrectValueChar(char input, bool expectedOutput)
    {
        // Act
        bool result = StringBuilderExtensions.ShouldEscape(input);

        // Assert
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void ShouldEscape_ShouldReturnTrue_WhenCharacterIsLessThan32()
    {
        // Arrange
        char c = '\x1F';

        // Act
        bool result = StringBuilderExtensions.ShouldEscape(c);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShouldEscape_ShouldReturnTrue_WhenCharacterIsGreaterThan126()
    {
        // Arrange
        char c = '\x7F';

        // Act
        bool result = StringBuilderExtensions.ShouldEscape(c);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShouldEscape_ShouldReturnTrue_WhenCharacterIsDoubleQuote()
    {
        // Arrange
        char c = '\"';

        // Act
        bool result = StringBuilderExtensions.ShouldEscape(c);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShouldEscape_ShouldReturnTrue_WhenCharacterIsBackslash()
    {
        // Arrange
        char c = '\\';

        // Act
        bool result = StringBuilderExtensions.ShouldEscape(c);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShouldEscape_ShouldReturnFalse_WhenCharacterIsLetterA()
    {
        // Arrange
        char c = 'a';

        // Act
        bool result = StringBuilderExtensions.ShouldEscape(c);

        // Assert
        Assert.False(result);
    }

#if NET6_0_OR_GREATER

    [Fact]
    public void EscapeAllCharactersAndVerify()
    {
        StringBuilder sb = new StringBuilder();

        for (int i = char.MinValue; i <= char.MaxValue; i++)
        {
            sb.Append((char)i);
        }

        string allCharacters = sb.ToString();

        string escapedAllCharacters = new StringBuilder().AppendAndQouteAndFormatLiteral(allCharacters).ToString();


        string code = @"
using System;
namespace RuntimeCode
{
    public class Program
    {
        public static string GetMessage()
        {
            return " + escapedAllCharacters + @";
        }
    }
}";

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

        string assemblyName = Path.GetRandomFileName();
        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location)
        };

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using (var ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            Assert.True(result.Success);

            ms.Seek(0, SeekOrigin.Begin);

            Assembly assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
            MethodInfo method = assembly.GetType("RuntimeCode.Program")!.GetMethod("GetMessage")!;

            string output = (string)method.Invoke(null, null)!;
            
            Assert.Equal(allCharacters, output);
        }
    }

#endif

}