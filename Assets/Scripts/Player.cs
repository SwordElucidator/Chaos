using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Person
{
    // Update is called once per frame
    public FloatingJoystick joystick;
    
    private void FixedUpdate()
    {
        // var moveHorizontal = Input.GetAxis("Horizontal");
        // var moveVertical = Input.GetAxis("Vertical");
        // Move(moveHorizontal, moveVertical);
        Move(joystick.Horizontal, joystick.Vertical);
    }
}
