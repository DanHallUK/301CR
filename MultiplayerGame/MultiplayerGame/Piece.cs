using System;
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
        public Rectangle DrawRectangle;
        public bool CanGoBackward;

        public Piece(Point position, Color color)
        {
            Position = position;
            Color = color;
            UpdateDrawPosition();
        }

        public void Draw(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(Color);
            graphics.FillEllipse(brush, DrawRectangle);
        }

        private void UpdateDrawPosition()
        {
            DrawRectangle = new Rectangle(Position.X * ApplicationSettings.CellSize.Width, 
                                        Position.Y * ApplicationSettings.CellSize.Height, 
                                        ApplicationSettings.CellSize.Width, 
                                        ApplicationSettings.CellSize.Height);
        }
    }
}
