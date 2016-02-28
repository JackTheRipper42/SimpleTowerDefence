using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class TileSet
    {
        private readonly Tile[] _tiles;
        private readonly int _firstId;
        private readonly float _tileTextureWidth;
        private readonly float _tileTextureHeight;
        private readonly float _tileMeshWidth;
        private readonly float _tileMeshHeight;
        private readonly string _imageSource;

        public TileSet(
            int firstId, 
            int tileWidth, 
            int tileHeight, 
            int textureWidth, 
            int textureHeight, 
            float pixelPerUnit, 
            [NotNull] string imageSource, 
            [NotNull] IDictionary<int, Dictionary<string, string>> tiles)
        {
            if (imageSource == null)
            {
                throw new ArgumentNullException("imageSource");
            }
            if (tiles == null)
            {
                throw new ArgumentNullException("tiles");
            }

            var horizontalCount = Mathf.FloorToInt((float)textureWidth / tileWidth);
            var verticalCount = Mathf.FloorToInt((float)textureHeight / tileHeight);

            _firstId = firstId;
            _imageSource = imageSource;
            _tiles = new Tile[horizontalCount * verticalCount];
            _tileTextureWidth = (float) tileWidth/ textureWidth;
            _tileTextureHeight = (float) tileHeight/ textureHeight;
            _tileMeshWidth = tileWidth / pixelPerUnit;
            _tileMeshHeight = tileHeight / pixelPerUnit;

            var index = 0;
            for(var y = 0; y < verticalCount; ++y)
            {
                for(var x = 0; x < horizontalCount; ++x)
                {
                    string tileClass = string.Empty;

                    Dictionary<string, string> tileProperties;
                    if (tiles.TryGetValue(index, out tileProperties))
                    {
                        tileProperties.TryGetValue("Class", out tileClass);
                    }

                    Tile tile;
                    var id = index + _firstId;
                    switch (tileClass)
                    {
                        //case "Wall":
                        //    tile = new TileWall(id, x, y);
                        //    break;
                        //case "WallConstruction":
                        //    tile = new TileWallConstruction(id, x, y);
                        //    break;
                        //case "Path":
                        //    tile = new TilePath(id, x, y);
                        //    break;
                        //case "Bridge":
                        //    tile = new TileBridge(id, x, y);
                        //    break;
                        //case "BridgeConstruction":
                        //    tile = new TileBridgeConstruction(id, x, y);
                        //    break;
                        //case "Water":
                        //    tile = new TileWater(id, x, y);
                        //    break;
                        //case "Blocking":
                        //    tile = new Tile(id, x, y, true);
                        //    break;
                        //case "NonBlocking":
                        //    tile = new Tile(id, x, y, false);
                        //    break;
                        default:
                            tile = new Tile(id, x, y);
                            break;
                    }

                    _tiles[index] = tile;
                    index++;
                }
            }
        }

        public Tile GetTile(int id)
        {
            return _tiles[id - _firstId];
        }

        public float TileTextureWidth
        {
            get { return _tileTextureWidth; }
        }

        public float TileTextureHeight
        {
            get { return _tileTextureHeight; }
        }

        public float TileMeshWidth
        {
            get { return _tileMeshWidth; }
        }

        public float TileMeshHeight
        {
            get { return _tileMeshHeight; }
        }

        public string ImageSource
        {
            get { return _imageSource; }
        }
    }
}
