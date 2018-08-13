using System.Collections;
using System.Collections.Generic;
using HIPR.Encoding;
using MHLab.Games.Matching.Generators;
using MHLab.SlidingTilePuzzle;
using MHLab.SlidingTilePuzzle.Data;
using MHLab.UI;
using MHLab.Web.Storage;
using UnityEngine;
using UnityEngine.UI;

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

        public int MinimumAmountOfGroupedTiles = 2;
        public int SingleTileScore = 100;
        public int SingleTileScoreIncrement = 10;
        

        public EnableForLimitedTime LetsGoPopup;
        public AudioClip[] OnMoveSounds;
        public AudioClip OnVictorySound;

        public Text ScorePopupText;
        public EnableForLimitedTime ScorePopup;

        public ParticleSystem OnMoveParticles;

        private AudioSource _audioSource;
        private MatchingGrid _grid;

        protected void Awake()
        {
            LocalStorage.Store(StorageKeys.ColorBlindnessMode, 1);
            _audioSource = GetComponent<AudioSource>();
            _grid = new MatchingGrid((int)Size.x, (int)Size.y, TilePrefabs, Border, Angle, BorderBall, this, ref Image);
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
                            StartCheckingForMove(tile);

                            if (_grid.IsCompleted())
                            {
                                OnVictory();
                            }
                        }
                    }
                }
            }
            
        }

        private void StartCheckingForMove(MatchingTile tile)
        {
            var tiles = _grid.GetValidTilesGroup(tile);
            if (tiles.Count < MinimumAmountOfGroupedTiles) return;
            
            _audioSource.PlayOneShot(OnMoveSounds[UnityEngine.Random.Range(0, OnMoveSounds.Length)]);
            _grid.PerformMove(tiles);

            OnMoveParticles.transform.position = tile.transform.position;
            OnMoveParticles.Play();

            var score = CalculateScore(tiles.Count);
            ScorePopupText.text = "+" + score;
            ScorePopup.transform.position = Camera.main.WorldToScreenPoint(tile.transform.position);
            ScorePopup.EnableFor(0.5f);
            ScoreCounter.AddScore(score);
        }

        private void OnVictory()
        {
            GameTimerUpdater.StopTimer();
            _audioSource.PlayOneShot(OnVictorySound);

            var decryptedText = Steganography.Decode(Image);
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