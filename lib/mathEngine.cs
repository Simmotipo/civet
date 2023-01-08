using System;

namespace civet
{
    class MathEngine
    {
        public static double SimpleMath(double a, double b, char operation)
        {
            switch (operation)
            {
                case '+':
                    return a + b;
                case '*':
                    return a * b;
                case '/':
                    return a / b;
                case '-':
                    return a - b;
                case '^':
                    return Math.Pow(a, b);
                case '%':
                    return a % b;
                default:
                    MathErrors.InvalidOperation(Program.workingMem);
                    return -1;
            }
        }

        public static double Sum(double[] nums)
        {
            double n = 0;
            foreach (double num in nums) n += num;
            return n;
        }

        public static double SimpleString(string input, bool variablesParsed = true)
        {
            while (input.EndsWith(" ")) input = input[0..^1];
            double result = 0;
            try
            {
                if (!variablesParsed)
                {
                    while (input.Contains('¬'))
                    {
                        int x = 0;
                        while (x < input.Length)
                        {
                            if (input[x] == '¬')
                            {
                                int xx = 1;
                                while (input[x + xx] != '¬') xx++;
                                xx++;
                                string varName = input.Substring(x, xx);
                                input = input.Replace(varName, Convert.ToString(VarMan.GetVar(varName.Replace("¬", ""), true)));
                            }
                            x++;
                        }
                    }
                }

                string[] keys = input.Split(' ');

                bool firstCycle = true;

                int i = 0;
                while (i < keys.Length - 1)
                {
                    double n1 = result;
                    double n2 = Convert.ToDouble(keys[i + 2]); ;
                    if (firstCycle) n1 = Convert.ToDouble(keys[i]);

                    switch (keys[i+1])
                    {
                        case "+":
                            result = n1 + n2;
                            break;
                        case "*":
                            result = n1 * n2;
                            break;
                        case "/":
                            if (n2 == 0) { MathErrors.DivideByZeroException(input); return -1; }
                            result = n1 / n2;
                            break;
                        case "-":
                            result = n1 - n2;
                            break;
                        case "^":
                            result = Math.Pow(n1, n2);
                            break;
                        case "%":
                            result = n1%n2;
                            break;
                        default:
                            MathErrors.InvalidOperation(input);
                            return -1;
                    }

                    if (firstCycle) firstCycle = false;
                    i += 2;
                }

                return result;
            }
            catch (Exception e)
            {
                MathErrors.UnhandledMathError(e, $"{input} => {result}");
                return -1;
            }
        }

        public static double BodmasString(string input)
        {
            MathErrors.UnimplementedFunction($"Input: {input}");
            return -1;
        }
    }
}
