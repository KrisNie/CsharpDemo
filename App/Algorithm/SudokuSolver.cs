using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Algorithm.MS
{
    // https://leetcode-cn.com/problems/sudoku-solver/
    public class SudokuSolver
    {
        private bool[,] _row = new bool[9, 9];
        private bool[,] _column = new bool[9, 9];
        private bool[,,] _block = new bool[3, 3, 9];
        private bool _valid = false;
        private List<int[]> _spaces = new();

        public void SolveSudoku(char[][] board)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
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
            for (int digit = 0; digit < 9 && !_valid; digit++)
            {
                if (!_row[i, digit] && !_column[j, digit] && !_block[i / 3, j / 3, digit])
                {
                    _row[i, digit] = _column[j, digit] = _block[i / 3, j / 3, digit] = true;
                    board[i][j] = (char)(digit + '0' + 1);
                    Dfs(board, position + 1);
                    _row[i, digit] = _column[j, digit] = _block[i / 3, j / 3, digit] = false;
                }
            }
        }
    }
}