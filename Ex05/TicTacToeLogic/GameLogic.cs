using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex05.TicTacToeLogic
{
    public delegate void ValidMoveDelegate(ushort i_Row, ushort i_Col);
    public class GameLogic
    {
        private Board m_Board;
        private Player[] m_Player = new Player[2];
        private static ushort[] s_PlayerScore = new ushort[2] { 0, 0 };
        private eWinnerOfTheMatch m_WinnerOfTheMatch;
        private int m_NumOfAvailableCells;
        public event ValidMoveDelegate ValidMove;

        public enum eWinnerOfTheMatch
        {
            Player1,
            Player2,
            Tie,
            None
        }

        public GameLogic(Player.ePlayerType i_SecondPlayerType, ushort i_BoardSize)
        {
            m_Player[0] = new Player(Board.ePlayerNumber.Player1, Player.ePlayerType.User);
            m_Player[1] = new Player(Board.ePlayerNumber.Player2, i_SecondPlayerType);
            m_Board = new Board(i_BoardSize);
            m_WinnerOfTheMatch = eWinnerOfTheMatch.None;
            m_NumOfAvailableCells = i_BoardSize * i_BoardSize;
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }

        public eWinnerOfTheMatch WinnerOfTheMatch
        {
            get
            {
                return m_WinnerOfTheMatch;
            }
        }

        public ushort[] PlayerScore
        {
            get
            {
                return s_PlayerScore;
            }
        }

        public bool SecondPlayerIsComputer()
        {
            bool isComputer = m_Player[1].Type == Player.ePlayerType.Computer ? true : false;

            return isComputer;
        }

        public void Move(ushort i_PlayerNum, ushort? i_X, ushort? i_Y)
        {
            if (m_Player[i_PlayerNum].Type == Player.ePlayerType.User)
            {
                if (i_X.HasValue && i_Y.HasValue)
                {
                    m_Board.SetCell(i_X.Value, i_Y.Value, m_Player[i_PlayerNum].PlayerNumber);
                    m_NumOfAvailableCells--;
                    OnValidMove(i_X.Value, i_Y.Value);
                }
                else
                {
                    throw new ArgumentException("Both i_X and i_Y must have values.");
                }
            }
            else
            {
                makeComputerMove();
            }
        }

        protected virtual void OnValidMove(ushort i_Row, ushort i_Col)
        {
            if (ValidMove != null)
            {
                ValidMove.Invoke(i_Row, i_Col);
            }
        }

        public bool IsGameOver()
        {
            bool winnerIsFound = false;

            for(int i = 0; i < 2; i++)
            {
                for(ushort row = 0; row < m_Board.Size; row++)
                {
                    int sequence = 0;

                    for(ushort col = 0; col < m_Board.Size; col++)
                    {
                        if(m_Board.GetCell(row, col) == m_Player[i].PlayerNumber)
                        {
                            sequence++;
                            if(sequence == m_Board.Size)
                            {
                                s_PlayerScore[1 - i]++;
                                m_WinnerOfTheMatch = (eWinnerOfTheMatch)(1 - i);
                                winnerIsFound = true;
                                break;
                            }
                        }
                    }

                    if(winnerIsFound)
                    {
                        break;
                    }
                }

                if(winnerIsFound)
                {
                    break;
                }

                for(ushort col = 0; col < m_Board.Size; col++)
                {
                    int sequence = 0;

                    for(ushort row = 0; row < m_Board.Size; row++)
                    {
                        if(m_Board.GetCell(row, col) == m_Player[i].PlayerNumber)
                        {
                            sequence++;
                            if(sequence == m_Board.Size)
                            {
                                s_PlayerScore[1 - i]++;
                                m_WinnerOfTheMatch = (eWinnerOfTheMatch)(1 - i);
                                winnerIsFound = true;
                                break;
                            }
                        }
                    }

                    if(winnerIsFound)
                    {
                        break;
                    }
                }

                if(winnerIsFound)
                {
                    break;
                }

                int mainDiagonal = 0;
                int subDiagonal = 0;

                for(ushort row = 0; row < m_Board.Size; row++)
                {
                    if(m_Board.GetCell(row, row) == m_Player[i].PlayerNumber)
                    {
                        mainDiagonal++;
                        if (mainDiagonal == m_Board.Size)
                        {
                            s_PlayerScore[1 - i]++;
                            m_WinnerOfTheMatch = (eWinnerOfTheMatch)(1 - i);
                            winnerIsFound = true;
                            break;
                        }
                    }

                    if (m_Board.GetCell(row, (ushort)(m_Board.Size - 1 - row)) == m_Player[i].PlayerNumber)
                    {
                        subDiagonal++;
                        if (subDiagonal == m_Board.Size)
                        {
                            s_PlayerScore[1 - i]++;
                            m_WinnerOfTheMatch = (eWinnerOfTheMatch)(1 - i);
                            winnerIsFound = true;
                            break;
                        }
                    }
                }

                if(winnerIsFound)
                {
                    break;
                }
            }

            bool isTie = m_NumOfAvailableCells == 0;

            if (!winnerIsFound)
            {
                m_WinnerOfTheMatch = isTie ? eWinnerOfTheMatch.Tie : eWinnerOfTheMatch.None;
            }

            return isTie || winnerIsFound;
        }

        public void restartMatch()
        {
            ushort boardSize = m_Board.Size;
            m_Board = new Board(boardSize);
            m_WinnerOfTheMatch = eWinnerOfTheMatch.None;
            m_NumOfAvailableCells = boardSize * boardSize;
        }

        private void makeComputerMove()
        {
            Random random = new Random();

            // Generate random row and column indices
            ushort randomRow = (ushort)random.Next(0, m_Board.Size);
            ushort randomCol = (ushort)random.Next(0, m_Board.Size);

            // Check if the randomly generated cell is available
            while (m_Board.GetCell(randomRow, randomCol).HasValue)
            {
                randomRow = (ushort)random.Next(0, m_Board.Size);
                randomCol = (ushort)random.Next(0, m_Board.Size);
            }

            // Set the randomly generated cell for the computer player
            m_Board.SetCell(randomRow, randomCol, m_Player[1].PlayerNumber);
            m_NumOfAvailableCells--;

            // Trigger the valid move event
            OnValidMove(randomRow, randomCol);
        }
    }
}
