# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.1.1 (2026-03-22)

- Improve README compliance: remove Requirements section, simplify Development section, fix License format
- Add dates to changelog entries

## [0.1.0] - 2026-03-21

### Added

- `TreeNode<T>` generic tree node with parent/children references
- Depth-first and breadth-first traversal via `Traverse(TraversalMode)`
- `Find` and `FindAll` methods for searching nodes by predicate
- `PathToRoot` method for ancestor chain retrieval
- `TreeBuilder.FromFlat` for converting flat collections into trees
- `TreeExtensions` with `Flatten`, `Count`, `Height`, and `ToTreeString`
- `TraversalMode` enum (`DepthFirst`, `BreadthFirst`)

[0.1.0]: https://github.com/philiprehberger/dotnet-tree-structure/releases/tag/v0.1.0
