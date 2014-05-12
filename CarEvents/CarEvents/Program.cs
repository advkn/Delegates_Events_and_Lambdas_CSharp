using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarEvents
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

        //Define a delegate type that accepts a custom event as a parameter.
        public delegate void CarEngineHandler(object sender, CarEventArgs e);

        //This car can send these events
        public event CarEngineHandler Exploded;
        public event CarEngineHandler AboutToBlow;

        /*  Utilizing the generic EventHandler<T> delegate
        public EventHandler<CarEventArgs> Exploded;
        public EventHandler<CarEventArgs> AboutToBlow;
        */

        public void Accelerate(int delta)
        {
            //If this car is 'dead', send message
            if (carIsDead)
            {
                if (Exploded != null)
                {
                    Exploded(this, new CarEventArgs("Sorry, this car is dead..."));
                }
            }
            else
            {
                CurrentSpeed += delta;

                //Is this car almost dead'?
                if (10 == (MaxSpeed - CurrentSpeed) && AboutToBlow != null)
                {
                    AboutToBlow(this, new CarEventArgs("Careful buddy! Gonna blow!"));
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

    public class CarEventArgs : EventArgs
    {
        public readonly string msg;
        public CarEventArgs(string message)
        {
            msg = message;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Car c1 = new Car("SlugBug", 100, 10);
       
            //Register the events with their specific targets
            c1.AboutToBlow += CarAboutToBlow;
            c1.AboutToBlow += CarIsAlmostDoomed;
            c1.Exploded += CarExploded;

            //accelerate the car
            for (int i = 0; i < 6; i++)
            {
                c1.Accelerate(20);
            }

            Console.ReadLine();
        }

        //Targets for events
        public static void CarAboutToBlow(object sender, CarEventArgs e)
        {
            //Perform a runtime check before casting
            if (sender is Car)
            {
                Car c = (Car)sender;
                Console.WriteLine("Critical message from {0}: {1}", c.PetName, e.msg);
            }
        }

        public static void CarIsAlmostDoomed(object sender, CarEventArgs e)
        {
            Console.WriteLine("=>Critical message from {0}: {1}",sender, e.msg);
        }

        public static void CarExploded(object sender, CarEventArgs e)
        {
            Console.WriteLine(e.msg);
        }
    }
}
