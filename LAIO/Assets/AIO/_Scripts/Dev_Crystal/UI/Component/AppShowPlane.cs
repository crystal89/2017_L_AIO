using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class AppShowPlane : Singleton<AppShowPlane>
{
    public enum ShowType
    {
        NONE,
        IMAGE,
        MODEL,
        VIDEO,

        UNINSTALL
    }

    private GameObject mImage;
    private GameObject mRawImage;
    private GameObject mModel;
    private Animation modelAnimation;

    private GameObject mUninstall;

    protected override void Awake()
    {
        base.Awake();

        mImage = transform.Find("ImagePlane").gameObject;
        if (mImage == null)
        {
            //添加一个ImagePlane
        }

        mRawImage = transform.Find("VideoPlane").gameObject;
        if (mRawImage == null)
        {
            //添加一个VideoPlane
        }

        mModel = transform.Find("ModelPlane").gameObject;
        modelAnimation = mModel.GetComponentInChildren<Animation>();
        if (mModel == null)
        {
            //添加一个ModelPlane
        }

        mUninstall = transform.Find("Uninstall").gameObject;
    }

    public void Uninstall(bool isShow)
    {
        if (isShow)
        {
            if (mUninstall && !mUninstall.activeSelf)
            {
                mUninstall.SetActive(true);
            }
        }
        else
        {
            if (mUninstall && mUninstall.activeSelf)
            {
                mUninstall.SetActive(false);
            }
        }
    }
    
    public IEnumerator DoShow(ShowType showType)
    {
        switch (showType)
        {
            case ShowType.IMAGE:
                mRawImage.SetActive(false);
                mModel.SetActive(false);
                mImage.SetActive(true);

                mImage.GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
                yield return new WaitForSeconds(2f);
                mImage.SetActive(false);
                break;

            case ShowType.VIDEO:
                break;

            case ShowType.MODEL:
                mImage.SetActive(false);
                mRawImage.SetActive(false);

                if (modelAnimation && !modelAnimation.isPlaying)
                {
                    mModel.SetActive(true);

                    //播放动画
                    if (modelAnimation && modelAnimation.isPlaying)
                    {
                        modelAnimation.Stop();
                    }
                    modelAnimation.Play();
                    yield return new WaitForSeconds(modelAnimation.clip.length * 1.5f);
                    mModel.SetActive(false);
                }
                break;

            case ShowType.NONE:
                mImage.SetActive(false);
                mRawImage.SetActive(false);
                mModel.SetActive(false);
                break;
        }

        yield return new WaitForEndOfFrame();
    }
}
