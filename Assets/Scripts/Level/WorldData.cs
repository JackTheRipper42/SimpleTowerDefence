using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class WorldData
    {
        private readonly int _width;
        private readonly int _height;
        private readonly float _tileMeshWidth;
        private readonly float _tileMeshHeight;
        private readonly TileSet _tileSet;
        private readonly int _chunkSize;
        private readonly Dictionary<Position, ChunkData> _chunks;

        public WorldData(
            int width,
            int height,
            int chunkSize,
            int tileWidth,
            int tileHeight,
            float pixelPerUnit,
            [NotNull] TileSet tileSet)
        {
            if (tileSet == null)
            {
                throw new ArgumentNullException("tileSet");
            }

            _chunkSize = chunkSize;
            _width = width;
            _height = height;
            _tileMeshWidth = tileWidth/pixelPerUnit;
            _tileMeshHeight = tileHeight/pixelPerUnit;
            _tileSet = tileSet;
            _chunks = new Dictionary<Position, ChunkData>();
        }

        public IEnumerable<ChunkData> Chunks
        {
            get { return _chunks.Values; }
        }

        public ChunkData GetChunkData(Position position)
        {
            return _chunks[position];
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public float TileMeshWidth
        {
            get { return _tileMeshWidth; }
        }

        public float TileMeshHeight
        {
            get { return _tileMeshHeight; }
        }

        public TileSet TileSet
        {
            get { return _tileSet; }
        }

        public int GetId(Position position)
        {
            var chunk = GetChunk(position);
            return chunk.GetId(position - chunk.Position);
        }

        public void SetId(Position position, int id)
        {
            var chunk = GetChunk(position);
            chunk.SetId(position - chunk.Position, id);
        }

        private Position GetChunkPosition(int x, int z)
        {
            var chunkX = Mathf.FloorToInt(x / (float)_chunkSize) * _chunkSize;
            var chunkZ = Mathf.FloorToInt(z / (float)_chunkSize) * _chunkSize;

            return new Position(chunkX, chunkZ);
        }

        private ChunkData GetChunk(Position position)
        {
            var chunkPosition = GetChunkPosition(position.X, position.Z);
            ChunkData chunk;
            if (_chunks.TryGetValue(chunkPosition, out chunk))
            {
                return chunk;
            }
            chunk = new ChunkData(_chunkSize, chunkPosition, this);
            _chunks.Add(chunkPosition, chunk);
            return chunk;
        }
    }
}
