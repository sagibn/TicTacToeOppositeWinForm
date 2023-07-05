using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex05.TicTacToeLogic
{
    public class Board
    {
        private ushort m_Size;
        private ePlayerNumber?[,] m_Cells;
        internal static readonly int sr_MinBoardSize = 4;
        internal static readonly int sr_MaxBoardSize = 10;

        public enum ePlayerNumber
        {
            Player1,
            Player2
        }

        public Board(ushort i_Size)
        {
            if (i_Size < sr_MinBoardSize || i_Size > sr_MaxBoardSize)
            {
                throw new ArgumentOutOfRangeException("i_size", "Size must be between 3 and 9.");
            }

            m_Size = i_Size;
            m_Cells = new ePlayerNumber?[m_Size, m_Size];
            for (int row = 0; row < m_Size; row++)
            {
                for (int col = 0; col < m_Size; col++)
                {
                    m_Cells[row, col] = null;
                }
            }
        }

        public int MinBoardSize
        {
            get
            {
                return sr_MinBoardSize;
            }
        }

        public int MaxBoardSize
        {
            get
            {
                return sr_MaxBoardSize;
            }
        }

        public ushort Size
        {
            get
            {
                return m_Size;
            }
        }

        public ePlayerNumber? GetCell(ushort i_Row, ushort i_Col)
        {
            if (i_Row < 0 || i_Row >= m_Size)
            {
                throw new ArgumentOutOfRangeException("row", "Row must be between 1 and " + (m_Size) + ".");
            }

            if (i_Col < 0 || i_Col >= m_Size)
            {
                throw new ArgumentOutOfRangeException("col", "Column must be between 1 and " + (m_Size) + ".");
            }

            return m_Cells[i_Row, i_Col];
        }

        public void SetCell(ushort i_Row, ushort i_Col, ePlayerNumber? i_PlayerNum)
        {
            if (i_Row < 0 || i_Row >= m_Size)
            {
                throw new ArgumentOutOfRangeException("row", "Row must be between 1 and " + (m_Size) + ".");
            }

            if (i_Col < 0 || i_Col >= m_Size)
            {
                throw new ArgumentOutOfRangeException("col", "Column must be between 1 and " + (m_Size) + ".");
            }

            if (!m_Cells[i_Row, i_Col].HasValue || !i_PlayerNum.HasValue)
            {
                m_Cells[i_Row, i_Col] = i_PlayerNum;
            }
            else
            {
                throw new InvalidOperationException("Cell already has a value.");
            }
        }
    }
}
