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

        public BinNode<T> Root => _root;

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

            return binNode.Height;
        }

        public int N_Nodes(BinNode<T> binNode)=>
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
            if (binNode.IsLChild)
                binNode.Parent.L_Child = null;
            else if (binNode.IsRChild)
                binNode.Parent.R_Child = null;
            else if (binNode == _root)
                _root = null;

            binNode.Parent = null;
            BinTree<T> newTree = new ();
            newTree._root = binNode;
            return newTree;
        }

        public void TravLevel(Action<T> action) =>
            _root?.TravLevel(action);

        public void TravPre(Action<T> action) =>
            _root?.TravPre(action);

        public void TravIn(Action<T> action) =>
            _root?.TravIn(action);

        public void TravPost(Action<T> action) =>
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
            (L_Child = new(data,this));

        /// <summary>
        /// Insert data as the right child of current node. Previous right child will be discard.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The new node inserted.</returns>
        public BinNode<T> InsertAsRC(T data) =>
            (R_Child = new(data,this));

        /// <summary>
        /// 
        /// </summary>
        public BinNode<T> Succ => L_Child;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void TravLevel(Action<T> action)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void TravPre(Action<T> action)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void TravIn(Action<T> action)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void TravPost(Action<T> action)
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
}
