using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace civet
{
    static class StringErrors
    {

        public static void Generic(string details)
        {

            Console.WriteLine($"ERROR 0x06: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Generic StringUtils error. \n] Additional Details: {details}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void ReplaceError(string input, string details = "")
        {

            Console.WriteLine($"ERROR 0x07: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Error performing replace command {input} \n] Additional Details: {details}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }

    static class FileErrors
    {
        public static void FileNotFound(string name, string details = "")
        {
            Console.WriteLine($"ERROR 0x08: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Unable to find file at specified path of fileObject {name}.\n] {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void FileObjectNotFound(string name, string details = "")
        {
            Console.WriteLine($"ERROR 0x09: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] No fileObject exists with the name {name}.\n] {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void FileObjectAlreadyExists(string name, string details = "")
        {
            Console.WriteLine($"ERROR 0x0A: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] fileObject already exists with the name {name}.\n] {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void Generic(string details)
        {
            Console.WriteLine($"ERROR 0x0B: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Generic fileObject Error.\n] Additional Details: {details}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void ObjectDoesNotExist(string name)
        {
            Console.WriteLine($"ERROR 0x0C: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] No fileObject exists with the name {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void DirectoryNotEmpty(string name, string details = "")
        {
            Console.WriteLine($"ERROR 0x0D: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Requested delete directory {name} is not empty.\n] {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void NotYetImplemented(string details)
        {
            Console.WriteLine($"ERROR 0x0E: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Requested operation is recognised but not yet implemented.\n] {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void ReadOnlyFS(string details)
        {
            Console.WriteLine($"ERROR 0x10: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Write operation requested in read-only program.\n] {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void UnknownFileManagerInstruction(string input)
        {
            Console.WriteLine($"ERROR 0x0F: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Unknown File Manager command {input}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }

    static class VarErrors
    {
        public static void VariableAlreadyExists(string name)
        {
            Console.WriteLine($"ERROR 0xA1: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Variable already exists with the name {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void VariableDoesntExist(string name)
        {
            Console.WriteLine($"ERROR 0xA2: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] No variable exists with the name {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void VariableInitFailed(string name, string value)
        {
            Console.WriteLine($"ERROR 0xA3: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Variable with name {name} and value {value} failed to initialise.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void VariableNotNumerable(string name, string value)
        {
            Console.WriteLine($"ERROR 0xA4: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Variable {name} has non-numerable value {value}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void BoundsOutsideOfArray(string name, string value)
        {
            Console.WriteLine($"ERROR 0xA8: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Request index {value} is outside the bounds of array {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void ValueNotFound(string name, string value)
        {
            Console.WriteLine($"ERROR 0xA9: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Value {value} not found in array {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void VariableNotDisposed(string name)
        {
            Console.WriteLine($"ERROR 0xA5: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Failed to dispose variable {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void InvalidVariableName(string name)
        {
            Console.WriteLine($"ERROR 0xA6: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Invalid variable name, {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void UnknownVariableCommand(string keyword)
        {
            Console.WriteLine($"ERROR 0xA7: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Unknown variable command {keyword}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void Generic(Exception e, string details = "")
        {

            Console.WriteLine($"ERROR 0xA0: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Generic variable error. \n] {e}\n] Additional Details: {details}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }


    static class HttpErrors
    {
        public static void ServerAlreadyExists(string type, string name)
        {
            Console.WriteLine($"ERROR 0xB1: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Server of type {type} already exists on port {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void ServerDoesntExist(string type, string name)
        {
            Console.WriteLine($"ERROR 0xB2: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] No server of type {type} exists on port {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void ServerInitFailed(string type, string name)
        {
            Console.WriteLine($"ERROR 0xB3: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Server of type {type} failed to initialise on port {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void ServerPathAlreadyRegistered(string name, string path)
        {
            Console.WriteLine($"ERROR 0xB4: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Server on port {name} already has a registered GoPoint for path {path}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void ServerNotDisposed(string type, string name)
        {
            Console.WriteLine($"ERROR 0xB5: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Failed to dispose server of type {type} from port {name}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void ServerPathNotRegistered(string name, string path)
        {
            Console.WriteLine($"ERROR 0xB6: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Server on port {name} does not have a registered GoPoint to remove for path {path}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void UnknownServerCommand(string keyword)
        {
            Console.WriteLine($"ERROR 0xB7: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Unknown Server command {keyword}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void Generic(Exception e, string details = "")
        {

            Console.WriteLine($"ERROR 0xB0: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Generic HttpServer error. \n] {e}\n] Additional Details: {details}.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }

    static class KernelErrors
    {
        public static void Generic(Exception e, string details = "")
        {
            try
            {
                if (Program.printErrors) Console.WriteLine($"ERROR 0x00: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Generic Interpreter error. \n] {e}.\n] Additional Details: {details}");
                if (Program.breakOnError)
                {
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                KernelPanic(ex, e);
            }
        }

        public static void UnknownCommand()
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0x01: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Unknown command.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void UnknownSysVar()
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0x05: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Unknown SysVar Toggle.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void InvalidSysVarSet()
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0x11: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Invalid set operation on Sys Var.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void UnimplementedCommand()
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0x02: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Command not yet implemented.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void UndefinedGotoPoint()
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0x03: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Requested goto point does not exist.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void InvalidConditional()
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0x04: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Invalid conditionals.");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void KernelPanic(Exception kernelException, Exception sourceException = null, string details = null)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine("CIVET KERNEL PANIC!!\n\nUh oh, something went super wrong, and the kernel couldn't take it anymore... here's all the info I could get for you:\n");

            try { Console.WriteLine($"KERNEL EXCEPTION: {kernelException}"); } catch { }
            try { Console.WriteLine($"SOURCE EXCEPTION: {sourceException}"); } catch  { }
            try { Console.WriteLine($"INDEX AT EXCEPTION: {Program.index}"); } catch { }
            try { Console.WriteLine($"LAST PROCESSED LINE: {Program.currentLine}"); } catch { }
            try { Console.WriteLine($"WORKING MEMORY: {Program.workingMem}"); } catch { }
            try { Console.WriteLine($"\nAdditional Details: {details}"); } catch { }

            Console.ReadLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    static class MathErrors
    {
        public static void InvalidOperation(string details = "")
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0xFE: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Invalid operation.\n] Additional Details: {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void UnimplementedFunction(string details = "")
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0xFF: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Function not yet implemented.\n] Additional Details: {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void UnhandledMathError(Exception e, string details = "")
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0xFD: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Unhandled math error.\n] {e}\n] Additional Details: {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void DivideByZeroException(string details = "")
        {
            if (Program.printErrors) Console.WriteLine($"ERROR 0xFC: Line {Program.index}\n] {File.ReadAllLines(Program.filePath)[Program.index]}\n] Math attempted divide by zero.\n] Additional Details: {details}");
            if (Program.breakOnError)
            {
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
