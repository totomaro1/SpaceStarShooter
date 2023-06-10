using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Totomaro.Board;

namespace Totomaro.Board
{
    public class BoardEnumerator
    {
        Totomaro.Board.Board m_Board;

        public BoardEnumerator(Totomaro.Board.Board board)
        {
            this.m_Board = board;
        }

        public bool IsCageTypeCell(int nRow, int nCol)
        {
            return false;
        }
    }
}