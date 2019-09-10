using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle : MonoBehaviour, IPointerDownHandler
{
    public MeshRenderer meshRenderer { get; private set; }

    public bool IsComplete => x == boardX && y == boardY;

    private GameSceneManager gameSceneManager;
    public int x;
    public int y;
    private int boardX;
    private int boardY;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(GameSceneManager gameSceneManager, int x, int y)
    {
        this.gameSceneManager = gameSceneManager;
        this.x = x;
        this.y = y;
        UpdateBoardIndex(this.x, this.y);
    }

    public void Move(int split)
    {
        var halfSplit = split * 0.5f;
        var pos = transform.position;
        pos.x = (boardX - halfSplit) * meshRenderer.bounds.size.x;
        pos.y = (boardY - halfSplit) * meshRenderer.bounds.size.y;
        transform.position = pos;
    }

    public void UpdateBoardIndex(int x, int y)
    {
        this.boardX = x;
        this.boardY = y;
    }

    public void OnPointerDown(PointerEventData data)
    {
        gameSceneManager.Slide(boardX, boardY);
    }
}
