using System;
using System.Collections.Generic;
using System.Text;

namespace Numeral_Integration
{
    class RPN_Calculator
    {
        public double Calculation(string getinput, double x)
        {
            int maxlengt, numbercounter = 0, counterOP = 0;
            double value1, value2, output;
            string[] ArrayList;
            var inputStack = new Stack<double>();

            if (string.IsNullOrWhiteSpace(getinput))
            {
                throw new Exception("No input given!!!");
            }

            ArrayList = getinput.Split(' ');
            maxlengt = ArrayList.Length;

            foreach (string stringInput in ArrayList)
            {
                if (string.IsNullOrEmpty(stringInput))
                {
                    continue;
                }
                if (double.TryParse(stringInput, out double num))
                {
                    inputStack.Push(Convert.ToDouble(stringInput));
                    numbercounter++;
                }
                else if(stringInput == "x")
                {
                    inputStack.Push(x);
                    numbercounter++;
                }
                else
                {
                    value2 = inputStack.Last.Data;
                    inputStack.Pop();
                    value1 = inputStack.Last.Data;
                    inputStack.Pop();

                    switch (stringInput)
                    {
                        case "+": output = value1 + value2; inputStack.Push(output); break;
                        case "-": output = value1 - value2; inputStack.Push(output); break;
                        case "*": output = value1 * value2; inputStack.Push(output); break;
                        case "/": output = value1 / value2; inputStack.Push(output); break;
                        case "^": output = Math.Pow(value1, value2); inputStack.Push(output); break;
                        default: Console.WriteLine("Unexpected operator (" + stringInput + ")"); Console.ReadLine(); break;
                    }
                    counterOP++;
                }
            }

            if (numbercounter - 1 > counterOP)
            {
                throw new Exception("Wrong equation: not enough operators");
            }

            return inputStack.Last.Data;
        }

        private int Precedence(char Operator)
        {
            switch (Operator)
            {
                case '+': return 2;
                case '-': return 2;
                case '*': return 3;
                case '/': return 3;
                case '^': return 4;
                case '(': return 1;
                default: throw new Exception($"Invalid operator exception ({Operator})");
            }
        }

        public string InfixToPostfix(string getinput)
        {
            int maxlengt;
            string result = "";
            string[] ArrayList;
            var inputStack = new Stack<char>();

            ArrayList = getinput.Split(' ');
            maxlengt = ArrayList.Length;

            if (string.IsNullOrWhiteSpace(getinput))
            {
                throw new Exception("No input given");
            }

            foreach (string stringInput in ArrayList)
            {
                if (double.TryParse(stringInput, out double num) || stringInput == "x")
                {
                    result += $"{stringInput} ";
                }
                else if (stringInput == "+" || stringInput == "-" || stringInput == "*" || stringInput == "/" || stringInput == "^")
                {
                    while (inputStack.Count != 0 && (Precedence(inputStack.Peek()) > Precedence(Convert.ToChar(stringInput)) || (Precedence(inputStack.Peek()) == Precedence(Convert.ToChar(stringInput)) && stringInput != "^")))
                    {
                        result += ($"{inputStack.Last.Data} ");
                        inputStack.Pop();
                    }
                    inputStack.Push(Convert.ToChar(stringInput));
                }
                else if (stringInput == "(")
                {
                    inputStack.Push(Convert.ToChar(stringInput));
                }
                else if (stringInput == ")")
                {
                    while (inputStack.Peek() != '(')
                    {
                        result += ($"{inputStack.Last.Data} ");
                        inputStack.Pop();
                    }
                    inputStack.Pop();
                }
                else
                {
                    throw new Exception($"Invalid operator '{stringInput}'");
                }
            }

            while (inputStack.Count > 0)
            {
                result += ($"{inputStack.Last.Data} ");
                inputStack.Pop();
            }

            result.Remove(result.Length - 1);
            return result;
        }

    }
}
