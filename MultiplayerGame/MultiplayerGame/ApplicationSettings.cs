using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MultiplayerGame
{
    public static class ApplicationSettings
    {
        public static Size CellSize = new Size(50, 50);
        public static Size PieceSize = new Size(30, 30);
        public static Size BoardSize = new Size(8, 8);

        public static Color CellColorZero = Color.FromArgb(103, 53, 20);
        public static Color CellColorOne = Color.FromArgb(201, 160, 70);
        public static Color BorderColor = Color.FromArgb(0, 0, 0);

        public static Color PieceColorZero = Color.FromArgb(31, 31, 31);
        public static Color PieceColorOne = Color.FromArgb(206, 178, 94);

        public static short RowsOfStartPieces = 3;
        public static short WidthOfSelectedBorder = 2;
    }
}
