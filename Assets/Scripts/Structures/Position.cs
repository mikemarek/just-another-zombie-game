/**
* Position.cs
* Created by Michael Marek (2016)
*
* A simple data structure for storing integer x- and y-coordinates. Mainly used for referencing the
* positions of items in an inventory system grid.
**/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Position : System.Object
{
    public int x;
    public int y;

    /**
    **/
    public Position(int a = 0, int b = 0)
    {
        x = a;
        y = b;
    }

    /**
    **/
    public Position Clone()
    {
        return new Position(x, y);
    }

    /**
    **/
    public static Position operator +(Position p1, Position p2)
    {
        return new Position(p1.x + p2.x, p1.y + p2.y);
    }

    /**
    **/
    public static Position operator -(Position p1, Position p2)
    {
        return new Position(p1.x - p2.x, p1.y - p2.y);
    }

    /**
    **/
    public static bool operator ==(Position p1, Position p2)
    {
        return p1.x == p2.x && p1.y == p2.y;
    }

    /**
    **/
    public static bool operator !=(Position p1, Position p2)
    {
        return p1.x != p2.x || p1.y != p2.y;
    }

    /**
    **/
    public override int GetHashCode()
    {
        return this.GetHashCode();
    }

    /**
    **/
    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }

    /**
    **/
    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;

        Position p = obj as Position;
        if ((System.Object)p == null)
            return false;

        return (x == p.x) && (y == p.y);
    }

    /**
    **/
    public bool Equals(Position p)
    {
        if ((object)p == null)
            return false;

        return (x == p.x) && (y == p.y);
    }

    /**
    **/
    public bool Valid()
    {
        return !(x < 0 || y < 0);
    }

    /**
    **/
    public bool Nonzero()
    {
        return !(x == 0 || y == 0);
    }

    /**
    **/
    public bool isZero()
    {
        return (x == 0 && y == 0);
    }
}
