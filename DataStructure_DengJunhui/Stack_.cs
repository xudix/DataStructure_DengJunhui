using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class Stack_<T>: Vector_<T> where T : IComparable<T>, IEquatable<T>, IComparable
    {
        public void Push(T newElement) => 
            Insert(newElement);

        public T Pop() =>
            Remove(_size - 1);

        public T Top() =>
            this[_size - 1];
    }

    public class StackExercices
    {
        public static void Main()
        {
            Console.WriteLine("Test");
        }
    }


}
