using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex05.TicTacToeUI
{
    public class TicTacToe
    {
        private GameSettingsForm m_GameSettingsForm;

        public TicTacToe()
        {
            m_GameSettingsForm = new GameSettingsForm();
        }

        public void RunGame()
        {
            m_GameSettingsForm.ShowDialog();
        }
    }
}
