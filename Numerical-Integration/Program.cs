using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Numeral_Integration
{
    /// Main program is here
    class Program
    {
        static void Main(string[] args)
        {
            string pilosrvt;
            do
            {
            Console.Clear();
            string getinput;
            double boundary_1, boundary_2;

            Console.WriteLine("You can type only numbers example (1 2 3 it will look like 1x^2 + 2x + 3).. or\n" +
                "Please write the coefficients of polynomial function example (x ^ 2 + 2 * x + 3):");
                Console.WriteLine("==>");
            getinput = Console.ReadLine();
            Polynom_Prmtrs polynom = new Polynom_Prmtrs(getinput);

            Console.WriteLine("The function you've entered:");
            Console.WriteLine(polynom.ToString());

            bool whileval = true;
            do         
            {
                Console.WriteLine("Please write the boundaries of integration: ");
             
                do
                {
                    Console.Write("a = ");
                    whileval = !double.TryParse(Console.ReadLine(), out boundary_1);
                } while (whileval);

                do
                {
                    Console.Write("b = ");
                    whileval = !double.TryParse(Console.ReadLine(), out boundary_2);
                } while (whileval);
                if(boundary_1 > boundary_2)
                {
                    Console.WriteLine("b must be greater than a");
                }
            } while (boundary_1 > boundary_2); 

            Console.WriteLine("Integration by trapezoidal method: " + polynom.Get_Trapeze(boundary_1, boundary_2, 100));
            Console.WriteLine("Integration by Simpson's method: " + polynom.Get_Simpson(boundary_1, boundary_2, 100));

                Console.WriteLine("type any to continue.. or exit");
                pilosrvt = Console.ReadLine();
            } while (pilosrvt != "exit");
        }
    }

    /// Stacks aisle is here ==>
    class Stack<T>
    {
        public StackNode<T> First { get; set; } 
        public StackNode<T> Last { get; set; }
        public int Count { get; set; }

        public Stack()
        {
            Count = 0;
        }

        public void Push(T data)
        {

            if (Count == 0)
            {
                First = new StackNode<T>(data, null);
                Last = new StackNode<T>(data, First);
            }
            else
            {
                Last = new StackNode<T>(data, Last);
            }
            Count++;
        }

        public void Pop()
        {
            Last = Last.Previous;
            Count--;
        }

        public T Peek()
        {
            return Last.Data;
        }

        public bool isEmpty()
        {
            return Count == 0;
        }

        public void Clear()
        {
            First = null;
            Last = null;
        }
    }
    class StackNode<T>
    {
        public T Data { get; set; }
        public StackNode<T> Previous { get; set; }
        public StackNode(T data, StackNode<T> previous)
        {
            Data = data;
            Previous = previous;
        }
    }

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
                else if (stringInput == "x")
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

    class Polynom_Prmtrs
    {
        private List<double> Parameters = new List<double>();
        private List<bool> ParameterSigns = new List<bool>();
        private string inputStr;
        private RPN_Calculator rpn = new RPN_Calculator();
        bool yesnovalue;

        public Polynom_Prmtrs(string input)
        {
            yesnovalue = CheckInput(input);
            inputStr = input;
            if (yesnovalue)
            {
                CreateLists();
            }
        }

        public double Solve(double x)
        {
            double answer = Parameters.Last();

            for (int i = 0; i < Parameters.Count() - 1; i++)
            {
                answer += Parameters[i] * Math.Pow(x, Parameters.Count() - 1 - i);
            }

            return answer;
        }
        public void ShowSolution(double x)
        {
            Console.WriteLine($"f({x}) = {Solve(x)}");
        }

        public override string ToString()
        {
            string function = "f(x) = ";
            if (yesnovalue)
            {
                string sign = ParameterSigns.First() ? "" : "-";

                function += $"{sign}{Math.Round(Parameters.First(), 5).ToString().Replace("-", "")}x^{Parameters.Count() - 1} ";

                for (int i = 1; i < Parameters.Count() - 1; i++)
                {
                    sign = ParameterSigns[i] ? "+" : "-";
                    function += $"{sign} {Math.Round(Parameters[i], 5).ToString().Replace("-", "")}x^{Parameters.Count() - 1 - i} ";
                }

                sign = ParameterSigns.Last() ? "+" : "-";
                function += $"{sign} {Math.Round(Parameters.Last(), 5).ToString().Replace("-", "")}";
            }
            else
            {
                function += inputStr;
            }
            return function;
        }
        public double Get_Trapeze(double a, double b, int n)
        {
            double h = (b - a) / n;
            double integral;
            if (yesnovalue)
            {
                integral = h / 2 * (Solve(a) + Solve(b));
                for (int i = 1; i < n; i++)
                {
                    integral += h * Solve(a + i * h);
                }
            }
            else
            {
                integral = h / 2 * (rpn.Calculation(rpn.InfixToPostfix(inputStr), a) + rpn.Calculation(rpn.InfixToPostfix(inputStr), b));
                for (int i = 1; i < n; i++)
                {
                    integral += h * rpn.Calculation(rpn.InfixToPostfix(inputStr), a + i * h);
                }
            }

            return integral;
        }
        public double Get_Simpson(double a, double b, int n)
        {
            double h = (b - a) / n;
            double integral = 0;
            if (yesnovalue)
            {
                integral = Solve(a) + Solve(b);
                for (int i = 1; i < n; i++)
                {
                    int coefficient = i % 2 == 0 ? 2 : 4;
                    integral += coefficient * Solve(a + i * h);
                }
            }
            else
            {
                integral = (rpn.Calculation(rpn.InfixToPostfix(inputStr), a) + rpn.Calculation(rpn.InfixToPostfix(inputStr), b));
                for (int i = 1; i < n; i++)
                {
                    int coefficient = i % 2 == 0 ? 2 : 4;
                    integral += coefficient * rpn.Calculation(rpn.InfixToPostfix(inputStr), a + i * h);
                }
            }
            integral *= h / 3;
            return integral;
        }

        public bool CheckInput(string input)
        {
            return input.Replace(" ", "").Replace("-", "").All(char.IsDigit);
        }

        private void CreateLists()
        {
            foreach (string s in inputStr.Split(' '))
            {
                Parameters.Add(double.Parse(s));
            }

            foreach (double num in Parameters)
            {
                ParameterSigns.Add(num >= 0);
            }
        }
    }

}
