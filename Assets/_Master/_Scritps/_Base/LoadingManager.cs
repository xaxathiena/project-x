using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LoadingManager : Singleton<LoadingManager>
{
    public GameObject loadingView;
    private Coroutine coroutine_;
    public Image progress;
    public Text progressLB;
    public void LoadSceneByIndex(int index, Action calBack)
    {
        if (coroutine_ != null)
            StopCoroutine(coroutine_);
        coroutine_ = StartCoroutine(LoadingSceneIndex(index, calBack));

    }
    IEnumerator LoadingSceneIndex(int index, Action callback)
    {
        loadingView.SetActive(true);
        AsyncOperation progress_ = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        WaitForSeconds wait = new WaitForSeconds(0.01f);
        float count = 0;
        while (!progress_.isDone)
        {
            count += 0.01f;
            if (count > 0.5f)
            {
                count = 0.5f;
                if (progress_.progress > 0.5f)
                    count = progress_.progress;
            }

            progress.fillAmount = count;
            progressLB.text = Mathf.RoundToInt(count * 100).ToString() + "%";
            yield return wait;

        }
        progress.fillAmount = 1;
        progressLB.text = "100%";
        yield return new WaitForSeconds(1);
        callback?.Invoke();
        loadingView.SetActive(false);
    }
}
