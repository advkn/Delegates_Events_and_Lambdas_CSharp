using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDelegate
{

    /*THis delegae can point to any method, taking two integers and returning an integer.*/
    public delegate int BinaryOp(int x, int y);

    //THis class contains methods BinaryOp will point to.
    public class SimpleMath {

        public int Add(int x, int y) {
            return x + y;
        }

        public int Subtract(int x, int y)
        {
            return x - y;
        }
    }



    class Program
    {
        static void DisplayDelegateInfo(Delegate delObj) {
            //Print the names of each member in the delegate's invocation list.
            foreach (Delegate d in delObj.GetInvocationList())
            {
                Console.WriteLine("Method name: {0}", d.Method);
                Console.WriteLine("Type name: {0}", d.Target);
            }
        }

        static void Main(string[] args)
        {

            Console.WriteLine("***** Simple delegate example *****\n");

            //Create BinaryOp delegate that points to "SimpleMath.Add". This version is for static methods.
            //BinaryOp b = new BinaryOp(SimpleMath.Add);

            //.NET delegates can also point to instance methods as well.
            SimpleMath m = new SimpleMath();
            BinaryOp b = new BinaryOp(m.Add);

            //Invoke Add() method indirectly using delegate object.
            Console.WriteLine("10 + 10 is {0} \n\n", b(10, 10));

            DisplayDelegateInfo(b);
            Console.ReadLine();
        }
    }
}
