using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Exploring delegates*/
namespace CarDelegate
{

    public class Car
    {
        //Internal state data
        public int CurrentSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public string PetName { get; set; }

        //is the car alive or dead?
        private bool carIsDead;

        //Class constructors
        public Car()
        {
            MaxSpeed = 100;
        }

        public Car(string name, int maxSp, int currSp)
        {
            CurrentSpeed = currSp;
            MaxSpeed = maxSp;
            PetName = name;
        }

        //Define a delegate type
        public delegate void CarEngineHandler(string msgForCaller);

        //Define a member variable of this delegate
        private CarEngineHandler listOfHandlers;

        //Add a registration function for the caller
        public void RegisterWithCarEngine(CarEngineHandler methodToCall)
        {
            listOfHandlers += methodToCall;
        }

        //Unregister a function from the caller
        public void UnregisterWithCarEngine(CarEngineHandler methodToCall)
        {
            listOfHandlers -= methodToCall;
        }

        public void Accelerate(int delta)
        {
            //If this car is 'dead', send message
            if (carIsDead)
            {
                if (listOfHandlers != null)
                {
                    listOfHandlers("Sorry, this car is dead...");
                }
            }
            else
            {
                CurrentSpeed += delta;

                //Is this car almost dead'?
                if (10 == (MaxSpeed - CurrentSpeed) && listOfHandlers != null)
                {
                    listOfHandlers("Careful buddy! Gonna blow!");
                }

                if (CurrentSpeed >= MaxSpeed)
                {
                    carIsDead = true;
                }
                else
                {
                    Console.WriteLine("Current speed = {0}", CurrentSpeed);
                }
            }
        }
    }

    class Program
    {

        //Target for incoming events
        public static void OnCarEngineEvent2(string msg)
        {
            Console.WriteLine("=> {0}", msg.ToUpper());
        }

        //Target for incoming events
        public static void OnCarEngineEvent(string msg)
        {
            Console.WriteLine("\n ***** Message from Car Object *****");
            Console.WriteLine("=> {0}", msg);
            Console.WriteLine("**************************************\n");
        }


        static void Main(string[] args)
        {
            Console.WriteLine("*****Delegates as event enablers *****\n");

            //First make a Car object, and register it.
            Car c1 = new Car("SlugBug", 100, 10);
            //using 'method group conversion syntax' to supply a direct method name instead of a delegate object.
            c1.RegisterWithCarEngine(OnCarEngineEvent);     

            //Create second delegate and hold on to it so we can unregister it later.
            Car.CarEngineHandler handler2 = new Car.CarEngineHandler(OnCarEngineEvent2);
            c1.RegisterWithCarEngine(handler2);


            //Speed up (this will trigger the events)
            Console.WriteLine("****** Speeding up ******");
            for (int i = 0; i < 6; i++)
            {
                c1.Accelerate(20);
            } 

            //Unregister the second handler
            c1.UnregisterWithCarEngine(handler2);

            //We won't see the uppercae values anymore.
            Console.WriteLine("****** Speeding up ******");
            for (int i = 0; i < 6; i++)
            {
                c1.Accelerate(20);
            } 
            Console.ReadLine();
        }
    }
}
