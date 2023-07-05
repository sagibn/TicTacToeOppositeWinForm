using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Ex05.TicTacToeLogic;

namespace Ex05.TicTacToeUI
{
    public class GameForm : Form
    {
        private Button[,] m_ButtonCell;
        private Label m_LabelPlayer1Name;
        private Label m_LabelPlayer2Name;
        private Label m_LabelPlayer1Score;
        private Label m_LabelPlayer2Score;
        private GameLogic m_TicTacToe;
        private ushort m_WhichPlayerTurn;
        private readonly ushort r_BoardSize;
        private readonly Player.ePlayerType r_SecondPlayerType;

        public GameForm(Player.ePlayerType i_SecondPlayerType, ushort i_BoardSize,
            string i_FirstPlayerName, string i_SecondPlayerName)
        {
            r_BoardSize = i_BoardSize;
            r_SecondPlayerType = i_SecondPlayerType;
            m_TicTacToe = new GameLogic(r_SecondPlayerType, r_BoardSize);
            m_ButtonCell = new Button[r_BoardSize, r_BoardSize];
            m_LabelPlayer1Name = new Label { Text = i_FirstPlayerName + ":", AutoSize = true, Name = i_FirstPlayerName };
            m_LabelPlayer1Score = new Label { Text = (m_TicTacToe.PlayerScore[0]).ToString(), AutoSize = true };
            m_LabelPlayer2Name = new Label { Text = i_SecondPlayerName + ":", AutoSize = true, Name = i_SecondPlayerName };
            m_LabelPlayer2Score = new Label { Text = (m_TicTacToe.PlayerScore[1]).ToString(), AutoSize = true };
            m_WhichPlayerTurn = 0;

            m_TicTacToe.ValidMove += m_TicTacToe_ValidMove;
            initForm();
            initControls();
        }

        private void m_TicTacToe_ValidMove(ushort i_Row, ushort i_Col)
        {
            if(i_Row < r_BoardSize && i_Col < r_BoardSize)
            {
                string playerSymbol = m_WhichPlayerTurn == 0 ? "X" : "O";

                m_ButtonCell[i_Row, i_Col].Text = playerSymbol;
                m_ButtonCell[i_Row, i_Col].Enabled = false;
                changeTurn();
                if (m_TicTacToe.IsGameOver())
                {
                    string winnerMessage;
                    string stateMessage = "A Win!";
                    string askForRematchMessage = "Would you like to play another round?";

                    if(m_TicTacToe.WinnerOfTheMatch == GameLogic.eWinnerOfTheMatch.Player1 ||
                        m_TicTacToe.WinnerOfTheMatch == GameLogic.eWinnerOfTheMatch.Player2)
                    {
                        int winnerNumber = m_TicTacToe.WinnerOfTheMatch == GameLogic.eWinnerOfTheMatch.Player1 ? 0 : 1;
                        Label winnerLabel = winnerNumber == 0 ? m_LabelPlayer1Score : m_LabelPlayer2Score;
                        string winnerName = winnerNumber == 0 ? m_LabelPlayer1Name.Name : m_LabelPlayer2Name.Name;

                        winnerLabel.Text = (m_TicTacToe.PlayerScore[winnerNumber]).ToString();
                        winnerMessage = "The winner is " + winnerName + "!\n" + askForRematchMessage;
                    }
                    else
                    {
                        winnerMessage = "Tie!\n" + askForRematchMessage;
                        stateMessage = "A Tie!";
                    }

                    var rematch = MessageBox.Show(winnerMessage, stateMessage, MessageBoxButtons.YesNo);

                    if(rematch == DialogResult.No)
                    {
                        this.Close();
                    }
                    else
                    {
                        restartMatch();
                    }
                }
                else
                {
                    if (m_TicTacToe.SecondPlayerIsComputer() && m_WhichPlayerTurn == 1)
                    {
                        m_TicTacToe.Move(m_WhichPlayerTurn, null, null);
                    }
                }
            }
        }

        private void restartMatch()
        {
            m_TicTacToe.RestartMatch();
            m_WhichPlayerTurn = 0;
            m_LabelPlayer1Name.Font = new Font(m_LabelPlayer1Name.Font, FontStyle.Bold);
            m_LabelPlayer1Score.Font = new Font(m_LabelPlayer1Score.Font, FontStyle.Bold);
            m_LabelPlayer2Name.Font = new Font(m_LabelPlayer2Name.Font, FontStyle.Regular);
            m_LabelPlayer2Score.Font = new Font(m_LabelPlayer2Score.Font, FontStyle.Regular);

            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    m_ButtonCell[row, col].Text = "";
                    m_ButtonCell[row, col].Enabled = true;
                }
            }
        }

        private void initForm()
        {
            this.Text = "TicTacToeMisere";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(60*r_BoardSize + 30, 60*r_BoardSize + 70);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Controls.Add(m_LabelPlayer1Name);
            this.Controls.Add(m_LabelPlayer2Name);
            this.Controls.Add(m_LabelPlayer1Score);
            this.Controls.Add(m_LabelPlayer2Score);
        }

        private void initControls()
        {
            for(int row = 0; row < r_BoardSize; row++)
            {
                for(int col = 0; col < r_BoardSize; col++)
                {
                    int currentRow = row;
                    int currentCol = col;

                    m_ButtonCell[row, col] = new Button();
                    this.Controls.Add(m_ButtonCell[row, col]);
                    m_ButtonCell[row, col].Text = "";
                    m_ButtonCell[row, col].Width = 50;
                    m_ButtonCell[row, col].Height = 50;
                    m_ButtonCell[row, col].Visible = true;
                    m_ButtonCell[row, col].Enabled = true;
                    m_ButtonCell[row, col].Click += (sender, e) =>
                    {
                        m_ButtonCell_Click((ushort)currentRow, (ushort)currentCol);
                    };

                    if (row == 0)
                    {
                        if (col == 0)
                        {
                            m_ButtonCell[row, col].Location = new Point(10, 10);
                        }
                        else
                        {
                            m_ButtonCell[row, col].Location = new Point(m_ButtonCell[row, col - 1].Right + 10, 10);
                        }
                    }
                    else
                    {
                        m_ButtonCell[row, col].Location = new Point(m_ButtonCell[row - 1, col].Location.X, m_ButtonCell[row - 1, col].Bottom + 10);
                    }
                }
            }

            m_LabelPlayer1Name.Location = new Point((this.ClientSize.Width / 2) - 70, m_ButtonCell[r_BoardSize - 1, r_BoardSize - 1].Bottom + 10);
            m_LabelPlayer1Name.Font = new Font(m_LabelPlayer1Name.Font, FontStyle.Bold);
            m_LabelPlayer1Name.Visible = true;

            m_LabelPlayer1Score.Location = new Point(m_LabelPlayer1Name.Right + 1, m_LabelPlayer1Name.Location.Y);
            m_LabelPlayer1Score.Font = new Font(m_LabelPlayer1Score.Font, FontStyle.Bold);
            m_LabelPlayer1Score.Visible = true;

            m_LabelPlayer2Name.Location = new Point(m_LabelPlayer1Score.Right + 20, m_LabelPlayer1Name.Location.Y);
            m_LabelPlayer2Name.Visible = true;

            m_LabelPlayer2Score.Location = new Point(m_LabelPlayer2Name.Right + 1, m_LabelPlayer2Name.Location.Y);
            m_LabelPlayer2Score.Visible = true;
        }

        private void m_ButtonCell_Click(ushort i_Row, ushort i_Col)
        {
            m_ButtonCell[i_Row, i_Col].Enabled = false;
            m_TicTacToe.Move(m_WhichPlayerTurn, i_Row, i_Col);
        }

        private void changeTurn()
        {
            m_LabelPlayer1Name.Font = new Font(m_LabelPlayer1Name.Font, m_LabelPlayer1Name.Font.Style ^ FontStyle.Bold);
            m_LabelPlayer2Name.Font = new Font(m_LabelPlayer2Name.Font, m_LabelPlayer2Name.Font.Style ^ FontStyle.Bold);
            m_LabelPlayer1Score.Font = new Font(m_LabelPlayer1Score.Font, m_LabelPlayer1Score.Font.Style ^ FontStyle.Bold);
            m_LabelPlayer2Score.Font = new Font(m_LabelPlayer2Score.Font, m_LabelPlayer2Score.Font.Style ^ FontStyle.Bold);
            m_WhichPlayerTurn = (ushort)(1 - m_WhichPlayerTurn);
        }
    }
}
