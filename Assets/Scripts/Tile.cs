using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] public Color checkerColor;
    private SpriteRenderer spriteRenderer;
    private GameObject highlight;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        highlight = transform.Find("Highlight").gameObject;
    }

    public void SetColor(bool checker)
    {
        if (checker)
            spriteRenderer.color = checkerColor;
        else
            spriteRenderer.color = normalColor;
    }

    void OnMouseEnter() { highlight.SetActive(true); }
    void OnMouseExit() { highlight.SetActive(false); }

    private void OnMouseDown()
    {
        Vector2Int gridPos = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        OthelloManager.instance.TryPutStone(gridPos);
    }
}
