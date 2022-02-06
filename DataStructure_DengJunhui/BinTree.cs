using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class BinTree<T>
    {
        protected int _size;
        protected BinNode<T>? _root;


        public BinTree()
        {
            _size = 0;
            _root = null;
        }

        public int Size => _size;

        public bool IsEmpty => _root == null;

        public BinNode<T>? Root => _root;

        /// <summary>
        /// Insert data as the root. 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public BinNode<T> InsertAsRoot(T data)
        {
            BinNode<T> newRoot = new(data, null, _root);
            if (_root != null)
                _root.Parent = newRoot;
            _size++;
            newRoot.UpdateHeight();
            return _root = newRoot; 
        }

        public BinNode<T> InsertAsLC(BinNode<T> binNode, T data)
        {
            var newNode = binNode.InsertAsLC(data);
            UpdateHeightAbove(binNode);
            _size++;
            return newNode;
        }

        public BinNode<T> InsertAsRC(BinNode<T> binNode, T data)
        {
            var newNode = binNode.InsertAsRC(data);
            UpdateHeightAbove(binNode);
            _size++;
            return newNode;
        }


        /// <summary>
        /// Attach the subTree to binNode as its left child.
        /// </summary>
        /// <param name="binNode">Node where subTree is attached. Should have no left child</param>
        /// <param name="subTree"></param>
        /// <returns>Node where subTree is attached.</returns>
        public BinNode<T> AttachAsLC(BinNode<T> binNode, BinTree<T> subTree) {
            if((binNode.L_Child = subTree.Root) != null)
                binNode.L_Child.Parent = binNode;
            _size += subTree.Size;
            UpdateHeightAbove(binNode);
            return binNode;
        }

        /// <summary>
        /// Attach the subTree to binNode as its right child.
        /// </summary>
        /// <param name="binNode">Node where subTree is attached. Should have no right child</param>
        /// <param name="subTree"></param>
        /// <returns>Node where subTree is attached.</returns>
        public BinNode<T> AttachAsRC(BinNode<T> binNode, BinTree<T> subTree)
        {
            if ((binNode.R_Child = subTree.Root) != null)
                binNode.R_Child.Parent = binNode;
            _size += subTree.Size;
            UpdateHeightAbove(binNode);
            return binNode;
        }

        /// <summary>
        /// Delete the sub tree rooted at binNode.
        /// </summary>
        /// <param name="binNode"></param>
        /// <returns>The number of the nodes removed.</returns>
        public int Remove(BinNode<T> binNode)
        {
            if (binNode.Parent == null) // This is the root
            {
                _root = null;
            }
            else
            {
                if (binNode.IsLChild)
                    binNode.Parent.L_Child = null;
                else
                    binNode.Parent.R_Child = null;
                UpdateHeightAbove(binNode.Parent);
            }
            int n_removed = N_Nodes(binNode);
            _size -= n_removed;
            return n_removed;
        }

        public static int N_Nodes(BinNode<T> binNode)=>
            binNode == null ? 0 : 1 + N_Nodes(binNode) + N_Nodes(binNode);
         

        public void UpdateHeightAbove(BinNode<T>? binNode)
        {
            while (binNode != null)
            {
                if (binNode.Height == binNode.UpdateHeight())
                    break;
                binNode = binNode.Parent;
            }
        }

        /// <summary>
        /// Remove the sub tree rooted at binNode, and return the sub Tree as a new Tree.
        /// </summary>
        /// <param name="binNode"></param>
        /// <returns>The sub tree removed.</returns>
        public BinTree<T> Secede(BinNode<T> binNode)
        {
            BinTree<T> newTree = new();
            if (binNode.Parent == null) // This is the root
            {
                _root = null;
            }
            else
            {
                if (binNode.IsLChild)
                    binNode.Parent.L_Child = null;
                else
                    binNode.Parent.R_Child = null;
                UpdateHeightAbove(binNode.Parent);
            }
            newTree._size = N_Nodes(binNode);
            _size -= newTree.Size;
            binNode.Parent = null;
            newTree._root = binNode;
            return newTree;
        }

        public void TravLevel(TraversalAction<T> action) =>
            _root?.TravLevel(action);

        public void TravPre(TraversalAction<T> action) =>
            _root?.TravPre(action);

        public void TravIn(TraversalAction<T> action) =>
            _root?.TravIn(action);

        public void TravPost(TraversalAction<T> action) =>
            _root?.TravPost(action);

    }


    public class BinNode<T>
    {
        public T? Data;

        public BinNode<T>? Parent, L_Child, R_Child;

        public int Height, NPL; // height and null path length

        public RBColor RBColor;

        public BinNode(T? data = default, BinNode<T>? parent = null, BinNode<T>? l_Child = null, BinNode<T>? r_Child = null, int height = 0, int nPL = 1, RBColor rBColor = RBColor.RB_RED)
        {
            Data = data;
            Parent = parent;
            L_Child = l_Child;
            R_Child = r_Child;
            Height = height;
            NPL = nPL;
            RBColor = rBColor;
        }


        /// <summary>
        /// The total number of descendants
        /// </summary>
        public int Size;

        public static int Stature(BinNode<T>? binNode) =>
            binNode != null ? binNode.Height : -1;

        /// <summary>
        /// Insert data as the left child of current node. Previous left child will be discard.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The new node inserted.</returns>
        public BinNode<T> InsertAsLC(T data) =>
            (L_Child = new(data, this));

        /// <summary>
        /// Insert data as the right child of current node. Previous right child will be discard.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The new node inserted.</returns>
        public BinNode<T> InsertAsRC(T data) =>
            (R_Child = new(data, this));

        /// <summary>
        /// The succeding node of the current node in Inorder Traversal
        /// </summary>
        /// <returns>The succeding node. If this node is the last node, return null.</returns>
        public BinNode<T>? Succ() {
            BinNode<T> result = this;
            if (result.HasRChild) // If it has right child, the succ is in the right child subtree.
            {
                result = result.R_Child;
                while(result.HasLChild)
                    result = result.L_Child;
            }
            else // Otherwise, the succ is in the first ancestor that has this node in left child subtree.
            {
                while (result.IsRChild)
                    result = result.Parent;
                result = result.Parent;
            }
            return result;
        }

        /// <summary>
        /// Level-order Traversal
        /// </summary>
        /// <param name="action"></param>
        public void TravLevel(TraversalAction<T> action)
        {
            BinNode<T> current = this;
            Queue_<BinNode<T>> nodeQueue = new(current.Size>>1);
            nodeQueue.Enqueue(current);
            while(!nodeQueue.Empty)
            {
                current = nodeQueue.Dequeue();
                action(ref current.Data);
                if (current.L_Child != null)
                    nodeQueue.Enqueue(current.L_Child);
                if (current.R_Child != null)
                    nodeQueue.Enqueue(current.R_Child);
            }
        }

        /// <summary>
        /// Preorder traversal.
        /// </summary>
        /// <param name="action"></param>
        public void TravPre(TraversalAction<T> action)
        {
            BinNode<T>? current = this;
            Stack_<BinNode<T>> nodeStack = new();
            do
            {
                action(ref current.Data);
                if (current.HasRChild)
                    nodeStack.Push(current.R_Child);
                if ((current = current.L_Child) == null)
                    current = !nodeStack.Empty ? nodeStack.Pop() : null;
            } while (current != null);
            
        }

        /// <summary>
        /// Inorder Traversal.
        /// </summary>
        /// <param name="action"></param>
        public void TravIn(TraversalAction<T> action)
        {
            BinNode<T> current = this;
            while (current.HasLChild)
                current = current.L_Child;
            while(current != null)
            {
                action(ref current.Data);
                current = current.Succ();
            }
        }

        /// <summary>
        /// Inorder Traversal using stack method.
        /// </summary>
        /// <param name="action"></param>
        public void TravIn_STK(TraversalAction<T> action)
        {
            BinNode<T>? current = this;
            Stack_<BinNode<T>> nodeStack = new();
            //nodeStack.Push(current);
            //while (!nodeStack.Empty )
            //{
            //    if(current != null && current.HasLChild)
            //        nodeStack.Push(current = current.L_Child);
            //    else
            //    {
            //        current = nodeStack.Pop();
            //        action(ref current.Data);
            //        if((current = current.R_Child)!=null)
            //            nodeStack.Push(current);
            //    }
            //}
            do
            {
                if (current != null)
                {
                    nodeStack.Push(current);
                    current = current.L_Child;
                }
                else
                {
                    current = nodeStack.Pop();
                    action(ref current.Data);
                    current = current.R_Child;
                }
            } while (!nodeStack.Empty || current != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void TravPost(TraversalAction<T> action)
        {
            
        }

        public int UpdateHeight() =>
            Height = 1 + Stature(L_Child) > Stature(R_Child) ? Stature(L_Child) : Stature(R_Child);


        


        public bool IsRoot => Parent == null;
        public bool IsLeaf => L_Child == null && R_Child == null;
        public bool IsLChild => Parent != null && this == Parent.L_Child;
        public bool IsRChild => Parent != null && this == Parent.R_Child;
        public bool HasParent => Parent != null;
        public bool HasRChild => R_Child != null;
        public bool HasLChild => L_Child != null;
        public bool HasChild => L_Child != null || R_Child != null;
        public bool HasBothChild => L_Child != null && R_Child != null;
        public BinNode<T>? Sibling =>
            Parent == null ? null : (IsLChild ? Parent.R_Child : Parent.L_Child);
        public BinNode<T>? Uncle =>
            Parent?.Sibling;

    }

    public enum RBColor { RB_RED, RB_BLACK};

    public delegate void TraversalAction<T>(ref T? value);
}
