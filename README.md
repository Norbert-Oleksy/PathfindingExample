# PathfindingExample
Recruitment task<br>
Into the Unknown<br>
[General Game Developer - Unity](https://www.skillshot.pl/jobs/37156-general-game-developer-unity-at-into-the-unknown)

## Video
The video shows how the application works in practice

[Watch video](https://youtu.be/Prk-Y9uYvgU)

## Quick Overview
### Game flow
The application starts in the editor menu, where we can edit various parameters.<br>
The first tab, "Generator", allows us to set the size of the board.<br>
By pressing the GENERATE button, a board is generated based on the specified width and height.<br>

The next tab is "Paint".<br>
When the map is generated, each tile starts as a Traversable type.<br>
In this tab, we can choose which tile type we want to set, and then by clicking on them in the 3D space, we change their type to the selected one.<br>

The last tab of the editor is "Placement".<br>
Here we can set the player’s movement and attack range,<br>
as well as place the player or an enemy on the map.<br>

Then, all we need to do is press Play to start testing.<br>
In this mode, clicking on tiles allows us to move the player or attack an enemy.<br>
### Controls
- **Arrow Keys** – Move the camera
- **Backspace** – Reset camera postion
- **Mouse Scroll** – Zoom in/out
- **Left Click** – Interact with objects and UI
- **Alt + F4** – Close the application

## Task
### Objective
- the map should consist of square tiles that are either traversable (0), obstacle (1) or a cover (2) and create an orthogonally connected grid (i.e. each inner tile has 4 neighbours, edge tile has 3 neighbours and corner tile has 2 neighbours). you can move from each tile in one of 4 directions (N, S, E, W) unless you're on an edge of the map or there's an obstacle blocking your way. each move between neighbouring tiles has the same "cost" (1).
- map should contain a player unit and an enemy unit. They can only be placed on traversable tile.
- unit may only move through traversable tile and attack through traversable and cover tiles.
- player unit has MoveRange and AttackRange parameters.
- player unit may only move to other tile if move path to that tile is shorter than MoveRange and attack enemy unit only if attack path to enemy is shorter than AttackRange.
- to display units use default Unity character model
- the goal of the exercise is to use a pathfinding algorithm and architecture optimally suited for the task. please explain what algorithm you've chosen and why? if you've made any adjustments to its standard implementation also explain what and why?
#### The end user should be able to:
- adjust the size of the map (either via a config file or during runtime - i.e. in an edit mode of the demo)
- adjust the placement of obstacles and units and player unit parameters(preferably during runtime)
- select traversable tile on the map and be shown a optimal path between player unit and it. If tile contains enemy show attack path instead. If path is too long (out of MoveRange or AttackRange) show full path and indicate the "out of range" segment
- freely look around the map
### Extra Credit
- if possible move path is selected add option to move unit to destination on click. make the default Unity character model run the path on the map
- if possible attack path is selected add option to destroy enemy on click. If selecting tile with enemy unit and it is out of attack range, show path to closest tile from which unit can attack and an attack path from that tile. If both path are possible click should move unit to attack position and then enemy should be destroyed.

### Answers
Please explain what algorithm you've chosen and why? if you've made any adjustments to its standard implementation also explain what and why?<br>
I chose A* for movement pathfinding and Breadth-First Search (BFS) for handling attack pathfinding.<br>

The A* algorithm for movement was chosen because in this mechanic I wanted to obtain the shortest path to a selected point in the most efficient way.<br>
This is exactly what A* offers, as it is optimal for shortest pathfinding on a grid with uniform movement cost.<br>

Attacking was more difficult. The idea was for the player to attack the enemy from as far away as possible, which was influenced by the attack range.<br>
This required checking multiple points.<br>
BFS naturally explores the grid level by level, which allows us to find all reachable tiles in order of distance.<br>
This ensures we can select the tile that maximizes distance from the enemy while still being reachable within the player’s movement range.<br>
BFS is simpler and more efficient in this case.<br>

## Used
- Unity (version 6000.0.33f1)
- Character from Unity Starter Assets
