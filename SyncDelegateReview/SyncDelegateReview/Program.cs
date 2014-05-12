using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;


//Working with the Asynchronous nature of delegates.

namespace SyncDelegateReview
{

    public delegate int BinaryOp(int x, int y);

    class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("***** Sync Delegate Review *****");

            //Print out the id of the executing thread.
            Console.WriteLine("Main() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            /*Here we are invoking the Add() function in a asynchronous manner.
             We do this by first caching the IAsyncResult compatible object returned by BeginInvoke()
             in an IAsyncResult variable.
             We then pass that object to 'EndInvoke()' (see a couple lines down) when we ready to obtain the result of the method invocation.*/
            BinaryOp b = new BinaryOp(Add);
            IAsyncResult iftAR = b.BeginInvoke(10, 10, new AsyncCallback(AddComplete), "Main() thanks you for adding these numbers.");
            
            //Do other work on primary thread. This method will keep printing until the Add() method is finished.
            /*Using the 'IsCompleted' property of the IAsyncResult variable, the calling thread (Main()) is able to 
             determine whether the asynchronous call has indeed completed before calling EndInvoke().
             If the method (Add()) has not completed, IsCompleted returns false and the calling thread(Main()) is
             free to carry on its work.  If IsCompleted is true, the calling thread is able to obtain the result in the
             least blocking manner as possible*/
            while (!iftAR.AsyncWaitHandle.WaitOne(1000,true))   //replace !iftAR.IsCompleted
            {
                Console.WriteLine("Working...");
            }

            Console.ReadLine();
        }

        static int Add(int x, int y)
        {
            //Print out the ID of the executing thread.
            Console.WriteLine("Add() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            //Pause to simulate a lengthy operation
            Thread.Sleep(5000);
            return x+y;
        }

        //this will by invoked by the AsyncCallback delegate when the Add() method has completed.
        static void AddComplete(IAsyncResult itfAR)
        {
            Console.WriteLine("AddComplete() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Your addition is complete.");

            //Now get the result
            AsyncResult ar = (AsyncResult)itfAR;
            BinaryOp b = (BinaryOp)ar.AsyncDelegate;
            Console.WriteLine("10 + 10 is {0}.", b.EndInvoke(itfAR));

            //Retrieve the informational object and cast it to a string.
            string msg = (string)itfAR.AsyncState;
            Console.WriteLine(msg);
        }
    }
}
