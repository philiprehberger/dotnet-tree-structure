namespace Philiprehberger.TreeStructure.Tests;

public class TreeNodeMutationTests
{
    [Fact]
    public void Remove_DetachesNodeFromParent()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");

        child.Remove();

        Assert.Empty(root.Children);
        Assert.Null(child.Parent);
        Assert.True(child.IsRoot);
    }

    [Fact]
    public void Remove_RootNode_ThrowsInvalidOperationException()
    {
        var root = TreeNode<string>.Create("root");

        Assert.Throws<InvalidOperationException>(() => root.Remove());
    }

    [Fact]
    public void Remove_PreservesSubtree()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");
        var grandchild = child.AddChild("grandchild");

        child.Remove();

        Assert.Single(child.Children);
        Assert.Equal(grandchild, child.Children[0]);
        Assert.Equal(child, grandchild.Parent);
    }

    [Fact]
    public void MoveTo_TransfersNodeToNewParent()
    {
        var root = TreeNode<string>.Create("root");
        var child1 = root.AddChild("child-1");
        var child2 = root.AddChild("child-2");
        var grandchild = child1.AddChild("grandchild");

        grandchild.MoveTo(child2);

        Assert.Empty(child1.Children);
        Assert.Single(child2.Children);
        Assert.Equal(child2, grandchild.Parent);
    }

    [Fact]
    public void MoveTo_NullParent_ThrowsArgumentNullException()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");

        Assert.Throws<ArgumentNullException>(() => child.MoveTo(null!));
    }

    [Fact]
    public void MoveTo_RootNode_ThrowsInvalidOperationException()
    {
        var root = TreeNode<string>.Create("root");
        var other = TreeNode<string>.Create("other");

        Assert.Throws<InvalidOperationException>(() => root.MoveTo(other));
    }

    [Fact]
    public void MoveTo_UnderSelf_ThrowsInvalidOperationException()
    {
        var root = TreeNode<string>.Create("root");
        var child = root.AddChild("child");
        var grandchild = child.AddChild("grandchild");

        Assert.Throws<InvalidOperationException>(() => child.MoveTo(grandchild));
    }

    [Fact]
    public void InsertBefore_PlacesNodeAtCorrectPosition()
    {
        var root = TreeNode<string>.Create("root");
        var child1 = root.AddChild("child-1");
        var child2 = root.AddChild("child-2");
        var newNode = TreeNode<string>.Create("new");

        newNode.InsertBefore(child2);

        Assert.Equal(3, root.Children.Count);
        Assert.Equal("child-1", root.Children[0].Value);
        Assert.Equal("new", root.Children[1].Value);
        Assert.Equal("child-2", root.Children[2].Value);
        Assert.Equal(root, newNode.Parent);
    }

    [Fact]
    public void InsertAfter_PlacesNodeAtCorrectPosition()
    {
        var root = TreeNode<string>.Create("root");
        var child1 = root.AddChild("child-1");
        var child2 = root.AddChild("child-2");
        var newNode = TreeNode<string>.Create("new");

        newNode.InsertAfter(child1);

        Assert.Equal(3, root.Children.Count);
        Assert.Equal("child-1", root.Children[0].Value);
        Assert.Equal("new", root.Children[1].Value);
        Assert.Equal("child-2", root.Children[2].Value);
        Assert.Equal(root, newNode.Parent);
    }

    [Fact]
    public void InsertBefore_RootSibling_ThrowsInvalidOperationException()
    {
        var root = TreeNode<string>.Create("root");
        var newNode = TreeNode<string>.Create("new");

        Assert.Throws<InvalidOperationException>(() => newNode.InsertBefore(root));
    }

    [Fact]
    public void InsertAfter_RootSibling_ThrowsInvalidOperationException()
    {
        var root = TreeNode<string>.Create("root");
        var newNode = TreeNode<string>.Create("new");

        Assert.Throws<InvalidOperationException>(() => newNode.InsertAfter(root));
    }

    [Fact]
    public void InsertBefore_MovesExistingNodeFromOldParent()
    {
        var root = TreeNode<string>.Create("root");
        var child1 = root.AddChild("child-1");
        var child2 = root.AddChild("child-2");
        var grandchild = child1.AddChild("grandchild");

        grandchild.InsertBefore(child2);

        Assert.Empty(child1.Children);
        Assert.Equal(3, root.Children.Count);
        Assert.Equal("grandchild", root.Children[1].Value);
    }
}
