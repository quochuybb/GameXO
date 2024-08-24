
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Card
{
    public int _cardType;
    public int priorityLevel;
    public string _cardName;
    public bool _isUsed;
    public string _getName()
    {
        return this._cardName;
    }
    public int _getPriority()
    {
        return this.priorityLevel;
    }
    public int _getID()
    {
        return this._cardType;
    }

    public bool _getIsUsed()
    {
        return this._isUsed;
    }
}



















