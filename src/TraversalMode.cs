namespace Philiprehberger.TreeStructure;

/// <summary>
/// Specifies the traversal strategy for navigating tree nodes.
/// </summary>
public enum TraversalMode
{
    /// <summary>
    /// Depth-first traversal (pre-order). Visits each node before its children.
    /// </summary>
    DepthFirst,

    /// <summary>
    /// Breadth-first traversal (level-order). Visits all nodes at each depth before moving deeper.
    /// </summary>
    BreadthFirst
}
