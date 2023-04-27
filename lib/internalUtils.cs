using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace civet
{
    internal class internalUtils
    {

        public static string[] splitString(string str, string delimiter, bool keepDelimiters)
        {
            string[] result = new string[str.Split(delimiter).Length + timesInString(str, delimiter)];

            if (Program.debugMode) Console.WriteLine("Times " + delimiter + " in " + str + " - " + timesInString(str,delimiter));

            if (timesInString(str, delimiter) == 0)
            {
                result = new string[1] { str };
                return result;
            }
            int i = 0;
            while (i < str.Split(delimiter).Length)
            {
                result[i * 2] = str.Split(delimiter)[i];
                if (result.Length > (i*2)+1) result[(i * 2) + 1] = delimiter;
                i++;
                
            }

            return result;
        }

        public static int timesInString(string str, char c)
        {
            int i = 0;
            foreach (char ch in str) if (ch == c) i++;
            return i;
        }

        public static int timesInString(string str1, string str2)
        {
            int lenDif = str1.Length - str1.Replace(str2, "").Length;
            return lenDif / str2.Length;
        }


        //There's a bug here somewhere to do with {{ and }}
        public static string[] splitString(string str, string[] delimiters, bool keepDelimiters = false)
        {
            string[] workingList = new string[1];
            workingList[0] = str;

            string[] outputList = new string[0];
            string[] mem;

            foreach (string delimiter in delimiters)
            {
                foreach (string entry in workingList)
                {
                    mem = splitString(entry, delimiter, keepDelimiters);
                    outputList = mergeList(outputList, mem);
                }
                workingList = outputList;
                outputList = new string[0];
            }
            return workingList;
        }

        public static string[] mergeList(string[] list1, string[] list2, bool ignoreDuplicates = false)
        {
            string[] output = new string[list1.Length + list2.Length];
            for (int i = 0; i < list1.Length; i++) output[i] = list1[i];
            for (int i = list1.Length; i < output.Length; i++) output[i] = list2[i - list1.Length];
            return output;
        }

        public static string trimWithin(string str, char delimiter)
        {
            if (Program.debugMode) Console.WriteLine($"Delimiting by {delimiter} of \"{str}\"");
            string s = "";
            int i = 0;
            while (str[i] != delimiter) { i++; }
            i++;
            while (str[i] != delimiter) { s += str[i]; i++; }
            return s;
        }

    }
}
