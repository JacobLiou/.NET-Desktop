using System;

namespace TetrisGame.Models
{
    public class GameBoard
    {
        private bool[,] grid;
        private int rows;
        private int columns;

        public bool[,] Grid => grid;

        public int Width { get; internal set; }

        public GameBoard(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            grid = new bool[rows, columns];
        }

        public void Clear()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    grid[i,j] = false;
                }
            }
        }

        public bool IsValidMove(Block block)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (block.Shape[i,j])
                    {
                        int newRow = block.Y + i;
                        int newCol = block.X + j;

                        // 检查是否超出边界
                        if (newRow < 0 || newRow >= rows || 
                            newCol < 0 || newCol >= columns)
                        {
                            return false;
                        }

                        // 检查是否与已有方块重叠
                        if (grid[newRow, newCol])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void PlaceBlock(Block block)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (block.Shape[i,j])
                    {
                        grid[block.Y + i, block.X + j] = true;
                    }
                }
            }
        }

        public int ClearFullLines()
        {
            int linesCleared = 0;

            for (int row = rows - 1; row >= 0; row--)
            {
                if (IsLineFull(row))
                {
                    ClearLine(row);
                    ShiftLinesDown(row);
                    linesCleared++;
                    row++; // 重新检查当前行，因为上面的行已经下移
                }
            }

            return linesCleared;
        }

        private bool IsLineFull(int row)
        {
            for (int col = 0; col < columns; col++)
            {
                if (!grid[row, col])
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearLine(int row)
        {
            for (int col = 0; col < columns; col++)
            {
                grid[row, col] = false;
            }
        }

        private void ShiftLinesDown(int clearedRow)
        {
            for (int row = clearedRow - 1; row >= 0; row--)
            {
                for (int col = 0; col < columns; col++)
                {
                    grid[row + 1, col] = grid[row, col];
                    grid[row, col] = false;
                }
            }
        }
    }
}
