﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class Enemy12CPU : MonoBehaviour
{
    enum ActionStep
    {
        One, //移动到目标Y坐标
        Two, //瞄准player攻击
        Three //逃跑
    }

    public float FireTime = 0.05f; //子弹发射时间密度，越小越多子弹
    float FireStartTime = 0.15f;
    float FireStopTime = 0.6f;
    public float MoveTargetY = 3f; //最终移动到的目标Y坐标
    public float MoveTime = 0.35f; //移动的时间，越小速度越快
    public float GoToEndTime = 5f; //停留的时间（秒），最终移动到的目标Y坐标后，这个时间到了就跑到画面边界执行销毁
    float NextFireTime = 0f;
    bool DoChangeFireStatus = false; //是否正在执行ChangeFireStatus方法，确保ChangeFireStatus方法只执行一次
    bool DoFire = true;
    ActionStep CurrActionStep;

    Transform PlayerLayer;
    Transform BulletLayer;
    Transform HideLayer;

    Transform PlayerPlane;
    Vector3 BulletEulerAngles;

    bool GameIsPause = false;

    // Start is called before the first frame update
    void Start()
    {
        Constant.ObjectIsPlayingSound(this);
		
		if (transform.parent.name != "HideLayer")
        {
			PlayerLayer = Camera.main.transform.Find("PlayerLayer");
            BulletLayer = transform.parent.Find("BulletLayer");
            HideLayer = Camera.main.transform.Find("HideLayer");

            PlayerPlane = PlayerLayer.Find(Constant.PlayerPlane);

            CurrActionStep = ActionStep.One;
            transform.DOLocalMoveY(MoveTargetY, MoveTargetY * MoveTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                CurrActionStep = ActionStep.Two;
                StartCoroutine(GoToEndDelay());
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.name != "HideLayer")
        {
            if (Constant.GameIsPause)
            {
                if (!GameIsPause)
                {
                    transform.DOPause();
                    GameIsPause = Constant.GameIsPause;
                }
                return;
            }
            else if (!Constant.GameIsPause && GameIsPause)
            {
                transform.DOPlay();
                GameIsPause = Constant.GameIsPause;
                return;
            }

            if (CurrActionStep != ActionStep.Three)
            {
                if (PlayerPlane == null || PlayerPlane.localPosition.x <= -500)
                {
                    StopCoroutine(GoToEndDelay());
                    GoToEnd();
                    return;
                }

                //角度
                float angle = FHUtility.Angle360(transform.localPosition, PlayerPlane.localPosition);
                angle -= 90f;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);

                //子弹部分
                if (Time.time > NextFireTime && CurrActionStep == ActionStep.Two)
                {
                    if (!DoChangeFireStatus)
                    {
                        StartCoroutine(ChangeFireStatus());
                        DoChangeFireStatus = true;
                    }

                    if (DoFire)
                    {
                        int c = 8; //同时发出的子弹数
                        float f = 10f; //子弹发出的角度浮动

                        for (int i = 0; i < c; i++)
                        {
                            string bullet_n = Constant.EnemyBullets01[Random.Range(0, Constant.EnemyBullets01.Length - 1)];
                            Transform bullet = Instantiate(HideLayer.Find(bullet_n));
                            bullet.SetParent(BulletLayer);

                            Vector3 bullet_pos = transform.localPosition;
                            bullet.eulerAngles = BulletEulerAngles;

                            bullet.localPosition = bullet_pos;

                            float tmp_angle = bullet.eulerAngles.z - 90f;
                            Vector2 tmp = FHUtility.HypotenuseAngle2Position(20f, Random.Range(tmp_angle - f, tmp_angle + f));
                            Vector3 pos = new Vector3(tmp.x, tmp.y, 0);
                            pos = bullet.localPosition + pos;
                            bullet.DOLocalMove(pos, 4.0f).SetEase(Ease.Linear);
                        }

                    }

                    NextFireTime = Time.time + FireTime;
                }
            }

        }
    }
	
	private void OnDestroy()
    {
        transform.DOKill(true);
    }

    IEnumerator GoToEndDelay()
    {
        yield return new WaitForSeconds(GoToEndTime);
        GoToEnd();
    }

    void GoToEnd()
    {
        CurrActionStep = ActionStep.Three;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        transform.DOLocalMoveY(transform.localPosition.y - 20f, 12f * MoveTime).SetEase(Ease.Linear);
    }

    IEnumerator ChangeFireStatus()
    {
        if (DoFire)
        {
            BulletEulerAngles = transform.eulerAngles;
            yield return new WaitForSeconds(FireStartTime);
            DoFire = false;
        }
        else
        {
            yield return new WaitForSeconds(FireStopTime);
            DoFire = true;
        }
        StartCoroutine(ChangeFireStatus());
    }

}
