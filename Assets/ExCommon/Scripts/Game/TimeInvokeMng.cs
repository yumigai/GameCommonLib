using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInvokeMng : MonoBehaviour
{
    /// <summary>
    /// 指定時間後、アクション
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static TimeInvokeMng TimerAction( System.Action callback, float time, GameObject go) {
        var timer = GetTimer(callback, go);
        timer.TimerAction(callback, time);
        return timer;
    }

    public static TimeInvokeMng FrameEndAction(System.Action callback, GameObject go = null) {
        if (go == null) {
            go = CommonProcess.getCommonObject();
        }
        var timer = GetTimer(callback, go);
        timer.FrameAction(callback);
        return timer;
    }

    protected static TimeInvokeMng GetTimer(System.Action callback, GameObject go) {
        var timer = go.GetComponent<TimeInvokeMng>();
        if (timer == null) {
            timer = go.AddComponent<TimeInvokeMng>();
        }

        return timer;
    }

    /// <summary>
    /// 指定時間後、破棄
    /// </summary>
    /// <param name="go"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static TimeInvokeMng TimerDestroy(float time, GameObject go) {
        return TimerAction(()=>{ Destroy(go); },time, go);
    }

    /// <summary>
    /// 指定時間後、隠す
    /// </summary>
    /// <param name="go"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static TimeInvokeMng TimerHide(float time, GameObject go) {
        return TimerAction(() => { go.SetActive(false); }, time, go);
    }


    public void TimerAction(System.Action callback, float time) {
        StartCoroutine(Invoke(callback, time));
    }

    public void FrameAction(System.Action callback) {
        StartCoroutine(Invoke(callback));
    }

    IEnumerator Invoke(System.Action callback, float time) {
        yield return new WaitForSeconds(time);
        callback();
    }

    IEnumerator Invoke(System.Action callback) {
        yield return new WaitForEndOfFrame();
        callback();
    }
}
