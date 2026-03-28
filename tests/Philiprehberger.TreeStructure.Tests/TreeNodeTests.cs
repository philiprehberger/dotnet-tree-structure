namespace Philiprehberger.TreeStructure.Tests;

public class TreeNodeTests
{
    [Fact]
    public void Create_ReturnsRootNode()
    {
        var root = TreeNode<string>.Create("root");

        Assert.Equal("root", root.Value);
        Assert.True(root.IsRoot);
        Assert.Null(root.Parent);
        Assert.Empty(root.Children);
    }

    [Fact]
    public void AddChild_SetsParentAndAddsToChildren()
    {
        var root = TreeNode<string>.Create("root");

        var child = root.AddChild("child");

        Assert.Equal(root, child.Parent);
        Assert.Single(root.Children);
        Assert.Equal(child, root.Children[0]);
    }

    [Fact]
    public void Depth_RootIsZero()
    {
        var root = TreeNode<int>.Create(1);

        Assert.Equal(0, root.Depth);
    }

    [Fact]
    public void Depth_ChildIsOne()
    {
        var root = TreeNode<int>.Create(1);
        var child = root.AddChild(2);

        Assert.Equal(1, child.Depth);
    }

    [Fact]
    public void IsLeaf_NodeWithNoChildren_ReturnsTrue()
    {
        var root = TreeNode<int>.Create(1);

        Assert.True(root.IsLeaf);
    }

    [Fact]
    public void IsLeaf_NodeWithChildren_ReturnsFalse()
    {
        var root = TreeNode<int>.Create(1);
        root.AddChild(2);

        Assert.False(root.IsLeaf);
    }

    [Fact]
    public void Find_MatchingNode_ReturnsNode()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("target");

        var found = root.Find(v => v == "target");

        Assert.Equal(child, found);
    }

    [Fact]
    public void Find_NoMatch_ReturnsNull()
    {
        var root = TreeNode<string>.Create("root");

        var found = root.Find(v => v == "missing");

        Assert.Null(found);
    }

    [Fact]
    public void PathToRoot_ReturnsCorrectPath()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");
        var grandchild = child.AddChild("grandchild");

        var path = grandchild.PathToRoot();

        Assert.Equal(3, path.Count);
        Assert.Equal(grandchild, path[0]);
        Assert.Equal(child, path[1]);
        Assert.Equal(root, path[2]);
    }
}
