using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MultiplayerGame
{
    enum EColour
    {
        UNSET,
        WHITE,
        BLACK
    };

    public class GameEngine
    {
        private static GameEngine _gameEngine;
        private Graphics _graphics;
        private Bitmap _bitmap;
        private List<Piece> _playerZeroPieces;
        private List<Piece> _playerOnePieces;
        private Piece _selectedPiece;
        private EColour eColour;
        private bool bWhiteTurn;

        private GameEngine(Size sizeOfDrawing)
        {
            _bitmap = new Bitmap(sizeOfDrawing.Width, sizeOfDrawing.Height);
            _graphics = Graphics.FromImage(_bitmap);
            eColour = EColour.UNSET;
            bWhiteTurn = true;
            StartGame();
        }

        public void ResetGame()
        {
            eColour = EColour.UNSET;
            bWhiteTurn = true;
            StartGame();
        }

        public static GameEngine GetGameEngineInstance(Size sizeOfDrawing)
        {
            if (_gameEngine == null)
                return _gameEngine = new GameEngine(sizeOfDrawing);
            return _gameEngine;
        }

        public Bitmap Draw()
        {
            DrawBoard();
            DrawPieces();
            return _bitmap;
        }

        private void DrawBoard()
        {
            SolidBrush solidBrush;
            SolidBrush solidBrushZero = new SolidBrush(ApplicationSettings.CellColorZero);
            SolidBrush solidBrushOne = new SolidBrush(ApplicationSettings.CellColorOne);
            Pen pen;
            Rectangle rectangle;
            for (int i = 0; i < ApplicationSettings.BoardSize.Width; i++)
            {
                for (int j = 0; j < ApplicationSettings.BoardSize.Height; j++)
                {
                    solidBrush = (i + j) % 2 == 0 ? solidBrushZero : solidBrushOne;
                    rectangle = new Rectangle(i * ApplicationSettings.CellSize.Width, j * ApplicationSettings.CellSize.Height, ApplicationSettings.CellSize.Width, ApplicationSettings.CellSize.Height);
                    _graphics.FillRectangle(solidBrush, rectangle);
                }
            }

            pen = new Pen(ApplicationSettings.BorderColor);
            rectangle = new Rectangle(0, 0, ApplicationSettings.CellSize.Width * ApplicationSettings.BoardSize.Width, ApplicationSettings.CellSize.Height * ApplicationSettings.BoardSize.Height);

            //Border
            _graphics.DrawRectangle(pen, rectangle);
        }

        private void DrawPieces()
        {
            foreach (Piece p in _playerZeroPieces.Union(_playerOnePieces))
            {
                p.Draw(_graphics);
            }
        }

        private void StartGame()
        {
            _playerZeroPieces = new List<Piece>();
            _playerOnePieces = new List<Piece>();
            for (int i = 0; i < ApplicationSettings.BoardSize.Width; i++)
            {
                for (int j = 0; j < ApplicationSettings.RowsOfStartPieces; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        _playerZeroPieces.Add(new Piece(new Point(i, j), ApplicationSettings.PieceColorZero, false, true));
                    }
                }
            }

            for (int i = 0; i < ApplicationSettings.BoardSize.Width; i++)
            {
                for (int j = ApplicationSettings.BoardSize.Height - 1; j > ApplicationSettings.BoardSize.Height - ApplicationSettings.RowsOfStartPieces - 1; j--)
                {
                    if ((i + j) % 2 == 0)
                    {
                        _playerOnePieces.Add(new Piece(new Point(i, j), ApplicationSettings.PieceColorOne, true, false));
                    }
                }
            }
        }

        public bool Click(Point cellClicked, bool bOpponent = false)
        {
            Piece clickedPiece;
            clickedPiece = _playerZeroPieces.Union(_playerOnePieces).FirstOrDefault(a => a.PieceClicked(cellClicked));

            //Check if it is the correct turn to move this piece
            if (ApplicationSettings.TurnBased)
            {
                if (!(((bWhiteTurn && eColour == EColour.WHITE) || (bOpponent && bWhiteTurn && eColour == EColour.BLACK)) || ((!bWhiteTurn && eColour == EColour.BLACK) || (bOpponent && !bWhiteTurn && eColour == EColour.WHITE))))
                {
                    return false;
                }
            }

            if (clickedPiece != null)
            {
                if (!bOpponent)
                {
                    if (clickedPiece.Color == ApplicationSettings.PieceColorZero) // Black Piece
                    {
                        if (eColour == EColour.WHITE)
                        {
                            return false;
                        }
                    }
                    if (clickedPiece.Color == ApplicationSettings.PieceColorOne) // White Piece
                    {
                        if (eColour == EColour.BLACK)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (clickedPiece.Color == ApplicationSettings.PieceColorZero) // Black Piece
                    {
                        if (eColour == EColour.BLACK)
                        {
                            return false;
                        }
                    }
                    if (clickedPiece.Color == ApplicationSettings.PieceColorOne) // White Piece
                    {
                        if (eColour == EColour.WHITE)
                        {
                            return false;
                        }
                    }
                }
            }

            //Check if a piece is already selected
            if (_selectedPiece == null)
            {
                //Check if a piece was clicked this time
                if (clickedPiece != null)
                {

                    clickedPiece.IsSelected = true;
                    _selectedPiece = clickedPiece;
                }
                else//An empty area was clicked this time
                {

                }
            }
            else//A piece is already selected
            {
                    //Check if a piece was clicked this time
                    if (clickedPiece != null)
                {
                    //Check if the ckicked piece is the one that is also selected
                    if (clickedPiece == _selectedPiece)
                    {
                        clickedPiece.IsSelected = false;
                        _selectedPiece = null;
                    }
                    else//Don't do anything
                    {

                    }
                }
                else//Empty area clicked or outside the board
                {
                    Piece middlePiece;
                    if (IsLegalMove(_selectedPiece, _selectedPiece.Position, cellClicked))
                    {
                        _selectedPiece.Move(cellClicked);
                        _selectedPiece.IsSelected = false;
                        _selectedPiece = null;
                        bWhiteTurn = !bWhiteTurn;
                    }
                    else if (IsLegalJump(_selectedPiece, _selectedPiece.Position, cellClicked, out middlePiece))
                    {
                        if (_playerZeroPieces.Contains(_selectedPiece) && _playerOnePieces.Contains(middlePiece))
                        {
                            _playerOnePieces.Remove(middlePiece);
                        }
                        else if (_playerOnePieces.Contains(_selectedPiece) && _playerZeroPieces.Contains(middlePiece))
                        {
                            _playerZeroPieces.Remove(middlePiece);
                        }
                        _selectedPiece.Move(cellClicked);
                        _selectedPiece.IsSelected = false;
                        _selectedPiece = null;
                        bWhiteTurn = !bWhiteTurn;
                    }
                }
            }
            return true;
        }

        private bool IsLegalMove(Piece piece, Point from, Point to)
        {
            if (to.X >= ApplicationSettings.BoardSize.Width || to.Y >= ApplicationSettings.BoardSize.Height) return false;
            if (Math.Abs(from.Y - to.Y) != 1) return false;
            if (Math.Abs(from.X - to.X) != 1) return false;
            if ((piece.IsBottomPiece || piece.CanGoBackward) && from.Y > to.Y) return true;
            if ((piece.IsTopPiece || piece.CanGoBackward) && from.Y < to.Y) return true;
            return false;
        }

        private bool IsLegalJump(Piece piece, Point from, Point to, out Piece middlePiece)
        {
            middlePiece = null;
            if (to.X >= ApplicationSettings.BoardSize.Width || to.Y >= ApplicationSettings.BoardSize.Height) return false;
            if (Math.Abs(from.Y - to.Y) != 2) return false;
            if (Math.Abs(from.X - to.X) != 2) return false;
            Point middleCell = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            middlePiece = _playerZeroPieces.Union(_playerOnePieces).FirstOrDefault(a => a.PieceClicked(middleCell));
            if (middlePiece == null) return false;
            if ((piece.IsBottomPiece || piece.CanGoBackward) && from.Y > to.Y) return true;
            if ((piece.IsTopPiece || piece.CanGoBackward) && from.Y < to.Y) return true;
            return false;
        }

        public void SetColour(bool bIsWhite)
        {
            if (bIsWhite)
            {
                eColour = EColour.WHITE;
            }
            else
            {
                eColour = EColour.BLACK;
            }
        }
    }
}
