using System.Collections;

public class VoxelSquare
{
    public bool TopLeft;
    public bool TopRight;
    public bool BottomRight;
    public bool BottomLeft;

    public VoxelSquare(bool topLeft, bool topRight, bool bottomRight, bool bottomLeft)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;
    }

    public int Type()
    {
        int type = 0;
        type |= TopLeft ? 1 : 0;
        type |= TopRight ? 2 : 0;
        type |= BottomRight ? 4 : 0;
        type |= BottomLeft ? 8 : 0;

        return type;
    }
}
