using System;

public static class Error
{
    public static void error(int line, int column, string errorMessage)
    {
        report(line, column, errorMessage);
    }

    private static void report(int line, int column, string errorMessage)
    {
        Console.WriteLine("Error in line " + line + "\n");
        
        string? sourceCode = "    " + getSourceCode(line);
        
        string here = "    ";

        for(int i = 1; i <= sourceCode.Length; i++)
        {
            if(i == column)
                here += "^";
            else
                here += ".";
        }

        Console.WriteLine(sourceCode + "\n" + here);
    }

    private static string? getSourceCode(int line)
    {
        return Compiler.sourceCode[line - 1];
    }
}