using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    MAX_X, MAX_Y, MAX_BOTH, MIN_X, MIN_Y, MIN_BOTH, OUT 
}
public struct Direction
{
    public static DirectionType GetDirection(Board board, int y, int x) {
        if (x == board.max_x && y == board.max_x)
        {
            return DirectionType.MAX_BOTH;
        }
        else if (x == board.max_x)
        {
            return DirectionType.MAX_X;
        }
        else if (y == board.max_y)
        {
            return DirectionType.MAX_Y;
        }
        else if (x == board.min_x && y == board.min_y)
        {
            return DirectionType.MIN_BOTH;
        }
        else if (x == board.min_x)
        {
            return DirectionType.MIN_X;
        }
        else if (y == board.min_y)
        {
            return DirectionType.MIN_Y;
        }
        else {
            return DirectionType.OUT;
        }
    }


}
