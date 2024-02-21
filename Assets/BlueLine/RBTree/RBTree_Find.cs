using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RedBlackTree<T>
{
    public RedBlackTreeNode<T> FindIndexRecursive(int index, RedBlackTreeNode<T> root)
    {
        if (root.Index == index)
            return root;
        if (index < root.Index)
            return FindIndexRecursive(index, root.Left);
        else
            return FindIndexRecursive(index, root.Right);

    }

    private bool FindRecursive(T value, RedBlackTreeNode<T> root)
    {
        if (root == null)
            return false;

        if (root.Value.Equals(value))
            return true;

        bool foundInLeftSubtree = false, foundInRightSubtree = false;
        if (root.Left != null)
            foundInLeftSubtree = FindRecursive(value, root.Left);
        if (root.Right != null)
            foundInRightSubtree = FindRecursive(value, root.Right);

        return foundInLeftSubtree || foundInRightSubtree;
    }

    private int IndexOfRecursif(T value, RedBlackTreeNode<T> root)
    {
        if(root.Value.Equals(value))
            return root.Index;
        int gauche = -1, droite = -1;
        if (root.Left != null)
            gauche = IndexOfRecursif(value, root.Left);
        if (root.Right != null)
            droite = IndexOfRecursif(value, root.Right);

        return gauche != -1 ? gauche : droite != -1 ? droite : -1;
    }

    private T FindIndex(int index)
    {
        if (index < 0 || index > Count - 1)
        {
            Debug.LogError("Index out of range");
            return default(T); // Retourne la valeur par défaut de T si l'index est hors limites
        }
        else
        {
            //if (index % 2 == 1)
            //    index *= -1;
            RedBlackTreeNode<T> node = FindIndexRecursive(index, root);
            return node != null ? node.Value : default(T); // Retourne la valeur T du nœud trouvé ou la valeur par défaut de T si le nœud n'est pas trouvé
        }
    }
    // Méthode pour définir la valeur à l'index spécifié
    private void SetIndex(int index, T value)
    {
        RedBlackTreeNode<T> nodeToUpdate = FindNodeIndex(index);

        if (nodeToUpdate != null)
        {
            // Met à jour la valeur du nœud à l'index spécifié avec la nouvelle valeur
            nodeToUpdate.Value = value;
            // Assure-toi de mettre à jour correctement les autres propriétés de ton arbre Red-Black Tree après cette modification.
        }
    }

}
