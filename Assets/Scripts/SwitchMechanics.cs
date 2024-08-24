using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMechanics : MonoBehaviour, ICellMechanics
{
    private Cell _cell;
    public void CallMechanics(int mechanic, int[] direction, CellTypes turn)
    {
        switch (mechanic)
        {
            case 0:
                Delete(CellTypes.None, direction[0]);
                break;
            case 1:
                Delete(CellTypes.None, direction);
                break;
            case 2:
                Mark(turn ,direction[0]);
                break;
            case 3:
                Mark(turn ,direction);
                break;
        }
    }
    public  void Mark(CellTypes turn, int direction)
    {
        _cell = transform.GetChild(direction).GetComponent<Cell>();
        _cell.ChangeCellImage(turn);
        
    }
    public  void Mark(CellTypes turn, int[] direction)
    {
        _cell = transform.GetChild(direction[0]).GetComponent<Cell>();
        _cell.ChangeCellImage(turn);
        _cell = transform.GetChild(direction[1]).GetComponent<Cell>();
        if (_cell.SetInMatrix(turn))
        {
            _cell.ChangeCellImage(turn);
        }
    }
    public  void Delete(CellTypes turn, int direction)
    {
        _cell = transform.GetChild(direction).GetComponent<Cell>();
        if (_cell.SetInMatrix(turn))
        {
            _cell.ChangeCellImage(turn);
        }
        
    }
    public  void Delete(CellTypes turn, int[] direction)
    {
        _cell = transform.GetChild(direction[0]).GetComponent<Cell>();
        if (_cell.SetInMatrix(turn))
        {
            _cell.ChangeCellImage(turn);
        }
        _cell = transform.GetChild(direction[1]).GetComponent<Cell>();
        if (_cell.SetInMatrix(turn))
        {
            _cell.ChangeCellImage(turn);
        }
    }
    
    public void Remove(int direction)
    {
        _cell = transform.GetChild(direction).GetComponent<Cell>();
        _cell.ChangeCellImage(CellTypes.None);
    }
}
