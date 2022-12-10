using System;
using System.Linq;

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
}
