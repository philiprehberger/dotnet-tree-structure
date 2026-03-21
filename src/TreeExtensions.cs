using System.Text;

namespace Philiprehberger.TreeStructure;

/// <summary>
/// Provides extension methods for <see cref="TreeNode{T}"/>.
/// </summary>
public static class TreeExtensions
{
    /// <summary>
    /// Flattens the tree into a sequence of values using depth-first traversal.
    /// </summary>
    /// <typeparam name="T">The type of value stored in the tree nodes.</typeparam>
    /// <param name="node">The root node to flatten from.</param>
    /// <returns>An enumerable sequence of values from all nodes in the tree.</returns>
    public static IEnumerable<T> Flatten<T>(this TreeNode<T> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return node.Traverse(TraversalMode.DepthFirst).Select(n => n.Value);
    }

    /// <summary>
    /// Counts the total number of nodes in the tree starting from this node.
    /// </summary>
    /// <typeparam name="T">The type of value stored in the tree nodes.</typeparam>
    /// <param name="node">The root node to count from.</param>
    /// <returns>The total number of nodes in the subtree.</returns>
    public static int Count<T>(this TreeNode<T> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return node.Traverse(TraversalMode.DepthFirst).Count();
    }

    /// <summary>
    /// Calculates the height of the tree starting from this node. A leaf node has a height of 0.
    /// </summary>
    /// <typeparam name="T">The type of value stored in the tree nodes.</typeparam>
    /// <param name="node">The root node to calculate height from.</param>
    /// <returns>The height of the subtree.</returns>
    public static int Height<T>(this TreeNode<T> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node.IsLeaf)
        {
            return 0;
        }

        var maxChildHeight = 0;

        foreach (var child in node.Children)
        {
            var childHeight = child.Height();

            if (childHeight > maxChildHeight)
            {
                maxChildHeight = childHeight;
            }
        }

        return maxChildHeight + 1;
    }

    /// <summary>
    /// Generates an indented text representation of the tree.
    /// </summary>
    /// <typeparam name="T">The type of value stored in the tree nodes.</typeparam>
    /// <param name="node">The root node to render.</param>
    /// <returns>A string containing the indented tree representation.</returns>
    public static string ToTreeString<T>(this TreeNode<T> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        var sb = new StringBuilder();
        AppendTreeString(sb, node, "", true);
        return sb.ToString().TrimEnd();
    }

    private static void AppendTreeString<T>(StringBuilder sb, TreeNode<T> node, string indent, bool isLast)
    {
        if (node.Parent is not null)
        {
            sb.Append(indent);
            sb.Append(isLast ? "└── " : "├── ");
            sb.AppendLine(node.Value?.ToString() ?? "(null)");
            indent += isLast ? "    " : "│   ";
        }
        else
        {
            sb.AppendLine(node.Value?.ToString() ?? "(null)");
        }

        for (var i = 0; i < node.Children.Count; i++)
        {
            AppendTreeString(sb, node.Children[i], indent, i == node.Children.Count - 1);
        }
    }
}
