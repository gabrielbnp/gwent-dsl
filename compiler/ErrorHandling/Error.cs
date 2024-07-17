using System;

public static class Error
{
    public static void error(int line, int column, string errorMessage)
    {
        report(line, column, errorMessage);
        Compiler.hadError = true;
    }

    private static void report(int line, int column, string errorMessage)
    {
        Console.WriteLine("Error in line " + line + ": " + errorMessage + "\n");
        
        string? sourceCode = "    " + getSourceCode(line);
        
        string here = "    ";

        for(int i = 0; i < sourceCode.Length - 4; i++)
        {
            if(i == column)
                here += "^";
            else
                here += ".";
        }

        Console.WriteLine(sourceCode + "\n" + here + "\n");
    }

    private static string? getSourceCode(int line)
    {
        return Compiler.sourceCode[line - 1];
    }
}