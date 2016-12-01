using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MultiplayerGame
{
    public class GameEngine
    {
        private static GameEngine _gameEngine;
        private Graphics _graphics;
        private Bitmap _bitmap;

        private GameEngine(Size sizeOfDrawing)
        {
            Bitmap bitmap = new Bitmap(sizeOfDrawing.Width, sizeOfDrawing.Height);
            _graphics = Graphics.FromImage(bitmap);
        }

        public static GameEngine GetGameEngineInstance(Size sizeOfDrawing)
        {
            if (_gameEngine == null)
                return _gameEngine = new GameEngine(sizeOfDrawing);
            return _gameEngine;
        }

        public Bitmap Draw()
        {
            
            return _bitmap;
        }

        private void DrawBoard()
        {
            for(int i = 0; i < ApplicationSettings.BoardSize.Width; i ++)
            {
                for (int j = 0; j < ApplicationSettings.BoardSize.Height; j++)
                {
                    SolidBrush solidBrush = (i + j) % 2 == 0 ? new SolidBrush(Color.FromArgb(0, 0, 0)) : new SolidBrush(Color.FromArgb(255, 255, 255));
                    Rectangle rectangle = new Rectangle(i * ApplicationSettings.CellSize.Width, j * ApplicationSettings.CellSize.Height, ApplicationSettings.CellSize.Width, ApplicationSettings.CellSize.Height);
                    _graphics.FillRectangle(solidBrush, rectangle);
                }
            }
        }
    }
}
