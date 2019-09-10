using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] private Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        float time = PlayerPrefs.GetFloat("Time");
        int minuts = Mathf.FloorToInt(time / 60);
        int second = Mathf.FloorToInt(time) % 60;
        timeText.text = "クリア時間：" + minuts.ToString("D2") + ":" + second.ToString("D2");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("Title");
        }
    }
}
