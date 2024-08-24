using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ControlCard : MonoBehaviour
{
    public TextMeshProUGUI textCard;
    public int numberCard;
    private ControlDrawCard _controlDrawCard;
    private Board _board;
    private Button _button;
    private int _cardEffect;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _board = FindAnyObjectByType<Board>();
        _button.onClick.AddListener(ReadCard);
    }
    public void ShowTextCard(int index, List<GameObject> currentDeck)
    {
        _controlDrawCard = FindAnyObjectByType<ControlDrawCard>();
        textCard = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textCard.text = _controlDrawCard.GetComponent<ControlDrawCard>().deck._getCard(index)._cardName;
        _cardEffect= _controlDrawCard.GetComponent<ControlDrawCard>().deck._getCard(index)._cardType;
        currentDeck.Add(gameObject);
        gameObject.SetActive(false);
    }

    public void ReadCard()
    {
        if(_board.currentTurn == CellTypes.X && transform.parent.gameObject.CompareTag("Player"))
        {
            _controlDrawCard.cardMechanic = _cardEffect;
            _controlDrawCard.numberCardInHand--;
            _controlDrawCard.numberCardUsed++;
            _controlDrawCard.useCard = true;
            GivetoGraveyard(_controlDrawCard.graveYard);
        }
        else if (_board.currentTurn == CellTypes.O && transform.parent.gameObject.CompareTag("Opponent"))
        {
            _controlDrawCard.cardMechanic = _cardEffect;
            _controlDrawCard.numberCardInHandOp--;
            _controlDrawCard.numberCardUsedOp++;
            _controlDrawCard.useCard = true;
            GivetoGraveyard(_controlDrawCard.graveYardOp);
        }
        
        
    }

    public void GivetoGraveyard(List<GameObject> graveyard)
    {
        graveyard.Add(gameObject);
        gameObject.SetActive(false);
    }
}
