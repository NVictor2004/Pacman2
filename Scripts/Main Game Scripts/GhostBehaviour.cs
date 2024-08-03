using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GhostBehaviour : MonoBehaviour {
  public float duration; // Stores the amount of time this behaviour should last for
  public virtual void Enable() {
    duration = UnityEngine.Random.Range(5, 20); // Randomly reset this behaviour's duration
    enabled = true;                             // Switch this Ghost behaviour on
    Invoke(nameof(Disable), duration);          // Invoke the Disable() subroutine after the correct amount of time has passed
    // (stored in the variable "duration")
  }
  public void Disable() {
    enabled = false; // Switch this Ghost behaviour off
    CancelInvoke();  // Cancel the Invoke of the Disable() Subroutine
  }
}