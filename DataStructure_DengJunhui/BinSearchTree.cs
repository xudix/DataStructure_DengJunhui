using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class BinSearchTree<T>: BinTree<T> where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// The last visted non-null node in search.
        /// </summary>
        protected BinNode<T>? _hot;
        protected BinNode<T> Connect34(BinNode<T> node1, BinNode<T> node2, BinNode<T> node3, BinNode<T> node4, BinNode<T> node5, BinNode<T> node6, BinNode<T> node7)
        {
            throw new NotImplementedException();
        }

        protected BinNode<T> RotateAt(BinNode<T> node)
        {
            throw new NotImplementedException();
        }


        public virtual BinNode<T>? Search(T target)=>
            Search(target, _root, out _hot);

        public static BinNode<T>? Search(in T target, in BinNode<T> root, out BinNode<T>? hot)
        {
            hot = null;
            BinNode<T>? result = root;
            while(result != null)
            {

                switch (target.CompareTo(result.Data))
                {
                    case 0:
                        return result;
                    case > 0:
                        hot = result;
                        result = result.R_Child;
                        break;
                    case < 0:
                        hot = result;
                        result = result.L_Child;
                        break;
                }
            }
            return result;
        }

        public virtual BinNode<T> Insert(T newEntry)
        {
            BinNode<T>? location = Search(newEntry);
            if (location != null)
                return location;
            else if(_hot!= null) //non empty tree
            {
                if (newEntry.CompareTo(_hot.Data) > 0)
                    return InsertAsRC(_hot, newEntry);
                else
                    return InsertAsLC(_hot, newEntry);
            }
            else // empty tree
            {
                return InsertAsRoot(newEntry);
            }
        }

        public virtual bool Remove(T target)
        {
            var loc = Search(target);
            if (loc == null) // target entry does not exist
                return false;
            Remove(loc);
            return true;
        }

        protected virtual BinNode<T>? Remove(BinNode<T> loc)
        {
            BinNode<T>? node2Remove = loc, succ = null;
            _size--;
            if (!loc.HasLChild) // no left child or no child
            {
                succ = loc.R_Child;
            }
            else if (!loc.HasRChild) // no right child
            {
                succ=loc.L_Child;
            }
            else // has both children
            {
                node2Remove = loc.Succ();
                loc.Data = node2Remove.Data;
                succ = node2Remove.R_Child; // node2Remove won't have L child
                _hot = node2Remove.Parent; // _hot is always the parent of node2Remove
            }
            if(succ!= null) succ.Parent = _hot;
            if (_hot != null) // node2Remove is not root
                if (node2Remove.IsLChild)
                {
                    _hot.L_Child = succ;
                }
                else
                {
                    _hot.R_Child = succ;
                }
            else // node2Remove is root
                _root = succ;
            return succ;

        }
    }


    public struct Entry <TKey, TVal>:IComparable<Entry<TKey,TVal>>, IEquatable<Entry<TKey, TVal>> where TKey: IComparable<TKey>, IEquatable<TKey>
    {
        TKey Key;
        TVal Val;

        public Entry(TKey key = default, TVal val = default)
        {
            Key = key;
            Val = val;
        }

        public Entry(Entry<TKey, TVal> entry)
        {
            Key = entry.Key;
            Val = entry.Val;
        }

        public override string ToString() => Val.ToString();

        public int CompareTo(Entry<TKey, TVal> other) =>
            Key.CompareTo(other.Key);

        public bool Equals(Entry<TKey, TVal> other) =>
            Key.Equals(other.Key);
    }


    public class BinSearchTreeExercise
    {
        public static void main()
        {
            var BST = new BinSearchTree<Entry<int, string>>();
            BST.Insert(new Entry<int, string>(36, "thirty six"));
            BST.Insert(new Entry<int, string>(27, "twenty seven"));
            BST.Insert(new Entry<int, string>(6, "six"));
            BST.Insert(new Entry<int, string>(58, "fifty eight"));
            BST.Insert(new Entry<int, string>(69, "sixty nine"));
            BST.Insert(new Entry<int, string>(53, "fifty three"));
            BST.Insert(new Entry<int, string>(46, "forty six"));

        }
    }
}
