using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour // This class stores the attributes of each cell in the randomly generated maze
{
  public GameObject left;                   // Stores the left Wall Gameobject (set in the Unity Editor)
  public GameObject right;                  // Stores the right Wall Gameobject (set in the Unity Editor)
  public GameObject up;                     // Stores the upper Wall Gameobject (set in the Unity Editor)
  public GameObject down;                   // Stores the down Wall Gameobject (set in the Unity Editor)
  public Vector2 gridPos;                   // Stores the position of a node in the maze
  public List<Vector2> availableDirections; // A list storing the directions that Pacman and the ghosts can move through
  //(the directions where the walls of the cell are disabled)
}
