using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GhostChase : GhostBehaviour {
  [SerializeField] Transform target; // This variable will store Pacman's location
  public NavMeshAgent agent;         // This variable will store the current ghost's NavMeshAgent component
  public GhostScatter scatter;
  public Transform ghost;
  public Ghost ghost_script;
  void Start() {
    // Set the NavMeshAgent component's attributes
    agent.updateRotation = false;
    agent.updateUpAxis = false;
  }
  void Update() {
    // Set Pacman as the current ghost's target
    agent.SetDestination(target.position);
  }
  private void OnDisable() // This subroutine will be called when the Chase behaviour is disabled
  {
    if (!ghost_script.isfrightened) {
      // Disable the NavMeshAgent component
      agent.enabled = false;
      // When the current ghost needs to switch from the Chase behaviour back to the Scatter behaviour, it must be at a Node for the Scatter behaviour to work
      // The FindClosestNode() subroutine will find the nearest node to the current ghost
      // This command will make the ghost go there
      ghost.position = FindClosestNode();
      // Set this ghost's current behaviour to Scattering
      scatter.Enable();
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
}
