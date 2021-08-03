using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUIController : MonoBehaviour
{
    public Joystick joysick;
    public ActionButton actionButton;
    public Rigidbody2D player;
    public float playerSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        //round them for snappy movement
        int joystickHorizontal = Mathf.RoundToInt(joysick.Horizontal);
        int joystickVertical = Mathf.RoundToInt(joysick.Vertical);
        player.velocity = new Vector2(joystickHorizontal * playerSpeed + Input.GetAxisRaw("Horizontal") * playerSpeed, 
            joystickVertical * playerSpeed + Input.GetAxisRaw("Vertical") * playerSpeed);
    }
}
