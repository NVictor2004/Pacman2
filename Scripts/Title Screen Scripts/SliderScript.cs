using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderScript : MonoBehaviour {
  // Sliders will be used to allow the user to change the values of three different variables:
  // The number of Diamonds in the Maze
  // The number of Power Pellets in the maze
  // The time delay between ghosts spawning
  public Slider slider;              // stores a Slider gameobject
  public TextMeshProUGUI sliderText; // Stores the Slider's label
  // This label will display the current value of the Slider
  // Start is called before the first frame update
  public void Start() {
    slider.onValueChanged.AddListener((v) => {
      sliderText.text = v.ToString("0"); // Connect the label to the Slider (so that it displays the correct value)
    });
  }
  // These three subroutines will be used to take the value of a Slider and store it in the appropriate static variable in the class TransferVariables
  // Each Slider will use one of these subroutines, depending on which variable the slider is being used to change
  public void DiamondSlider() // Used to set the number of diamonds in the maze
  {
    TransferVariables.DiamondNumber = Convert.ToInt32(slider.value);
  }
  public void EnergizerSlider() // Used to set the number of power pellets in the maze
  {
    TransferVariables.EnergizerNumber = Convert.ToInt32(slider.value);
  }
  public void DelaySlider() // Used to set the time delay between ghosts spawning in the maze
  {
    TransferVariables.GhostDelay = Convert.ToInt32(slider.value);
  }
}