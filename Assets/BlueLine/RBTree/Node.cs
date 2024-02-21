
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
        Index = 0; // Mettre à 0 ou autre valeur par défaut selon besoin
        IsRed = true; // Nouveau nœud inséré est rouge par défaut
        Left = null;
        Right = null;
        Parent = null;
    }

}
