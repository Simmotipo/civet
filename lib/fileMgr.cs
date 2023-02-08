using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace civet
{
    //All of the below must support quotemark-encapsulation for inputs!!
    //file <fileObjectName> filePath <filePath>
    //file <fileObjectName> readBytes
    //file <fileObjectName> readText
    //file <fileObjectName> readTextLine
    //file <fileObjectName> writeText <content>

    class FileMgr
    {
        static readonly List<FileObject> files = new List<FileObject>();
        static readonly List<string> fileObjects = new List<string>();
        public static string Go(string cmd)
        {
            string name = cmd.Split(' ')[0];
            string keyword = cmd.Split(' ')[1].ToLower();

            if (keyword != "filepath" && !fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
            switch (keyword)
            {
                case "filepath":
                    if (fileObjects.Contains(name)) { FileErrors.FileObjectAlreadyExists(name); return ""; }
                    FileObject f = new FileObject
                    {
                        fPath = cmd[(name.Length + 1 + keyword.Length + 1)..],
                        name = name
                    };
                    fileObjects.Add(name);
                    files.Add(f);
                    break;
                case "deletefileobject":
                    for (int i = 0; i < fileObjects.Count; i++)
                    {
                        if (fileObjects[i] == name)
                        {
                            fileObjects.RemoveAt(i);
                            files.RemoveAt(i);
                        }
                    }
                    break;
                case "renamefileobject":
                    for (int i = 0; i < fileObjects.Count; i++)
                    {
                        if (fileObjects[i] == name)
                        {
                            fileObjects[i] = cmd[(name.Length + 1 + keyword.Length + 1)..];
                        }
                    }
                    break;
                case "readtext": //Update these to use List<FileObject>.GetIndex()
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) return files[i].ReadText();
                    FileErrors.Generic(name);
                    break;
                case "readtextline":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) return files[i].ReadTextLine(Convert.ToInt32(cmd.Split(' ')[^1]));
                    FileErrors.Generic(name);
                    break;
                case "deletefile":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].DeleteFile();
                    break;
                case "writetext":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].WriteText(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "appendtext":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].AppendText(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "copyto":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].CopyTo(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "moveto":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].MoveTo(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "renamefile":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].Rename(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "readbytesasint":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].ReadBytesToArray(cmd.Split(' ')[2], "int");
                    break;
                case "readbytesashex":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].ReadBytesToArray(cmd.Split(' ')[2], "hex");
                    break;
                case "readbytesaschar":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].ReadBytesToArray(cmd.Split(' ')[2], "char");
                    break;
                case "readbytes":
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].ReadBytesToArray(cmd.Split(' ')[2], cmd.Split(' ')[3]);
                    break;
                default:
                    FileErrors.UnknownFileManagerInstruction(keyword);
                    break;
            }

            return "";
        }
    }

    class FileObject
    {
        public string fPath = "";
        public string name = "";

        public void ReadBytesToArray(string arrayName, string format)
        {
            byte[] bytes = File.ReadAllBytes(fPath);
            if (format == "int")
            {
                int[] ints = new int[bytes.Length];
                for (int i = 0; i < bytes.Length; i++) ints[i] = Convert.ToInt32(bytes[i]);
                ArrayMan.ArrayMgr($"{arrayName} create");
                for (int i = 0; i < ints.Length; i++) ArrayMan.ArrayMgr($"{arrayName} add {ints[i]}");
            }
            else if (format == "char")
            {
                char[] chars = new char[bytes.Length];
                for (int i = 0; i < bytes.Length; i++) chars[i] = (char)bytes[i];
                ArrayMan.ArrayMgr($"{arrayName} create");
                for (int i = 0; i < chars.Length; i++) ArrayMan.ArrayMgr($"{arrayName} add {chars[i]}");
            }
            else if (format == "hex")
            {
                string[] hexes = new string[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    byte[] tHex = new byte[1];
                    tHex[0] = bytes[i];
                    hexes[i] = Convert.ToHexString(tHex);
                }
                ArrayMan.ArrayMgr($"{arrayName} create");
                for (int i = 0; i < hexes.Length; i++) ArrayMan.ArrayMgr($"{arrayName} add {hexes[i]}");
            }

        }

        public void Rename(string newName)
        {
            if (newName.Contains("\\") || newName.Contains("/")) MoveTo(newName);
            else
            {
                string newFileName = fPath.Substring(0, fPath.Length - fPath.Split("/")[^1].Length);
                if (!newFileName.EndsWith("/")) newFileName += "/";
                newFileName += newName;
                MoveTo(newFileName);
            }
        }

        public void CopyTo (string newFilePath)
        {
            try
            {
                File.Copy(fPath, newFilePath);
            } catch (Exception e)
            {
                FileErrors.Generic(e.ToString());
            }
        }

        public void MoveTo(string newFilePath)
        {
            try
            {
                File.Move(fPath, newFilePath);
            }
            catch (Exception e)
            {
                FileErrors.Generic(e.ToString());
            }
        }

        public string ReadText()
        {
            try
            {
                return File.ReadAllText(fPath);
            }
            catch
            {
                FileErrors.FileNotFound(name, fPath);
                return "";
            }
        }
        public string ReadTextLine(int index)
        {
            try
            {
                return File.ReadAllLines(fPath)[index];
            }
            catch
            {
                if (!File.Exists(fPath)) FileErrors.FileNotFound(name, fPath);
                else
                {
                    FileErrors.Generic($"{fPath} @ {index}");
                }
                return "";
            }
        }
        public void WriteText(string content)
        {
            try
            {
                File.WriteAllText(fPath, content);
            }
            catch (Exception e)
            {
                FileErrors.Generic(e.ToString());
            }
        }
        public void AppendText(string content)
        {
            try
            {
                File.AppendAllText(fPath, content);
            }
            catch (Exception e)
            {
                FileErrors.Generic(e.ToString());
            }
        }
        public void DeleteFile()
        {
            try
            {
                File.Delete(fPath);
            }
            catch (Exception e)
            {
                FileErrors.Generic(e.ToString());
            }
        }
    }
}
