using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RedBlackTree<T>
{
    private RedBlackTreeNode<T> DeleteRecursive(RedBlackTreeNode<T> node, T value)
    {
        if (node == null)
            return null;

        // Chercher l'�l�ment � supprimer
        if (Comparer<T>.Default.Compare(value, node.Value) < 0)
        {
            node.Left = DeleteRecursive(node.Left, value);
        }
        else if (Comparer<T>.Default.Compare(value, node.Value) > 0)
        {
            node.Right = DeleteRecursive(node.Right, value);
        }
        else
        {

            // Cas o� le n�ud � supprimer a �t� trouv�

            // Si le n�ud a des enfants
            if (node.Left == null || node.Right == null)
            {
                RedBlackTreeNode<T> temp = node.Left ?? node.Right;

                // Si le n�ud n'a pas d'enfant, ou un seul enfant
                if (temp == null)
                {
                    if (node.IsRed)
                    {
                        // Le n�ud est rouge et n'a pas d'enfant, donc il peut �tre supprim�
                        return null;
                    }
                    else
                    {
                        // Le n�ud est noir et n'a pas d'enfant, la suppression peut causer un d�s�quilibre
                        return node;
                    }
                }
                else
                {
                    // Le n�ud a un enfant
                    return temp;
                }
            }
            else
            {
                // Le n�ud a deux enfants
                RedBlackTreeNode<T> successor = GetMinimumValueNode(node.Right);
                node.Value = successor.Value;
                node.Right = DeleteRecursive(node.Right, successor.Value);
            }
        }

        BalanceTree(node);
        // R��quilibrage de l'arbre apr�s suppression
        // ...

        return node;
    }

    private RedBlackTreeNode<T> GetMinimumValueNode(RedBlackTreeNode<T> node)
    {
        RedBlackTreeNode<T> current = node;

        // Trouver le n�ud le plus � gauche
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }
}
