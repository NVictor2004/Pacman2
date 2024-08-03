using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MazeGenerator : MonoBehaviour {
  public Node nodePrefab;                                                      // Store the Node Prefab
  public GameObject mazeParent;                                                // Stores the parent object of the maze (used for organisational purposes only)
  public Dictionary<Vector2, Node> allNodes = new Dictionary<Vector2, Node>(); // Stores all nodes in the maze,
  // referencing them through their position in the maze
  private Vector2[] neighbourPositions = new Vector2[]{new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1)};
  // Stores the translations needed to go from one cell to each of its four neighbours
  private List<Node> unvisited = new List<Node>(); // Stores all nodes that have not been visited yet
  // Stores the positions of the four corners of the maze
  private Vector2[] corners = new Vector2[]{new Vector2(1, 1), new Vector2(1, 20), new Vector2(20, 1), new Vector2(20, 20)};
  private Node currentNode;                   // Stores the node the program is currently on
  private List<Node> path = new List<Node>(); // List to store current "path" of nodes
  public NavMeshSurface2d surface;            // Store the NavMesh Surface
  // Start is called before the first frame update
  void Start() {
    Generator();
    surface.BuildNavMesh(); // This will create a NavMesh surface for the ghosts to move in
    // The walls of the nodes are NavMesh obstacles, thus the ghosts will be unable to move through them
  }
  public void Generator() {
    float cellSize = nodePrefab.transform.localScale.x; // Store the size of each node
    // Create the 400 nodes, and put them in the correct places
    // Start in the bottom left corner of the maze
    Vector2 startPos = new Vector2(-(cellSize * 10) + (cellSize / 2), -(cellSize * 10) + (cellSize / 2));
    Vector2 spawnPos = startPos;
    // Iterate through every 1 of the 400 positions
    for (int x = 1; x <= 20; x++) {
      for (int y = 1; y <= 20; y++) {
        CreateNode(spawnPos, new Vector2(x, y)); // Create a Node in this position
        spawnPos.y = spawnPos.y + cellSize;      // Increase the y-coordinate of the spawn point
      }
      // Once a column is completed
      spawnPos.y = startPos.y; // Reset spawnPos y
      spawnPos.x += cellSize;  // Move to the next column
    }
    currentNode = allNodes[corners[Random.Range(0, 3)]]; // Randomly choose one of the four corners
    // Start making the maze in that corner
    CreateMaze();
  }
  public void CreateNode(Vector2 pos, Vector2 keyPos) // This subroutine will be used to create a Node
  {
    Node new_node = Instantiate(nodePrefab, pos, nodePrefab.transform.rotation); // Create a Node using the Node Prefab
    new_node.name = "X:" + keyPos.x + " Y:" + keyPos.y;                          // Set its name (its X-position followed by its Y-position)
    new_node.transform.parent = mazeParent.transform;                            // Set its parent object to be the parent object of the maze
    new_node.gridPos = keyPos;                                                   // Set its position
    new_node.gameObject.SetActive(true);                                         // Turn it on
    // Turn all walls on
    new_node.left.SetActive(true);
    new_node.right.SetActive(true);
    new_node.up.SetActive(true);
    new_node.down.SetActive(true);
    allNodes[keyPos] = new_node; // Add to list of all Nodes
    unvisited.Add(new_node);     // Add to list of all unvisited Nodes
  }
  public List<Node> Neighbours(Node currentNode, bool all) // This subroutine will iterate through all neighbours of a Node
  // It wil return all neighbours or just unvisited neighbours
  // depending on the value of the variable all
  {
    List<Node> neighbours = new List<Node>();
    foreach (Vector2 trans in neighbourPositions) // Iterate through all translations
    {
      Vector2 newPos = currentNode.gridPos + trans; // Use translation to find a neighbour's position
      if (allNodes.ContainsKey(newPos))             // If the Node exists...
      {
        Node newNode = allNodes[newPos]; // Find and Store this Node
        if (all || unvisited.Contains(newNode))
          neighbours.Add(newNode); // If looking for all Nodes, add to list
        // If only looking for unvisited Nodes, check if unvisited
        // Add to list if it is
      }
    }
    return neighbours; // return list of Nodes
  }
  public void BreakWalls(Node currentNode, Node newNode) // This subroutine will compare the positions of the two inputted Nodes
  // and break the walls between them
  {
    // If newNode is to the left of currentNode...
    if (newNode.gridPos.x < currentNode.gridPos.x) {
      newNode.right.SetActive(false);                    // Disable newNode's right wall
      currentNode.left.SetActive(false);                 // Disable currentNode's left wall
      newNode.availableDirections.Add(Vector2.right);    // Add right to newNode's available directions list
      currentNode.availableDirections.Add(Vector2.left); // Add left to currentNode's available directions list
    }
    // If newNode is to the right of currentNode...
    else if (newNode.gridPos.x > currentNode.gridPos.x) {
      newNode.left.SetActive(false);                      // Disable newNode's left wall
      currentNode.right.SetActive(false);                 // Disable currentNode's right wall
      newNode.availableDirections.Add(Vector2.left);      // Add left to newNode's available directions list
      currentNode.availableDirections.Add(Vector2.right); // Add right to currentNode's available directions list
    }
    // If newNode is above currentNode...
    else if (newNode.gridPos.y > currentNode.gridPos.y) {
      newNode.down.SetActive(false);                   // Disable newNode's down wall
      currentNode.up.SetActive(false);                 // Disable currentNode's up wall
      newNode.availableDirections.Add(Vector2.down);   // Add down to newNode's available directions list
      currentNode.availableDirections.Add(Vector2.up); // Add up to currentNode's available directions list
    }
    // If newNode is below currentNode...
    else if (newNode.gridPos.y < currentNode.gridPos.y) {
      newNode.up.SetActive(false);                       // Disable newNode's up wall
      currentNode.down.SetActive(false);                 // Disable currentNode's down wall
      newNode.availableDirections.Add(Vector2.up);       // Add up to newNode's available directions list
      currentNode.availableDirections.Add(Vector2.down); // Add down to currentNode's available directions list
    }
  }
  public void CreateMaze() // This will convert the grid of Nodes into a maze using Recursive Backtracking
  {
    bool deadend = true; // In a normal Recursive Backtracking algorithm, every path will end in a deadend
    // In a Pacman maze, this should not occur
    // In this algorithm, a deadend is created when the program backtracks through the first node
    // This variable deadend is used to make sure that a wall is knocked down before the program backtracks through its first node
    // If the program continues to backtrack, no other walls are knocked down
    // Until the program stops backtracking, and starts carving out a new path
    // This eliminates the vast majority of deadends
    while (unvisited.Count > 1) // While the maze is not complete...
    {
      unvisited.Remove(currentNode);                                   // Remove the current Node from the unvisited list
      List<Node> unvisitedNeighbours = Neighbours(currentNode, false); // Find all nodes that the program can move to from the current node
      // As long as these nodes have not already been entered
      if (unvisitedNeighbours.Count > 0) // If there are such nodes available
      {
        deadend = true;                                                                  // Reset deadend to True
        Node nextNode = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)]; // Randomly pick a node
        BreakWalls(currentNode, nextNode);                                               // Break the walls between these nodes
        path.Add(currentNode);                                                           // Move to this node
        currentNode = nextNode;
      } else // There are no such nodes available, therefore, the program must backtrack
      {
        if (deadend) // For the first backtrack...
        {
          deadend = false;                                                    // Prevent all future backtracks from resulting in breaking walls (until the program stops backtracking)
          List<Node> neighbours = Neighbours(currentNode, true);              // Find all nodes that the program can move to from the current node
          neighbours.Remove(path[path.Count - 1]);                            // Remove the node you have just left from the list
          Node another_break = neighbours[Random.Range(0, neighbours.Count)]; // Randomly pick a node from the list
          BreakWalls(currentNode, another_break);                             // Break the walls between the current node and the node you have just chosen
        }
        currentNode = path[path.Count - 1]; // Go back a step
        path.Remove(currentNode);
      }
    }
  }
}