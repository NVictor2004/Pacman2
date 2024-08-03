using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ghost : MonoBehaviour {
  // Define variables for all scripts
  public GhostScatter scatter;
  public GhostChase chase;
  public GhostFrightened frightened;
  public NavMeshAgent agent;
  public bool isfrightened = false;        // Stores whether the ghost is frightened or not
  public Sprite[] sprites = new Sprite[0]; // Will store the three colours that the ghosts can be (red, blue or green)
  public SpriteRenderer spriteRenderer;    // Stores the ghost's sprite renderer
  // Stores an array containing the positions of all four corners of the maze
  private Vector2[] corners = new Vector2[]{new Vector2(9.5f, 9.5f), new Vector2(9.5f, -9.5f), new Vector2(-9.5f, 9.5f), new Vector2(-9.5f, -9.5f)};
  public Sprite[] pacman_sprites = new Sprite[0]; // List of all Pacman sprites (in order of red, blue, green), set in the Unity Editor
  public Sprite[] ghost_sprites = new Sprite[0];  // List of all Pacman ghost sprites (in order of red, blue, green), set in the Unity Editor
  public GameManager manager;                     // Stores the GameManager script
  private void Start() { Initialise(); }
  public void Initialise() {
    agent.enabled = false; // Disable NavMeshAgent component
    frightened.Disable();  // Disable Frightened and Chase scripts
    chase.Disable();
    scatter.Enable(); // Enable Scatter script
    RandomisePosition();
    spriteRenderer.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)]; // Randomly assign the ghost a colour (red, blue or green)
  }
  public void RandomisePosition() // This subroutine moves the ghost to one of the corners (randomly selected)
  {
    transform.position = corners[UnityEngine.Random.Range(0, corners.Length)];
    // Set the position of the ghost to this corner
  }
  public bool SameColour(Pacman pacman) {
    int pacman_index = 0;
    for (int i = 0; i < pacman_sprites.Length; i++) {
      if (pacman_sprites[i] == pacman.spriteRenderer.sprite)
        pacman_index = i;
    }
    int ghost_index = 0;
    for (int i = 0; i < ghost_sprites.Length; i++) {
      if (ghost_sprites[i] == spriteRenderer.sprite)
        ghost_index = i;
    }
    return (ghost_index == pacman_index) || isfrightened;
  }
  public void OnTriggerEnter2D(Collider2D other) // If something collides with a Ghost, this subroutine will be called
  {
    Pacman pacman = other.GetComponent<Pacman>(); // If the ghost collided with Pacman...
    if (pacman != null) {
      if (SameColour(pacman)) // Call the SameColour() subroutine on Pacman
      // If it returns True, increase the user's score and delete the ghost
      {
        manager.audioSource.PlayOneShot(manager.ghost_death); // Play Ghost Eaten sound
        manager.score = manager.score + 100;                  // Increase score by 100
        manager.SetScore();
        manager.ghosts.Remove(this);      // Remove ghost from Ghosts list
        this.gameObject.SetActive(false); // Turn it off
      } else {
        manager.EndRound();
      }
    }
  }
}
