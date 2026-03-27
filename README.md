# Philiprehberger.TreeStructure

[![CI](https://github.com/philiprehberger/dotnet-tree-structure/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-tree-structure/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.TreeStructure.svg)](https://www.nuget.org/packages/Philiprehberger.TreeStructure)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-tree-structure)](LICENSE)
[![Sponsor](https://img.shields.io/badge/sponsor-GitHub%20Sponsors-ec6cb9)](https://github.com/sponsors/philiprehberger)

Generic tree data structure with DFS/BFS traversal, path finding, LINQ enumeration, and flat-to-tree conversion.

## Installation

```bash
dotnet add package Philiprehberger.TreeStructure
```

## Usage

### Build a Tree

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

// Find and path
var found = root.Find(v => v == "grandchild-1");
var path = found?.PathToRoot(); // [grandchild-1, child-1, root]
```

### Flat-to-Tree Conversion

```csharp
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
| `TreeNode<T>` | `Traverse(TraversalMode)` | Enumerates nodes in DFS or BFS order |
| `TreeNode<T>` | `Find(predicate)` | Finds the first matching node |
| `TreeNode<T>` | `FindAll(predicate)` | Finds all matching nodes |
| `TreeNode<T>` | `PathToRoot()` | Returns nodes from this node to the root |
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

## License

MIT
