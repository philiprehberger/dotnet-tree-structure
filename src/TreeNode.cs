namespace Philiprehberger.TreeStructure;

/// <summary>
/// Represents a node in a generic tree data structure.
/// </summary>
/// <typeparam name="T">The type of value stored in the node.</typeparam>
public class TreeNode<T>
{
    private readonly List<TreeNode<T>> _children = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeNode{T}"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value to store in this node.</param>
    private TreeNode(T value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the value stored in this node.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Gets the parent node, or <c>null</c> if this is the root node.
    /// </summary>
    public TreeNode<T>? Parent { get; private set; }

    /// <summary>
    /// Gets the children of this node.
    /// </summary>
    public IReadOnlyList<TreeNode<T>> Children => _children.AsReadOnly();

    /// <summary>
    /// Gets a value indicating whether this node is the root of the tree (has no parent).
    /// </summary>
    public bool IsRoot => Parent is null;

    /// <summary>
    /// Gets a value indicating whether this node is a leaf (has no children).
    /// </summary>
    public bool IsLeaf => _children.Count == 0;

    /// <summary>
    /// Gets the depth of this node in the tree. The root node has a depth of 0.
    /// </summary>
    public int Depth
    {
        get
        {
            var depth = 0;
            var current = Parent;

            while (current is not null)
            {
                depth++;
                current = current.Parent;
            }

            return depth;
        }
    }

    /// <summary>
    /// Creates a new root tree node with the specified value.
    /// </summary>
    /// <param name="value">The value to store in the root node.</param>
    /// <returns>A new <see cref="TreeNode{T}"/> instance.</returns>
    public static TreeNode<T> Create(T value)
    {
        return new TreeNode<T>(value);
    }

    /// <summary>
    /// Adds a child node with the specified value to this node.
    /// </summary>
    /// <param name="value">The value to store in the child node.</param>
    /// <returns>The newly created child <see cref="TreeNode{T}"/>.</returns>
    public TreeNode<T> AddChild(T value)
    {
        var child = new TreeNode<T>(value) { Parent = this };
        _children.Add(child);
        return child;
    }

    /// <summary>
    /// Traverses the tree starting from this node using the specified traversal mode.
    /// </summary>
    /// <param name="mode">The traversal strategy to use. Defaults to <see cref="TraversalMode.DepthFirst"/>.</param>
    /// <returns>An enumerable sequence of tree nodes in traversal order.</returns>
    public IEnumerable<TreeNode<T>> Traverse(TraversalMode mode = TraversalMode.DepthFirst)
    {
        return mode switch
        {
            TraversalMode.DepthFirst => TraverseDepthFirst(),
            TraversalMode.BreadthFirst => TraverseBreadthFirst(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown traversal mode.")
        };
    }

    /// <summary>
    /// Finds the first node matching the specified predicate using depth-first traversal.
    /// </summary>
    /// <param name="predicate">A function to test each node's value.</param>
    /// <returns>The first matching <see cref="TreeNode{T}"/>, or <c>null</c> if no match is found.</returns>
    public TreeNode<T>? Find(Func<T, bool> predicate)
    {
        foreach (var node in TraverseDepthFirst())
        {
            if (predicate(node.Value))
            {
                return node;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds all nodes matching the specified predicate using depth-first traversal.
    /// </summary>
    /// <param name="predicate">A function to test each node's value.</param>
    /// <returns>An enumerable sequence of matching tree nodes.</returns>
    public IEnumerable<TreeNode<T>> FindAll(Func<T, bool> predicate)
    {
        foreach (var node in TraverseDepthFirst())
        {
            if (predicate(node.Value))
            {
                yield return node;
            }
        }
    }

    /// <summary>
    /// Returns the path from this node to the root node.
    /// </summary>
    /// <returns>A list of nodes starting from this node and ending at the root.</returns>
    public IReadOnlyList<TreeNode<T>> PathToRoot()
    {
        var path = new List<TreeNode<T>>();
        TreeNode<T>? current = this;

        while (current is not null)
        {
            path.Add(current);
            current = current.Parent;
        }

        return path.AsReadOnly();
    }

    private IEnumerable<TreeNode<T>> TraverseDepthFirst()
    {
        var stack = new Stack<TreeNode<T>>();
        stack.Push(this);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            yield return current;

            for (var i = current._children.Count - 1; i >= 0; i--)
            {
                stack.Push(current._children[i]);
            }
        }
    }

    private IEnumerable<TreeNode<T>> TraverseBreadthFirst()
    {
        var queue = new Queue<TreeNode<T>>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            foreach (var child in current._children)
            {
                queue.Enqueue(child);
            }
        }
    }
}
