using System;
using System.Collections.Generic;
using System.Text;

namespace civet
{

    //Need to implement arrays
    //array <name> Replace <oldValue> <newValue> -- Replaces first occurance of <oldValue>
    //array <name> ReplaceAll <oldValue> <newValue> -- Replaces all occurances of <oldValue>

    class ArrayMan
    {
        static readonly List<Array> arrays = new List<Array>();
        static readonly List<string> arrayNames = new List<string>();
        public static string ArrayMgr(string cmd)
        {
            string name = cmd.Split(' ')[0];
            string keyword = cmd.Split(' ')[1].ToLower();

            //string value = "";

            switch (keyword)
            {
                case "create":
                    if (arrayNames.Contains(name)) { VarErrors.VariableAlreadyExists(name); return ""; }
                    else { 
                        arrayNames.Add(name);
                        Array a = new Array
                        {
                            name = name
                        }; arrays.Add(a);
                    }
                    break;
                case "delete":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return ""; }
                    for (int i = 0; i <arrays.Count; i++)
                    {
                        if (arrayNames[i] == name) { arrayNames.RemoveAt(i); arrays.RemoveAt(i); return ""; }
                    }
                    break;
                case "add":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return ""; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name)
                        {
                            arrays[i].AddValue(cmd[(name.Length + 1 + keyword.Length + 1)..]);
                        }
                    }
                    break;
                case "contains":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return ""; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name)
                        {
                            if (arrays[i].GetValues().Contains(cmd[(name.Length + 1 + keyword.Length + 1)..])) return "true";
                            else return "false";
                        }
                    }
                    break;
                case "count":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return ""; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name) return Convert.ToString(arrays[i].GetValues().Count);
                    }
                    break;
                case "exclusiveadd":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return ""; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name)
                        {
                            arrays[i].AddValue(cmd[(name.Length + 1 + keyword.Length + 1)..], true);
                        }
                    }
                    break;
                case "indexof":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return "-1"; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name)
                        {
                            return Convert.ToString(arrays[i].IndexOf(cmd[(name.Length + 1 + keyword.Length + 1)..]));
                        }
                    }
                    break;
                case "remove":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return "-1"; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name) { arrays[i].Remove(cmd[(name.Length + 1 + keyword.Length + 1)..]); break; }
                    }
                    break;
                case "removeall":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return "-1"; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name) { arrays[i].RemoveAll(cmd[(name.Length + 1 + keyword.Length + 1)..]); break; }
                    }
                    break;
                case "removeat":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return "-1"; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name) { arrays[i].RemoveAt(Convert.ToInt32(cmd[(name.Length + 1 + keyword.Length + 1)..])); break; }
                    }
                    break;
                case "get":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return "-1"; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name)
                        {
                            return arrays[i].Get(Convert.ToInt32(cmd[(name.Length + 1 + keyword.Length + 1)..]));
                        }
                    }
                    break;
                case "set":
                    if (!arrayNames.Contains(name)) { VarErrors.VariableDoesntExist(name); return "-1"; }
                    for (int i = 0; i < arrays.Count; i++)
                    {
                        if (arrayNames[i] == name) {

                            arrays[i].Set(Convert.ToInt32(cmd.Split(' ')[2]), cmd[(name.Length + 1 + keyword.Length + 1 + Convert.ToInt32(cmd.Split(' ')[2].Length + 1))..]);
                        }
                    }

                    break;
                case "renamearray":
                    if (arrayNames.Contains(name))
                    {
                        int n = 0;
                        while (arrayNames[n] != name) n++;
                        arrayNames[n] = cmd[(name.Length + 1 + keyword.Length + 1)..];
                        arrays[n].name = cmd[(name.Length + 1 + keyword.Length + 1)..];
                    }
                    else
                    {
                        VarErrors.VariableDoesntExist(name);
                    }
                    break;
                default:
                    VarErrors.UnknownVariableCommand(keyword);
                    break;
            }
            return "";
        }
    }

    class Array
    {
        static readonly List<Variable> values = new List<Variable>();
        public string name = "";

        public void AddValue(string value, bool exclusive = false)
        {
            bool addable = true;
            if (exclusive)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i].value == value) { addable = false; break; }
                }
            }
            if (addable)
            {
                Variable v = new Variable("n", value);
                values.Add(v);
            }
        }

        public List<string> GetValues()
        {
            List<string> vals = new List<string>();
            foreach (Variable v in values) vals.Add(v.value);
            return vals;
        }

        public int IndexOf(string value)
        {
            for (int i = 0; i <values.Count; i++)
            {
                if (values[i].value == value) { return i; }
            }
            return -1;
        }

        public void Replace(string oldVal, string newVal)
        {
            int i = IndexOf(oldVal);
            if (i == -1) VarErrors.ValueNotFound(name, oldVal);
            else values[i].value = newVal;
        }

        public void ReplaceAll(string oldVal, string newVal)
        {
            int i = 0;
            while (i != -1)
            {
                i = IndexOf(oldVal);
                if (i != -1) values[i].value = newVal;
            }
        }

        public void Remove(string value)
        {
            int i = IndexOf(value);
            if (i == -1) VarErrors.ValueNotFound(name, value);
            else values.RemoveAt(i);
        }

        public void RemoveAll(string value)
        {
            int i = 0;
            while (i != -1)
            {
                i = IndexOf(value);
                if (i != -1) values.RemoveAt(i);
            }
        }

        public void RemoveAt(int index)
        {
            if (values.Count <= index) VarErrors.BoundsOutsideOfArray(name, Convert.ToString(index));
            else values.RemoveAt(index);
        }

        public string Get(int index)
        {
            if (values.Count <= index) { VarErrors.BoundsOutsideOfArray(name, Convert.ToString(index)); return ""; }
            return values[index].value;
        }

        public void Set(int index, string value)
        {
            if (values.Count <= index) VarErrors.BoundsOutsideOfArray(name, Convert.ToString(index));
            else
            {
                values[index].value = value;
            }
        }
    }

    class VarMan
    {
        static readonly List<Variable> vars = new List<Variable>();
        static readonly List<string> varNames = new List<string>();

        public static string VarMgr(string cmd)
        {
            string name = cmd.Split(' ')[0];
            string keyword = cmd.Split(' ')[1];

            string value = "";

            switch (keyword)
            {
                case "create":
                    if (cmd.Split(' ').Length > 2) value = cmd[(name.Length + 1 + keyword.Length + 1)..];
                    NewVar(name, value);
                    break;
                case "set":
                    value = cmd[(name.Length + 1 + keyword.Length + 1)..];
                    SetVar(name, value);
                    break;
                case "delete":
                    DelVar(name);
                    break;
                case "get":
                    return GetVar(name);
                case "renameVar":
                    if (varNames.Contains(name))
                    {
                        int n = 0;
                        while (varNames[n] != name) n++;
                        varNames[n] = cmd[(name.Length + 1 + keyword.Length + 1)..];
                    }
                    else
                    {
                        VarErrors.VariableDoesntExist(name);
                    }
                    break;
                default:
                    VarErrors.UnknownVariableCommand(keyword);
                    break;
            }
            return "";
        }

        public static void NewVar(string name, string value)
        {
            if (varNames.Contains(name)) VarErrors.VariableAlreadyExists(name);
            else if (name.Contains("#")) VarErrors.InvalidVariableName(name);
            else
            {
                varNames.Add(name);
                Variable v = new Variable(name, value);
                vars.Add(v);
            }
        }


        public static void SetVar(string name, string value)
        {
            if (varNames.Contains(name))
            {
                int n = 0;
                while (varNames[n] != name) n++;
                vars[n].Init(name, value);
            }
            else
            {
                VarErrors.VariableDoesntExist(name);
            }
        }

        public static void DelVar(string name)
        {
            if (varNames.Contains(name))
            {
                try
                {
                    int n = 0;
                    while (varNames[n] != name) n++;
                    varNames.RemoveAt(n);
                    vars.RemoveAt(n);
                }
                catch
                {
                    VarErrors.VariableNotDisposed(name);
                }
            }
            else
            {
                VarErrors.VariableDoesntExist(name);
            }
        }

        //These should support arrays... forexample ¬arrayName#3¬ -- look for # in variable names to indicate this. (Same function as array <name> get <index> where ¬<name>#><index>¬)
        public static string GetVar(string name)
        {
            if (varNames.Contains(name))
            {
                int n = 0;
                while (varNames[n] != name) n++;
                return vars[n].value;
            }
            else
            {
                VarErrors.VariableDoesntExist(name);
                return "";
            }
        }

        public static double GetVar(string name, bool asInt)
        {
            if (!asInt) KernelErrors.KernelPanic(null);
            if (varNames.Contains(name))
            {
                int n = 0;
                while (varNames[n] != name) n++;
                if (vars[n].Numerable) return Convert.ToDouble(vars[n].value);
                else
                {
                    VarErrors.VariableNotNumerable(name, vars[n].value);
                    return -1;
                }
            }
            else
            {
                VarErrors.VariableDoesntExist(name);
                return -1;
            }
        }
    }


    class Variable
    {
        public string name;
        public string value;

        public bool Numerable { get; private set; }

        public Variable(string n, string v) //Think we can remove string n
        {
            try
            {
                name = n;
                value = v;
                try
                {
                    Convert.ToDouble(v);
                    Numerable = true;
                }
                catch
                {
                    Numerable = false;
                }
            }
            catch
            {
            }
        }

        public void Init(string n, string v) //Think we can remove string n
        {
            try
            {
                name = n;
                value = v;
                try
                {
                    Convert.ToDouble(v);
                    Numerable = true;
                }
                catch
                {
                    Numerable = false;
                }
            }
            catch
            {
            }
        }
    }
}
