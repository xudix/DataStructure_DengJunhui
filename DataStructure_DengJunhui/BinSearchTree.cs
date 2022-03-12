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

        /// <summary>
        /// Connect the given nodes and subTrees in the 3-4 manner.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="node3"></param>
        /// <param name="subTree1"></param>
        /// <param name="subTree2"></param>
        /// <param name="subTree3"></param>
        /// <param name="subTree4"></param>
        /// <returns></returns>
        protected virtual BinNode<T> Connect34(BinNode<T> node1, BinNode<T> node2, BinNode<T> node3, BinNode<T>? subTree1, BinNode<T>? subTree2, BinNode<T>? subTree3, BinNode<T>? subTree4)
        {
            node1.L_Child = subTree1;
            if (subTree1 != null)
                subTree1.Parent = node1;
            node1.R_Child = subTree2;
            if (subTree2 != null)
                subTree2.Parent = node1;
            node3.L_Child = subTree3;
            if (subTree3 != null)
                subTree3.Parent = node3;
            node3.R_Child = subTree4;
            if (subTree4 != null)
                subTree4.Parent = node3;
            node1.UpdateHeight();
            node3.UpdateHeight();
            node2.L_Child = node1;
            node1.Parent = node2;
            node2.R_Child = node3;
            node3.Parent = node2;
            node2.UpdateHeight();
            return node2;
        }

        protected virtual BinNode<T> BalanceAt(BinNode<T> node)
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

        /// <summary>
        /// Removes target from the Tree
        /// </summary>
        /// <param name="target">The target to be removed</param>
        /// <returns>True if the target was in the tree. False is the target was not in the tree to begin with.</returns>
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
            _size--;
            UpdateHeightAbove(_hot);
            return succ;

        }

        protected void Zig(BinNode<T> binNode)
        {
            var newNode = binNode.L_Child;
            if (newNode != null)
            {
                newNode.Parent = binNode.Parent;
                if (binNode.Parent == null) //binNode was the root
                    _root = newNode;
                else if (binNode.IsLChild)
                    binNode.Parent.L_Child = newNode;
                else
                    binNode.Parent.R_Child = newNode;
                binNode.L_Child = newNode.R_Child;
                if (binNode.L_Child != null)
                    binNode.L_Child.Parent = binNode;
                newNode.R_Child = binNode;
                binNode.Parent = newNode;
                UpdateHeightAbove(binNode);
            }
        }

        protected void Zag(BinNode<T> binNode)
        {
            var newNode = binNode.R_Child;
            if (newNode != null)
            {
                newNode.Parent = binNode.Parent;
                if (binNode.Parent == null) //binNode was the root
                    _root = newNode;
                else if (binNode.IsLChild)
                    binNode.Parent.L_Child = newNode;
                else
                    binNode.Parent.R_Child = newNode;
                binNode.R_Child = newNode.L_Child;
                if (binNode.R_Child != null)
                    binNode.R_Child.Parent = binNode;
                newNode.L_Child = binNode;
                binNode.Parent = newNode;
                UpdateHeightAbove(binNode);
            }
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
