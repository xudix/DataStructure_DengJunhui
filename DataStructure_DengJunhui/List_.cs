using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class List_<T>
    {
        private int _size;
        private ListNode<T> header, trailer;

        #region Constructor

        public List_()=>Init();

        public List_(in List_<T> source, int lo, int hi)
        {
            Init();
            CopyNodes(source[lo], hi-lo);
        }

        public List_(in List_<T> source)
        {
            Init();
            CopyNodes(source.First, source.Size);
        }

        

        public List_(in ListNode<T> firstNode, int n_nodes)
        {
            Init();
            CopyNodes(firstNode, n_nodes);
        }

        public List_(params T[] initial_Vals)
        {
            Init();
            for (int i = 0; i < initial_Vals.Length; InsertAsLast(initial_Vals[i++])) ;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Initialize the List with header and trailer
        /// </summary>
        protected void Init()
        {
            _size = 0;
            header = new ListNode<T>();
            trailer = new ListNode<T>();
            header.Succ = trailer;
            trailer.Pred = header;
        }

        protected void CopyNodes(ListNode<T> firstNode, int n_nodes)
        {
            while (n_nodes-- > 0)
            {
                InsertAsLast(firstNode.Data);
                firstNode = firstNode.Succ;
            }
        }

        protected int Clear() {
            int oldSize = _size;
            Init();
            return oldSize;
        }

        

        #endregion

        #region Read only interfaces

        public int Size { get => _size; }
        public bool Empty { get => _size == 0; }
        public ListNode<T> First { get => header.Succ; }
        public ListNode<T> Last { get => trailer.Pred; }

        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public ListNode<T> this[int index]
        {
            get {
                if (index >= _size || index < 0) throw new IndexOutOfRangeException();
                ListNode<T> ans = this.First;
                while(index-- > 0)
                    ans= ans.Succ;
                return ans;
            }
            set
            {
                if (index >= _size || index < 0) throw new IndexOutOfRangeException();
                this[index].Data = value.Data;
            }
        }

        

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            ListNode<T> current = First;
            for (int i = 0; i < _size; ++i)
            {
                sb.Append(current.Data.ToString()).Append(',');
                current = current.Succ;
            }
            sb.AppendLine().AppendLine("Size: " + _size);
            return sb.ToString();
        }


        #endregion

        #region Writable interfaces

        public ListNode<T> InsertAsFirst(in T data)
        {
            _size++;
            return header.InsertAsSucc(data);
        }

        public ListNode<T> InsertAsLast(in T data)
        {
            _size++;
            return trailer.InsertAsPred(data);
        }

        public ListNode<T> InsertBefore(in ListNode<T> p, in T data)
        {
            _size++;
            return p.InsertAsPred(data);
        }

        public ListNode<T> InsertAfter(in ListNode<T> p, in T data)
        {
            _size++;
            return p.InsertAsSucc(data);
        }

        public T Remove(ListNode<T> listNode)
        {
            T removed_data = listNode.Data;
            listNode.Succ.Pred = listNode.Pred;
            listNode.Pred.Succ = listNode.Succ;
            _size--;
            return removed_data;
        }



        #endregion
    }

    public class ListNode<T> 
    {
        public T? Data { get; set; }
        
        public ListNode<T>? Pred { get; set; }
        public ListNode<T>? Succ { get; set; }


        public ListNode() { }

        public ListNode(T? data, ListNode<T>? pred = null, ListNode<T>? succ = null)
        {
            Data = data;
            Pred = pred;
            Succ = succ;
        }

        public ListNode<T> InsertAsPred(in T data)
        {
            ListNode<T> newNode = new ListNode<T>(data, this.Pred, this);
            this.Pred.Succ = newNode;
            this.Pred = newNode;
            return newNode;
        }
        public ListNode<T> InsertAsSucc(in T data)
        {
            ListNode<T> newNode = new ListNode<T>(data, this, this.Succ);
            this.Succ.Pred = newNode;
            this.Succ = newNode;
            return newNode;
        }
    }

    public class List_With_Sort_<T>: List_<T> where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// Search for target in sorted list.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="lastNode"></param>
        /// <param name="n_node"></param>
        /// <returns>The greatest node that is not greater than the target in a sorted list.</returns>
        public ListNode<T> Search(T target, ListNode<T> lastNode, int n_node)
        {
            while (n_node-- > 0)
                if (lastNode.Data.CompareTo(target) > 0)
                    lastNode = lastNode.Pred;
                else
                    break;
            return lastNode;
        }

        protected void Merge(ListNode<T> p_start1, int n_nodes1, List_<T> list2, ListNode<T> p_start2, int n_nodes2)
        {
            throw new NotImplementedException();
        }

        protected void MergeSort(ListNode<T> p_start, int n_nodes)
        {
            throw new NotImplementedException();
        }

        protected void InsertionSort(ListNode<T> p_start, int n_nodes)
        {
            ListNode<T> current = p_start;
            int sorted = 1;
            while (sorted++ < n_nodes)
            {
                if (current.Data.CompareTo(current.Succ.Data) > 0)
                {
                    T insert = Remove(current.Succ);
                    InsertAfter(Search(insert, current, sorted), insert);
                }
                else
                    current = current.Succ;
            }
        }

        protected void SelectionSort(ListNode<T> p_start, int n_nodes)
        {
            throw new NotImplementedException();
        }

        public void Sort()
        {
            InsertionSort(First, Size);
        }
    }
}
