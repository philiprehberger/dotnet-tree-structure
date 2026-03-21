namespace Philiprehberger.TreeStructure;

/// <summary>
/// Provides methods for constructing trees from flat data sources.
/// </summary>
public static class TreeBuilder
{
    /// <summary>
    /// Builds a tree from a flat list of items using key and parent-key selectors.
    /// </summary>
    /// <typeparam name="T">The type of items in the flat list.</typeparam>
    /// <typeparam name="TKey">The type of the key used to identify items and their parent relationships.</typeparam>
    /// <param name="items">The flat collection of items to convert into a tree.</param>
    /// <param name="keySelector">A function to extract the unique key from each item.</param>
    /// <param name="parentKeySelector">A function to extract the parent key from each item. Returns <c>null</c> for root items.</param>
    /// <returns>The root <see cref="TreeNode{T}"/> of the constructed tree.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/>, <paramref name="keySelector"/>, or <paramref name="parentKeySelector"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no root item is found or multiple root items exist.</exception>
    public static TreeNode<T> FromFlat<T, TKey>(
        IEnumerable<T> items,
        Func<T, TKey> keySelector,
        Func<T, TKey?> parentKeySelector)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(keySelector);
        ArgumentNullException.ThrowIfNull(parentKeySelector);

        var itemList = items.ToList();
        var nodeMap = new Dictionary<TKey, TreeNode<T>>();
        var parentMap = new Dictionary<TKey, TKey>();
        TreeNode<T>? root = null;

        // Create nodes for all items
        foreach (var item in itemList)
        {
            var key = keySelector(item);
            var node = TreeNode<T>.Create(item);
            nodeMap[key] = node;

            var parentKey = parentKeySelector(item);

            if (parentKey is null)
            {
                if (root is not null)
                {
                    throw new InvalidOperationException("Multiple root items found. Exactly one item must have a null parent key.");
                }

                root = node;
            }
            else
            {
                parentMap[key] = parentKey;
            }
        }

        if (root is null)
        {
            throw new InvalidOperationException("No root item found. Exactly one item must have a null parent key.");
        }

        // Build parent-child relationships
        foreach (var (key, parentKey) in parentMap)
        {
            if (!nodeMap.TryGetValue(parentKey, out var parentNode))
            {
                throw new InvalidOperationException($"Parent with key '{parentKey}' not found for item with key '{key}'.");
            }

            var childValue = nodeMap[key].Value;
            parentNode.AddChild(childValue);
        }

        return root;
    }
}
