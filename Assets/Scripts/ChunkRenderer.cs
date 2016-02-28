using System;
using Assets.Scripts.Level;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Material))]
    public class ChunkRenderer : MonoBehaviour
    {
        private MeshFilter _filter;
        private int _chunkSize;
        private ChunkData _chunkData;

        private ChunkData ChunkData
        {
            get { return _chunkData ?? (_chunkData = GetChunkDataFromParant()); }
        }

        public Position Position { get; set; }

        protected virtual void Start()
        {
            _filter = GetComponent<MeshFilter>();
            if (_filter == null)
            {
                throw new NotImplementedException();
                //throw Exceptions.ComponentNotFound<MeshFilter>();
            }
        }

        protected virtual void Update()
        {
            if (!ChunkData.RendererUpdated)
            {
                var meshData = new FilterMeshData();
                for (var x = 0; x < _chunkSize; x++)
                {
                    for (var z = 0; z < _chunkSize; z++)
                    {
                        meshData.Update(ChunkData, x, z);
                    }
                }
                UpdateMesh(meshData);
                ChunkData.RendererUpdated = true;
            }
        }

        private void UpdateMesh(FilterMeshData meshData)
        {
            if (_filter.sharedMesh == null)
            {
                _filter.sharedMesh = new Mesh();
            }

            _filter.sharedMesh.Clear();
            _filter.sharedMesh.vertices = meshData.Vertices;
            _filter.sharedMesh.triangles = meshData.Triangles;
            _filter.sharedMesh.uv = meshData.Uv;
            _filter.sharedMesh.RecalculateNormals();
        }

        private World GetWorldInParent()
        {
            var world = GetComponentInParent<World>();
            if (world == null)
            {
                throw new NotImplementedException();
                //throw Exceptions.ComponentNotFoundInParent<World>();
            }
            return world;
        }

        private ChunkData GetChunkDataFromParant()
        {
            var world = GetWorldInParent();
            var chunkData = world.GetChunkData(Position);
            _chunkSize = chunkData.Size;
            return chunkData;
        }
    }
}
