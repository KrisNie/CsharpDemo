using System.Collections.Generic;

namespace App.Algorithm;

// https://leetcode-cn.com/problems/sudoku-solver/
public class SudokuSolver
{
    private readonly bool[,,] _block = new bool[3, 3, 9];
    private readonly bool[,] _column = new bool[9, 9];
    private readonly bool[,] _row = new bool[9, 9];
    private readonly List<int[]> _spaces = new();
    private bool _valid;

    public void SolveSudoku(char[][] board)
    {
        for (var i = 0; i < 9; i++)
        {
            for (var j = 0; j < 9; j++)
                if (board[i][j] == '.')
                {
                    _spaces.Add(new[] { i, j });
                }
                else
                {
                    var digit = board[i][j] - '0' - 1;
                    _row[i, digit] = _column[j, digit] = _block[i / 3, j / 3, digit] = true;
                }
        }

        Dfs(board, 0);
    }

    private void Dfs(char[][] board, int position)
    {
        if (position == _spaces.Count)
        {
            _valid = true;
            return;
        }

        var space = _spaces[position];
        var i = space[0];
        var j = space[1];
        for (var digit = 0; digit < 9 && !_valid; digit++)
            if (!_row[i, digit] && !_column[j, digit] && !_block[i / 3, j / 3, digit])
            {
                _row[i, digit] = _column[j, digit] = _block[i / 3, j / 3, digit] = true;
                board[i][j] = (char)(digit + '0' + 1);
                Dfs(board, position + 1);
                _row[i, digit] = _column[j, digit] = _block[i / 3, j / 3, digit] = false;
            }
    }
}