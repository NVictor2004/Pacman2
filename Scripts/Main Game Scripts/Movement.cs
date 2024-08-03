using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour {
  public float speed;               // Set the speed of Pacman and the ghosts (set in the Unity Editor)
  public Vector2 direction;         // Stores the direction the Gameobject is currently going in
  public new Rigidbody2D rigidbody; // Stores the Gameobject's 2D Rigidbody component
  private void FixedUpdate()        // This subroutine will be called every fixed frame
  // THis subroutine will calculate where Pacman and the ghosts should be, every time a new frame is created
  {
    Vector2 position = rigidbody.position;                         // Stores the current position of the Gameobject
    Vector2 translation = direction * speed * Time.fixedDeltaTime; // Calculates how the gameobject should have moved
    // in the time interval between different frames
    rigidbody.MovePosition(position + translation); // Move to the final position
  }
}