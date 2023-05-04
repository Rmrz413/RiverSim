using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    N, E, S, W
}

public static class DirectionExt
{
    public static Direction Opposite(this Direction direction)
    {
        return (int)direction < 2 ? (direction + 2) : (direction - 2);
    }

    public static Direction Previous(this Direction direction)
    {
        return direction == Direction.N ? Direction.W : (direction - 1);
    }

    public static Direction Next(this Direction direction)
    {
        return direction == Direction.W ? Direction.N : (direction + 1);
    }
}
