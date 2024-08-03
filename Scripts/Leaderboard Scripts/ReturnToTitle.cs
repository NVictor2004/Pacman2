using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnToTitle : MonoBehaviour {
  public void Title() {
    SceneManager.LoadScene("Title Screen"); // Loads the scene called "Title Screen"
  }
}
