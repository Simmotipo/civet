using System;
using System.Collections.Generic;
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
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
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
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++)
                    {
                        if (fileObjects[i] == name)
                        {
                            fileObjects[i] = cmd[(name.Length + 1 + keyword.Length + 1)..];
                        }
                    }
                    break;
                case "readtext":
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) return files[i].ReadText();
                    FileErrors.Generic(name);
                    break;
                case "readtextline":
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) return files[i].ReadTextLine(Convert.ToInt32(cmd.Split(' ')[^1]));
                    FileErrors.Generic(name);
                    break;
                case "deletefile":
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].DeleteFile();
                    break;
                case "writetext":
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].WriteText(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "appendtext":
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].AppendText(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "copyto":
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].CopyTo(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "moveto":
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].MoveTo(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                    break;
                case "renamefile":
                    if (!fileObjects.Contains(name)) { FileErrors.ObjectDoesNotExist(name); return ""; }
                    for (int i = 0; i < fileObjects.Count; i++) if (fileObjects[i] == name) files[i].Rename(cmd[(name.Length + 1 + keyword.Length + 1)..]);
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
