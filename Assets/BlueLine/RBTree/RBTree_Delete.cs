using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RedBlackTree<T>
{
    private RedBlackTreeNode<T> DeleteRecursive(RedBlackTreeNode<T> node, T value)
    {
        if (node == null)
            return null;

        // Chercher l'élément à supprimer
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

            // Cas où le nœud à supprimer a été trouvé

            // Si le nœud a des enfants
            if (node.Left == null || node.Right == null)
            {
                RedBlackTreeNode<T> temp = node.Left ?? node.Right;

                // Si le nœud n'a pas d'enfant, ou un seul enfant
                if (temp == null)
                {
                    if (node.IsRed)
                    {
                        // Le nœud est rouge et n'a pas d'enfant, donc il peut être supprimé
                        return null;
                    }
                    else
                    {
                        // Le nœud est noir et n'a pas d'enfant, la suppression peut causer un déséquilibre
                        return node;
                    }
                }
                else
                {
                    // Le nœud a un enfant
                    return temp;
                }
            }
            else
            {
                // Le nœud a deux enfants
                RedBlackTreeNode<T> successor = GetMinimumValueNode(node.Right);
                node.Value = successor.Value;
                node.Right = DeleteRecursive(node.Right, successor.Value);
            }
        }

        BalanceTree(node);
        // Rééquilibrage de l'arbre après suppression
        // ...

        return node;
    }

    private RedBlackTreeNode<T> GetMinimumValueNode(RedBlackTreeNode<T> node)
    {
        RedBlackTreeNode<T> current = node;

        // Trouver le nœud le plus à gauche
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }
}
