# Changelog

## 0.2.0 (2026-03-28)

- Add tree serialization with `Serialize()` and `Deserialize<T>(string)` using `System.Text.Json`
- Add subtree mutation methods: `Remove()`, `MoveTo(TreeNode<T>)`, `InsertBefore(TreeNode<T>)`, `InsertAfter(TreeNode<T>)`
- Add `FindLCA(TreeNode<T>, TreeNode<T>)` for lowest common ancestor lookup
- Add depth-limited traversal via optional `maxDepth` parameter on `Traverse()`
- Add unit tests with xUnit
- Add GitHub issue templates, dependabot config, and PR template
- Add Support section to README
- Update CI workflow to include test step

## 0.1.2 (2026-03-22)

- Fix README badge order to CI, NuGet, License
- Normalize changelog format

## 0.1.1 (2026-03-22)

- Improve README compliance: remove Requirements section, simplify Development section, fix License format
- Add dates to changelog entries

## 0.1.0 (2026-03-21)

- `TreeNode<T>` generic tree node with parent/children references
- Depth-first and breadth-first traversal via `Traverse(TraversalMode)`
- `Find` and `FindAll` methods for searching nodes by predicate
- `PathToRoot` method for ancestor chain retrieval
- `TreeBuilder.FromFlat` for converting flat collections into trees
- `TreeExtensions` with `Flatten`, `Count`, `Height`, and `ToTreeString`
- `TraversalMode` enum (`DepthFirst`, `BreadthFirst`)
