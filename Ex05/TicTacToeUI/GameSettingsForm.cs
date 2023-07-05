using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using Ex05.TicTacToeLogic;

namespace Ex05.TicTacToeUI
{
    public class GameSettingsForm : Form
    {
        private Label m_LabelPlayers;
        private Label m_LabelPlayer1;
        private Label m_LabelPlayer2;
        private Label m_LabelBoardSize;
        private Label m_LabelRows;
        private Label m_LabelCols;
        private CheckBox m_CheckBoxPlayer2;
        private TextBox m_TextBoxPlayer1;
        private TextBox m_TextBoxPlayer2;
        private NumericUpDown m_NumericUpDownRows;
        private NumericUpDown m_NumericUpDownCols;
        private Button m_ButtonStartGame;

        public GameSettingsForm()
        {
            m_LabelPlayers = new Label();
            m_LabelPlayer1 = new Label();
            m_LabelPlayer2 = new Label();
            m_LabelBoardSize = new Label();
            m_LabelRows = new Label();
            m_LabelCols = new Label();
            m_CheckBoxPlayer2 = new CheckBox();
            m_TextBoxPlayer1 = new TextBox();
            m_TextBoxPlayer2 = new TextBox();
            m_NumericUpDownRows = new NumericUpDown();
            m_NumericUpDownCols = new NumericUpDown();
            m_ButtonStartGame = new Button();
            initForm();
            initControls();
        }

        private void initForm()
        {
            this.Text = "Game Settings";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(250, 250);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            addControls();
        }

        private void addControls()
        {
            this.Controls.Add(m_LabelPlayers);
            this.Controls.Add(m_LabelPlayer1);
            this.Controls.Add(m_LabelPlayer2);
            this.Controls.Add(m_LabelBoardSize);
            this.Controls.Add(m_LabelRows);
            this.Controls.Add(m_LabelCols);
            this.Controls.Add(m_CheckBoxPlayer2);
            this.Controls.Add(m_TextBoxPlayer1);
            this.Controls.Add(m_TextBoxPlayer2);
            this.Controls.Add(m_NumericUpDownRows);
            this.Controls.Add(m_NumericUpDownCols);
            this.Controls.Add(m_ButtonStartGame);
        }

        private void initControls()
        {
            m_LabelPlayers.AutoSize = true;
            m_LabelPlayers.Text = "Players:";
            m_LabelPlayers.Location = new Point(10,10);
            m_LabelPlayers.Visible = true;

            m_LabelPlayer1.AutoSize = true;
            m_LabelPlayer1.Text = "Player 1:";
            m_LabelPlayer1.Location = new Point(m_LabelPlayers.Left + 10, m_LabelPlayers.Bottom + 10);
            m_LabelPlayer1.Visible = true;

            m_TextBoxPlayer1.Location = new Point(m_LabelPlayer1.Right + 50, m_LabelPlayer1.Location.Y - 2);
            m_TextBoxPlayer1.Visible = true;

            m_CheckBoxPlayer2.AutoSize = true;
            m_CheckBoxPlayer2.Location = new Point(m_LabelPlayer1.Left + 3, m_LabelPlayer1.Bottom + 10);
            m_CheckBoxPlayer2.Visible = true;
            m_CheckBoxPlayer2.CheckedChanged += m_CheckBoxPlayer2_CheckedChanged;

            m_LabelPlayer2.AutoSize = true;
            m_LabelPlayer2.Text = "Player 2:";
            m_LabelPlayer2.Location = new Point(m_CheckBoxPlayer2.Right + 3, m_CheckBoxPlayer2.Location.Y);
            m_LabelPlayer2.Visible = true;

            m_TextBoxPlayer2.Location = new Point(m_LabelPlayer1.Right + 50, m_LabelPlayer2.Location.Y - 2);
            m_TextBoxPlayer2.Enabled = false;
            m_TextBoxPlayer2.Text = "[Computer]";
            m_TextBoxPlayer2.Visible = true;

            m_LabelBoardSize.AutoSize = true;
            m_LabelBoardSize.Text = "Board Size:";
            m_LabelBoardSize.Location = new Point(m_LabelPlayers.Left, m_TextBoxPlayer2.Bottom + 30);
            m_LabelBoardSize.Visible = true;

            m_LabelRows.AutoSize = true;
            m_LabelRows.Text = "Rows:";
            m_LabelRows.Location = new Point(m_LabelPlayers.Left + 10, m_LabelBoardSize.Bottom + 10);
            m_LabelRows.Visible = true;

            m_NumericUpDownRows.AutoSize = false;
            m_NumericUpDownRows.Location = new Point(m_LabelRows.Right + 10, m_LabelRows.Bottom - 19);
            m_NumericUpDownRows.Visible = true;
            m_NumericUpDownRows.Minimum = Board.sr_MinBoardSize;
            m_NumericUpDownRows.Maximum = Board.sr_MaxBoardSize;
            m_NumericUpDownRows.Width = 40;
            m_NumericUpDownRows.Height = 50;
            m_NumericUpDownRows.ValueChanged += m_NumericUpDownRows_ValueChanged;

            m_LabelCols.AutoSize = true;
            m_LabelCols.Text = "Cols:";
            m_LabelCols.Location = new Point(m_NumericUpDownRows.Right + 33, m_LabelRows.Location.Y);
            m_LabelCols.Visible = true;

            m_NumericUpDownCols.AutoSize = false;
            m_NumericUpDownCols.Location = new Point(m_LabelCols.Right + 10, m_LabelCols.Bottom - 19);
            m_NumericUpDownCols.Visible = true;
            m_NumericUpDownCols.Minimum = Board.sr_MinBoardSize;
            m_NumericUpDownCols.Maximum = Board.sr_MaxBoardSize;
            m_NumericUpDownCols.Width = 40;
            m_NumericUpDownCols.Height = 50;
            m_NumericUpDownCols.ValueChanged += m_NumericUpDownCols_ValueChanged;

            m_ButtonStartGame.AutoSize = false;
            m_ButtonStartGame.Location = new Point(m_LabelPlayers.Left + 3, m_NumericUpDownRows.Bottom + 20);
            m_ButtonStartGame.Text = "Start!";
            m_ButtonStartGame.Visible = true;
            m_ButtonStartGame.Width = 205;
            m_ButtonStartGame.Height = 20;
            m_ButtonStartGame.Click += m_ButtonStartGame_Click;
        }

        private void m_NumericUpDownCols_ValueChanged(object sender, EventArgs e)
        {
            m_NumericUpDownRows.Value = m_NumericUpDownCols.Value;
        }

        private void m_NumericUpDownRows_ValueChanged(object sender, EventArgs e)
        {
            m_NumericUpDownCols.Value = m_NumericUpDownRows.Value;
        }

        private void m_ButtonStartGame_Click(object sender, EventArgs e)
        {
            if((Regex.IsMatch(m_TextBoxPlayer1.Text, @"[^0-9\p{L}]") || m_TextBoxPlayer1.Text.Length == 0)
                || (m_TextBoxPlayer2.Enabled && (Regex.IsMatch(m_TextBoxPlayer2.Text, @"[^0-9\p{L}]") || m_TextBoxPlayer2.Text.Length == 0)))
            {
                MessageBox.Show(string.Format(@"Must provide a valid nickname!
Only letters and numbers are allowed"), "Error", MessageBoxButtons.OK);
            }
            else if(m_TextBoxPlayer1.Text == Regex.Replace(m_TextBoxPlayer2.Text, @"[^0-9\p{L}]", ""))
            {
                MessageBox.Show(string.Format(@"Both player can't have the same nickname!"), "Error", MessageBoxButtons.OK);
            }
            else
            {
                initGame();
            }
        }

        private void initGame()
        {
            Player.ePlayerType secondPlayerType = m_CheckBoxPlayer2.Checked ? Player.ePlayerType.User : Player.ePlayerType.Computer;
            ushort boardSize = (ushort)m_NumericUpDownRows.Value;
            string firstPlayerName = m_TextBoxPlayer1.Text;
            string secondPlayerName = Regex.Replace(m_TextBoxPlayer2.Text, @"[^0-9\p{L}]", "");
            GameForm gameForm = new GameForm(secondPlayerType, boardSize, firstPlayerName, secondPlayerName);

            this.Close();
            gameForm.ShowDialog();
        }

        private void m_CheckBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            m_TextBoxPlayer2.Text = m_TextBoxPlayer2.Enabled ? "[Computer]" : "";
            m_TextBoxPlayer2.Enabled = !m_TextBoxPlayer2.Enabled;
        }
    }
}
