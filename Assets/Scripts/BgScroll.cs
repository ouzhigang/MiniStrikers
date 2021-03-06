﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroll : MonoBehaviour
{
    //纵向卷轴背景

    int CurrBgIndex;
    Transform Bg1, Bg2;

    // Start is called before the first frame update
    void Start()
    {
		Constant.ObjectIsPlayingSound(this);
		
        CurrBgIndex = 0;

        Bg1 = transform.Find("stage_bg_1");
        Bg2 = transform.Find("stage_bg_2");
    }

    // Update is called once per frame
    void Update()
    {
        if (Constant.GameIsPause)
        {
            return;
        }

        Camera.main.transform.Translate(new Vector3(0, Constant.BgScroll * Time.deltaTime, 0));

        float size2 = Camera.main.orthographicSize * 2f;

        int bg_index = Mathf.FloorToInt(Camera.main.transform.localPosition.y / size2);
        if (bg_index > CurrBgIndex)
        {
            CurrBgIndex = bg_index;

            //这个判断是摄像头走到背景索引为奇数（奇数就走到了bg2背景）的时候就把bg1放到bg2上面，偶数就是bg2放到bg1上面
            bool is_even = Convert.ToBoolean(CurrBgIndex % 2);
            Transform bg = is_even ? Bg1 : Bg2;
            float bg_y = ((float)CurrBgIndex + 1f) * size2;
            bg.transform.localPosition = new Vector3(0f, bg_y, bg.transform.localPosition.z);

            //如果另一个没有处理的bg的y坐标的位置跟计算出来的结果匹配不上则修正
            bg = is_even ? Bg2 : Bg1;
            bg_y = (float)CurrBgIndex * size2;
            if (bg.transform.localPosition.y != bg_y)
            {
                bg.transform.localPosition = new Vector3(0f, bg_y, bg.transform.localPosition.z);
            }
        }

    }
}
