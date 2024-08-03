using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
  public List<Ghost> ghosts = new List<Ghost>();              // A list containing all Ghosts
  public Ghost ghost_prefab;                                  // Stores the instance of the Ghost class in the Ghost prefab
  private int new_ghost_delay = TransferVariables.GhostDelay; // Stores the time interval (in seconds) between Ghosts being created
  private Vector2[] corners = new Vector2[]{new Vector2(9.5f, 9.5f), new Vector2(9.5f, -9.5f), new Vector2(-9.5f, 9.5f), new Vector2(-9.5f, -9.5f)};
  // Stores the positions of the corners of the maze
  public GameObject ghostParent;                                 // Stores the parent Game Object of all ghosts (used for organisational purposes only)
  public Diamond diamondPrefab;                                  // Stores the Diamond Prefab
  public Energizer EnergizerPrefab;                              // Stores the Energizer Prefab
  public GameObject AllDiamonds;                                 // Stores the parent object of the Diamonds (used for organisational purposes only)
  public GameObject AllEnergizers;                               // Stores the parent object of the Energizers (used for organisational purposes only)
  private int numDiamonds = TransferVariables.DiamondNumber;     // Stores the number of Diamonds in the maze
  private int numEnergizers = TransferVariables.EnergizerNumber; // Stores the number of energizers in the maze
  public TextMeshProUGUI scoreText;                              // Stores the text "Score: ", followed by the current score
  public TextMeshProUGUI livesText;                              // Stores the text "Lives: ", followed by the number of lives remaining
  public int score = 0;                                          // Stores the current score (set in the Unity Editor)
  public int lives = 3;                                          // Stores the number of lives remaining (set in the Unity Editor)
  public List<Diamond> diamonds = new List<Diamond>();           // A list containing all Diamonds
  public List<Energizer> energizers = new List<Energizer>();     // A list containing all Energizers
  public Pacman pacman;                                          // Stores the Pacman script in the Pacman game object
  public TextMeshProUGUI gameover;                               // Stores the "GAME OVER" text
  public AudioSource audioSource;                                // Stores the Audio Source used to play all sounds when the game is running (set in the Unity Editor)
  public AudioClip pacman_eat;                                   // Stores the sound played when Pacman eats a diamond (set in the Unity Editor)
  public AudioClip pacman_death;                                 // Stores the sound played when Pacman is killed (set in the Unity Editor)
  public AudioClip ghost_death;                                  // Stores the sound played when a ghost is eaten (set in the Unity Editor)
  public int frightenedLength;                                   // Stores the length of time ghosts shoudl be frightened for
  public void Start() {
    NewRound();
    SetLives();
    SetScore();
    gameover.enabled = false;
  }
  public void NewRound() {
    CreateGhosts();
    CreateDiamonds();
    CreateEnergizers();
    pacman.enabled = true; // Reenable Pacman
    pacman.Initialise();   // Call the Initialise() subroutine on Pacman
  }
  public void SetLives() // this subroutine will set the current number of lives remaining in the livesText variable above the maze
  {
    livesText.text = "Lives: " + lives.ToString();
  }
  public void SetScore() // this subroutine will set the current score in the scoreText variable above the maze
  {
    scoreText.text = "Score: " + score.ToString();
  }
  public void CreateGhosts() // Creates new ghosts constantly after regular time intervals
  {
    InvokeRepeating(nameof(CreateNewGhost), 0, new_ghost_delay);
  }
  public void CreateDiamonds() // This subroutine will spawn a certain number of diamonds into the maze
  {
    for (int i = 0; i < numDiamonds; i++)
      CreateNewDiamond();
  }
  public void CreateEnergizers() // This subroutine will spawn a certain number of energizers into the maze
  {
    for (int i = 0; i < numEnergizers; i++)
      CreateNewEnergizer();
  }
  public void CreateNewGhost() // This subroutine will create a new ghost when called
  {
    // Use the Ghost prefab to instantiate a new ghost
    Ghost ghost = Instantiate(ghost_prefab, corners[UnityEngine.Random.Range(0, 3)], Quaternion.identity);
    // Add the new ghost to the ghosts list
    ghosts.Add(ghost);
    ghost.gameObject.SetActive(true);               // Enable the new ghost
    ghost.transform.parent = ghostParent.transform; // Make the new ghost a child of the parent Game Object of all ghosts (used for organisational purposes only)
    ghost.Initialise();                             // Call the Initialise() subroutine
  }
  public void CreateNewDiamond() // This subroutine will be used to create a new diamond object
  {
    // Create a new Diamond object using the diamond prefab
    Diamond diamond = Instantiate(diamondPrefab, diamondPrefab.transform.position, diamondPrefab.transform.rotation);
    // Add the new diamond to the diamonds list
    diamonds.Add(diamond);
    diamond.transform.parent = AllDiamonds.transform; // Set its parent object to be the parent object of all Diamonds
    diamond.gameObject.SetActive(true);               // Turn it on
  }
  public void CreateNewEnergizer() // This subroutine will be used to create a new Energizer object
  {
    // Create a new Energizer object using the Energizer prefab
    Energizer energizer = Instantiate(EnergizerPrefab, EnergizerPrefab.transform.position, EnergizerPrefab.transform.rotation);
    // Add the new energizer to the energizers list
    energizers.Add(energizer);
    energizer.transform.parent = AllEnergizers.transform; // Set its parent object to be the parent object of all Energizers
    energizer.gameObject.SetActive(true);                 // Turn it on
  }
  public void EndRound() {
    for (int i = 0; i < ghosts.Count; i++) // Disable all ghosts
    {
      ghosts[i].gameObject.SetActive(false);
    }
    ghosts.Clear();                          // Reset list of all ghosts to empty
    for (int i = 0; i < diamonds.Count; i++) // Disable all diamonds
    {
      diamonds[i].gameObject.SetActive(false);
    }
    diamonds.Clear();                          // Reset list of all diamonds to empty
    for (int i = 0; i < energizers.Count; i++) // Disable all energizers
    {
      energizers[i].gameObject.SetActive(false);
    }
    energizers.Clear(); // Reset list of all energizers to empty
    lives = lives - 1;  // Decrement number of lives
    SetLives();
    audioSource.PlayOneShot(pacman_death);    // Plays the Pacman Death sound
    CancelInvoke(nameof(CreateNewGhost));     // Stop making new ghosts
    pacman.movement.direction = Vector2.zero; // Stop Pacman from moving
    pacman.enabled = false;
    if (lives == 0) // If Pacman has run out of lives...
    {
      gameover.enabled = true;    // Enable "Game Over!" text
      Invoke(nameof(EndGame), 3); // End the game after 3 seconds
    } else                        // If Pacman still has lives...
    {
      Invoke(nameof(NewRound), 3); // Start a new round after 3 seconds
    }
  }
  public void EndGame() {
    TransferVariables.Score = score;       // Saves the current user's score
    SceneManager.LoadScene("Leaderboard"); // Loads the scene called "Leaderboard"
  }
  public void FrightenedLength() {
    frightenedLength = UnityEngine.Random.Range(5, 20); // Randomly generate the length of time the GhostFrightened behaviour should last for
    if (ghosts.Count > 0)                               // If there are ghosts already in the maze
    {
      CancelInvoke(nameof(CreateNewGhost));                                       // Stop making new ghosts
      InvokeRepeating(nameof(CreateNewGhost), frightenedLength, new_ghost_delay); // Restart making ghosts when the GhostFrightened behaviour stops
    }
  }
}
