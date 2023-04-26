﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;


//Welcome to Civet v0.5.0a1
//A programming language by Oliver Simpson
//(c) Feb 08 2023.

namespace civet
{
    class Program
    {
        public static bool debugMode = false;
        public static bool breakOnError = true;
        public static bool printErrors = true;
        public static bool readOnlyFS = false;

        public static int index = 0;
        public static string filePath = "";
        public static string workingMem = "";
        public static string version = "v0.5.0a1";
        public static string copyright = "(c) 26-Apr-2023";
        public static string[] lines;
        public static string currentLine = "";

        public static Random sysRand = new Random();

        public static void Main(string[] args)
        {
            Program p = new Program();
            p.Run(args);
        }

        void Run(string[] args)
        {
            if (args.Length == 0) PrintHelp(true);
            if (args.Length > 0)
            {
                if (args[0].ToLower() == "help") PrintHelp();
                if (!File.Exists(args[0]))
                {
                    if (File.Exists(args[0] + ".cvt")) args[0] = args[0] + ".cvt";
                    else if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + args[0])) args[0] = AppDomain.CurrentDomain.BaseDirectory + args[0];
                    else if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + args[0] + ".cvt")) args[0] = AppDomain.CurrentDomain.BaseDirectory + args[0] + ".cvt";
                }

                if (args.Length > 1)
                {
                    try
                    {
                        for (int i = 1; i < args.Length; i++)
                        {
                            string flag = args[i].Split('=')[0].ToLower();
                            string value = args[i].Split('=')[1].ToLower();
                            if (flag == "debug") debugMode = Convert.ToBoolean(value);
                            else if (flag == "breakonerror") breakOnError = Convert.ToBoolean(value);
                            else if (flag == "printerrors") printErrors = Convert.ToBoolean(value);
                            if (debugMode) Console.WriteLine($"f:{flag} v:{value}");
                        }
                    }
                    catch 
                    {
                        PrintHelp();
                    }
                }

                if (File.Exists(args[0]))
                {
                    if (debugMode) Console.WriteLine("Running in DEBUG mode.");
                    filePath = args[0];
                    if (File.ReadAllText(args[0]).Contains("¦+"))
                    {
                        Console.WriteLine("Error: This program is using Broken Bar Embedded commands (¦+ and +¦), which are no longer supported.\nPlease run this program using Civet v0.8.x or earlier.");
                    }
                    lines = File.ReadAllLines(args[0]);
                    Interpreter();
                }
                else
                {
                    Console.WriteLine("Could not find file with specified name, or at specified path");
                }
            }
            else PrintHelp();
        }

        static void PrintHelp(bool waitForKey = false)
        {
            for (int i = 0; i < $"Civet {version}".Length; i++) Console.Write('#'); Console.Write("\n");
            Console.WriteLine($"Civet {version}");
            Console.WriteLine("By Oliver Simpson");
            Console.WriteLine($"{copyright}");
            for (int i = 0; i < $"Civet {version}".Length; i++) Console.Write('#'); Console.Write("\n");
            Console.WriteLine("civet path/to/file variable=value");
            Console.WriteLine("Variables:\ndebug - true/false\nbreakOnError - true/false\nprintErrors - true/false");
            for (int i = 0; i < $"Civet {version}".Length; i++) Console.Write('#'); Console.Write("\n");
            if (waitForKey) Console.ReadKey();
            Environment.Exit(0);
        }

        void Interpreter()
        {
            index = 0;
            while (index < lines.Length)
            {
                Interpret(lines[index]);
                index++;
            }
        }

        public string Interpret(string line, bool subCmd = false)
        {
            try
            {

                if (line.Replace(" ", "") == "") return "";
                currentLine = line;
                if (line.StartsWith(">")) return "";
                if (subCmd)
                {
                    if (line.StartsWith(" ")) line = line[1..];
                    if (line.EndsWith(" ")) line = line[0..^1];
                }
                workingMem = "";

                while (line.Contains('|'))
                {
                    int x = 0;

                    while (line[x] != '|') x++;
                    int subLevel = 1;

                    int xx = 1;
                    while (subLevel != 0)
                    {
                        if (line[x + xx] == '+' && line[x + xx + 1] == '|') subLevel--;
                        else if (line[x + xx] == '|' && line[x + xx + 1] == '+') subLevel++;
                        xx++;
                    }
                    xx++;

                    string cmd = line.Substring(x+2, xx-4);
                    workingMem = "> subCmd generated - " + cmd;
                    if (debugMode) Console.WriteLine(workingMem);
                    line = line.Replace(line.Substring(x, xx), Interpret(cmd, true));
                    //if (subCmd) return line;
                }
                while (line.Contains('{'))
                {
                    int x = 0;

                    while (line[x] != '{') x++;
                    int subLevel = 1;

                    int xx = 1;
                    while (subLevel != 0)
                    {
                        if (line[x + xx] == '}') subLevel--;
                        else if (line[x + xx] == '{') subLevel++;
                        xx++;
                    }

                    if (debugMode) Console.WriteLine($"X={x},XX={xx}");
                    string cmd = line.Substring(x + 1, xx - 2);
                    workingMem = "> subCmd generated - " + cmd;
                    if (debugMode) Console.WriteLine(workingMem);
                    line = line.Replace(line.Substring(x, xx), Interpret(cmd, true));
                    //if (subCmd) return line;
                }
                if (!line.StartsWith("MathEngine"))
                {
                    while (line.Contains('¬'))
                    {
                        int x = 0;
                        while (x < line.Length)
                        {
                            if (line[x] == '¬')
                            {
                                int xx = 1;
                                while (line[x + xx] != '¬') xx++;
                                xx++;
                                string varName = line.Substring(x, xx);
                                line = line.Replace(varName, VarMan.GetVar(varName.Replace("¬", "")));
                                workingMem = line;
                            }
                            x++;
                        }
                    }
                }

                string keyword = line.Split(' ')[0];
                if (debugMode && subCmd) Console.WriteLine("Running subCMD - " + line);
                keyword = keyword.ToLower();
                switch (keyword)
                {
                    case "end":
                        Environment.Exit(0);
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "goto":
                        string dest = line.Split(' ')[1];
                        if (dest.StartsWith(">"))
                        {
                            int oldIndex = index;
                            index = 0;
                            while (index < lines.Length && lines[index].ToLower().Replace(" ", "") != dest.ToLower().Replace(" ", "")) index++;
                            if (lines[index].ToLower().Replace(" ", "") == dest.ToLower().Replace(" ", "")) return "";
                            else
                            {
                                index = oldIndex;
                                KernelErrors.UndefinedGotoPoint();
                            }
                        }
                        else index = Convert.ToInt32(dest) - 1;
                        break;
                    case "print":
                        workingMem = "";
                        Console.Write(line[6..]);
                        break;
                    case "println":
                        workingMem = "";
                        Console.WriteLine(line[8..]);
                        break;
                    case "sleep":
                        System.Threading.Thread.Sleep(Convert.ToInt32(line[6..]));
                        break;
                    case "pause":
                        System.Threading.Thread.Sleep(Convert.ToInt32(line[6..]));
                        break;
                    case "wait":
                        System.Threading.Thread.Sleep(Convert.ToInt32(line[5..]));
                        break;
                    case "newvar":
                        workingMem = "";
                        if (line.Split(' ').Length == 2) VarMan.NewVar(line.Split(' ')[1], "");
                        else
                        {
                            string remove = "newVar " + line.Split(' ')[1] + " ";
                            VarMan.NewVar(line.Split(' ')[1], line.Replace(remove, ""));
                        }
                        break;
                    case "delvar":
                        workingMem = "";
                        VarMan.DelVar(line.Split(' ')[1]);
                        break;
                    case "setvar":
                        workingMem = "";
                        if (line.Split(' ').Length == 2) VarMan.SetVar(line.Split(' ')[1], "");
                        else
                        {
                            string remove = "setVar " + line.Split(' ')[1] + " ";
                            VarMan.SetVar(line.Split(' ')[1], line.Replace(remove, ""));
                        }
                        break;
                    case "var": 
                        workingMem = VarMan.VarMgr(line[4..]);
                        break;
                    case "array": 
                        workingMem = ArrayMan.ArrayMgr(line[6..]);
                        break;
                    case "http":
                        workingMem = HttpMan.Go(line[5..]);
                        break;
                    case "udp":
                        workingMem = UdpMan.Go(line[4..]);
                        break;
                    case "math":
                        workingMem = Convert.ToString(MathEngine.SimpleString(line.Replace("math ", "")));
                        break;
                    case "MathEngine":
                        workingMem = Convert.ToString(MathEngine.SimpleString(line.Replace("MathEngine ", ""), false));
                        break;
                    case "read":
                        workingMem = Console.ReadLine();
                        break;
                    case "mkdir":
                        if (readOnlyFS) { FileErrors.ReadOnlyFS(); break; }
                        if (debugMode) Console.WriteLine($"mkdir {line[6..]}");
                        if (!Directory.Exists(line[6..])) Directory.CreateDirectory(line[6..]);
                        else FileErrors.FileObjectAlreadyExists(line[6..]);
                        break;
                    case "rmdir":
                        if (readOnlyFS) { FileErrors.ReadOnlyFS(); break; }
                        if (debugMode) Console.WriteLine($"rmdir {line[6..]}");
                        if (!Directory.Exists(line[6..])) FileErrors.ObjectDoesNotExist(line[6..]);
                        else if (Directory.GetFiles(line[6..]).Length + Directory.GetDirectories(line[6..]).Length == 0) Directory.Delete(line[6..]);
                        else FileErrors.DirectoryNotEmpty(line[6..]);
                        break;
                    case "mvdir":
                    case "rndir":
                        if (readOnlyFS) { FileErrors.ReadOnlyFS(); break; }
                        string oldPath = "";
                        bool firstHadQuotes = false;
                        string newPath = "";
                        if (!line.Split(' ')[1].StartsWith(' ')) oldPath = line.Split(' ')[1];
                        else
                        {
                            firstHadQuotes = true;
                            int n = 7;
                            while (line[n] != '\"')
                            {
                                oldPath += line[n];
                                n++;
                            }
                        }
                        if (!firstHadQuotes)
                        {
                            if (!line.Split(' ')[2].StartsWith(" ")) newPath = line.Split(" ")[2];
                            else
                            {
                                FileErrors.NotYetImplemented("It is not yet supported for the second path in RNDIR commands to be quote-encapsulated");
                                break;
                            }
                        }
                        else { FileErrors.NotYetImplemented("It is not yet supported for the first path in RNDIR commands to be quote-encapsulated"); break; }

                        if (Directory.Exists(oldPath) && !Directory.Exists(newPath)) Directory.Move(oldPath, newPath);
                        else if (Directory.Exists(newPath)) FileErrors.FileObjectAlreadyExists(newPath, $"MV {oldPath} > {newPath}");
                        else FileErrors.FileObjectNotFound(oldPath, $"MV {oldPath} > {newPath}");
                        break;
                    case "random":
                        return Convert.ToString(sysRand.Next(Convert.ToInt32(line.Split(' ')[1]), Convert.ToInt32(line.Split(' ')[2]) + 1));
                    case "simplereadfile":
                        string simpleReadPath = line[15..].Replace("`_", " "); //(add ability to encapsulate inputs in quotemarks instead)
                        if (debugMode) Console.WriteLine($"Reading from {simpleReadPath}");
                        if (File.Exists(simpleReadPath))
                        {
                            string content = File.ReadAllText(simpleReadPath);
                            while (content.StartsWith(" ")) content = content[1..];
                            return content;
                        }
                        else FileErrors.FileNotFound(line[15..]);
                        break;
                    case "simplewritefile":
                        if (readOnlyFS) { FileErrors.ReadOnlyFS(); break; }
                        string simpleWritePath = line.Split(' ')[1];
                        simpleWritePath = simpleWritePath.Replace("`_", " "); //(add ability to encapsulate inputs in quotemarks instead)
                        if (debugMode) Console.WriteLine($"Writing to {simpleWritePath}");
                        File.WriteAllText(simpleWritePath, line[(simpleWritePath.Replace(" ", "`_").Length + 16 + 1)..]);
                        break;
                    case "file":
                        workingMem = FileMgr.Go(line[5..]);
                        break;
                    case "if":
                        string onTrue = line.Split(' ')[^1];
                        if (debugMode) Console.WriteLine($"Evaluating {line.Replace($" {onTrue}", "")[3..]}");

                        string equation = line.Replace($" {onTrue}", "")[3..];


                        //string[] parts = line.Replace($" {onTrue}", "")[3..].Split(' ');
                        bool result = Conditionals.MultiProcess(equation);
                        if (result) Interpret($"goto {onTrue}");
                        break;
                    case "string":
                        return (StringUtils.Go(line[7..]));
                    case "clear":
                        Console.Clear();
                        break;
                    case "sysconfig":
                        workingMem = line.Split(' ')[2].ToLower();
                        bool val = false;
                        if (workingMem.ToLower() == "true") val = true;

                        switch (line.Split(' ')[1].ToLower())
                        {
                            case "debug":
                                if (val) debugMode = true;
                                else debugMode = false;
                                break;
                            case "breakonerror":
                                if (val) breakOnError = true;
                                else breakOnError = false;
                                break;
                            case "printerrors":
                                if (val) printErrors = true;
                                else printErrors = false;
                                break;
                            case "readonlyfs":
                            case "readonly":
                                if (val) readOnlyFS = true;
                                else KernelErrors.InvalidSysVarSet();
                                break;
                            default:
                                KernelErrors.UnknownSysVar();
                                break;
                        }
                        workingMem = "";
                        break;
                    default:
                        KernelErrors.UnknownCommand();
                        break;
                }
            }
            catch (Exception e)
            {
                KernelErrors.Generic(e, "workingMem: " + workingMem);
            }
            return workingMem;
        }
    }
}
