# Pathfinding system 🧭

> **A tactical grid-based movement system featuring custom pathfinding algorithms (A\*, BFS) and a runtime level editor.**

---

### 🔗 Links
[🎬 Watch video (YouTube)](https://youtu.be/Prk-Y9uYvgU)

---

### 📝 About
**Pathfinding system** is a technical demo implementing turn-based tactical movement logic on a square grid. The core objective was to create a robust architecture where units navigate through different terrain types (traversable, obstacles, covers) while respecting movement and attack ranges.

The project features a fully functional **runtime map editor**, allowing users to generate grids, paint terrain types, and place units to test pathfinding behavior in real-time.

### ✨ Key features
* **Tactical grid logic:** Orthogonal movement system with support for varying terrain types (traversable, cover, obstacles).
* **Runtime map editor:** Tools for procedural map generation, tile painting, and unit placement during play mode.
* **Visual feedback:** Real-time visualization of valid paths and "out of range" segments using line renderers and tile highlighting.
* **Unit interaction:** Implementation of movement and attack ranges with distinct logic for navigation and combat positioning.

### ⚙️ Technical highlights

The system focuses on efficiency and using modern C# features.

* **A\* algorithm (Movement):**
    Used for unit navigation to specific target tiles. A* was chosen as the optimal solution for finding the shortest path on a weighted graph with a known destination. It efficiently handles obstacle avoidance using heuristics.

* **Breadth-first search / BFS (Attack logic):**
    Implemented for calculating attack ranges and finding optimal firing positions. Unlike A*, BFS naturally explores the grid layer-by-layer (flood fill), making it superior for finding *all* reachable tiles within a specific range.

* **Dynamic grid management:**
    The grid system is designed for runtime manipulation. Tile states can be modified on the fly, immediately updating both the visual representation (materials) and the internal pathfinding data without scene reloads.

* **Modern C# Syntax:**
    The codebase utilizes modern C# features such as **Tuples** for coordinate handling and **Switch Expressions** for clean state management logic.

### 🛠️ Used
* Unity 6 (6000.0.33f1)
* Unity Starter Assets (Character controller base)
