using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICellMechanics
{
    void Mark(CellTypes turn, int[] _direction);
    void Delete(CellTypes turn, int[] _direction);
}
