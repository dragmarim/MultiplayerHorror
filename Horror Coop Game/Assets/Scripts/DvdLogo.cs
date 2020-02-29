using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DvdLogo : MonoBehaviour
{
    public float moonSpeedX;
    public float moonSpeedY;
    public float moonX;
    public float moonY;
    public float moonWidth;
    public float moonHeight;
    public float height;
    public float width;
    public float moveSpeed;
    public 

    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollision();
    }

    void Move() 
    {
        moonX += moonSpeedX;
        moonY += moonSpeedY;
    }

    void DetectCollision()
    {
        if (moonX + moonWidth >= width)
        {
            moonSpeedX *= -1;
        }
        if (moonY + moonHeight >= height)
        {
            moonSpeedY *= -1;
        }
        if (moonX <= width)
        {
            moonSpeedX *= -1;
        }
        if (moonY <= height)
        {
            moonSpeedY *= -1;
        }
    }
}
