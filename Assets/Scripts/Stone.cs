using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] private Color p1StoneColor;
    [SerializeField] private Color p2StoneColor;
    private SpriteRenderer spriteRenderer;
    private OthelloManager.StoneOwner owner;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //P1, P2중 어느 플레이어의 돌인지 설정
    public void SetStone(OthelloManager.StoneOwner whoPlayerPutStone)
    {
        switch (whoPlayerPutStone)
        {
            case OthelloManager.StoneOwner.P1:
                spriteRenderer.color = p1StoneColor;
                owner = OthelloManager.StoneOwner.P1;
                break;

            case OthelloManager.StoneOwner.P2:
                spriteRenderer.color = p2StoneColor;
                owner = OthelloManager.StoneOwner.P2;
                break;
        }
    }

    //상태 뒤집는 함수
    //코루틴으로 임시 애니메이션 넣어놓은 상태. 수정예정
    public void Reverse()
    {
        StartCoroutine("reverseAnim");
    }

    private IEnumerator reverseAnim()
    {
        for (int i = 0; i < 30; i++)
        {
            transform.Rotate(0, 3, 0);
            yield return new WaitForSeconds(0.0005f);
        }
        reverse();
        for (int i = 0; i < 30; i++)
        {
            transform.Rotate(0, 3, 0);
            yield return new WaitForSeconds(0.0005f);
        }
    }

    private void reverse()
    {
        switch (owner)
        {
            case OthelloManager.StoneOwner.P1:
                SetStone(OthelloManager.StoneOwner.P2);
                break;

            case OthelloManager.StoneOwner.P2:
                SetStone(OthelloManager.StoneOwner.P1);
                break;
        }
    }
}
