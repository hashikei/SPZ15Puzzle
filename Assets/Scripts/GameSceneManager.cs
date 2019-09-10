using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private int split;
    [SerializeField] private Puzzle puzzlePrefab;
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject gameStartObj;
    [SerializeField] private GameObject gameClearObj;
    [SerializeField] private GameObject maskObj;

    private Puzzle[,] puzzleBoard;
    private Puzzle lastPuzzle;

    private float elapsedTime;
    private bool isGamePlaing;
    private bool isGameClear;

    void Start()
    {
        Initialize(split);
        StartCoroutine(GameStart());
    }

    void Update()
    {
        if (!isGamePlaing || isGameClear)
            return;

        elapsedTime += Time.deltaTime;
        int minuts = Mathf.FloorToInt(elapsedTime / 60);
        int second = Mathf.FloorToInt(elapsedTime) % 60;
        timeText.text = "経過時間：" + minuts.ToString("D2") + ":" + second.ToString("D2");
    }

    void Initialize(int split)
    {
        elapsedTime = 0f;
        isGamePlaing = false;
        isGameClear = false;
        puzzleBoard = new Puzzle[split, split];

        int skipIndexX = Random.Range(0, split);
        int skipIndexY = Random.Range(0, split);

        for (int x = 0; x < split; ++x) {
            for (int y = 0; y < split; ++y) {
                var puzzle = Instantiate(puzzlePrefab, transform);
                puzzle.Initialize(this, x, y);
                var tiling = new Vector2(1f / split, 1f / split);
                var offset = new Vector2((float)x / split, (float)y / split);
                puzzle.meshRenderer.material.SetTextureScale("_MainTex", tiling);
                puzzle.meshRenderer.material.SetTextureOffset("_MainTex", offset);
                puzzle.Move(split);
                if (x == skipIndexX && y == skipIndexY) {
                    puzzle.gameObject.SetActive(false);
                    lastPuzzle = puzzle;
                } else {
                    puzzleBoard[x, y] = puzzle;
                }
            }
        }

        // シャッフル
        do {
            for (int i = 0; i < 1000; ++i) {
                int x = Random.Range(0, split);
                int y = Random.Range(0, split);
                Slide(x, y, true);
            }
        } while (IsComplete());
    }

    public void Slide(int x, int y, bool isInitialize = false)
    {
        int nextX = x;
        int nextY = y;

        if (x - 1 >= 0 && puzzleBoard[x - 1, y] == null) {
            // 左
            nextX = x - 1;
        } else if (x + 1 <= split - 1 && puzzleBoard[x + 1, y] == null) {
            // 右
            nextX = x + 1;
        } else if (y - 1 >= 0 && puzzleBoard[x, y - 1] == null) {
            // 下
            nextY = y - 1;
        } else if (y + 1 <= split - 1 && puzzleBoard[x, y + 1] == null) {
            // 上
            nextY = y + 1;
        } else {
            return;
        }

        var puzzle = puzzleBoard[x, y];
        if (puzzle == null)
            return;

        puzzleBoard[nextX, nextY] = puzzle;
        puzzleBoard[x, y] = null;
        puzzle.UpdateBoardIndex(nextX, nextY);
        puzzle.Move(split);

        if (!isInitialize && IsComplete()) {
            StartCoroutine(GameClear());
        }
    }

    bool IsComplete()
    {
        foreach (var puzzle in puzzleBoard) {
            if (puzzle == null)
                continue;

            if (!puzzle.IsComplete)
                return false;
        }
        return true;
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1f);

        gameStartObj.SetActive(false);
        maskObj.SetActive(false);

        isGamePlaing = true;
    }

    IEnumerator GameClear()
    {
        maskObj.SetActive(true);
        gameClearObj.SetActive(true);
        isGameClear = true;
        PlayerPrefs.SetFloat("Time", elapsedTime);

        lastPuzzle.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Result");
    }
}
