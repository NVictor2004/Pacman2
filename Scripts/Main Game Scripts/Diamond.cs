using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Diamond : MonoBehaviour {
  public Collider2D mazeArea; // This 2D Collider will specify the area within which a diamond can be created (set in the Unity Editor)
  public GameManager manager; // Stores the GameManager script
  private void Start() { RandomizePosition(); }
  public virtual void OnTriggerEnter2D(Collider2D other) // If something collides with a diamond, this subroutine will be called
  {
    Pacman pacman = other.GetComponent<Pacman>(); // If the ghost collided with Pacman...
    if (pacman != null) {
      RandomizePosition();
      manager.score = manager.score + 10;
      manager.SetScore();
      manager.audioSource.PlayOneShot(manager.pacman_eat); // Plays the Pacman Eating sound
    }
  }
  public void RandomizePosition() // This subroutine moves the diamond to a random position within the maze
  {
    Bounds bounds = mazeArea.bounds; // Stores the dimensions of the maze
    var x = 0f;
    var y = 0f;
    do {
      // Pick a random position inside the bounds
      // Round and tweak the values to make sure the diamond will not appear on top of any walls
      x = Mathf.Round(UnityEngine.Random.Range(bounds.min.x, bounds.max.x - 1)) + 0.5f;
      y = Mathf.Round(UnityEngine.Random.Range(bounds.min.y, bounds.max.y - 1)) + 0.5f;
    } while ((Mathf.Abs(x) > 7 && Mathf.Abs(y) > 7)); // If the diamond is too close to the ghost spawn points (the corners), recalculate X and Y
    transform.position = new Vector2(x, y);           // Set the position of the diamond to the generated position
  }
}