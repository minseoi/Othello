using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthelloManager : MonoBehaviour
{
    public static OthelloManager instance;

    public enum Turn
    {
        P1_turn,
        P2_turn
    }

    public enum StoneOwner
    {
        None,
        P1,
        P2
    }

    public Turn currentTurn;
    public Stone stonePrefab;

    public GridManager gridManager;
    public int width, height;
    private StoneOwner[,] othelloBoard;

    public int P1Score;
    public int P2Score;

    public UpdateUI uI;

    private void Awake()
    {
        if (width % 2 == 1 || height % 2 == 1)
            throw new System.Exception("OthelloManager: width and height only can even number.");

        instance = this;
    }

    private void Start()
    {
        gridManager.GenerateGrid(width, height);
        othelloBoard = new StoneOwner[width, height];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        currentTurn = Turn.P1_turn;
        P1Score = 0; P2Score = 0;

        //2차원 배열 othelloBoard를 빈상태로 초기화
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                othelloBoard[x, y] = StoneOwner.None;

        //그리드 Stone요소들 제거
        gridManager.RemoveAllElement();

        //중앙에 기본 돌 4개 두기
        int halfPosX = width / 2 - 1;
        int halfPosY = height / 2 - 1;

        TryPutStone(new Vector2Int(halfPosX, halfPosY), forced: true);
        TryPutStone(new Vector2Int(halfPosX + 1, halfPosY), forced: true);
        TryPutStone(new Vector2Int(halfPosX + 1, halfPosY + 1), forced: true);
        TryPutStone(new Vector2Int(halfPosX, halfPosY + 1), forced: true);
    }

    //처음 중앙에 4개 배치할때는, 유효한 수 인지 검사하지 않기 위해 forced에 true값을 줌
    public void TryPutStone(Vector2Int gridPos, bool forced = false)
    {
        //빈칸에만 돌을 두는것이 가능
        if (othelloBoard[gridPos.x, gridPos.y] != StoneOwner.None) return;

        //돌을 두면 뒤집히는 돌들의 좌표를 리스트로 받아옴.
        List<Vector2Int> reversibleStones = GetReversibleStone(gridPos, (currentTurn == Turn.P1_turn)?StoneOwner.P1:StoneOwner.P2);

        //뒤집힌는 돌이 없으면 돌을 둘 수 없다.
        //파라미터 forced==true로 강제성을 띌 경우 무시함
        if(reversibleStones.Count == 0 && forced == false)
            return;
        
        //돌 추가
        Stone newStone = gridManager.AddElement(gridPos.x, gridPos.y, stonePrefab);
        switch (currentTurn)
        {
            case Turn.P1_turn:
                othelloBoard[gridPos.x, gridPos.y] = StoneOwner.P1;
                newStone.SetStone(StoneOwner.P1);
                currentTurn = Turn.P2_turn;
                P1Score++;
                break;

            case Turn.P2_turn:
                othelloBoard[gridPos.x, gridPos.y] = StoneOwner.P2;
                newStone.SetStone(StoneOwner.P2);
                currentTurn = Turn.P1_turn;
                P2Score++;
                break;
        }
        //뒤집을수 있는 돌을 뒤집는다.
        foreach(Vector2Int sPos in reversibleStones)
            Reverse(sPos);
        
        //점수 갱신
        uI.UpdateScore();
    }

    private void Reverse(Vector2Int gridPos)
    {
        if (othelloBoard[gridPos.x, gridPos.y] == StoneOwner.P1)
        {
            P1Score--;
            P2Score++;
            othelloBoard[gridPos.x, gridPos.y] = StoneOwner.P2;
        }
        else
        {
            P2Score--;
            P1Score++;
            othelloBoard[gridPos.x, gridPos.y] = StoneOwner.P1;
        }
        gridManager.GetElement(gridPos.x, gridPos.y).Reverse();
    }

    /*
    for문을 사용하여 위쪽방향부터 시계방향으로 탐색
    한쪽 방향을 탐색할때는 거리를 늘려가며 tempStones리스트에 Stone을 넣으면서 감
    그러다 같은 돌을 만나면 tempStones리스트 안에 있는 모든 Stone을 reversibleStones리스트에 넣음.
    맵 끝까지 갔는데 없으면 tempStones리스트 비우고, 다른방향 탐색.
    */
    private List<Vector2Int> GetReversibleStone(in Vector2Int baseGridPos, in StoneOwner baseOwner)
    {
        int[] dx = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1, };
        int[] dy = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 };
        List<Vector2Int> reversibleStones = new List<Vector2Int>();
        List<Vector2Int> tempStones = new List<Vector2Int>();
        //8방향 탐색
        for (int i = 0; i < 8; i++)
        {
            for (int distance = 1; true; distance++)
            {
                int nextX = baseGridPos.x + (dx[i] * distance);
                int nextY = baseGridPos.y + (dy[i] * distance);

                //맵 밖으로 나가면 탐색 종료
                if (!((0 <= nextX && nextX < width) && (0 <= nextY && nextY < height))) break;

                StoneOwner _stone = othelloBoard[nextX, nextY];

                if (_stone == StoneOwner.None)
                {
                    break;
                }
                else if (_stone == baseOwner)
                {
                    reversibleStones.AddRange(tempStones);
                    break;
                }
                else
                {
                    tempStones.Add(new Vector2Int(nextX, nextY));
                }
            }
            tempStones.Clear();
        }
        return reversibleStones;
    }

    //임시 초기화용 함수
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InitializeBoard();
        }
    }
}