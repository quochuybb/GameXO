using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Board : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform board;
    private Transform newBoard;
    public GridLayoutGroup gridLayout;
    private SwitchMechanics _switchMechanics;
    public int boardSize;
    public int min_x;
    public int max_x;
    public int min_y;
    public int max_y;
    public string card;
    public CellTypes currentTurn;
    public Dictionary<(int, int), CellTypes> matrix;
    public int turn;
    // Start is called before the first frame update
    void Start()
    {
        this.currentTurn = CellTypes.X;
        matrix = new Dictionary<(int, int), CellTypes>();
        gridLayout.constraintCount = boardSize;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        _switchMechanics = FindAnyObjectByType<SwitchMechanics>();
        CreateBoard(boardSize);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void AddPoint(int i, int j, CellTypes cell_type, Dictionary<(int, int), CellTypes> matrix) {
        matrix.Add((i, j), cell_type);
    }

    private void CreateBoard(int board_size) {
        this.min_x = 0;
        this.max_x = boardSize - 1;
        this.min_y = 0;
        this.max_y = boardSize - 1;
        Size size = GetSize();
        for (int i = 0; i <= size.width - 1; i++) {
            for (int j = 0; j <= size.height - 1; j++) {
                GameObject cellTransform = Instantiate(cellPrefab, board);
                Cell cell = cellTransform.GetComponent<Cell>();
                cell.row = i;
                cell.col = j;
                AddPoint(i, j, CellTypes.None, this.matrix);
            }
        }
    }

    public void ReRenderBoard ()
    {
        // Clear the new board and matrix

        Size size = GetSize();
        Debug.LogFormat($"Size: {size.ToString()}");
        Dictionary<(int, int), CellTypes> newMatrix = new Dictionary<(int, int), CellTypes>();

        for (int i = 0; i <= size.width - 1; i++) {
            for (int j = 0; j <= size.height - 1; j++) {
                GameObject cellTransform = Instantiate(cellPrefab, newBoard);
                Cell cell = cellTransform.GetComponent<Cell>();
                cell.row = i + this.min_x;
                cell.col = j + this.min_y;

                if (matrix.TryGetValue((i + this.min_x, j + this.min_y), out CellTypes cell_type))
                {
                   AddPoint(i + this.min_x, j + this.min_y, cell_type, newMatrix);
                    cell.SetCellType(cell_type);
                }
                else {
                   AddPoint(i + this.min_x, j + this.min_y, cell_type, newMatrix);
                    cell.SetCellType(CellTypes.None);
                }
            }
        }

        this.board = newBoard;
        this.matrix = newMatrix;
    }

    /// <returns>(x_length, y_length)</returns>
    private Size GetSize(){
        int width = this.max_x - this.min_x + 1;
        int height = this.max_y - this.min_y + 1;
        Debug.LogFormat($"min_x: {this.min_x} - min_y: {this.min_y}");
        return new Size ( height, width );
    }

    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns>Cell's type</returns>
    private CellTypes GetCell(int row, int col)
    {
        bool test = this.matrix.TryGetValue((row, col), out CellTypes values);
        return this.matrix.TryGetValue((row, col), out CellTypes value) ? value : CellTypes.None;
    }

    /// <summary>
    /// It will return 1 when the cell is pressed same with current turn's cell type
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns>
    /// 1 if the cell same with current turn. Otherwise, 0
    /// </returns>
    private int GetValue (int row, int col) {
        if (this.matrix.TryGetValue((row,col), out CellTypes value) && value == currentTurn )
        {
            return 1;
        }
        else {
            return 0;
        }
    }
    public bool Check(int row, int col) {
        int min_row_index = Math.Max(min_y, row - 2);

        int count = 
            GetValue(min_row_index, col) + 
            GetValue(min_row_index + 1, col) + 
            GetValue(min_row_index + 2, col);

        if (count >= 3)
        {
            DeleteLineWin(min_row_index,col);
            DeleteLineWin(min_row_index +1 ,col);
            DeleteLineWin(min_row_index + 2,col);
            return true;
        }

        // Vertical: Y 
        for (int i = min_row_index + 1; i <= min_row_index + 2 && i + 2 <= max_y; i++) {
            if (GetCell(i + 2, col) != currentTurn)
            {
                 i += 2;
                continue;
            }

            count =
                GetValue(i, col) +
                GetValue(i + 1, col) +
                GetValue(i + 2, col);

            if (count >= 3) {
                DeleteLineWin(i,col);
                DeleteLineWin(i +1 ,col);
                DeleteLineWin(i + 2,col);
                return true;
            }
        };

        // Horizontal: X 
        int min_col_index = Math.Max(min_x, col - 2);

        count = GetValue(row, min_col_index) +
                GetValue(row, min_col_index + 1) +
                GetValue(row, min_col_index + 2);

        if (count >= 3) {
            DeleteLineWin(row,min_col_index);
            DeleteLineWin(row, min_col_index + 1);
            DeleteLineWin(row,min_col_index + 2);
            return true;
        }

        for (int i = min_col_index + 1; i <= min_col_index + 2 && i + 2 <= max_x; i++) {
            if (GetCell(row, i + 2) != currentTurn) {
                i += 2;
                continue;
            }

            count =
                GetValue(row, i) +
                GetValue(row, i + 1) +
                GetValue(row, i + 2);

            if (count == 3) {
                DeleteLineWin(row,i);
                DeleteLineWin(row, i + 1);
                DeleteLineWin(row,i + 2);
                return true;
            }

        };

        // Diagonal
        // Top - Left to Bottom - Right
        int row_index = min_row_index;
        int col_index = min_col_index;

        while (row_index <= row + 2 && col_index <= col + 2 && row_index + 2 <= max_y && col_index + 2 <= max_x) {

            if (GetCell(row_index, col_index) != currentTurn) {
                row_index += 2;
                col_index += 2;
                continue;
            }

            count =
                GetValue(row_index, col_index) +
                GetValue(row_index + 1, col_index + 1) +
                GetValue(row_index + 2, col_index + 2);

            if (count >= 3) {
                DeleteLineWin(row_index,col_index);
                DeleteLineWin(row_index +1 , col_index + 1);
                DeleteLineWin(row_index + 2,min_col_index + 2);
                return true;
            }

            row_index++;
            col_index++;
        }

        row_index = Math.Min(max_y, row + 2);
        col_index = min_col_index;
        // Bottom - Left to Top - Right
         while (row_index >= row - 2 && col_index <= col + 2 && row_index - 2 >= min_y && col_index + 2 <= max_x) {
            if (GetCell(row_index - 2, col_index + 2) != currentTurn) {
                row_index -= 2;
                col_index += 2;
                continue;
            }

            count =
                GetValue(row_index, col_index) +
                GetValue(row_index - 1, col_index + 1) +
                GetValue(row_index - 2, col_index + 2);

            if (count >= 3) {
                DeleteLineWin(row_index,col_index);
                DeleteLineWin(row_index -1 , col_index + 1);
                DeleteLineWin(row_index - 2,min_col_index + 2);
                return true;
            }

            row_index--;
            col_index++;
        }   

        return false;
    }

    public void DeleteLineWin( int row, int col)
    {
        if (this.matrix.TryGetValue((row,col), out CellTypes value) && value == currentTurn )
        {
            matrix[(row, col)] = CellTypes.None;
            _switchMechanics.Remove(row*3 + col);
        }
        else {
            return;
        }
    }
    public bool ChangeSize(int x, int y) {
         DirectionType type = Direction.GetDirection(this, x, y);

        switch (type) {
            case DirectionType.MAX_BOTH:
                this.max_x++;
                this.max_y++;
                gridLayout.constraintCount += 1;
                return true;
            case DirectionType.MAX_X:
                this.max_x++;
                gridLayout.constraintCount += 1;
                return true;
            case DirectionType.MAX_Y:
                this.max_y++;
                return true;
            case DirectionType.MIN_BOTH:
                this.min_x--;
                this.min_y--;
                return true;
            case DirectionType.MIN_X:
                this.min_x--;
                gridLayout.constraintCount += 1;
                return true;
            case DirectionType.MIN_Y:
                this.min_y--;
                return true;
            default:
                return false;
        }
    }
}
