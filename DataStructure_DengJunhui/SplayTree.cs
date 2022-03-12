using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_DJ
{
    public class SplayTree<T>: BinSearchTree<T> where T: IComparable<T>, IEquatable<T>
    {

        /// <summary>
        /// Splay the tree so that node will move to the root
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected BinNode<T>? Splay(BinNode<T> node)
        {
            if(node == null)
                return null;
            BinNode<T>? parent, grandParent;
            while ((parent = node.Parent)!= null && (grandParent = parent.Parent) != null)
            {
                node.Parent = grandParent.Parent;
                if (grandParent.Parent != null)
                    if (grandParent.IsLChild)
                        grandParent.Parent.L_Child = node;
                    else
                        grandParent.Parent.R_Child = node;
                else // grandParent is root
                    _root = node;
                if (parent.IsLChild)
                {
                    if (node.IsLChild) // zig-zig
                    {
                        parent.L_Child = node.R_Child;
                        if (parent.L_Child != null)
                            parent.L_Child.Parent = parent;
                        node.R_Child = parent;
                        parent.Parent = node;
                        //
                        grandParent.L_Child = parent.R_Child;
                        if(grandParent.L_Child!=null)
                            grandParent.L_Child.Parent = grandParent;
                        parent.R_Child = grandParent;
                        grandParent.Parent = parent;
                        
                    }
                    else //node.IsRChild. zag zig
                    {
                        Connect34(parent, node, grandParent, parent.L_Child, node.L_Child, node.R_Child, grandParent.R_Child);
                    }
                }
                else //parent.IsRChild
                {
                    if (node.IsLChild) // zig-zag
                        Connect34(grandParent, node, parent, grandParent.L_Child, node.L_Child, node.R_Child, parent.R_Child);
                    else // node.IsRChild. zag-zag
                    {
                        parent.R_Child = node.L_Child;
                        if(parent.R_Child != null)
                            parent.R_Child.Parent = parent;
                        parent.Parent = node;
                        node.L_Child = parent;
                        grandParent.R_Child = parent.L_Child;
                        if(grandParent.R_Child!=null)
                            grandParent.R_Child.Parent = grandParent;
                        grandParent.Parent = parent;
                        parent.L_Child = grandParent;
                    }
                }
                grandParent.UpdateHeight();
                parent.UpdateHeight();
                node.UpdateHeight();
            }
            if(parent != null) // node is not the root yet
            {
                if (node.IsLChild) //zig
                    Zig(parent);
                else
                    Zag(parent);
                UpdateHeightAbove(parent);
            }
            return node;
        }

    }
}
