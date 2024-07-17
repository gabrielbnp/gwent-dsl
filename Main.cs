using System;
using System.IO;

public class Compiler
{
    public static List<string?> sourceCode = new List<string?>();
    public static Boolean hadError = false;

    public static void Main(string[] args)
    {
        if(args.Length == 0)
        {
            throw new Exception();
        }
        else if(args.Length > 1)
        {
            throw new Exception();
        }
        else
        {
            readFile(args[0]);
        }
    }

    private static void readFile(string path)
    {
        StreamReader file = new StreamReader(path);

        string? line = file.ReadLine();
        int numLine = 1;

        while( line != null )
        {
            storeLine(line);

            // scan this line

            Lexer lexer = new Lexer(line, numLine);
            List<Token> tokens = lexer.getTokens;

            foreach(Token token in tokens)
                Console.WriteLine(token);

            // indicates an error in the code
            if(hadError)
                throw new Exception("Fix compilation errors and run the code again.");

            Parser parser = new Parser(tokens);

            line = file.ReadLine();
            numLine++;
        }
    }

    private static void storeLine(string? line)
    {
        sourceCode.Add(line);
    }
}