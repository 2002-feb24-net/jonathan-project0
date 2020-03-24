using System;


public static class Wrappers
{
    //Trims and parses console input and returns an int
    //reprompts user if input is invalid by default
    public static int ReadInt(bool reprompt = true)
    {
        do
        {
            try
            {
                string input = Console.ReadLine().Trim().ToLower();
                if (input == "q") return -1;
                return int.Parse(input);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter only numbers");
            }
        } while (true && reprompt);
        return -1;
    }
    //Reads console response to @param prompt into out param input
    //Returns false if user entered a quit sequence specified by quit or white space only
    public static bool ReadString(string prompt,out string input,string quit="")
    {
        Console.WriteLine(prompt);
        input = Console.ReadLine().Trim();
        return (input==quit||input=="");

    }
    public static bool IsInt(string input)
    {
        input = input.Trim();
        try
        {
            int.Parse(input);
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}
