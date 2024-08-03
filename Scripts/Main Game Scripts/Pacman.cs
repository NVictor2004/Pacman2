using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pacman : MonoBehaviour {
  public Movement movement;
  public Collider2D mazeArea;              // This 2D Collider will specify the area within which a diamond can be created (set in the Unity Editor)
  public Sprite[] sprites = new Sprite[0]; // Will store the three colours that Pacman can be (red, blue or green)
  public SpriteRenderer spriteRenderer;    // Stores Pacman's sprite renderer
  public void Start() { Initialise(); }
  public void Initialise() {
    RandomisePosition();
    spriteRenderer.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)]; // Randomly assign Pacman a colour (red, blue or green)
    transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
  }
  public void RandomisePosition() // This subroutine moves Pacman to a random position within the maze
  {
    Bounds bounds = mazeArea.bounds; // Stores the dimensions of the maze
    var x = 0f;
    var y = 0f;
    do {
      // Pick a random position inside the bounds
      // Round and tweak the values to make sure Pacman will not appear on top of any walls
      x = Mathf.Round(UnityEngine.Random.Range(bounds.min.x, bounds.max.x - 1)) + 0.5f;
      y = Mathf.Round(UnityEngine.Random.Range(bounds.min.y, bounds.max.y - 1)) + 0.5f;
    } while ((Mathf.Abs(x) > 7 && Mathf.Abs(y) > 7)); // If Pacman is too close to the ghost spawn points (the corners), recalculate X and Y
    transform.position = new Vector2(x, y);           // Set the position of Pacman to the generated position
  }
  private void Update() {
    // If the user presses the Up arrow or the letter "W", Pacman will move up
    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
      movement.direction = Vector2.up;
      transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
    }
    // If the user presses the Down arrow or the letter "S", Pacman will move down
    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
      movement.direction = Vector2.down;
      transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
    }
    // If the user presses the Left arrow or the letter "A", Pacman will move left
    else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
      movement.direction = Vector2.left;
      transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
    }
    // If the user presses the Right arrow or the letter "D", Pacman will move right
    else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
      movement.direction = Vector2.right;
      transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }
  }
}
