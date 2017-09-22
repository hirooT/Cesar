using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  //IComparable interface

public class Movie : IComparable<Movie>
{

    public string type;
    public int index;

    public Movie(string newType, int newIndex)
    {
        type = newType;
        index = newIndex;

    }

    public int CompareTo(Movie other)
    {
        throw new NotImplementedException();
    }
}
