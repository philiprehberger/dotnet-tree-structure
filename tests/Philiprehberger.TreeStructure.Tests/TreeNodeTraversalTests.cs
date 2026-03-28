namespace Philiprehberger.TreeStructure.Tests;

public class TreeNodeTraversalTests
{
    [Fact]
    public void Traverse_DepthFirst_MaxDepthZero_ReturnsOnlyRoot()
    {
        var root = TreeNode<string>.Create("root");
        root.AddChild("child-1");
        root.AddChild("child-2");

        var result = root.Traverse(TraversalMode.DepthFirst, maxDepth: 0).ToList();

        Assert.Single(result);
        Assert.Equal("root", result[0].Value);
    }

    [Fact]
    public void Traverse_DepthFirst_MaxDepthOne_ReturnsRootAndChildren()
    {
        var root = TreeNode<string>.Create("root");
        var child1 = root.AddChild("child-1");
        root.AddChild("child-2");
        child1.AddChild("grandchild");

        var result = root.Traverse(TraversalMode.DepthFirst, maxDepth: 1).ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal("root", result[0].Value);
        Assert.Equal("child-1", result[1].Value);
        Assert.Equal("child-2", result[2].Value);
    }

    [Fact]
    public void Traverse_BreadthFirst_MaxDepthZero_ReturnsOnlyRoot()
    {
        var root = TreeNode<string>.Create("root");
        root.AddChild("child-1");

        var result = root.Traverse(TraversalMode.BreadthFirst, maxDepth: 0).ToList();

        Assert.Single(result);
        Assert.Equal("root", result[0].Value);
    }

    [Fact]
    public void Traverse_BreadthFirst_MaxDepthOne_ReturnsRootAndChildren()
    {
        var root = TreeNode<string>.Create("root");
        root.AddChild("child-1");
        root.AddChild("child-2");
        root.Children[0].AddChild("grandchild");

        var result = root.Traverse(TraversalMode.BreadthFirst, maxDepth: 1).ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal("root", result[0].Value);
        Assert.Equal("child-1", result[1].Value);
        Assert.Equal("child-2", result[2].Value);
    }

    [Fact]
    public void Traverse_NullMaxDepth_ReturnsAllNodes()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");
        child.AddChild("grandchild");

        var result = root.Traverse(TraversalMode.DepthFirst, maxDepth: null).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Traverse_NegativeMaxDepth_ThrowsArgumentOutOfRangeException()
    {
        var root = TreeNode<string>.Create("root");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            root.Traverse(TraversalMode.DepthFirst, maxDepth: -1).ToList());
    }

    [Fact]
    public void Traverse_DefaultMaxDepth_IsUnlimited()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");
        var grandchild = child.AddChild("grandchild");
        grandchild.AddChild("great-grandchild");

        var result = root.Traverse().ToList();

        Assert.Equal(4, result.Count);
    }
}
