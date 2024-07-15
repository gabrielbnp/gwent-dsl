using System;
using System.IO;

public class Compiler
{
    public static List<string?> sourceCode = new List<string?>();

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

            line = file.ReadLine();
            numLine++;
        }
    }

    private static void storeLine(string? line)
    {
        sourceCode.Add(line);
    }
}