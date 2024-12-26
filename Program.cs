using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPS_Practice
{
    class Program
    {
        public int Number;
        public string name;


        /*
        static void Main(string[] args)
        {
            Program newObj = new Program(34, "Kalai selvan");

            Console.WriteLine(newObj.name);
            Console.ReadLine();
        }
        */
    }

}

namespace OOPS_Practice
{
    class Programmers:Program
    {
        public Programmers(int age, string actor) {
            Number = age;
            name = actor;
        }
        
        static void Main(string[] args)
        {
            Programmers newObj = new Programmers(34, "Ajith Kumar");

            Console.WriteLine(newObj.name);
            Console.ReadLine();
        }
        
    }

}
