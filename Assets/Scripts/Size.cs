using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Size {
    public int height;
    public int width;

    public Size(int height, int width) {
        this.height = height;
        this.width = width;
    }

    public (int, int) Get()
    {
        return (this.height, this.width);
    }

    public int Height () {
        return this.height;
    }

    public int Width () { 
        return this.width;
    }

    public override string ToString() => $"height: {this.height} - width: {this.width}";
}

