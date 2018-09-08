using UnityEngine;
using System.Collections.Generic;
using HIPR.Encoding;

namespace MHLab.Games.Matching.Generators
{
    public class MatchingGenerator : IGenerator
    {
        public MatchingTile[,] Generate(int width, int height, MatchingTile[] prefabs, GameObject border, GameObject angle, BorderBall borderBall, Transform owner, ref Texture2D image)
        {
            var grid = new MatchingTile[width, height];

            var startingPosition = owner.transform.position;
            startingPosition.x = startingPosition.x - ((float)width / 2);
            startingPosition.y = startingPosition.y - ((float)height / 2);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var prefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
                    var tile = GameObject.Instantiate<MatchingTile>(prefab);
                    tile.name = "MatchingTile [" + i + "," + j + "]";
                    tile.transform.parent = owner;
                    tile.transform.position = new Vector3(startingPosition.x + i, startingPosition.y + j, startingPosition.z);
                    tile.GridPosition = new Vector2(i, j);
                    tile.Owner = owner.GetComponent<MatchingGame>().Grid;

                    grid[i, j] = tile;
                }
            }

            for (int i = 0; i < width; i++)
            {
                var b = GameObject.Instantiate(border);
                b.name = "Border [" + i + ", 0]";
                b.transform.parent = owner;
                b.transform.position = new Vector3(startingPosition.x + i, startingPosition.y - 0.75f, startingPosition.z);
            }

            for (int i = 0; i < width; i++)
            {
                var b = GameObject.Instantiate(border);
                b.name = "Border [" + i + ", " + (height - 1) + "]";
                b.transform.parent = owner;
                b.transform.position = new Vector3(startingPosition.x + i, startingPosition.y + (height - 1) + 0.75f, startingPosition.z);
            }

            for (int i = 0; i < height; i++)
            {
                var b = GameObject.Instantiate(border);
                b.name = "Border [0, " + i + "]";
                b.transform.parent = owner;
                b.transform.position = new Vector3(startingPosition.x - 0.75f, startingPosition.y + i, startingPosition.z);
                b.transform.eulerAngles = new Vector3(b.transform.eulerAngles.x, b.transform.eulerAngles.y, b.transform.eulerAngles.z + 90f);
            }

            for (int i = 0; i < height; i++)
            {
                var b = GameObject.Instantiate(border);
                b.name = "Border [" + (width - 1) + ", " + i + "]";
                b.transform.parent = owner;
                b.transform.position = new Vector3(startingPosition.x + (width - 1) + 0.75f, startingPosition.y + i, startingPosition.z);
                b.transform.eulerAngles = new Vector3(b.transform.eulerAngles.x, b.transform.eulerAngles.y, b.transform.eulerAngles.z + 90f);
            }

            var passingPoints = new List<Vector3>();

            var angle1 = GameObject.Instantiate(angle);
            angle1.name = "Angle [0, 0]";
            angle1.transform.parent = owner;
            angle1.transform.position = new Vector3(startingPosition.x - 0.75f, startingPosition.y - 0.75f, startingPosition.z);
            angle1.transform.eulerAngles = new Vector3(angle1.transform.eulerAngles.x, angle1.transform.eulerAngles.y, angle1.transform.eulerAngles.z);
            passingPoints.Add(angle1.transform.Find("AngleCenter").position);

            var angle2 = GameObject.Instantiate(angle);
            angle2.name = "Angle [0, " + (height - 1 ) + "]";
            angle2.transform.parent = owner;
            angle2.transform.position = new Vector3(startingPosition.x - 0.75f, startingPosition.y + (height - 1) + 0.75f, startingPosition.z);
            angle2.transform.eulerAngles = new Vector3(angle2.transform.eulerAngles.x, angle2.transform.eulerAngles.y, angle2.transform.eulerAngles.z + 90f);
            passingPoints.Add(angle2.transform.Find("AngleCenter").position);

            var angle3 = GameObject.Instantiate(angle);
            angle3.name = "Angle [" + (width - 1) + ", " + (height - 1) + "]";
            angle3.transform.parent = owner;
            angle3.transform.position = new Vector3(startingPosition.x + (width - 1) + 0.75f, startingPosition.y + (height - 1) + 0.75f, startingPosition.z);
            angle3.transform.eulerAngles = new Vector3(angle3.transform.eulerAngles.x, angle3.transform.eulerAngles.y, angle3.transform.eulerAngles.z + 180f);
            passingPoints.Add(angle3.transform.Find("AngleCenter").position);

            var angle4 = GameObject.Instantiate(angle);
            angle4.name = "Angle [" + (width - 1) + ", 0]";
            angle4.transform.parent = owner;
            angle4.transform.position = new Vector3(startingPosition.x + (width - 1) + 0.75f, startingPosition.y - 0.75f, startingPosition.z);
            angle4.transform.eulerAngles = new Vector3(angle4.transform.eulerAngles.x, angle4.transform.eulerAngles.y, angle4.transform.eulerAngles.z + 270f);
            passingPoints.Add(angle4.transform.Find("AngleCenter").position);

            var ball = GameObject.Instantiate(borderBall);
            ball.Points = passingPoints;
            ball.transform.parent = owner;
            ball.transform.position = angle1.transform.position;

            // Creates the quad
            var meshFilter = owner.gameObject.AddComponent<MeshFilter>();
            var mesh = new Mesh();

            var vertices = new Vector3[4];
            vertices[0] = new Vector3(startingPosition.x - 0.5f, startingPosition.y - 0.5f, startingPosition.z);
            vertices[1] = new Vector3(startingPosition.x + (width - 1) + 0.5f, startingPosition.y - 0.5f, startingPosition.z);
            vertices[2] = new Vector3(startingPosition.x - 0.5f, startingPosition.y + (height - 1) + 0.5f, startingPosition.z);
            vertices[3] = new Vector3(startingPosition.x + (width - 1) + 0.5f, startingPosition.y + (height - 1) + 0.5f, startingPosition.z);

            var triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
            triangles[3] = 1;
            triangles[4] = 2;
            triangles[5] = 3;

            var normals = new Vector3[4];
            normals[0] = -Vector3.forward;
            normals[1] = -Vector3.forward;
            normals[2] = -Vector3.forward;
            normals[3] = -Vector3.forward;

            var uvs = new Vector2[4];
            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(1, 0);
            uvs[2] = new Vector2(0, 1);
            uvs[3] = new Vector2(1, 1);

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uvs;

            meshFilter.mesh = mesh;

            InitializeTextures(ref image);
            var renderer = owner.GetComponent<MeshRenderer>();
            renderer.material.mainTexture = image;

            return grid;
        }

        private void InitializeTextures(ref Texture2D image)
        {
            image = Steganography.Encode(image, "1234");
        }
    }
}
