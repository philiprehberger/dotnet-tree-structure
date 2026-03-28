namespace Philiprehberger.TreeStructure.Tests;

public class TreeNodeLcaTests
{
    [Fact]
    public void FindLCA_SiblingsShareParent()
    {
        var root = TreeNode<string>.Create("root");
        var child1 = root.AddChild("child-1");
        var child2 = root.AddChild("child-2");

        var lca = TreeNode<string>.FindLCA(child1, child2);

        Assert.Equal(root, lca);
    }

    [Fact]
    public void FindLCA_NodeAndDescendant_ReturnsAncestor()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");
        var grandchild = child.AddChild("grandchild");

        var lca = TreeNode<string>.FindLCA(child, grandchild);

        Assert.Equal(child, lca);
    }

    [Fact]
    public void FindLCA_CousinNodes_ReturnsGrandparent()
    {
        var root = TreeNode<string>.Create("root");
        var child1 = root.AddChild("child-1");
        var child2 = root.AddChild("child-2");
        var grandchild1 = child1.AddChild("gc-1");
        var grandchild2 = child2.AddChild("gc-2");

        var lca = TreeNode<string>.FindLCA(grandchild1, grandchild2);

        Assert.Equal(root, lca);
    }

    [Fact]
    public void FindLCA_SameNode_ReturnsSelf()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");

        var lca = TreeNode<string>.FindLCA(child, child);

        Assert.Equal(child, lca);
    }

    [Fact]
    public void FindLCA_DifferentTrees_ReturnsNull()
    {
        var tree1 = TreeNode<string>.Create("tree1");
        var tree2 = TreeNode<string>.Create("tree2");

        var lca = TreeNode<string>.FindLCA(tree1, tree2);

        Assert.Null(lca);
    }

    [Fact]
    public void FindLCA_NullNodeA_ThrowsArgumentNullException()
    {
        var root = TreeNode<string>.Create("root");

        Assert.Throws<ArgumentNullException>(() => TreeNode<string>.FindLCA(null!, root));
    }

    [Fact]
    public void FindLCA_NullNodeB_ThrowsArgumentNullException()
    {
        var root = TreeNode<string>.Create("root");

        Assert.Throws<ArgumentNullException>(() => TreeNode<string>.FindLCA(root, null!));
    }
}
