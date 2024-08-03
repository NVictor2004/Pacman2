using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GhostFrightened : GhostBehaviour {
  public bool pink = true;              // Stores whether the ghost should flash pink or remain red
  public Sprite pink_sprite;            // Stores the Pink ghost sprite
  public Sprite red_sprite;             // Stores the Red ghost sprite
  public SpriteRenderer spriteRenderer; // Stores the ghost's SpriteRenderer
  // Stores an array containing the positions of all four corners of the maze
  private Vector2[] corners = new Vector2[]{new Vector2(9.5f, 9.5f), new Vector2(9.5f, -9.5f), new Vector2(-9.5f, 9.5f), new Vector2(-9.5f, -9.5f)};
  // Stores the position of Pacman
  [SerializeField] Transform target;
  public NavMeshAgent agent;                      // Stores the ghost's NavMeshAgent
  public GhostScatter scatter;                    // Stores the ghost's Scatter behaviour
  public GhostChase chase;                        // Stores the ghost's Chase behaviour
  public Ghost ghost_script;                      // Stores the Ghost script
  public Transform ghost;                         // Stores the location of the ghost
  public Sprite[] pacman_sprites = new Sprite[0]; // List of all Pacman sprites (in order of red, blue, green), set in the Unity Editor
  public Sprite[] ghost_sprites = new Sprite[0];  // List of all Pacman ghost sprites (in order of red, blue, green), set in the Unity Editor
  public Pacman pacman;                           // Stores the Pacman script in the Pacman game object
  private Sprite current_sprite;                  // Stores the ghost sprite with the same colour as the sprite Pacman is currently using
  public GameManager manager;                     // Stores the GameManager script
  public override void Enable() {
    CancelInvoke(); // Cancel the Invokes of the Disable() and Flash() subroutines
    if (!enabled)   // if not already enabled...
    {
      enabled = true; // Switch this Ghost behaviour on
    } else            // if already enabled...
    {
      OnEnable(); // Call the OnEnable() subroutine
    }
    Invoke(nameof(Disable), manager.frightenedLength);                          // Invoke the Disable() subroutine after the correct amount of time has passed
    InvokeRepeating(nameof(Flash), (manager.frightenedLength * 2) / 3f, 0.25f); // Flash (change colour between Pacman's colour and pink)
    // Start Flashing in the last third of the duration
    // While flashing, change colour every 0.25 seconds
    // This is done by repeatedly Invoking the Flash() subroutine
  }
  private void Flash() {
    if (pink)
      spriteRenderer.sprite = pink_sprite; // If the variable pink is true, become pink
    else
      spriteRenderer.sprite = current_sprite; // If the variable pink is false, become Pacman's colour
    pink = !pink;                             // Change the value of the variable pink
  }
  private Vector2 FurthestCorner() {
    // Find the corner furthest away from Pacman
    float maxDistance = 0;
    Vector2 my_corner = new Vector2(0, 0);
    // Iterate through all four corners
    foreach (Vector2 corner in corners) {
      // Calculate distance from corner to Pacman
      if (Vector2.Distance(corner, target.position) > maxDistance) {
        maxDistance = Vector2.Distance(corner, target.position);
        my_corner = corner;
      }
    }
    return my_corner;
  }
  private void OnEnable() // When a ghost becomes frightened...
  {
    ghost_script.isfrightened = true;
    current_sprite = GetSprite();           // Store the ghost sprite with the same colour as the sprite Pacman is currently using
    spriteRenderer.sprite = current_sprite; // Apply it to the ghost
    // Disable the other two behaviours
    scatter.enabled = false;
    chase.enabled = false;
    agent.enabled = true;
    // Sort out the NavMeshAgent's variables
    agent.updateRotation = false;
    agent.updateUpAxis = false;
    agent.SetDestination(FurthestCorner()); // Set the NavMeshAgent's destination to the furthest corner
  }
  private void OnDisable() // When the ghost is no longer frightened
  {
    ghost_script.isfrightened = false;
    // Find the closest node and go there
    ghost.position = FindClosestNode();
    spriteRenderer.sprite = ghost_sprites[UnityEngine.Random.Range(0, ghost_sprites.Length)]; // Randomly pick ghost colour
    int number = UnityEngine.Random.Range(1, 3);                                              // Pick a number between 1 and 2
    if (number == 1)                                                                          // If the number is 1, enable Scattering
    {
      // Disable NavMeshAgent
      agent.enabled = false;
      // Enable the Scatter behaviour
      scatter.Enable();
    } else // If the number is 2, enable Chasing
    {
      // Enable the Chase behaviour
      chase.Enable();
    }
  }
  public Vector3 FindClosestNode() {
    // Generate list of all nodes
    GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
    // Find closest node
    GameObject closest = null;       // this Game Object will store the closest node that has been found so far
    float distance = Mathf.Infinity; // this variable will store the distance from this ghost to the closest node that has been found so far
    // this value is initialised to Infinity so that there will be no chance
    // that the distance between this ghost and the closest node to it is larger than the initial value
    Vector3 position = transform.position; // Store the ghost's current position
    foreach (GameObject node in nodes)     // iterate through every node in the maze
    {
      Vector3 diff = node.transform.position - position; // Find the 3D Vector leading from the ghost to the node
      float curDistance = diff.sqrMagnitude;             // Find the magnitude of this 3D Vector (the distance between the ghost and this node)
      if (curDistance < distance)                        // If this distance is smaller than the smallest distance we have found so far...
      {
        closest = node;         // Store this node
        distance = curDistance; // Store the distance from this node to the ghost
      }
    }
    return closest.transform.position; // return the position of the closest node to the ghost
  }

  public Sprite GetSprite() // Get the ghost sprite with the same colour as Pacman
  {
    int index = -1;                                 // Initialise the index value
    for (int i = 0; i < pacman_sprites.Length; i++) // Iterate through list of all Pacman sprites
    {
      if (pacman_sprites[i] == pacman.spriteRenderer.sprite) // If this sprite is currently being used...
      {
        index = i; // Save the index
      }
    }
    return ghost_sprites[index]; // return the ghost sprite at the same index (this sprite will have the same colour as Pacman)
  }
}
