using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
public enum CellTypes { 
    X,
    O,
    Del,
    None
}
public class Cell : MonoBehaviour
{

    public int row;
    public int col;
    public CellTypes type;
    public int totalPlayer = 10;
    public int currentPlayer;
    private Board board;

    public Sprite xSprite;
    public Sprite oSprite;
    public Sprite delSprite;
    public Image image;
    private Button button;
    private ControlDrawCard _controlDrawCard;
    private SwitchMechanics _switchMechanics;
    private bool _canChange;
    private int _cardEffect;
    private int _direction;
    private int[] _listDirection;
    private bool _scroll = false;
    private AIOpponent _aiOpponent;
    private AddCardToHand _addCardToHand;
    private void Awake()
    {
        currentPlayer = totalPlayer;
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void Start()
    {
        board = FindAnyObjectByType<Board>();
        _addCardToHand = FindAnyObjectByType<AddCardToHand>();
        _controlDrawCard = FindAnyObjectByType<ControlDrawCard>();
        _switchMechanics = FindAnyObjectByType<SwitchMechanics>();
        _aiOpponent = FindAnyObjectByType<AIOpponent>();
    }

    public void FixedUpdate()
    {
        if (_controlDrawCard.useCard)
        {
            if (Input.GetKey(KeyCode.F))
            {
                _scroll = true;
            }
            else if(Input.GetKey(KeyCode.G))
            {
                _scroll = false;
            }
        }
    }

    public void ChangeCellImage(CellTypes turn) {
        switch (turn) {
            case CellTypes.X:
                this.type = turn;
                SetCellType(this.type);
                break;
            case CellTypes.O:
                this.type = turn;
                SetCellType(this.type);
                break;
            case CellTypes.None:
                this.type = turn;
                SetCellType(this.type);
                break;
        }
    }
    public void SetCellType(CellTypes type) {
        switch (type) {
            case CellTypes.X:
                this.image.sprite = xSprite;
                break;
            case CellTypes.O:
                this.image.sprite = oSprite;
                break; 
            case CellTypes.None:
                this.image.sprite = delSprite;
                break;
        } 
    }

    public bool SetInMatrix(CellTypes turn) {
        if (board.matrix[(this.row, this.col)] != CellTypes.None && !_canChange) {
            return false;
        }
        
        board.matrix[(this.row, this.col)] = turn;
        return true;
    }

    public void ChangeTurn (CellTypes curr_turn) {
        switch (curr_turn) {
            case CellTypes.X:
                board.currentTurn = CellTypes.O;
                break;
            case CellTypes.O:
                board.currentTurn = CellTypes.X;
                break;
        }
    }
    private void GetDirection(int row, int col, int boardsize,bool scroll)
    {
        if (scroll)
        {
            _listDirection = new[] { (row) * boardsize + col,row * boardsize + (col-1), row * boardsize + (col+1)};
        }
        else
        {
            _listDirection = new[] {(row) * boardsize + col, (row-1) * boardsize + col, (row+1) * boardsize + col};
        }
    }
    public void OnClick() {
        Debug.LogFormat($"{this.row};{this.col}");
        int mechanic = _controlDrawCard.cardMechanic;
        _canChange = _controlDrawCard.useCard;
        _aiOpponent.GetPlayerDirection(this.row,this.col);
        if (SetInMatrix(board.currentTurn) || _canChange)
        {
            if (_canChange)
            {
                GetDirection(row , col, board.boardSize,_scroll);
                _switchMechanics.CallMechanics(mechanic,_listDirection, board.currentTurn);
                if (board.Check(this.row, this.col))
                {
                    Debug.Log(board.currentTurn + " is Winner!");
                    
                    if (board.currentTurn == CellTypes.X)
                    {
                        currentPlayer--;
                        Debug.Log(board.currentTurn + "HP: " + currentPlayer);
                        _addCardToHand.AddCard(true);
                    }
                    else
                    {
                        _aiOpponent._currentHp--;
                        _addCardToHand.AddCard(false);
                        Debug.Log(board.currentTurn + "HP: " + _aiOpponent._currentHp);
                    }
                    ChangeTurn(board.currentTurn);
                }
                
                ChangeTurn(board.currentTurn);
                _controlDrawCard.useCard = false;
            }
            else
            {
                ChangeCellImage(board.currentTurn);
                // if (board.ChangeSize(this.row, this.col)) {
                //     board.ReRenderBoard();
                // };
        
                if (board.Check(this.row, this.col))
                {
                    Debug.Log(board.currentTurn + " is Winner!");
                    if (board.currentTurn == CellTypes.X)
                    {
                        currentPlayer--;
                        Debug.Log(board.currentTurn + "HP: " + currentPlayer);
                        _addCardToHand.AddCard(true);
                    }
                    else
                    {
                        _aiOpponent._currentHp--;
                        Debug.Log(board.currentTurn + "HP: " + _aiOpponent._currentHp);
                        _addCardToHand.AddCard(false);
                    }
                    ChangeTurn(board.currentTurn);
                }
                ChangeTurn(board.currentTurn);
            }
        }
    }
    
}