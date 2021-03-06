﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    bool IsDirectionUp = false;
    bool IsDirectionDown = false;

    int MenuSelectedIndex = 0;
    Transform Items;
    Transform MenuCursor;
    int MenuLength = 0;

    // Start is called before the first frame update
    void Start()
    {
		Constant.ObjectIsPlayingSound(this);
		
        Items = transform.Find("Items");
        MenuCursor = transform.Find("MenuCursor");
        MenuLength = Items.childCount;

        //移动端不显示光标
#if UNITY_ANDROID || UNITY_IPHONE
        MenuCursor.localScale = Vector3.zero;
#endif
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_STANDALONE || UNITY_EDITOR
        UpdatePC();
#endif

    }

    void UpdatePC()
    {
        //======================================
        //方向上
        if (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") < -0.5f)
        {
            if (!IsDirectionUp)
            {
                IsDirectionUp = true;

                if (MenuSelectedIndex > 0)
                {
                    MenuSelectedIndex--;
                    GetComponent<AudioSource>().Play();

                    Transform MenuSelectedItem = Items.GetChild(MenuSelectedIndex);
                    MenuCursor.localPosition = new Vector3(MenuCursor.localPosition.x, MenuSelectedItem.localPosition.y, MenuCursor.localPosition.z);
                }
            }
        }
        else
        {
            if (IsDirectionUp)
            {
                IsDirectionUp = false;
            }
        }
        //方向上
        //======================================

        //======================================
        //方向下
        if (Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") > 0.5f)
        {
            if (!IsDirectionDown)
            {
                IsDirectionDown = true;

                if (MenuSelectedIndex + 1 < MenuLength)
                {
                    MenuSelectedIndex++;
                    GetComponent<AudioSource>().Play();

                    Transform MenuSelectedItem = Items.GetChild(MenuSelectedIndex);
                    MenuCursor.localPosition = new Vector3(MenuCursor.localPosition.x, MenuSelectedItem.localPosition.y, MenuCursor.localPosition.z);
                }
            }
        }
        else
        {
            if (IsDirectionDown)
            {
                IsDirectionDown = false;
            }
        }
        //方向下
        //======================================

        //======================================
        //按了确认键
        if (Input.GetKey(KeyCode.L) || Input.GetButton("Fire2_JS"))
        {
            //恢复初始状态
            Constant.GameIsPause = false;
            Constant.PlayerLifeCurr = Constant.PlayerLife;
            Constant.ScoreCurr = 0;
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            //恢复初始状态 end

            if (MenuSelectedIndex == 0)
            {
                //Continue，初始化
                PlayerPrefs.SetInt(Constant.NextSceneIndex, Constant.StageCurr);
                SceneManager.LoadScene(Constant.LoadingScene);
            }
            else if (MenuSelectedIndex == 1)
            {
                //End
                Constant.StageCurr = Constant.Stage01Scene;
                PlayerPrefs.SetInt(Constant.NextSceneIndex, Constant.MainScene);
                SceneManager.LoadScene(Constant.LoadingScene);
            }

        }
        //按了确认键
        //======================================

    }

}
