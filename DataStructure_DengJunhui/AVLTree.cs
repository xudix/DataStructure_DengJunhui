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

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binNode"></param>
        /// <returns></returns>
        protected BinNode<T> TallerChild(BinNode<T> binNode)
        {
            if (BinNode<T>.Stature(binNode.L_Child) > BinNode<T>.Stature(binNode.R_Child))
                return binNode.L_Child;
            else if (BinNode<T>.Stature(binNode.L_Child) < BinNode<T>.Stature(binNode.R_Child))
                return binNode.R_Child;
            else
                return binNode.IsLChild ? binNode.L_Child : binNode.R_Child;
        }

        public override BinNode<T> Insert(T newEntry)
        {
            var newNode = base.Insert(newEntry);
            for(var node = _hot; node != null; node = node.Parent)
            {
                if (!AvlBalanced(node))
                {
                    BalanceAt(node); // when inserting, the height will not change after BalanceAt()
                    break;
                }
            }
            return newNode;
        }

        public override bool Remove(T target)
        {
            bool result = base.Remove(target);
            if (result) // target found and removed. May need to rebalance the AVL tree.
            {
                for (var node = _hot; node != null; node = node.Parent)
                {
                    if (!AvlBalanced(node))
                        node = BalanceAt(node);
                        // After balancing this node, the ancestors may become unbalanced. So no break. Keep going.
                    else // BalanceAt() method will update the height.
                        node.UpdateHeight();
                }
            }
            return result;
        }

        /// <summary>
        /// Make the (un-balanced) AVL tree balanced again.
        /// </summary>
        /// <param name="binNode">First unbalanced node</param>
        /// <returns>The root of the subTree after rebalancing.</returns>
        protected override BinNode<T> BalanceAt(BinNode<T> binNode)
        {
            BinNode<T> child = TallerChild(binNode);
            BinNode<T> grandChild = TallerChild(child);
            BinNode<T> left, mid, right;
            BinNode<T>? subTree1, subTree2, subTree3, subTree4;
            if (child.IsLChild) 
            {
                right = binNode;
                subTree4 = binNode.R_Child;
                if (grandChild.IsLChild)
                {
                    left = grandChild;
                    mid = child;
                    subTree1 = left.L_Child;
                    subTree2 = left.R_Child;
                    subTree3 = mid.R_Child;
                }
                else
                {
                    left = child;
                    mid = grandChild;
                    subTree1 = left.L_Child;
                    subTree2 = mid.L_Child;
                    subTree3 = mid.R_Child;
                }
            }
            else // child.IsRChild
            {
                left = binNode;
                subTree1 = binNode.L_Child;
                if (grandChild.IsLChild)
                {
                    mid = grandChild;
                    right = child;
                    subTree2 = mid.L_Child;
                    subTree3 = mid.R_Child;
                    subTree4 = right.R_Child;
                }
                else // grandChild.IsRChild
                {
                    mid = child;
                    right = grandChild;
                    subTree2 = mid.L_Child;
                    subTree3 = right.L_Child;
                    subTree4 = right.R_Child;
                }
            }
            if ((mid.Parent = binNode.Parent) != null) // binNode is not root
                if(binNode.IsLChild)
                    mid.Parent.L_Child = mid;
                else // binNode.IsRChild
                    mid.Parent.R_Child = mid;
            else // binNode.IsRoot
                _root = mid;
            return Connect34(left, mid, right, subTree1, subTree2, subTree3, subTree4);
        }

        
    }
}
