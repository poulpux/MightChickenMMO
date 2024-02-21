using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RedBlackTree<T>
{
    private void InsertRecursive(RedBlackTreeNode<T> root, int value, RedBlackTreeNode<T> node)
    {
        if (value < root.Index)
        {
            if (root.Left == null)
            {
                root.Left = node;
                root.Left.Parent = root;
                BalanceTree(root.Left);
                Count++;
            }
            else
            {
                InsertRecursive(root.Left, value, node);
            }
        }
        else if (value > root.Index)
        {
            if (root.Right == null)
            {
                root.Right = node;
                root.Right.Parent = root;
                BalanceTree(root.Right);
                Count++;
            }
            else
            {
                InsertRecursive(root.Right, value, node);
            }
        }
        else
        {
            Debug.Log("Value already exists in the tree: " + value);
        }
    }

    private void BalanceTree(RedBlackTreeNode<T> node)
    {
        while (node.Parent != null && node.Parent.IsRed)
        {
            if (node.Parent == node.Parent.Parent.Left)
            {
                RedBlackTreeNode<T> uncle = node.Parent.Parent.Right;

                if (uncle != null && uncle.IsRed)
                {
                    node.Parent.IsRed = false;
                    uncle.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Right)
                    {
                        node = node.Parent;
                        RotateLeft(node);
                    }

                    node.Parent.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    RotateRight(node.Parent.Parent);
                }
            }
            else
            {
                RedBlackTreeNode<T> uncle = node.Parent.Parent.Left;

                if (uncle != null && uncle.IsRed)
                {
                    node.Parent.IsRed = false;
                    uncle.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Left)
                    {
                        node = node.Parent;
                        RotateRight(node);
                    }

                    node.Parent.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    RotateLeft(node.Parent.Parent);
                }
            }
        }

        root.IsRed = false;
    }

    private void RotateLeft(RedBlackTreeNode<T> node)
    {
        if (node != null && node.Right != null)
        {
            RedBlackTreeNode<T> rightChild = node.Right;
            node.Right = rightChild.Left;

            if (rightChild.Left != null)
            {
                rightChild.Left.Parent = node;
            }

            rightChild.Parent = node.Parent;

            if (node.Parent == null)
            {
                root = rightChild;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = rightChild;
            }
            else
            {
                node.Parent.Right = rightChild;
            }

            rightChild.Left = node;
            node.Parent = rightChild;
        }
    }

    private void RotateRight(RedBlackTreeNode<T> node)
    {
        if (node != null && node.Left != null)
        {
            RedBlackTreeNode<T> leftChild = node.Left;
            node.Left = leftChild.Right;

            if (leftChild.Right != null)
            {
                leftChild.Right.Parent = node;
            }

            leftChild.Parent = node.Parent;

            if (node.Parent == null)
            {
                root = leftChild;
            }
            else if (node == node.Parent.Right)
            {
                node.Parent.Right = leftChild;
            }
            else
            {
                node.Parent.Left = leftChild;
            }

            leftChild.Right = node;
            node.Parent = leftChild;
        }
    }


}
