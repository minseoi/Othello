using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public struct GridVector
    {
        public int x, y;
    }
    
    public Camera cam;
    [Range(0, 1)] public float camMargin;

    public Tile tilePrefab;

    private int width, height;

    private float offset2Zero_X;
    private float offset2Zero_Y;

    private Stone[,] elements;
    private Transform elementsParent;
    private Transform tilesParent;

    private void Start()
    {
        if (transform.Find("tiles") == null)
            tilesParent = new GameObject("tiles").transform;
        tilesParent.SetParent(this.transform);

        if (transform.Find("elements") == null)
            elementsParent = new GameObject("elements").transform;
        elementsParent.SetParent(this.transform);
    }

    public void GenerateGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        elements = new Stone[width, height];

        //그리드의 세로크기에 맞춰 카메라 사이즈 변경.
        //아래 코드를 Update에 넣으면, 실시간으로 변경가능
        cam.orthographicSize = ((float)height / 2) * (camMargin + 1);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var ins = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                ins.name = $"Tile({x},{y})";
                ins.transform.SetParent(tilesParent);

                bool checker = (x % 2 == 1 && y % 2 == 0) || (x % 2 == 0 && y % 2 == 1);
                ins.SetColor(checker);
            }

        //그리드가 카메라 정중앙에 위치하도록 offset계산
        offset2Zero_X = (width - 1) / (float)2;
        offset2Zero_Y = (height - 1) / (float)2;
        transform.position = new Vector2(-offset2Zero_X, -offset2Zero_Y);
    }

    //Element 제어
    // public ref Stone AddElement(int x, int y, Stone stone)
    // {
    //     Stone new_stone = Instantiate(stone, Grid2WorldPosition(new Vector2(x, y)), Quaternion.identity);
    //     new_stone.name = $"Element({x}, {y})";
    //     new_stone.transform.SetParent(elementsParent);
    //     elements[x, y] = new_stone;

    //     return ref elements[x, y];
    // }
    public ref Stone AddElement(int x, int y, Stone stone)
    {
        Stone new_stone = Instantiate(stone);
        new_stone.transform.SetParent(elementsParent);
        new_stone.transform.localPosition = new Vector2(x,y);
        new_stone.name = $"Element({x}, {y})";
        elements[x, y] = new_stone;

        return ref elements[x, y];
    }
    public ref Stone GetElement(int x, int y)
    {
        return ref elements[x, y];
    }

    public void RemoveElement(int x, int y)
    {
        if (elements[x, y] != null)
        {
            Destroy(elements[x, y].gameObject);
            elements[x, y] = null;
        }
    }

    public void RemoveAllElement()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                RemoveElement(x, y);
    }

    //좌표 변환
    public Vector2 Grid2WorldPosition(Vector2Int gridPos)
    {
        Vector2 worldPos = new Vector2(gridPos.x - offset2Zero_X, gridPos.y - offset2Zero_Y);
        return worldPos;
    }
}
