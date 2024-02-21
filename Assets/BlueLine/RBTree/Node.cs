
public class RedBlackTreeNode<T>
{
    public int Index { get; set; }
    public T Value { get; set; }
    public bool IsRed { get; set; }
    public RedBlackTreeNode<T> Left { get; set; }
    public RedBlackTreeNode<T> Right { get; set; }
    public RedBlackTreeNode<T> Parent { get; set; }

    public RedBlackTreeNode(T value)
    {
        Value = value;
        Index = 0; // Mettre � 0 ou autre valeur par d�faut selon besoin
        IsRed = true; // Nouveau n�ud ins�r� est rouge par d�faut
        Left = null;
        Right = null;
        Parent = null;
    }

}
