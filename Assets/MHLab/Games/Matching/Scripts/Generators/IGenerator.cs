using UnityEngine;

namespace MHLab.Games.Matching.Generators
{
    public interface IGenerator
    {
        MatchingTile[,] Generate(int width, int height, MatchingTile[] prefabs, GameObject border, GameObject angle, BorderBall borderBall, Transform owner, ref Texture2D image);
    }
}
