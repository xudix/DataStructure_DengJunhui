using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class Queue_<T>
    {
        protected const int _defaultCapacity = 10;
        
        protected int _capacity;
        protected T[] _elem;
        protected int front = 0, rear = 0; // Assert: rear >= front

        public Queue_(int capacity = _defaultCapacity)
        {
            _capacity = capacity;
            _elem = new T[capacity];
        }

        public int Size => rear - front;
        public bool Empty => Size == 0;
        public T Front => _elem[front%_capacity];

        public T Enqueue(T data)
        {
            Expand();
            _elem[(rear++)%_capacity] = data;
            int q;
            if ((q = rear / _capacity) == (front / _capacity) && q > 0)
            {
                rear -= q * _capacity;
                front -= q * _capacity;
            }
            return data;
        }

        protected void Expand()
        {
            if (Size < _capacity)
                return;
            T[] oldElem = _elem;
            _elem = new T[_capacity<<1];
            int i;
            for (i = 0; i < Size; i++)
                _elem[(i)] = oldElem[(i+front) % _capacity];
            front = 0; rear = i;
            _capacity <<= 1;
        }

        public T Dequeue()
        {
            if (rear < front)
                throw new InvalidOperationException("Queue is already empty!");
            Shrink();
            return _elem[(front++)%_capacity];
        }

        protected void Shrink()
        {
            if (_capacity < (_defaultCapacity << 1) || Size > (_capacity >> 2))
                return;
            T[] oldElem = _elem;
            _elem = new T[_capacity >> 1];
            int i;
            for (i = 0; i < Size; i++)
                _elem[(i)] = oldElem[(i + front) % _capacity];
            front = 0; rear = i;
            _capacity >>= 1;
        }
    }
}
