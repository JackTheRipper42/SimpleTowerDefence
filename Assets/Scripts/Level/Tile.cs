using System;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Tile
    {
        private readonly int _texturePositionX;
        private readonly int _texturePositionY;
        private readonly int _id;

        public Tile(int id, int x, int y)
        {
            _id = id;
            _texturePositionX = x;
            _texturePositionY = y;
        }

        public int TexturePositionX
        {
            get { return _texturePositionX; }
        }

        public int TexturePositionY
        {
            get { return _texturePositionY; }
        }

        public int Id
        {
            get { return _id; }
        }
    }
}
