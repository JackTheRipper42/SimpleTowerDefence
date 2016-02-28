using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class FilterMeshData
    {
        private readonly List<Vector3> _vertices;
        private readonly List<int> _triangles;
        private readonly List<Vector2> _uv;

        public FilterMeshData()
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _uv = new List<Vector2>();
        }

        public Vector3[] Vertices
        {
            get { return _vertices.ToArray(); }
        }

        public int[] Triangles
        {
            get { return _triangles.ToArray(); }
        }

        public Vector2[] Uv
        {
            get { return _uv.ToArray(); }
        }

        public void Update(ChunkData chunkData, int x, int z)
        {
            var positon = new Position(x, z);
            var id = chunkData.GetId(positon);
            if (id == 0)
            {
                return;
            }

            var tileSet = chunkData.WorldData.TileSet;

            var tile = tileSet.GetTile(id);

            _vertices.Add(new Vector3(
                x*tileSet.TileMeshWidth,
                0f,
                (z + 1)*tileSet.TileMeshHeight));
            _vertices.Add(new Vector3(
                (x + 1)*tileSet.TileMeshWidth,
                0f,
                (z + 1)*tileSet.TileMeshHeight));
            _vertices.Add(new Vector3(
                (x + 1)*tileSet.TileMeshWidth,
                0f,
                z*tileSet.TileMeshHeight));
            _vertices.Add(new Vector3(
                x*tileSet.TileMeshWidth,
                0f,
                z*tileSet.TileMeshHeight));

            _triangles.Add(_vertices.Count - 4);
            _triangles.Add(_vertices.Count - 3);
            _triangles.Add(_vertices.Count - 2);
            _triangles.Add(_vertices.Count - 4);
            _triangles.Add(_vertices.Count - 2);
            _triangles.Add(_vertices.Count - 1);

            _uv.Add(new Vector2(
                tileSet.TileTextureWidth*tile.TexturePositionX,
                1f - tileSet.TileTextureHeight*tile.TexturePositionY));
            _uv.Add(new Vector2(
                tileSet.TileTextureWidth*(tile.TexturePositionX + 1),
                1f - tileSet.TileTextureHeight*tile.TexturePositionY));
            _uv.Add(new Vector2(
                tileSet.TileTextureWidth*(tile.TexturePositionX + 1),
                1f - tileSet.TileTextureHeight*(tile.TexturePositionY + 1)));
            _uv.Add(new Vector2(
                tileSet.TileTextureWidth*tile.TexturePositionX,
                1f - tileSet.TileTextureHeight*(tile.TexturePositionY + 1)));
        }
    }
}
