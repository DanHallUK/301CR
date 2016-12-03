﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MultiplayerGame
{
    public class Piece
    {
        public Point Position;
        public Color Color;
        public Rectangle BoaundingBox;
        public Rectangle DrawRectangle;
        public bool CanGoBackward;
        public bool IsSelected;
        public bool IsBottomPiece;
        public bool IsTopPiece;

        public Piece(Point position, Color color, bool isBottomPiece, bool isTopPiece)
        {
            Position = position;
            Color = color;
            IsBottomPiece = isBottomPiece;
            IsTopPiece = isTopPiece;
            UpdateDrawPosition();
        }

        public void Draw(Graphics graphics)
        {
            Pen pen;
            SolidBrush brush = new SolidBrush(Color);
            graphics.FillEllipse(brush, DrawRectangle);
            if (IsSelected)
            {
                pen = new Pen(Color.FromArgb(0, 255, 0));
                pen.Width = ApplicationSettings.WidthOfSelectedBorder;
                graphics.DrawEllipse(pen, DrawRectangle);
            }
            if (CanGoBackward)
            {
                pen = new Pen(Color.FromArgb(255, 0, 0));
                pen.Width = ApplicationSettings.WidthOfSelectedBorder;
                graphics.DrawEllipse(pen, new Rectangle(DrawRectangle.X + 4, DrawRectangle.Y + 4, DrawRectangle.Width - 8, DrawRectangle.Height - 8));
            }
        }

        private void UpdateDrawPosition()
        {
            DrawRectangle = new Rectangle(Position.X * ApplicationSettings.CellSize.Width + (ApplicationSettings.CellSize.Width - ApplicationSettings.PieceSize.Width) / 2,
                                        Position.Y * ApplicationSettings.CellSize.Height + (ApplicationSettings.CellSize.Height - ApplicationSettings.PieceSize.Height) / 2,
                                        ApplicationSettings.PieceSize.Width,
                                        ApplicationSettings.PieceSize.Height);
            BoaundingBox = new Rectangle(Position.X * ApplicationSettings.CellSize.Width,
                                        Position.Y * ApplicationSettings.CellSize.Height,
                                        ApplicationSettings.CellSize.Width,
                                        ApplicationSettings.CellSize.Height);
        }

        public bool PieceClicked(Point point)
        {
            return Position.X == point.X && Position.Y == point.Y;
        }

        public void Move(Point point)
        {
            Position = point;
            if (Position.Y == 0 || Position.Y == ApplicationSettings.BoardSize.Height - 1)
            {
                CanGoBackward = true;
            }
            UpdateDrawPosition();
        }
    }
}
