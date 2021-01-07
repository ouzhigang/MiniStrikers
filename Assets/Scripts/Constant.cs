﻿using System.Collections.Generic;
using UnityEngine;

public class Constant
{
    //场景索引
    public static readonly int OpeningScene = 0;
    public static readonly int OptionScene = 1;
    public static readonly int PlayerSelectScene = 2;
    public static readonly int LoadingScene = 3;
    public static readonly int GameOverScene = 4;
    public static readonly int Stage01Scene = 5;
    public static readonly int Stage02Scene = 6;
    public static readonly int Stage03Scene = 7;
    public static readonly int Stage04Scene = 8;
    public static readonly int Stage05Scene = 9;
    public static readonly int Stage06Scene = 10;
    public static readonly int Stage07Scene = 11;
    public static readonly int GameClearScene = 12;

    //背景滚动速度
    public static readonly float BgScroll = 0.5f;

    //是否暂停游戏
    public static bool GameIsPause = false;

    //自机数
    public static int PlayerLife = 2;

    //当前自机数
    public static int PlayerLifeCurr = 0;

    //最大分数
    public static readonly int ScoreMax = 99999999;

    //当前分数
    public static int ScoreCurr = 0;

    //当前关卡，对应上面的场景索引
    public static int StageSurr = Stage01Scene;

    //游戏中是否播放声音
    public static bool GameIsPlayingSound = true;

    //Player
    public static string PlayerPlane = "Plane0";

    //异步加载场景时用到
    public static readonly string NextSceneIndex = "next_scene_index";

    //在每个需要处理声音开关的程序的Start方法最前面执行
    public static void ObjectIsPlayingSound(MonoBehaviour obj)
    {
        foreach (var item in obj.GetComponents<AudioSource>())
        {
            if (GameIsPlayingSound)
            {
                item.volume = 1f;
            }
            else
            {
                item.volume = 0f;
            }
        }
    }

}
