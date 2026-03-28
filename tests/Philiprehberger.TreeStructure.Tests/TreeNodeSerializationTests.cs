using System.Text.Json;

namespace Philiprehberger.TreeStructure.Tests;

public class TreeNodeSerializationTests
{
    [Fact]
    public void Serialize_SingleNode_ReturnsValidJson()
    {
        var root = TreeNode<string>.Create("root");

        var json = root.Serialize();

        var doc = JsonDocument.Parse(json);
        Assert.Equal("root", doc.RootElement.GetProperty("value").GetString());
        Assert.Empty(doc.RootElement.GetProperty("children").EnumerateArray().ToList());
    }

    [Fact]
    public void Serialize_TreeWithChildren_IncludesAllNodes()
    {
        var root = TreeNode<string>.Create("root");
        root.AddChild("child-1");
        root.AddChild("child-2");

        var json = root.Serialize();

        var doc = JsonDocument.Parse(json);
        var children = doc.RootElement.GetProperty("children").EnumerateArray().ToList();
        Assert.Equal(2, children.Count);
        Assert.Equal("child-1", children[0].GetProperty("value").GetString());
        Assert.Equal("child-2", children[1].GetProperty("value").GetString());
    }

    [Fact]
    public void Deserialize_ValidJson_ReconstructsTree()
    {
        var json = """{"value":"root","children":[{"value":"child-1","children":[]},{"value":"child-2","children":[{"value":"grandchild","children":[]}]}]}""";

        var root = TreeNode<string>.Deserialize(json);

        Assert.Equal("root", root.Value);
        Assert.Equal(2, root.Children.Count);
        Assert.Equal("child-1", root.Children[0].Value);
        Assert.Equal("child-2", root.Children[1].Value);
        Assert.Single(root.Children[1].Children);
        Assert.Equal("grandchild", root.Children[1].Children[0].Value);
    }

    [Fact]
    public void Serialize_ThenDeserialize_ProducesEquivalentTree()
    {
        var root = TreeNode<int>.Create(1);
        var child = root.AddChild(2);
        root.AddChild(3);
        child.AddChild(4);

        var json = root.Serialize();
        var restored = TreeNode<int>.Deserialize(json);

        Assert.Equal(1, restored.Value);
        Assert.Equal(2, restored.Children.Count);
        Assert.Equal(2, restored.Children[0].Value);
        Assert.Equal(3, restored.Children[1].Value);
        Assert.Single(restored.Children[0].Children);
        Assert.Equal(4, restored.Children[0].Children[0].Value);
    }

    [Fact]
    public void Deserialize_ParentChildRelationships_AreCorrect()
    {
        var json = """{"value":"root","children":[{"value":"child","children":[]}]}""";

        var root = TreeNode<string>.Deserialize(json);

        Assert.True(root.IsRoot);
        Assert.Equal(root, root.Children[0].Parent);
    }

    [Fact]
    public void Deserialize_NullJson_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => TreeNode<string>.Deserialize(null!));
    }
}
