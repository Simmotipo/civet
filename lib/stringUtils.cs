using System;
using System.Collections.Generic;
using System.Text;

namespace civet
{

    //Need to be able to: (all must support encapsulating inputs in quotemarks)
    //string <input> split <splitter> <targetArray> (need to implement arrays first)
    //string <input> reverse

    class StringUtils
    {
        public static string Go(string input)
        {
            try
            {
                string str1 = "";
                int i = 1;
                if (input[0] == '"')
                {
                    while (input[i + 1] != '"') i++;
                    str1 = input.Substring(1, i);
                    i += 3;
                }
                else if (input[0] == '\'')
                {
                    while (input[i + 1] != '\'') i++;
                    str1 = input.Substring(1, i);
                    i += 3;
                }
                else { str1 = input.Split(' ')[0]; i = str1.Length + 1; }

                string postStr1 = input[i..];
                string keyword = postStr1.Split(' ')[0].ToLower();
                if (Program.debugMode) Console.WriteLine($"\"{postStr1}\"");

                switch (keyword)
                {
                    case "uppercase":
                        return str1.ToUpper();
                    case "lowercase":
                        return str1.ToLower();
                    case "substring":
                        if (postStr1.Split(' ').Length == 2) return str1[Convert.ToInt32(postStr1.Split(' ')[1])..];
                        else return str1.Substring(Convert.ToInt32(postStr1.Split(' ')[1]), Convert.ToInt32(postStr1.Split(' ')[2]));
                    case "replace":
                        return Replace(str1, postStr1[8..]);
                    case "reverse":
                        string output = "";
                        for (i = str1.Length - 1; i >= 0; i--) output += str1[i];
                        return output;
                    case "contains":
                        if (str1.Contains(postStr1[9..])) return "true";
                        else return "false";
                    case "startswith":
                        if (str1.StartsWith(postStr1[11..])) return "true";
                        else return "false";
                    case "indexof":
                        if (!str1.Contains(postStr1[8..])) return "-1";
                        else
                        {
                            return Convert.ToString(str1.IndexOf(postStr1[8..]));
                        }
                    case "length":
                        return Convert.ToString(str1.Length);


                }

                return str1;
            }
            catch (Exception e)
            {
                StringErrors.Generic(e.ToString());
                return "";
            }
        }
        public static string Replace(string original, string restOfCommand)
        {
            try
            {
                string oldText = "";
                int i = 1;
                if (restOfCommand[0] == '"')
                {
                    while (restOfCommand[i + 1] != '"') i++;
                    oldText = restOfCommand.Substring(1, i);
                    i += 3;
                }
                else if (restOfCommand[0] == '\'')
                {
                    while (restOfCommand[i + 1] != '\'') i++;
                    oldText = restOfCommand.Substring(1, i);
                    i += 3;
                }
                else { oldText = restOfCommand.Split(' ')[0]; i = oldText.Length + 1; }

                string newText = restOfCommand[i..];
                if (newText.StartsWith("\"")) newText = newText[1..^1];

                return original.Replace(oldText, newText);
            }
            catch (Exception e)
            {
                StringErrors.ReplaceError(original + " " + restOfCommand, e.ToString());
                return "";
            }
        }

    }
}
