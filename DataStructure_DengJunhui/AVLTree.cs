using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructure_DJ;

namespace DataStructure_DJ
{
    public class AVLTree<T>: BinSearchTree<T> where T : IComparable<T>, IEquatable<T>
    {
        public bool Balanced(BinNode<T> binNode) =>
            BinNode<T>.Stature(binNode.L_Child) == BinNode<T>.Stature(binNode.R_Child);

        public int BalFac(BinNode<T> binNode) =>
            BinNode<T>.Stature(binNode.L_Child) - BinNode<T>.Stature(binNode.R_Child);

        public bool AvlBalanced(BinNode<T> binNode)
        {
            var balFac = BalFac(binNode);
            return balFac > -2 && balFac < 2;
        }

    }
}
