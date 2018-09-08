using HIPR.Encoding;
using MHLab.SlidingTilePuzzle;
using MHLab.SlidingTilePuzzle.Data;
using MHLab.UI;
using MHLab.Web.Storage;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MHLab.Games.Matching
{
    public class MatchingGame : MonoBehaviour
    {
        public Vector2 Size;
        public MatchingTile[] TilePrefabs;
        public GameObject Border;
        public GameObject Angle;
        public BorderBall BorderBall;
        public Texture2D Image;

        public RectTransform OnCompletedPopup;
        public Text OnCompletedPopupText;

        public int MinimumAmountOfGroupedTiles = 2;
        public int SingleTileScore = 100;
        public int SingleTileScoreIncrement = 10;
        
        public EnableForLimitedTime LetsGoPopup;
        public AudioClip[] OnMoveSounds;
        public AudioClip OnVictorySound;

        public Text ScorePopupText;
        public EnableForLimitedTime ScorePopup;

        public ParticleSystem OnMoveParticles;
        public ParticleSystem OnScoreParticles;
        public ParticleSystem OnExplosionParticles;

        public MatchingGrid Grid;

        private AudioSource _audioSource;

        private MatchingTile _currentSelectedTile;
        private MatchingTile _targetSelectedTile;

        private bool _canMove = true;

        protected void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            Grid = new MatchingGrid((int)Size.x, (int)Size.y, TilePrefabs, Border, Angle, BorderBall, this, ref Image);
            LetsGoPopup.EnableFor(1);
            GameTimerUpdater.StartTimer();
        }

        protected void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform != null)
                    {
                        var tile = hit.transform.gameObject.GetComponent<MatchingTile>();
                        if (tile != null)
                        {
                            ManageMove2(tile);
                        }
                    }
                }
            }
        }

        private void ManageMove2(MatchingTile tile)
        {
            if (_canMove)
            {
                if (_currentSelectedTile == null)
                {
                    _currentSelectedTile = tile;
                    return;
                }

                _targetSelectedTile = tile;

                if (_currentSelectedTile == _targetSelectedTile)
                {
                    ClearMove();
                    return;
                }

                if (_targetSelectedTile.IsNearTo(_currentSelectedTile))
                {
                    _canMove = false;
                    // Here the move is valid and it can start.
                    Grid.ExchangeTiles(_currentSelectedTile, _targetSelectedTile, OnExchangingCompleted);
                }
                else
                {
                    ClearMove();
                    return;
                }
            }
        }

        private void ManageMove(MatchingTile tile)
        {
            if (_canMove)
            {
                if (_currentSelectedTile == null)
                {
                    _currentSelectedTile = tile;
                    return;
                }

                _targetSelectedTile = tile;

                if (_currentSelectedTile == _targetSelectedTile)
                {
                    ClearMove();
                    return;
                }

                if (_targetSelectedTile.IsNearTo(_currentSelectedTile))
                {
                    _canMove = false;
                    // Here the move is valid and it can start.
                    Grid.ExchangeTiles(_currentSelectedTile, _targetSelectedTile, OnExchangingCompleted);
                }
                else
                {
                    ClearMove();
                    return;
                }
            }
        }

        public void OnExchangingCompleted(object parameters)
        {
            var tiles = (MatchingTile[])parameters;
            var tilesGroup1 = Grid.GetValidTilesGroup(tiles[0]);
            var tilesGroup2 = Grid.GetValidTilesGroup(tiles[1]);

            if (tilesGroup1.Count < MinimumAmountOfGroupedTiles && tilesGroup2.Count < MinimumAmountOfGroupedTiles)
            {
                Grid.ExchangeTiles(tiles[0], tiles[1], (obj) =>
                {
                    ClearMove();
                    _canMove = true;
                });
                return;
            }

            var tilesToPop = new List<MatchingTile>();
            if(tilesGroup1.Count >= MinimumAmountOfGroupedTiles)
                tilesToPop.AddRange(tilesGroup1);
            if (tilesGroup2.Count >= MinimumAmountOfGroupedTiles)
                tilesToPop.AddRange(tilesGroup2);

            _audioSource.PlayOneShot(OnMoveSounds[UnityEngine.Random.Range(0, OnMoveSounds.Length)]);
            foreach (var matchingTile in tilesToPop)
            {
                Grid.PopTile(matchingTile);
            }
            OnMoveParticles.transform.position = new Vector3(tiles[0].transform.position.x, tiles[0].transform.position.y, tiles[0].transform.position.z - 1f);
            OnMoveParticles.Play();
            OnExplosionParticles.transform.position = new Vector3(tilesToPop[tilesToPop.Count - 1].transform.position.x, tilesToPop[tilesToPop.Count - 1].transform.position.y, tilesToPop[tilesToPop.Count - 1].transform.position.z - 1f);
            OnExplosionParticles.Play();
            var score = CalculateScore(tilesToPop.Count);
            ScorePopupText.text = "+" + score;
            ScorePopup.transform.position = Camera.main.WorldToScreenPoint(tiles[0].transform.position);
            ScorePopup.EnableFor(0.5f);
            OnScoreParticles.transform.position = new Vector3(tiles[0].transform.position.x, tiles[0].transform.position.y, tiles[0].transform.position.z - 1f);
            OnScoreParticles.Play();
            ScoreCounter.AddScore(score);

            Grid.Compact(() =>
            {
                ClearMove();
                if (Grid.IsCompleted())
                {
                    OnVictory();
                }
                _canMove = true;
            });

            

            /*_grid.OnExchangingCompleted(parameters, () =>
            {
                var tiles = (MatchingTile[])parameters;
                StartCheckingForMove(tiles[0], tiles[1]);
                //StartCheckingForMove(tiles[1]);
                ClearMove();

                if (_grid.IsCompleted())
                {
                    OnVictory();
                }

                _canMove = true;
            });*/
        }

        private void ClearMove()
        {
            _currentSelectedTile = null;
            _targetSelectedTile = null;
        }

        private void StartCheckingForMove(MatchingTile tile, MatchingTile tile2)
        {
            var tiles = Grid.GetValidTilesGroup(tile);
            tiles.AddRange(Grid.GetValidTilesGroup(tile2));
            if (tiles.Count < MinimumAmountOfGroupedTiles) return;
            
            _audioSource.PlayOneShot(OnMoveSounds[UnityEngine.Random.Range(0, OnMoveSounds.Length)]);
            Grid.PerformMove(tiles, OnExplosionParticles);

            OnMoveParticles.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - 1f);
            OnMoveParticles.Play();

            var score = CalculateScore(tiles.Count);
            ScorePopupText.text = "+" + score;
            ScorePopup.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position);
            ScorePopup.EnableFor(0.5f);
            OnScoreParticles.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - 1f);
            OnScoreParticles.Play();
            ScoreCounter.AddScore(score);
        }

        private void OnVictory()
        {
            GameTimerUpdater.StopTimer();
            _audioSource.PlayOneShot(OnVictorySound);

            var decryptedText = Steganography.Decode(Image);

            var amount = LocalStorage.GetInt(StorageKeys.DecryptedAmountKey).Value + 1;

            OnCompletedPopupText.text = "You won 1 Herc token and decrypted\nHerciD: " + amount.ToString("000-000-000");
            OnCompletedPopup.gameObject.SetActive(true);

            LocalStorage.Store(StorageKeys.DecryptedAmountKey, amount);
        }

        private int CalculateScore(int tilesCount)
        {
            if (tilesCount == MinimumAmountOfGroupedTiles)
                return (tilesCount * SingleTileScore);

            int score = 0;
            for (int i = 0; i < tilesCount; i++)
            {
                var singleScore = SingleTileScore + (SingleTileScoreIncrement * i);
                score += singleScore;
            }

            return score;
        }
    }
}