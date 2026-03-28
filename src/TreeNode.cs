using System.Text.Json;
using System.Text.Json.Serialization;

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
    /// Removes this node from its parent, detaching it and its subtree from the tree.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the node has no parent (is a root node).</exception>
    public void Remove()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException("Cannot remove a root node.");
        }

        Parent._children.Remove(this);
        Parent = null;
    }

    /// <summary>
    /// Moves this node and its subtree to a new parent, removing it from the current parent.
    /// </summary>
    /// <param name="newParent">The new parent node to attach to.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="newParent"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when attempting to move a root node or move a node under itself.</exception>
    public void MoveTo(TreeNode<T> newParent)
    {
        ArgumentNullException.ThrowIfNull(newParent);

        if (Parent is null)
        {
            throw new InvalidOperationException("Cannot move a root node.");
        }

        // Prevent moving a node under itself or its descendants
        var current = newParent;
        while (current is not null)
        {
            if (ReferenceEquals(current, this))
            {
                throw new InvalidOperationException("Cannot move a node under itself or its descendants.");
            }

            current = current.Parent;
        }

        Parent._children.Remove(this);
        Parent = newParent;
        newParent._children.Add(this);
    }

    /// <summary>
    /// Inserts this node before the specified sibling in the sibling's parent children list.
    /// The node is removed from its current parent if it has one.
    /// </summary>
    /// <param name="sibling">The sibling node to insert before.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="sibling"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the sibling has no parent.</exception>
    public void InsertBefore(TreeNode<T> sibling)
    {
        ArgumentNullException.ThrowIfNull(sibling);

        if (sibling.Parent is null)
        {
            throw new InvalidOperationException("Cannot insert before a root node.");
        }

        if (Parent is not null)
        {
            Parent._children.Remove(this);
        }

        var targetParent = sibling.Parent;
        var index = targetParent._children.IndexOf(sibling);
        Parent = targetParent;
        targetParent._children.Insert(index, this);
    }

    /// <summary>
    /// Inserts this node after the specified sibling in the sibling's parent children list.
    /// The node is removed from its current parent if it has one.
    /// </summary>
    /// <param name="sibling">The sibling node to insert after.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="sibling"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the sibling has no parent.</exception>
    public void InsertAfter(TreeNode<T> sibling)
    {
        ArgumentNullException.ThrowIfNull(sibling);

        if (sibling.Parent is null)
        {
            throw new InvalidOperationException("Cannot insert after a root node.");
        }

        if (Parent is not null)
        {
            Parent._children.Remove(this);
        }

        var targetParent = sibling.Parent;
        var index = targetParent._children.IndexOf(sibling);
        Parent = targetParent;
        targetParent._children.Insert(index + 1, this);
    }

    /// <summary>
    /// Traverses the tree starting from this node using the specified traversal mode.
    /// </summary>
    /// <param name="mode">The traversal strategy to use. Defaults to <see cref="TraversalMode.DepthFirst"/>.</param>
    /// <param name="maxDepth">The maximum depth to traverse. Depth 0 returns only this node, depth 1 includes direct children, and so on. Pass <c>null</c> for unlimited depth.</param>
    /// <returns>An enumerable sequence of tree nodes in traversal order.</returns>
    public IEnumerable<TreeNode<T>> Traverse(TraversalMode mode = TraversalMode.DepthFirst, int? maxDepth = null)
    {
        if (maxDepth.HasValue && maxDepth.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDepth), maxDepth, "Max depth must be non-negative.");
        }

        return mode switch
        {
            TraversalMode.DepthFirst => TraverseDepthFirst(maxDepth),
            TraversalMode.BreadthFirst => TraverseBreadthFirst(maxDepth),
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
        foreach (var node in TraverseDepthFirst(null))
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
        foreach (var node in TraverseDepthFirst(null))
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

    /// <summary>
    /// Finds the lowest common ancestor of two nodes by comparing their paths to the root.
    /// </summary>
    /// <param name="nodeA">The first node.</param>
    /// <param name="nodeB">The second node.</param>
    /// <returns>The deepest shared ancestor of <paramref name="nodeA"/> and <paramref name="nodeB"/>, or <c>null</c> if the nodes are in different trees.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="nodeA"/> or <paramref name="nodeB"/> is <c>null</c>.</exception>
    public static TreeNode<T>? FindLCA(TreeNode<T> nodeA, TreeNode<T> nodeB)
    {
        ArgumentNullException.ThrowIfNull(nodeA);
        ArgumentNullException.ThrowIfNull(nodeB);

        var pathA = nodeA.PathToRoot();
        var pathB = new HashSet<TreeNode<T>>(nodeB.PathToRoot(), ReferenceEqualityComparer.Instance);

        foreach (var ancestor in pathA)
        {
            if (pathB.Contains(ancestor))
            {
                return ancestor;
            }
        }

        return null;
    }

    /// <summary>
    /// Serializes this node and its subtree to a JSON string.
    /// Each node is represented as <c>{ "value": ..., "children": [...] }</c>.
    /// </summary>
    /// <param name="options">Optional JSON serializer options for value serialization.</param>
    /// <returns>A JSON string representing this node and all descendants.</returns>
    public string Serialize(JsonSerializerOptions? options = null)
    {
        var dto = ToSerializable();
        var resolvedOptions = options ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        return JsonSerializer.Serialize(dto, resolvedOptions);
    }

    /// <summary>
    /// Deserializes a JSON string into a tree structure.
    /// The JSON must follow the format <c>{ "value": ..., "children": [...] }</c>.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="options">Optional JSON serializer options for value deserialization.</param>
    /// <returns>The root <see cref="TreeNode{T}"/> of the deserialized tree.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="json"/> is <c>null</c>.</exception>
    /// <exception cref="JsonException">Thrown when the JSON is invalid or cannot be deserialized.</exception>
    public static TreeNode<T> Deserialize(string json, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(json);

        var resolvedOptions = options ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var dto = JsonSerializer.Deserialize<TreeNodeDto<T>>(json, resolvedOptions)
                  ?? throw new JsonException("Failed to deserialize tree node from JSON.");

        return FromSerializable(dto);
    }

    private TreeNodeDto<T> ToSerializable()
    {
        return new TreeNodeDto<T>
        {
            Value = Value,
            Children = _children.Select(c => c.ToSerializable()).ToList()
        };
    }

    private static TreeNode<T> FromSerializable(TreeNodeDto<T> dto)
    {
        var node = Create(dto.Value!);

        foreach (var childDto in dto.Children)
        {
            var child = FromSerializable(childDto);
            child.Parent = node;
            node._children.Add(child);
        }

        return node;
    }

    private IEnumerable<TreeNode<T>> TraverseDepthFirst(int? maxDepth)
    {
        var stack = new Stack<(TreeNode<T> Node, int Depth)>();
        stack.Push((this, 0));

        while (stack.Count > 0)
        {
            var (current, depth) = stack.Pop();
            yield return current;

            if (maxDepth.HasValue && depth >= maxDepth.Value)
            {
                continue;
            }

            for (var i = current._children.Count - 1; i >= 0; i--)
            {
                stack.Push((current._children[i], depth + 1));
            }
        }
    }

    private IEnumerable<TreeNode<T>> TraverseBreadthFirst(int? maxDepth)
    {
        var queue = new Queue<(TreeNode<T> Node, int Depth)>();
        queue.Enqueue((this, 0));

        while (queue.Count > 0)
        {
            var (current, depth) = queue.Dequeue();
            yield return current;

            if (maxDepth.HasValue && depth >= maxDepth.Value)
            {
                continue;
            }

            foreach (var child in current._children)
            {
                queue.Enqueue((child, depth + 1));
            }
        }
    }
}

/// <summary>
/// Data transfer object for tree node serialization.
/// </summary>
/// <typeparam name="T">The type of value stored in the node.</typeparam>
internal class TreeNodeDto<T>
{
    /// <summary>
    /// Gets or sets the value stored in the node.
    /// </summary>
    [JsonPropertyName("value")]
    public T? Value { get; set; }

    /// <summary>
    /// Gets or sets the child nodes.
    /// </summary>
    [JsonPropertyName("children")]
    public List<TreeNodeDto<T>> Children { get; set; } = [];
}
