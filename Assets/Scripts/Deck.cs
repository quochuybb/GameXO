using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardConfig",menuName = "Config")]
public class Deck : ScriptableObject
{
    public Card[] cards;
    public Card _getCard(int index)
    {
        return cards[index];
    }

    public int _getLength()
    {
        return cards.Length;
    }
    
}















