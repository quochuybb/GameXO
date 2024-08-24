using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class AIOpponent : MonoBehaviour
{
    private Board _board;
    private Cell _cell;
    private Dictionary<(int, int), CellTypes> _virtualMatrix;
    private CellTypes _currentTurn;
    private int row;
    private int col;
    private DateTime _currentTime;
    private bool useCard;
    private Direction _playerMove;
    private int _totalHp = 10;
    public int _currentHp;
    private ControlDrawCard _controlDrawCard;
    struct Direction
    {
        public int MoveX;
        public int MoveY;
    }
    private void Start()
    {
        _currentHp = _totalHp;
        _currentTime = DateTime.Now;
        _board = FindAnyObjectByType<Board>();
        _virtualMatrix = new Dictionary<(int, int), CellTypes>();
        _controlDrawCard = FindAnyObjectByType<ControlDrawCard>();

    }
    private CellTypes GetCell(int row, int col)
    {
        bool test = _virtualMatrix.TryGetValue((row, col), out CellTypes values);
        return _virtualMatrix.TryGetValue((row, col), out CellTypes value) ? value : CellTypes.None;
    }
    private int GetValue (int row, int col) {
        if (this._virtualMatrix.TryGetValue((row,col), out CellTypes value) && value == _currentTurn )
        {
            return 1;
        }
        else {
            return 0;
        }
    }
    public bool CheckVirtual(int row, int col) {
        int min_row_index = Math.Max(_board.min_y, row - 2);

        int count = 
            GetValue(min_row_index, col) + 
            GetValue(min_row_index + 1, col) + 
            GetValue(min_row_index + 2, col);

        if (count >= 3) {
            return true;
        }

        // Vertical: Y 
        for (int i = min_row_index + 1; i <= min_row_index + 2 && i + 2 <= _board.max_y; i++) {
            if (GetCell(i + 2, col) != _currentTurn)
            {
                 i += 2;
                continue;
            }

            count =
                GetValue(i, col) +
                GetValue(i + 1, col) +
                GetValue(i + 2, col);

            if (count >= 3) {
                return true;
            }
        };

        // Horizontal: X 
        int min_col_index = Math.Max(_board.min_x, col - 2);

        count = GetValue(row, min_col_index) +
                GetValue(row, min_col_index + 1) +
                GetValue(row, min_col_index + 2);

        if (count >= 3) {
            return true;
        }

        for (int i = min_col_index + 1; i <= min_col_index + 2 && i + 2 <= _board.max_x; i++) {
            if (GetCell(row, i + 2) != _currentTurn) {
                i += 2;
                continue;
            }

            count =
                GetValue(row, i) +
                GetValue(row, i + 1) +
                GetValue(row, i + 2);

            if (count == 3) {
                return true;
            }

        };

        // Diagonal
        // Top - Left to Bottom - Right
        int row_index = min_row_index;
        int col_index = min_col_index;

        while (row_index <= row + 2 && col_index <= col + 2 && row_index + 2 <= _board.max_y && col_index + 2 <= _board.max_x) {

            if (GetCell(row_index, col_index) != _currentTurn) {
                row_index += 2;
                col_index += 2;
                continue;
            }

            count =
                GetValue(row_index, col_index) +
                GetValue(row_index + 1, col_index + 1) +
                GetValue(row_index + 2, col_index + 2);

            if (count >= 3) {
                return true;
            }

            row_index++;
            col_index++;
        }

        row_index = Math.Min(_board.max_y, row + 2);
        col_index = min_col_index;
        // Bottom - Left to Top - Right
         while (row_index >= row - 2 && col_index <= col + 2 && row_index - 2 >= _board.min_y && col_index + 2 <= _board.max_x) {
            if (GetCell(row_index - 2, col_index + 2) != _currentTurn) {
                row_index -= 2;
                col_index += 2;
                continue;
            }

            count =
                GetValue(row_index, col_index) +
                GetValue(row_index - 1, col_index + 1) +
                GetValue(row_index - 2, col_index + 2);

            if (count >= 3) {
                return true;
            }

            row_index--;
            col_index++;
        }   

        return false;
    }

    int EvaluateLines()
    {
        return 10;
    }
    int Evaluate(int row, int col)
    {
        
        if (CheckVirtual(row, col) &&  _currentTurn == CellTypes.X)
        {
            return -100;
        }
        else if (CheckVirtual(row, col) && _currentTurn == CellTypes.O)
        {
            return 100;
        }

        return 0; 
    }
    bool IsMovesLeft()
    {

        foreach (var cell in _virtualMatrix)
        {
            if (cell.Value == CellTypes.None)
            {
                return true;
            }
        }
        return false;
    }
    int Minimax(int depth, bool isMax, int alpha, int beta)
    {
        int score = Evaluate(this.row,this.col);
        // score += EvaluateLines();
        // if (depth == 3)
        // {
        //     return score - depth;
        // }
        if (score == 100)
            return score - depth;
        if (score == -100)
            return score - depth;
        if (!IsMovesLeft())
            return 0;

        if (isMax)
        {
            int best = int.MinValue;
            for (int i = 0; i <= _board.max_x; i++)
            {
                for (int j = 0; j <= _board.max_y; j++)
                {
                    if (_virtualMatrix[(i,j)] == CellTypes.None)
                    {
                        _virtualMatrix[(i, j)] = CellTypes.O;
                        row = i;
                        col = j;
                        _currentTurn = CellTypes.O;
                        best = Math.Max(best, Minimax(depth + 1, !isMax, alpha, beta));
                        alpha = Math.Max(alpha, best);
                        _virtualMatrix[(i, j)] = CellTypes.None;
                        if (beta <= alpha)
                            break;
                    }
                }
                
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;

            for (int i = 0; i <= _board.max_x; i++)
            {
                for (int j = 0; j <= _board.max_y; j++)
                {
                    if (_virtualMatrix[(i,j)] == CellTypes.None)
                    {
                        _virtualMatrix[(i, j)] = CellTypes.X;
                        row = i;
                        col = j;
                        _currentTurn = CellTypes.X;
                        best = Math.Min(best, Minimax(depth + 1, !isMax, alpha, beta));
                        beta = Math.Min(beta, best);
                        _virtualMatrix[(i, j)] = CellTypes.None;
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return best;
        }
    }
    
    Direction FindBestMove()
    {
        int bestVal = int.MinValue;
        Direction move = new Direction();
        _virtualMatrix = _board.matrix.ToDictionary(entry => entry.Key,entry => entry.Value);
        for (int i = 0; i <= _board.max_x; i++)
        {
            for (int j = 0; j <= _board.max_y; j++)
            {
                
                if (_virtualMatrix[(i,j)] == CellTypes.None)
                {
                    _virtualMatrix[(i, j)] = CellTypes.O;
                    int moveVal = Minimax( 0, false, int.MinValue, int.MaxValue);
                    _virtualMatrix[(i, j)] = CellTypes.None;
                    if (moveVal > bestVal)
                    {
                        move.MoveX = i;
                        move.MoveY = j;
                        bestVal = moveVal;
                    }
                }
            }
            
        }

        return move;
    }

    public void GetPlayerDirection(int rowPlayer, int colPlayer)
    {
        _playerMove.MoveX = rowPlayer;
        _playerMove.MoveY = colPlayer;
    }

    private void ReadCard()
    {
        
    }
    private bool FindWay()
    {
        if ((_board.turn == 0 && CanUseCardFirstTurn()) || (_totalHp - _currentHp == 1))
        {
            
            return true;
        }
        if (_currentHp >= _totalHp * 0.7)
        {
            
            Debug.Log("Phase 1");
        }
        else if (_currentHp >= _totalHp * 0.3)
        {
            Debug.Log("Phase 2");
        }
        else
        {
            Debug.Log("Phase 3");
        }

        return false;
    }
    private Direction FindMove()
    {
        Direction move = new Direction();
        if (_board.turn == 0)
        {
            if (_playerMove.MoveX >= 1 && _playerMove.MoveX <= 3)
            {
                if (_playerMove.MoveY >= 1 && _playerMove.MoveY <= 3)
                {
                    if (_currentTime.Second > 40)
                    {
                        move.MoveX = _playerMove.MoveX + 1;
                    }
                    else
                    {
                        move.MoveX = _playerMove.MoveX - 1;
                    }
                    if (_currentTime.Second % 3 == 0)
                    {
                        move.MoveY = _playerMove.MoveY + 1;
                    }
                    else if (_currentTime.Second % 3 == 1)
                    {
                        move.MoveY = _playerMove.MoveY;
                    }
                    else
                    {
                        move.MoveY = _playerMove.MoveY - 1;
                    }
                    return move;
                }
                else
                {
                    if (_playerMove.MoveY == _board.max_y)
                    {
                        if (_currentTime.Second > 40)
                        {
                            move.MoveX = _playerMove.MoveX + 1;
                        }
                        else
                        {
                            move.MoveX = _playerMove.MoveX - 1;
                        } 
                        if (_currentTime.Second % 3 == 0)
                        {
                            move.MoveY = _playerMove.MoveY;
                        }
                        else
                        {
                            move.MoveY = _playerMove.MoveY - 1;
                        }
                        return move;
                    }
                    else
                    { 
                        if (_currentTime.Second > 40)
                        {
                            move.MoveX = _playerMove.MoveX + 1;
                        }
                        else
                        {
                            move.MoveX = _playerMove.MoveX - 1;
                        } 
                        if (_currentTime.Second % 3 == 0)
                        {
                            move.MoveY = _playerMove.MoveY;
                        }
                        else
                        {
                            move.MoveY = _playerMove.MoveY + 1;
                        }
                        return move;
                    }
                }
            }
            else
            {
                if (_playerMove.MoveY >= 1 && _playerMove.MoveY <= 3)
                {
                    if (_playerMove.MoveX == _board.max_x)
                    {
                        if (_currentTime.Second > 30)
                        {
                            move.MoveX = _playerMove.MoveX ;
                        }
                        else
                        {
                            move.MoveX = _playerMove.MoveX - 1;
                        } 
                        move.MoveY = _playerMove.MoveY - 1;
                        return move;
                    }
                    else
                    { 
                        move.MoveX = _playerMove.MoveX + 1;
                        if (_currentTime.Second % 3 == 0)
                        {
                            move.MoveY = _playerMove.MoveY;
                        }
                        else
                        {
                            move.MoveY = _playerMove.MoveY - 1;
                        }
                        return move;
                    }
                }
                else
                {
                    if (_playerMove.MoveY == _board.min_y)
                    {
                        if (_playerMove.MoveX == _board.min_x)
                        {
                            if (_currentTime.Second % 2 == 0)
                            {
                                move.MoveX = _playerMove.MoveX + 1;
                            }
                            else
                            {
                                move.MoveX = _playerMove.MoveX;
                            } 
                            move.MoveY = _playerMove.MoveY + 1;
                            return move;

                        }
                        else
                        {
                            if (_currentTime.Second % 2 == 0)
                            {
                                move.MoveX = _playerMove.MoveX - 1;
                            }
                            else
                            {
                                move.MoveX = _playerMove.MoveX;
                            }
                            move.MoveY = _playerMove.MoveY + 1;
                            return move;
                        }
                    }
                    else
                    {
                        if (_playerMove.MoveX == _board.min_x)
                        {
                            if (_currentTime.Second % 2 == 0)
                            {
                                move.MoveX = _playerMove.MoveX + 1;
                            }
                            else
                            {
                                move.MoveX = _playerMove.MoveX;
                            }
                            move.MoveY = _playerMove.MoveY - 1;
                            return move;
                        }
                        else
                        {
                            if (_currentTime.Second % 2 == 0)
                            {
                                move.MoveX = _playerMove.MoveX - 1;
                            }
                            else
                            {
                                move.MoveX = _playerMove.MoveX;
                            }
                            move.MoveY = _playerMove.MoveY - 1;
                            return move;
                        }
                    }
                }
            }
        }
        return move;
    }
    
    private void CanUseCard()
    {
        
    }
    private bool CanUseCardFirstTurn()
    {
        if (_currentTime.Minute % 2 == 0)
        {
            return true;
        }
        return false;
    }
    private void FixedUpdate()
    {
        if (_board.currentTurn == CellTypes.O)
        {

            // if (FindWay())
            // {
            //     Debug.Log("Use Card");
            //     _board.turn++;
            //     useCard = false;
            //     transform.GetChild(0).GetComponent<Cell>().ChangeTurn(CellTypes.O);
            // }
            // else
            // {
            //     Direction move = FindMove();
            //     transform.GetChild(move.MoveX*3 + move.MoveY).GetComponent<Cell>().OnClick();
            //     _board.turn++;
            // }
            Direction move = FindBestMove();
            Debug.Log(move.MoveX+"|"+move.MoveY);
            transform.GetChild(move.MoveX*3 + move.MoveY).GetComponent<Cell>().OnClick();
            _board.turn++;
        }
    }
}
