using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class Vector_<T>
    {
        private const int defaul_Capacity = 10;

        protected int _size, _capacity;
        protected T?[] _elem;

        public delegate void DelegateMethod(ref T? obj);

        #region protected methods

        /// <summary>
        /// Copy elements from source[lo] to source[hi-1] to this vector
        /// Assumes the capacity is sufficient
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        protected void CopyFrom(in T[] source, int lo, int hi) {
            _elem = new T[_capacity = (hi - lo) << 1];
            _size = 0;
            while (lo < hi)
                _elem[_size++] = source[lo++];
        }

        /// <summary>
        /// Use before inserting any element to the vector
        /// </summary>
        protected void Expand()
        {
            if (_size < _capacity) return;
            if (_capacity < defaul_Capacity) _capacity = defaul_Capacity;
            T[] oldElem = _elem;
            _elem = new T[_capacity <<= 1];
            for (int i = 0; i < _size; i++)
                _elem[i] = oldElem[i];
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Shrink()
        {
            if (_size << 2 > _capacity || _capacity < defaul_Capacity << 1) return;
            T[] oldElem = _elem;
            _elem = new T[_capacity >>= 1];
            for (int i = 0; i < _size; i++)
                _elem[i] = oldElem[i];
        }

        

        
        #endregion

        #region Constructors

        public Vector_(int capacity = defaul_Capacity, int size = 0, T initial_Val = default)
        {
            _elem = new T[_capacity = capacity > size ? capacity:size];
            for (_size = 0; _size < size; _size++)
                _elem[_size] = initial_Val;
        }


        //public Vector_(int capacity = defaul_Capacity, int size = 0, params T[] initial_Vals)
        //{
        //    size = initial_Vals.Length;
        //    _elem = new T[_capacity = capacity > size ? capacity : size];
        //    for (_size = 0; _size < size; _size++)
        //        _elem[_size] = initial_Vals[_size];
        //}

        public Vector_(in T[] source, int lo, int hi) =>
            CopyFrom(source, lo, hi);

        public Vector_(in T[] source, int n) =>
            CopyFrom(source, 0, n);

        public Vector_(in Vector_<T> source, int lo, int hi) =>
            CopyFrom(source._elem, lo, hi);

        public Vector_(in Vector_<T> source) =>
            CopyFrom(source._elem, 0, source._size);


        #endregion


        #region Read only interfaces

        public int Size => _size;

        public T[] Elements => _elem;

        public bool Empty => _size == 0;

        public int Capacity => _capacity;

        

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < _size; ++i)
                sb.Append(_elem[i].ToString()).Append(',');
            sb.AppendLine().AppendLine("Capacity: " + _capacity + "; Size: " + _size);
            return sb.ToString();
        }

        public static void Swap(ref T t1, ref T t2)
        {
            T temp = t1;
            t1 = t2;
            t2 = temp;
        }


        #endregion

        #region Writable interfaces

        /// <summary>
        /// This is an Indexer. It allows getting an element in the vector like indexing an array.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get => _elem[index];
            set => _elem[index] = value; 
        }
            

        // Assignment operator = cannot be overloaded in C#

        /// <summary>
        /// Remove elements in the range [lo, hi).
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public int Remove(int lo, int hi)
        {
            int n_removed = hi - lo;
            if (n_removed <= 0) return 0;

            while (hi < _size)
                _elem[lo++] = _elem[hi++];
            _size -= n_removed;
            Shrink();
            return n_removed;
        }

        /// <summary>
        /// Remove the element at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Remove(int index)
        {
            T removed = _elem[index];
            Remove(index, index + 1);
            return removed;
        }

        /// <summary>
        /// Insert the newElement to location specified by index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newElement"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public int Insert(int index, in T newElement)
        {
            if (index > _size)
                throw new IndexOutOfRangeException();
            Expand();
            for (int i = _size; i > index; i--)
                _elem[i] = _elem[i - 1];
            _elem[index] = newElement;
            _size++;
            return index;
        }

        /// <summary>
        /// Insert newElement as the last element of the vector.
        /// </summary>
        /// <param name="newElement"></param>
        /// <returns></returns>
        public int Insert(in T? newElement)
        {
            Expand();
            _elem[_size++] = newElement;
            return _size-1;
        }




        /// <summary>
        /// Randomize the order of the elements in the range of lo and hi of the vector.
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        public void Unsort(int lo, int hi)
        {
            Random r = new Random();
            while(lo+1 < hi--)
                Swap(ref _elem[hi], ref _elem[lo+r.Next(hi-lo+1)]);
        }

        /// <summary>
        /// Randomize the order of all elements in the vector
        /// </summary>
        public void Unsort() =>
            Unsort(0, _size);



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Uniquify()
        {
            int lo = 0, hi = 0, oldSize = _size;
            while (++hi < _size)
                if(!_elem[lo].Equals(_elem[hi]))
                    _elem[++lo] = _elem[hi];
            _size = ++lo;
            Shrink();
            return oldSize - _size;
        }

        /// <summary>
        /// Applies delegateMethod on each element of the vector, in order to change them.
        /// </summary>
        /// <param name="delegateMethod">This can be a method like "void MethodName(ref double x)",
        /// or a lambda expression like "(ref double x) => x = ...".</param>
        public void Traverse(DelegateMethod delegateMethod)
        {
            for(int i = 0; i < _size; i++)
                delegateMethod(ref _elem[i]);
        }

        public static void TestMethod(ref double input) =>
            input -= 50;

        #endregion
    }

    public class Vector_with_Sort_<T>: Vector_<T> where T : IComparable<T>, IEquatable<T>, IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns>True if the array is already sorted</returns>
        protected bool Bubble(int lo, int hi)
        {
            bool sorted = true;
            while (++lo < hi)
                if (_elem[lo - 1].CompareTo(_elem[lo]) > 0)
                {
                    sorted = false;
                    Swap(ref _elem[lo - 1], ref _elem[lo]);
                }
            return sorted;


        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        protected void BubbleSort(int lo, int hi)
        {
            while (!Bubble(lo, hi--)) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>

        protected int Max(int lo, int hi)
        {
            int max = lo;
            T max_elem = _elem[lo];
            for (int i = lo; i < hi; i++)
                if (_elem[i].CompareTo(max_elem) > 0)
                    max_elem = _elem[max = i];
            return max;

        }

        protected int Disordered(int lo, int hi)
        {
            int disordered = 0;
            for (int i = lo + 1; i < hi; i++)
                if (_elem[i - 1].CompareTo(_elem[i]) > 0)
                    disordered++;
            return disordered;
        }

        /// <summary>
        /// Merge two subsets of the array with index range [lo, mi) and [mi,hi), respectively.
        /// The two sub arrays shouldl be sorted, respectively
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="mi"></param>
        /// <param name="hi"></param>
        protected void Merge(int lo, int mi, int hi)
        {
            int i, array1_len = mi - lo;
            T[] array1 = new T[array1_len];
            for (i = 0; i < array1_len; i++)
                array1[i] = _elem[i + lo];
            i = 0;
            while (lo < hi)
                _elem[lo++] = mi >= hi || (i < array1_len && array1[i].CompareTo(_elem[mi]) <= 0) ? array1[i++] : _elem[mi++];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        protected void MergeSort(int lo, int hi)
        {
            if (hi - lo == 1) return;
            int mi = (hi + lo) >> 1;
            MergeSort(lo, mi);
            MergeSort(mi, hi);
            Merge(lo, mi, hi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected int Partition(int lo, int hi)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        protected void QuickSort(int lo, int hi)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        protected void HeapSort(int lo, int hi)
        {

        }

        public int Disordered_Num => Disordered(0, _size);

        /// <summary>
        /// Find the target in disordered vector
        /// </summary>
        /// <param name="target"></param>
        /// <returns>The index of target in the vector. Returns -1 if the target is not found.</returns>
        public int Find(in T target) =>
            Find(target, 0, _size);

        /// <summary>
        /// Find the target in the range [lo, hi) of disordered vector
        /// </summary>
        /// <param name="target"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns>The index of target in the vector. Returns lo-1 if the target is not found.</returns>
        public int Find(in T target, int lo, int hi)
        {
            while (lo < hi-- && !_elem[hi].Equals(target)) ;
            return hi;
        }

        /// <summary>
        /// Search for target in SORTED vector
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public int Search(in T target) =>
            Search(target, 0, _size);

        /// <summary>
        /// Search for target in the range [lo, hi) of SORTED vector
        /// </summary>
        /// <param name="target"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns>Returns the index of target, or the largest element that is not greater than the target.</returns>
        public int Search(in T target, int lo, int hi)
        {
            int mi;
            while (lo < hi)
            {
                mi = (lo + hi) >> 1;
                if (_elem[mi].CompareTo(target) > 0)
                    hi = mi;
                else
                    lo = mi++;
            }
            return --lo;
        }

        public void Sort(int lo, int hi)
        {
            MergeSort(lo, hi);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Sort() =>
            Sort(0, _size);

        /// <summary>
        /// Remove all duplicated elements in disordered vector.
        /// </summary>
        /// <returns></returns>
        public int Deduplicate()
        {
            int i = 1, oldSize = _size;
            while (i < _size)
            {
                if (Find(_elem[i], 0, i) < 0)
                    i++;
                else
                    Remove(i);
            }
            return oldSize - _size;
        }
    }
}
