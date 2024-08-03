using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreateLeaderboard : MonoBehaviour {
  public RecordScript template; // Stores the RecordTemplate game object
  public GameObject parent;     // Stores the parent gameobject of all high score entries displayed on screen
  private string username;      // Will store the current user's username
  private int score;            // Will store the current user's score
  public class Table            // This class will be used to store the High score table in PlayerPrefs
  {
    public List<Entry> table; // Stores a list of the High score entries
  }
  [System.Serializable]
  public class Entry // This class will be used to store a high score entry
  {
    public int score;   // Stores the score gained
    public string name; // Stores the username of the person who gained it
  }
  public void CreateTable() {
    string data = PlayerPrefs.GetString("highscoretable"); // Extract the current high score table from PlayerPrefs
    // It is currently stored as a string in JSON format
    Table highscoretable = JsonUtility.FromJson<Table>(data); // Convert the high score table from a string in JSON format
    if (!CheckExisting(highscoretable)) {
      Entry new_entry = new Entry(){name = username, score = score};   // Create new Entry object
      highscoretable.table.Add(new_entry);                             // Add the new entry to the high score table
      highscoretable.table.Sort((b, a) => a.score.CompareTo(b.score)); // Sort the table so that it is in descending numerical order (by score)
      highscoretable.table = highscoretable.table.GetRange(0, 12);     // Keep only the top 12 entries (by score)
    }

    // Iterate through the 12 entries...
    for (int i = 1; i < 13; i++) {
      // Extract each entry
      Entry entry = highscoretable.table[i - 1];
      // Display it on screen
      CreateEntry(entry, i);
    }
    // Convert the high score table from a Table object into a string in JSON format
    string json = JsonUtility.ToJson(highscoretable);
    // Store and save the string in PlayerPrefs
    PlayerPrefs.SetString("highscoretable", json);
    PlayerPrefs.Save();
  }
  public void Awake() {
    if (!PlayerPrefs.HasKey("highscoretable")) {
      Table highscoretable = new Table() // Initialise high score table values
          {table = new List<Entry>(){
               new Entry(){score = 1, name = "A"},
               new Entry(){score = 1, name = "B"},
               new Entry(){score = 1, name = "C"},
               new Entry(){score = 1, name = "D"},
               new Entry(){score = 1, name = "E"},
               new Entry(){score = 1, name = "F"},
               new Entry(){score = 1, name = "G"},
               new Entry(){score = 1, name = "H"},
               new Entry(){score = 1, name = "I"},
               new Entry(){score = 1, name = "J"},
               new Entry(){score = 1, name = "K"},
               new Entry(){score = 1, name = "L"},
           }};
      // Convert the high score table from a Table object into a string in JSON format
      string json = JsonUtility.ToJson(highscoretable);
      // Store and save the string in PlayerPrefs
      PlayerPrefs.SetString("highscoretable", json);
      PlayerPrefs.Save();
    }
    username = TransferVariables.Username; // Extracts the current user's username from the TransferVariables class
    score = TransferVariables.Score;       // Extracts the current user's score from the TransferVariables class
    CreateTable();                         // Create the high score table
  }
  public void CreateEntry(Entry entry, int index) // This subroutine will create, and display on screen, a high score entry
  {
    float height = -80f;
    RecordScript record = Instantiate(template);                            // Using the Record template prefab, instantiate an empty high score entry object
    record.transform.SetParent(parent.transform, false);                    // Set the parent gameobject to be the parent gameobject of all high score entries
    record.transform.anchoredPosition = new Vector2(980, (height * index)); // Set its position
    // It will be 40 below the previous entry
    // Calculate the rank
    string rank;
    if (index == 1)
      rank = "1ST"; // If the entry is first in the high score table, the rank should be "1ST"
    else if (index == 2)
      rank = "2ND"; // If the entry is second in the high score table, the rank should be "2ND"
    else if (index == 3)
      rank = "3RD"; // If the entry is third in the high score table, the rank should be "3RD"
    else
      rank = index + "TH";                      // Otherwise, the rank should be the number, followed by the letters "TH" (e.g. "4TH", "5TH", etc)
    record.Position.text = rank;                // Display the rank on screen
    record.Score.text = entry.score.ToString(); // Display the score on screen
    record.Name.text = entry.name;              // Display the name on screen
  }
  public bool CheckExisting(Table highscoretable) // Check if the current username is already in the high score table
  {
    for (int i = 0; i < 12; i++) // Iterate through the entire table
    {
      if (highscoretable.table[i].name == username) // If an entry's username matches the current username...
      {
        if (highscoretable.table[i].score < score) // Update the score
        {
          highscoretable.table[i].score = score;
        }
        highscoretable.table.Sort((b, a) => a.score.CompareTo(b.score)); // Sort the table so that it is in descending numerical order (by score)
        return true;                                                     // Return True
      }
    }
    return false; // Otherwise, if the current username is not in the high score table, return False
  }
}
