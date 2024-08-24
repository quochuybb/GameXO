using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControlDrawCard : MonoBehaviour
{
    [SerializeField] public Deck deck;
    public Dictionary<int, GameObject> _nameCard;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] public Transform cardBoard;
    [SerializeField] public Transform cardBoardOp;
    public int numberCardInHand;
    public int numberCardUsed;
    public int numberCardInHandOp;
    public int numberCardUsedOp;
    public bool useCard = false;
    public int cardMechanic;
    public List<GameObject> currentDeck;
    public List<GameObject> graveYard;
    public List<GameObject> currentDeckOp;
    public List<GameObject> graveYardOp;
    public Dictionary<int,(String,int)> currentCardInAIHand;
    public Dictionary<int,(String,int)> currentCardInPlayerHand;
    private void Start()
    {
        numberCardInHand = 0;
        numberCardUsed = 0;
        for (int i = 0; i < deck._getLength(); i++)
        {
            CreateCard(i,cardPrefab,cardBoard,currentDeck);
        }
        ShuffleCard(true,currentDeck);
        numberCardInHandOp = 0;
        numberCardUsedOp = 0;
        for (int i = 0; i < deck._getLength(); i++)
        {
            CreateCard(i,cardPrefab,cardBoardOp,currentDeckOp);
        }
        ShuffleCard(true,currentDeckOp);
        
    }
    public void CreateCard(int index, GameObject CardPrefab, Transform CardBoard, List<GameObject> CurrentDeck)
    {
        GameObject cardTransform = Instantiate(CardPrefab, CardBoard);
        ControlCard card = cardTransform.GetComponent<ControlCard>();
        card.ShowTextCard(index,CurrentDeck);
    }
    public void ShuffleCard(bool isFirst, List<GameObject> CurrentDeck)
    {
        int[] index = new int[this.deck._getLength()];
        for (int i = 0; i < this.deck._getLength(); i++)
        {
            index[i] = i;
        }
        _nameCard = new Dictionary<int, GameObject>();
        for (int i = 0; i < this.deck._getLength(); i++)
        {
            _nameCard.Add(i,CurrentDeck[i]);
        }
        


        CurrentDeck.Clear();
        for (int i = _nameCard.Count - 1; i >= 0 ; i--)
        {
            int j = Random.Range(0, i + 1);
            
            var tmp = _nameCard[i];
            _nameCard[i] = _nameCard[j];
            _nameCard[j] = tmp;
            CurrentDeck.Add(_nameCard[i]);
        }

        if (isFirst)
        {
            for (int i = 0; i < 5; i++)
            {
                CurrentDeck[i].SetActive(true);
                if(object.ReferenceEquals(CurrentDeck,currentDeck))
                {
                    numberCardInHand++;
                }
                else
                {
                    numberCardInHandOp++;
                }
                
            }
        }
        else
        {
            CurrentDeck[0].SetActive(true);
            numberCardInHand = 1;
        }
        
    }
    private void Update()
    {
        if (numberCardInHand + numberCardUsed == 11)
        {
            
            ShuffleCard(false,currentDeck);
            numberCardUsed = 0;
        }
        else if (numberCardUsedOp + numberCardInHandOp == 11)
        {
            
            ShuffleCard(false,currentDeckOp);
            numberCardUsedOp = 0;
        }
    }
}
