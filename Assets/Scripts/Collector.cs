using UnityEngine;

public class Collector : MonoBehaviour
{
    public enum WallSide
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public WallSide side;

    void Start()
    {
        if (gameObject.name == "Top")
        {
            side = WallSide.Top;
        }
        else if (gameObject.name == "Right")
        {
            side = WallSide.Right;
        }
        else if (gameObject.name == "Bottom")
        {
            side = WallSide.Bottom;
        }
        else if (gameObject.name == "Left")
        {
            side = WallSide.Left;
        }
    }
}
