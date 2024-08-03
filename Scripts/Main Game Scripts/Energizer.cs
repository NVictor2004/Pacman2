using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Energizer : Diamond
{
  public GhostFrightened frightened;                      // Stores the Frightened behaviour
  public override void OnTriggerEnter2D(Collider2D other) // If something collides with an energizer, this subroutine will be called
  {
    Pacman pacman = other.GetComponent<Pacman>(); // If the ghost collided with Pacman...
    if (pacman != null)
    {
      manager.FrightenedLength();
      RandomizePosition(); // Change to a random position in the maze
      manager.score = manager.score + 10;
      manager.SetScore();
      manager.audioSource.PlayOneShot(manager.pacman_eat); // Plays the Pacman Eating sound
      FrightenAll();
    }
  }
  public void FrightenAll()
  {
    for (int i = 0; i < manager.ghosts.Count; i++)
      manager.ghosts[i].frightened.Enable();
  }
}