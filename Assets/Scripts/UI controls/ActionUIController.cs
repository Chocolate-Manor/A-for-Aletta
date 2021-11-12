using UnityEngine;
using UnityEngine.UI;

public class ActionUIController : MonoBehaviour
{
    public Joystick joystick;
    public ScrollRect scroll;
    public float scrollSpeed = 1000f;

    // Update is called once per frame
    // void Update()
    // {
    //     scroll.velocity = new Vector2(0, joystick.Vertical * scrollSpeed);
    // }
}
