using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public void Play() {
    SceneManager.LoadScene("Pacman 2"); // Loads the scene called "Pacman 2"
  }
  public void Settings() {
    // Reveal Settings menu
    // Hide Main Menu
  }
  public void Quit() {
    Debug.Log("Quit!"); // Display "Quit!" to the Console
    Application.Quit(); // Quit the game
  }
}