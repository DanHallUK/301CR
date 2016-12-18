using System;
using System.Collections.Generic;

namespace MultiplayerServer
{
    public class GameLogic
    {
        class Piece
        {
            public int iXPos;
            public int iYPos;
            public bool bIsWhite;
            public bool bIsKing;

            public Piece(int i_iXPos, int i_iYPos, bool i_bIsWhite)
            {
                iXPos = i_iXPos;
                iYPos = i_iYPos;
                bIsWhite = i_bIsWhite;
                bIsKing = false;
            }
        }

        private const int iBoardSize = 8;
        private const int iRowsOfPieces = 3;
        private Piece tSelectedPiece;
        private bool bWhiteTurn;
        private bool bTurnBased = false;

        private List<Piece> tPieces;

        public void StartGame()
        {
            bWhiteTurn = true;
            tPieces = new List<Piece>();
            for (int i = 0; i < iBoardSize; i++)
            {
                for (int j = 0; j < iBoardSize; j++)
                {
                    bool bBlackSquare = (i + j) % 2 == 0;
                    if (bBlackSquare && j >= iBoardSize - iRowsOfPieces)
                    {
                        tPieces.Add(new Piece(i, j, true));
                    }
                    if (bBlackSquare && j < iRowsOfPieces)
                    {
                        tPieces.Add(new Piece(i, j, false));
                    }
                }
            }
        }

        public bool ValidateMove(int i_xPos, int i_yPos)
        {
            Piece tClickedPiece = null;
            foreach (Piece p in tPieces)
            {
                if (p.iXPos == i_xPos && p.iYPos == i_yPos)
                {
                    tClickedPiece = p;
                }
            }

            //Check if a piece is already selected
            if (tSelectedPiece == null)
            {
                //Check if a piece was clicked this time
                if (tClickedPiece != null)
                {
                    if (!bTurnBased || tClickedPiece.bIsWhite && bWhiteTurn || !tClickedPiece.bIsWhite && !bWhiteTurn)
                    {
                        tSelectedPiece = tClickedPiece;
                    }
                    else
                    {
                        return false;
                    }
                }
                else//An empty area was clicked this time
                {

                }
            }
            else//A piece is already selected
            {
                //Check if a piece was clicked this time
                if (tClickedPiece != null)
                {
                    //Check if the clicked piece is the one that is also selected
                    if (tClickedPiece == tSelectedPiece)
                    {
                        tSelectedPiece = null;
                    }
                    else//Don't do anything
                    {

                    }
                }
                else//Empty area clicked or outside the board
                {
                    Piece tMiddlePiece;
                    if (IsLegalMove(tSelectedPiece, i_xPos, i_yPos))
                    {
                        tSelectedPiece.iXPos = i_xPos;
                        tSelectedPiece.iYPos = i_yPos;
                        tSelectedPiece = null;
                        bWhiteTurn = !bWhiteTurn;
                    }
                    else if (IsLegalJump(tSelectedPiece, i_xPos, i_yPos, out tMiddlePiece))
                    {
                        tPieces.Remove(tMiddlePiece);
                        tSelectedPiece.iXPos = i_xPos;
                        tSelectedPiece.iYPos = i_yPos;
                        tSelectedPiece = null;
                        bWhiteTurn = !bWhiteTurn;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsLegalMove(Piece piece, int i_xPos, int i_yPos)
        {
            if (i_xPos >= iBoardSize || i_yPos >= iBoardSize) return false;
            if (Math.Abs(piece.iXPos - i_xPos) != 1) return false;
            if (Math.Abs(piece.iYPos - i_yPos) != 1) return false;
            if ((piece.bIsWhite || piece.bIsKing) && piece.iYPos > i_yPos) return true;
            if ((!piece.bIsWhite || piece.bIsKing) && piece.iYPos < i_yPos) return true;
            return false;
        }

        private bool IsLegalJump(Piece piece, int i_xPos, int i_yPos, out Piece middlePiece)
        {
            middlePiece = null;
            if (i_xPos >= iBoardSize || i_yPos >= iBoardSize) return false;
            if (Math.Abs(piece.iXPos - i_xPos) != 2) return false;
            if (Math.Abs(piece.iYPos - i_yPos) != 2) return false;
            int iMiddleXPos = piece.iXPos + (i_xPos - piece.iXPos) / 2;
            int iMiddleYPos = piece.iYPos + (i_yPos - piece.iYPos) / 2;
            foreach (Piece p in tPieces)
            {
                if (p.iXPos == iMiddleXPos && p.iYPos == iMiddleYPos)
                {
                    middlePiece = p;
                }
            }
            if (middlePiece == null) return false;
            if ((piece.bIsWhite || piece.bIsKing) && piece.iYPos > i_yPos) return true;
            if ((!piece.bIsWhite || piece.bIsKing) && piece.iYPos < i_yPos) return true;
            return false;
        }
    }

}
