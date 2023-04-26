using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace civet
{
    class Conditionals
    {
        public static bool MultiProcess(string equation)
        {
            string[] operands = { "&&", "||", "x|", "!|", "=", "!=", "~=", "<", ">" };

            string[] parts = internalUtils.splitString(equation, operands, true);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Contains("`"))
                {
                    parts[i] = internalUtils.trimWithin(parts[i], '`');

                    //int x = 0;
                    //while (x < parts[i].Length)
                    //{
                    //    if (parts[i][x] == '`')
                    //    {
                    //        int xx = 1;
                    //        while (parts[i][x + xx] != '`') xx++;
                    //        xx++;
                    //        string varName = parts[i].Substring(x, xx);
                    //        parts[i] = parts[i].Replace(varName, VarMan.GetVar(varName.Replace("`", "")));
                    //    }
                    //    x++;
                    //}
                }
            }
            if (Program.debugMode) foreach (string part in parts) { Console.WriteLine(part); }

            int cond = 0;
            string[] firstBits = { parts[cond], parts[cond + 1], parts[cond + 2] };
            bool result = Process(firstBits);
            cond += 3;
            while (cond < parts.Length - 3)
            {
                string[] bits = { parts[cond + 1], parts[cond + 2], parts[cond + 3] };
                bool indivResult = Process(bits);
                if (Program.debugMode) Console.WriteLine($"Result of bits[] = {indivResult}");
                switch (parts[cond].ToLower())
                {
                    case "&&":
                        if (result && indivResult) result = true;
                        else result = false;
                        break;
                    case "||":
                        if (result || indivResult) result = true;
                        else result = false;
                        break;
                    case "x|":
                        if ((result && !indivResult) || (indivResult && !result)) result = true;
                        else result = false;
                        break;
                    case "!|":
                        if (!result && !indivResult) result = true;
                        else result = false;
                        break;
                    default:
                        KernelErrors.InvalidConditional();
                        break;
                }
                cond += 4;
            }

            return result;
        }

        public static bool Process(string[] parts)
        {
            try
            {
                bool result = false;
                if (Program.debugMode) Console.WriteLine($"Eval: {parts[1]}");
                if (parts[1] == "=")
                {
                    if (parts[0] == parts[2]) result = true;
                    else result = false;
                }
                else if (parts[1] == "!=")
                {
                    if (parts[0] != parts[2]) result = true;
                    else result = false;
                }
                else if (parts[1] == ">")
                {
                    if (Convert.ToInt32(parts[0]) > Convert.ToInt32(parts[2])) result = true;
                    else result = false;
                }
                else if (parts[1] == "<")
                {
                    if (Convert.ToInt32(parts[0]) > Convert.ToInt32(parts[2])) result = true;
                    else result = false;
                }
                else if (parts[1] == "~=")
                {
                    if (Convert.ToInt32(parts[0]) == Convert.ToInt32(parts[2])) result = true;
                    else result = false;
                }

                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}
