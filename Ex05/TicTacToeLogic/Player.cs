using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex05.TicTacToeLogic
{
    public struct Player
    {
        private readonly Board.ePlayerNumber? r_PlayerNum;
        private ePlayerType m_Type;

        public enum ePlayerType
        {
            User,
            Computer
        }

        public Player(Board.ePlayerNumber i_PlayerNum, ePlayerType i_Type)
        {
            r_PlayerNum = i_PlayerNum;
            m_Type = i_Type;
        }

        public Board.ePlayerNumber PlayerNumber
        {
            get
            {
                if (r_PlayerNum.HasValue)
                {
                    return r_PlayerNum.Value;
                }
                else
                {
                    throw new InvalidOperationException("Symbol has not been initialized.");
                }
            }
        }

        public ePlayerType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
    }
}
