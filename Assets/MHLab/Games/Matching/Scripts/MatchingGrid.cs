using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MHLab.Games.Matching.Generators;
using UnityEngine;

namespace MHLab.Games.Matching
{
    public class MatchingGrid
    {
        private readonly MatchingTile[,] _grid;
        private int _tilesCounter;
        private Vector2 _size;
        private MatchingGame Owner;
        private Vector2 StartingPosition;

        public MatchingGrid(int x, int y, MatchingTile[] prefabs, GameObject border, GameObject angle, BorderBall borderBall, MatchingGame owner, ref Texture2D image)
        {
            _size = new Vector2(x, y);
            var generator = new MatchingGenerator();
            _grid = generator.Generate(x, y, prefabs, border, angle, borderBall, owner.transform, ref image);
            _tilesCounter = x * y;
            Owner = owner;
            StartingPosition = owner.transform.position;
            StartingPosition.x = StartingPosition.x - ((float)x / 2);
            StartingPosition.y = StartingPosition.y - ((float)y / 2);
        }

        public List<MatchingTile> GetValidTilesGroup(MatchingTile startingTile)
        {
            var tiles = new List<MatchingTile> {startingTile};

            GetValidNeighbours(startingTile, ref tiles);

            return tiles;
        }

        private void GetValidNeighbours(MatchingTile tile, ref List<MatchingTile> neighbours)
        {
            for (int i = -1; i < 2; i++)
            {
                int index = (int)tile.GridPosition.x + i;
                if (index < 0 || index >= _size.x) continue;
                var neighbour = _grid[index, (int)tile.GridPosition.y];

                if (neighbour == null) continue;

                if (neighbour.Type == tile.Type)
                {
                    if (neighbours.Contains(neighbour)) continue;
                    neighbours.Add(neighbour);
                    GetValidNeighbours(neighbour, ref neighbours);
                }
            }

            for (int i = -1; i < 2; i++)
            {
                int index = (int)tile.GridPosition.y + i;
                if (index < 0 || index >= _size.y) continue;
                var neighbour = _grid[(int)tile.GridPosition.x, index];

                if (neighbour == null) continue;

                if (neighbour.Type == tile.Type)
                {
                    if (neighbours.Contains(neighbour)) continue;
                    neighbours.Add(neighbour);
                    GetValidNeighbours(neighbour, ref neighbours);
                }
            }
        }

        public void PerformMove(List<MatchingTile> tiles)
        {
            foreach (var matchingTile in tiles)
            {
                PopTile(matchingTile);
            }

            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    var currentTile = _grid[i, j];

                    if (currentTile == null)
                    {
                        PushTilesDown(i, j);
                    }
                }
            }

            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    var currentTile = _grid[i, j];

                    if (currentTile == null)
                    {
                        PushTilesLeft(i, j);
                    }
                }
            }
        }

        private void PopTile(MatchingTile tile)
        {
            _grid[(int)tile.GridPosition.x, (int)tile.GridPosition.y] = null;
            _tilesCounter--;
            GameObject.Destroy(tile.gameObject);
        }

        private void PushTilesDown(int x, int y)
        {
            var tiles = new List<MatchingTile>();
            for (int i = y; i < _size.y; i++)
            {
                var currentTile = _grid[x, i];
                if(currentTile != null)
                    tiles.Add(currentTile);
            }

            var newY = y;
            foreach (var matchingTile in tiles)
            {
                matchingTile.GridPosition.y = newY;
                _grid[x, newY] = matchingTile;
                matchingTile.transform.position = new Vector3(matchingTile.transform.position.x, StartingPosition.y + newY, matchingTile.transform.position.z);
                newY++;
            }

            for (int i = newY; i < _size.y; i++)
            {
                _grid[x, i] = null;
            }
        }

        private void PushTilesLeft(int x, int y)
        {
            var tiles = new List<MatchingTile>();
            for (int i = x; i < _size.x; i++)
            {
                var currentTile = _grid[i, y];
                if (currentTile != null)
                    tiles.Add(currentTile);
            }

            var newX = x;
            foreach (var matchingTile in tiles)
            {
                matchingTile.GridPosition.x = newX;
                _grid[newX, y] = matchingTile;
                matchingTile.transform.position = new Vector3(StartingPosition.x + newX, matchingTile.transform.position.y, matchingTile.transform.position.z);
                newX++;
            }

            for (int i = newX; i < _size.x; i++)
            {
                _grid[i, y] = null;
            }
        }

        public bool IsCompleted()
        {
            if (_tilesCounter < Owner.MinimumAmountOfGroupedTiles) return true;

            foreach (var matchingTile in _grid)
            {
                if (matchingTile == null) continue;
                var tilesGroup = GetValidTilesGroup(matchingTile);
                if (tilesGroup.Count >= Owner.MinimumAmountOfGroupedTiles) return false;
            }

            return true;
        }
    }
}
