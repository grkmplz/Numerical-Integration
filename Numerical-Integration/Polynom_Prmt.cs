using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Numeral_Integration
{
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
            if(yesnovalue)
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
            if(yesnovalue)
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
            foreach(string s in inputStr.Split(' '))
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
