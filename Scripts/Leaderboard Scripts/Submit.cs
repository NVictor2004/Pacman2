using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Submit : MonoBehaviour {
  public TMP_InputField username_text; // Stores the username textbox
  public GameObject InputWindow;       // Stores the Input Window section
  public GameObject Leaderboard;       // Stores the Leaderboard section
  public void SubmitUsername() {
    if (username_text.text.Length > 0) // If the user has entered a username...
    {
      TransferVariables.Username = username_text.text; // Save this username
      Leaderboard.gameObject.SetActive(true);          // Disable the Leaderboard section
      InputWindow.gameObject.SetActive(false);         // Disable the Input Window section
    }
  }
}