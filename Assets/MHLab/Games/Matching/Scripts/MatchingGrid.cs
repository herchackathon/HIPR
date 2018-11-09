using MHLab.Games.Matching.Generators;
using MHLab.UI.Tween;
using System;
using System.Collections.Generic;
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

        public void Compact(Action onCompletedAction)
        {
            for (int x = 0; x < (int)_size.x; x++)
            {
                for (int y = 0; y < (int)_size.y; y++)
                {
                    int nextY = GetNextBelowY(x, y);
                    if (nextY < y)
                    {
                        var matchingTile = _grid[x, y];

                        if (matchingTile == null) continue;

                        matchingTile.GridPosition.y = nextY;
                        _grid[x, nextY] = matchingTile;
                        _grid[x, y] = null;
                        matchingTile.transform.position = new Vector3(matchingTile.transform.position.x, StartingPosition.y + nextY, matchingTile.transform.position.z);
                    }
                }
            }

            for (int y = 0; y < (int)_size.y; y++)
            {
                for (int x = 0; x < (int)_size.x; x++)
                {
                    int leftX = GetNextLeftX(x, y);
                    if (leftX < x)
                    {
                        var matchingTile = _grid[x, y];

                        if (matchingTile == null) continue;

                        matchingTile.GridPosition.x = leftX;
                        _grid[leftX, y] = matchingTile;
                        _grid[x, y] = null;
                        matchingTile.transform.position = new Vector3(StartingPosition.x + leftX, matchingTile.transform.position.y, matchingTile.transform.position.z);
                    }
                }
            }

            onCompletedAction.Invoke();
        }

        public void PerformMove(List<MatchingTile> tiles, ParticleSystem onExplosionParticles)
        {
            foreach (var matchingTile in tiles)
            {
                PopTile(matchingTile);
            }

            var lastTile = tiles[tiles.Count - 1];
            onExplosionParticles.transform.position = new Vector3(lastTile.transform.position.x, lastTile.transform.position.y, lastTile.transform.position.z - 1f);
            onExplosionParticles.Play();

            PushTilesDown();
            /*for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    var currentTile = _grid[i, j];

                    if (currentTile == null)
                    {
                        PushTilesDown();
                        //PushTilesDown(i, j, PushTilesLeftCallback);
                        if (i * j == ((int)_size.x - 1) * ((int)_size.y - 1))
                        //if (i >= (int)_size.x - 1 && j >= (int)_size.y - 1)
                        {
                            PushTilesDown(i, j, PushTilesLeftCallback);
                        }
                        else
                        {
                            PushTilesDown(i, j);
                        }
                    }
                }
            }*/

            
        }
        
        public int GetNextBelowY(int x, int y)
        {
            int result = y;

            for (int i = y - 1; i >= 0; i--)
            {
                if (_grid[x, i] == null)
                {
                    result = i;
                }
            }

            return result;
        }

        public int GetNextLeftX(int x, int y)
        {
            int result = x;

            for (int i = x - 1; i >= 0; i--)
            {
                if (_grid[i, y] == null)
                {
                    result = i;
                }
            }

            return result;
        }

        private void PushTilesDown()
        {
            for (int i = 0; i < (int)_size.x; i++)
            {
                for (int j = 0; j < (int)_size.y; j++)
                {
                    var currentTile = _grid[i, j];

                    if (currentTile == null)
                    {
                        PushColumnDown(i, j);
                        j = (int)_size.y;
                    }
                }
            }
        }

        private void PushColumnDown(int x, int y)
        {
            var tilesToPushDown = new List<MatchingTile>();
            for (int i = y; i < (int)_size.y; i++)
            {
                var currentTile = _grid[x, i];
                if(currentTile != null)
                    tilesToPushDown.Add(currentTile);
            }

            int newY = y;
            foreach (var matchingTile in tilesToPushDown)
            {
                matchingTile.GridPosition.y = newY;
                _grid[x, newY] = matchingTile;
                matchingTile.transform.position = new Vector3(matchingTile.transform.position.x, StartingPosition.y + newY, matchingTile.transform.position.z);

                newY++;
            }
        }

        private void PushTilesLeftCallback(object parameters)
        {
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    var currentTile1 = _grid[i, j];

                    if (currentTile1 == null)
                    {
                        PushTilesLeft(i, j);
                    }
                }
            }
        }

        public void PopTile(MatchingTile tile)
        {
            _grid[(int)tile.GridPosition.x, (int)tile.GridPosition.y] = null;
            _tilesCounter--;
            GameObject.Destroy(tile.gameObject);
        }

        private void PushTilesDown(int x, int y, Action<object> callback = null)
        {
            var newY = -1;
            var tiles = new List<MatchingTile>();
            for (int i = 0; i < _size.y; i++)
            {
                var currentTile = _grid[x, i];

                if (currentTile == null && newY == -1)
                    newY = i;

                if (newY != -1)
                {
                    if(currentTile != null)
                        tiles.Add(currentTile);
                }

                /*if (currentTile != null)
                    tiles.Add(currentTile);

                if (currentTile == null && newY == -1)
                    newY = i;*/
            }

            var action = callback;
            if (callback == null) action = (obj) => { };

            if (newY == -1) newY = y;
            //var newY = y;
            int index = 1;
            foreach (var matchingTile in tiles)
            {
                matchingTile.GridPosition.y = newY;
                _grid[x, newY] = matchingTile;
                //matchingTile.transform.position = new Vector3(matchingTile.transform.position.x, StartingPosition.y + newY, matchingTile.transform.position.z);
                var parameters = iTween.Hash(
                    "position", new Vector3(matchingTile.transform.position.x, StartingPosition.y + newY, matchingTile.transform.position.z),
                    "easetype", iTween.EaseType.easeInBack,
                    "time", 0.3f
                );
                if (index == tiles.Count)
                {
                    parameters.Add("oncomplete", action);
                    parameters.Add("oncompleteparams", tiles);
                }
                iTween.MoveTo(matchingTile.gameObject, parameters);
                index++;
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
                /*iTween.MoveTo(matchingTile.gameObject, iTween.Hash(
                    "position", new Vector3(StartingPosition.x + newX, matchingTile.transform.position.y, matchingTile.transform.position.z),
                    "easetype", iTween.EaseType.easeInBack,
                    "time", 0.5f
                ));*/
                newX++;
            }

            for (int i = newX; i < _size.x; i++)
            {
                _grid[i, y] = null;
            }
        }

        public void ExchangeTilesGridPosition(MatchingTile tile1, MatchingTile tile2)
        {
            var gridPosition1 = new Vector2(tile1.GridPosition.x, tile1.GridPosition.y);
            tile1.GridPosition = new Vector2(tile2.GridPosition.x, tile2.GridPosition.y);
            tile2.GridPosition = gridPosition1;

            _grid[(int)tile2.GridPosition.x, (int)tile2.GridPosition.y] = tile2;
            _grid[(int)tile1.GridPosition.x, (int)tile1.GridPosition.y] = tile1;
        }

        public void OnExchangingCompleted(object parameters, Action callback)
        {
            var tiles = (MatchingTile[]) parameters;
            ExchangeTilesGridPosition(tiles[0], tiles[1]);
            callback.Invoke();
        }

        public void ExchangeTiles(MatchingTile tile1, MatchingTile tile2, Action<object> onCompleteAction)
        {
            var position1 = tile1.transform.position;
            var position2 = tile2.transform.position;

            ExchangeTilesGridPosition(tile1, tile2);

            var tiles = new MatchingTile[] {tile1, tile2};
            
            iTween.MoveTo(tile1.gameObject, iTween.Hash(
                "position", position2,
                "easetype", iTween.EaseType.easeInBack,
                "time", 0.5f
            ));
            iTween.MoveTo(tile2.gameObject, iTween.Hash(
                "position", position1,
                "easetype", iTween.EaseType.easeInBack,
                "time", 0.5f,
                "oncomplete", onCompleteAction,
                "oncompleteparams", tiles
            ));
            /*var position1 = new Vector3(tile2.transform.position.x, tile2.transform.position.y, tile2.transform.position.z);
            var gridPosition1 = new Vector2(tile2.GridPosition.x, tile2.GridPosition.y);

            tile2.GridPosition = tile1.GridPosition;
            tile2.transform.position = tile1.transform.position;
            tile1.GridPosition = gridPosition1;
            tile1.transform.position = position1;

            _grid[(int)tile2.GridPosition.x, (int)tile2.GridPosition.y] = tile2;
            _grid[(int)tile1.GridPosition.x, (int)tile1.GridPosition.y] = tile1;*/
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
