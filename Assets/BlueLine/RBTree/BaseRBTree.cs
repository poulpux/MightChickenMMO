using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public partial class RedBlackTree<T> 
{
    [SerializeField]
    public RedBlackTreeNode<T> root;
    [HideInInspector] public int Count;

    [SerializeField]
    private List<T> ValueToAdd = new List<T>(); // Utilisation d'un tableau s�rialis�

    public RedBlackTree()
    {
        root = null;
    }

    public void Add(T value)
    {
        RedBlackTreeNode<T> newNode = new RedBlackTreeNode<T>(value);
        //newNode.Index = Count%2 == 0 ? Count : -Count;
        newNode.Index = Count;
        if (root == null)
        {
            root = newNode;
            root.IsRed = false; // La racine est toujours noire
            Count++;
        }
        else
        {
            InsertRecursive(root, newNode.Index,newNode);
        }
    }
    public void AddInspector()
    {
        foreach (var value in ValueToAdd)
        {
            Add(value);
        }
    }
    public T this[int index]
    {
        get
        {
            return FindIndex(index);
        }
        set
        {
            SetIndex(index, value); // Appelle une m�thode pour d�finir la valeur � l'index sp�cifi�
        }
    }

    public int IndexOf(T value)
    {
        return IndexOfRecursif(value, root);
    }

    private RedBlackTreeNode<T> FindNodeIndex(int index)
    {
        if (index < 0 || index > Count - 1)
        {
            Debug.LogError("Index out of range");
            return new RedBlackTreeNode<T>(default(T));


        }
        else
        {
            RedBlackTreeNode<T> node = FindIndexRecursive(index, root);
            return node != null ? node : new RedBlackTreeNode<T>(default(T)); // Retourne la valeur T du n�ud trouv� ou la valeur par d�faut de T si le n�ud n'est pas trouv�
        }
    }
    public bool Find(T value)
    {
        return FindRecursive(value, root);
    }
    public void Delete(T value)
    {
        root = DeleteRecursive(root, value);
        Count--;
    }
    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal(root).GetEnumerator();
    }
}