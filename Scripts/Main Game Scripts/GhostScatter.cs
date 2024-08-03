using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GhostScatter : GhostBehaviour {
  public Movement movement;
  public NavMeshAgent agent;
  public GhostChase chase;
  public Ghost ghost_script;
  private void OnDisable() // When the Scatter behaviour is disabled...
  {
    if (!ghost_script.isfrightened) {
      // Enable the NavmeshAgent
      agent.enabled = true;
      // Enable the Chase behaviour
      chase.Enable();
    }
  }
  private void OnTriggerEnter2D(Collider2D other) // When the Ghost collides with something...
  {
    Node node = other.GetComponent<Node>(); // If the ghost collided with a node...
    if (node != null)                       // If the ghost collided with a node
    {
      transform.position = other.transform.position;               // Reset the ghost's position to the node's position
      int index = Random.Range(0, node.availableDirections.Count); // Randomly pick an index from the list of available directions
      // The ghost should not go back in the direction it came from
      // If this occurs, increase the index by 1 (if the list of available directions is large enough)
      // If index becomes larger than the length of the list, reset index back to 0
      if (node.availableDirections[index] == -movement.direction && node.availableDirections.Count > 1)
        index = (index + 1) % node.availableDirections.Count;
      // Extract the new direction from the available directions list (using the index)
      // Set the ghost's direction to the new direction
      movement.direction = node.availableDirections[index];
    }
  }
}
