# Philiprehberger.TreeStructure

[![CI](https://github.com/philiprehberger/dotnet-tree-structure/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-tree-structure/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.TreeStructure.svg)](https://www.nuget.org/packages/Philiprehberger.TreeStructure)
[![GitHub release](https://img.shields.io/github/v/release/philiprehberger/dotnet-tree-structure)](https://github.com/philiprehberger/dotnet-tree-structure/releases)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-tree-structure)](https://github.com/philiprehberger/dotnet-tree-structure/commits/main)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-tree-structure)](LICENSE)
[![Bug Reports](https://img.shields.io/github/issues/philiprehberger/dotnet-tree-structure/bug)](https://github.com/philiprehberger/dotnet-tree-structure/issues?q=is%3Aissue+is%3Aopen+label%3Abug)
[![Feature Requests](https://img.shields.io/github/issues/philiprehberger/dotnet-tree-structure/enhancement)](https://github.com/philiprehberger/dotnet-tree-structure/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)
[![Sponsor](https://img.shields.io/badge/sponsor-GitHub%20Sponsors-ec6cb9)](https://github.com/sponsors/philiprehberger)

Generic tree data structure with traversal, serialization, subtree mutation, lowest common ancestor, and flat-to-tree conversion.

## Installation

```bash
dotnet add package Philiprehberger.TreeStructure
```

## Usage

```csharp
using Philiprehberger.TreeStructure;

var root = TreeNode<string>.Create("root");
var child1 = root.AddChild("child-1");
var child2 = root.AddChild("child-2");
var grandchild = child1.AddChild("grandchild-1");

Console.WriteLine(root.IsRoot);       // True
Console.WriteLine(grandchild.IsLeaf); // True
Console.WriteLine(grandchild.Depth);  // 2
```

### Traverse

```csharp
using Philiprehberger.TreeStructure;

// Depth-first (default)
foreach (var node in root.Traverse())
{
    Console.WriteLine(node.Value);
}

// Breadth-first
foreach (var node in root.Traverse(TraversalMode.BreadthFirst))
{
    Console.WriteLine(node.Value);
}

// Depth-limited traversal (root + children only)
foreach (var node in root.Traverse(TraversalMode.DepthFirst, maxDepth: 1))
{
    Console.WriteLine(node.Value);
}

// Find and path
var found = root.Find(v => v == "grandchild-1");
var path = found?.PathToRoot(); // [grandchild-1, child-1, root]
```

### Flat-to-Tree Conversion

```csharp
using Philiprehberger.TreeStructure;

var items = new[]
{
    new { Id = 1, ParentId = (int?)null, Name = "Root" },
    new { Id = 2, ParentId = (int?)1,    Name = "Child A" },
    new { Id = 3, ParentId = (int?)1,    Name = "Child B" },
    new { Id = 4, ParentId = (int?)2,    Name = "Grandchild" },
};

var tree = TreeBuilder.FromFlat(items, x => x.Id, x => x.ParentId);
Console.WriteLine(tree.ToTreeString());
```

### Serialization

```csharp
using Philiprehberger.TreeStructure;

var root = TreeNode<string>.Create("root");
root.AddChild("child-1");
root.AddChild("child-2");

// Serialize to JSON
string json = root.Serialize();
// {"value":"root","children":[{"value":"child-1","children":[]},{"value":"child-2","children":[]}]}

// Deserialize back to a tree
var restored = TreeNode<string>.Deserialize(json);
```

### Subtree Mutation

```csharp
using Philiprehberger.TreeStructure;

var root = TreeNode<string>.Create("root");
var child1 = root.AddChild("child-1");
var child2 = root.AddChild("child-2");
var grandchild = child1.AddChild("grandchild");

// Move a node to a new parent
grandchild.MoveTo(child2);

// Remove a node from the tree
child1.Remove();

// Positional insertion
var newNode = TreeNode<string>.Create("new");
newNode.InsertBefore(child2);
newNode.InsertAfter(child2);
```

### Lowest Common Ancestor

```csharp
using Philiprehberger.TreeStructure;

var root = TreeNode<string>.Create("root");
var child1 = root.AddChild("child-1");
var child2 = root.AddChild("child-2");
var gc1 = child1.AddChild("gc-1");
var gc2 = child2.AddChild("gc-2");

var lca = TreeNode<string>.FindLCA(gc1, gc2); // root
var lca2 = TreeNode<string>.FindLCA(child1, gc1); // child-1
```

## API

| Type | Member | Description |
|------|--------|-------------|
| `TreeNode<T>` | `Create(T value)` | Creates a new root node |
| `TreeNode<T>` | `AddChild(T value)` | Adds a child node and returns it |
| `TreeNode<T>` | `Value` | Gets the stored value |
| `TreeNode<T>` | `Parent` | Gets the parent node or `null` |
| `TreeNode<T>` | `Children` | Gets the read-only list of children |
| `TreeNode<T>` | `IsRoot` / `IsLeaf` | Checks root or leaf status |
| `TreeNode<T>` | `Depth` | Gets the depth from the root |
| `TreeNode<T>` | `Traverse(TraversalMode, int?)` | Enumerates nodes in DFS or BFS order with optional depth limit |
| `TreeNode<T>` | `Find(predicate)` | Finds the first matching node |
| `TreeNode<T>` | `FindAll(predicate)` | Finds all matching nodes |
| `TreeNode<T>` | `PathToRoot()` | Returns nodes from this node to the root |
| `TreeNode<T>` | `Remove()` | Detaches node from its parent |
| `TreeNode<T>` | `MoveTo(TreeNode<T>)` | Moves node and subtree to a new parent |
| `TreeNode<T>` | `InsertBefore(TreeNode<T>)` | Inserts node before the specified sibling |
| `TreeNode<T>` | `InsertAfter(TreeNode<T>)` | Inserts node after the specified sibling |
| `TreeNode<T>` | `Serialize(JsonSerializerOptions?)` | Serializes tree to JSON string |
| `TreeNode<T>` | `Deserialize(string, JsonSerializerOptions?)` | Deserializes JSON string to tree |
| `TreeNode<T>` | `FindLCA(TreeNode<T>, TreeNode<T>)` | Finds lowest common ancestor of two nodes |
| `TreeBuilder` | `FromFlat<T, TKey>(...)` | Builds a tree from a flat collection |
| `TreeExtensions` | `Flatten<T>(node)` | Flattens tree values into a sequence |
| `TreeExtensions` | `Count<T>(node)` | Counts total nodes in the subtree |
| `TreeExtensions` | `Height<T>(node)` | Calculates the height of the subtree |
| `TreeExtensions` | `ToTreeString<T>(node)` | Renders an indented text representation |
| `TraversalMode` | `DepthFirst` / `BreadthFirst` | Traversal strategy enum values |

## Development

```bash
dotnet build src/Philiprehberger.TreeStructure.csproj --configuration Release
```

## Support

If you find this package useful, consider giving it a star on GitHub — it helps motivate continued maintenance and development.

[![LinkedIn](https://img.shields.io/badge/Philip%20Rehberger-LinkedIn-0A66C2?logo=linkedin)](https://www.linkedin.com/in/philiprehberger)
[![More packages](https://img.shields.io/badge/more-open%20source%20packages-blue)](https://philiprehberger.com/open-source-packages)

## License

[MIT](LICENSE)
