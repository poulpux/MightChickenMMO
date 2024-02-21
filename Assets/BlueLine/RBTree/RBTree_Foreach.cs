using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RedBlackTree<T>
{
    private IEnumerable<T> InOrderTraversal(RedBlackTreeNode<T> node)
    {
        if (node != null)
        {
            if (node.Left != null)
            {
                foreach (var item in InOrderTraversal(node.Left))
                    yield return item;
            }

            yield return node.Value; // Déplacement du retour de nœud après le traitement du nœud gauche

            if (node.Right != null)
            {
                foreach (var item in InOrderTraversal(node.Right))
                    yield return item;
            }
        }
    }
}
