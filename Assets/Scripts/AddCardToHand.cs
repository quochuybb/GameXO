using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCardToHand : MonoBehaviour
{
    private Button _button;
    private ControlDrawCard _controlDrawCard;
    [SerializeField] private Transform cardBoard;
    [SerializeField] private Transform cardBoardAI;
    void Start()
    {
        _controlDrawCard = FindAnyObjectByType<ControlDrawCard>();
    }

    // Update is called once per frame
    public void AddCard(bool hand)
    {
        
        if (hand)
        {
            _controlDrawCard.currentDeckOp[_controlDrawCard.numberCardInHandOp + _controlDrawCard.numberCardUsedOp].SetActive(true);
            _controlDrawCard.numberCardInHandOp++;
        }
        else 
        {
            
            _controlDrawCard.currentDeck[_controlDrawCard.numberCardInHand + _controlDrawCard.numberCardUsed].SetActive(true);
            _controlDrawCard.numberCardInHand++;
        }
    }
}
