using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpForceBarController : MonoBehaviour
{
    public Slider jumpForceSlider; // Reference to the UI Slider
    public PlayerMovement playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        jumpForceSlider.minValue = 0; // Minimum slider value
        jumpForceSlider.maxValue = playerMovement.maxHoldTime; // Maximum value is the players max hold time
        jumpForceSlider.value = 0; // Start with the slider empty
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update the slider value based on how long the player is holding the space bar
        if (playerMovement.isHoldingJump)
        {
            jumpForceSlider.value = playerMovement.holdTime;  // Set slider based on hold time
        }
        else
        {
            jumpForceSlider.value = 0f;  // Reset slider when not jumping
        }

    }
}
