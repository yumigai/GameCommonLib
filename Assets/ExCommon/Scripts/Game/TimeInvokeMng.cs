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
    public static TimeInvokeMng TimerAction( System.Action callback, float time, GameObject go = null) {
        var timer = GetTimer(go);
        timer.TimerAction(callback, time);
        return timer;
    }

    public static TimeInvokeMng TimerAction(System.Action callback, float time, float after, GameObject go = null) {
        var timer = GetTimer(go);
        timer.TimerAction(callback, time, after);
        return timer;
    }

    public static TimeInvokeMng FrameEndAction(System.Action callback, GameObject go = null) {
        var timer = GetTimer(go);
        timer.FrameAction(callback);
        return timer;
    }

    public static TimeInvokeMng TimerActionLoop(System.Action callback, float time, int count, GameObject go = null) {
        var timer = GetTimer(go);
        timer.TimerActionLoop(callback, time, count);
        return timer;
    }


    public static TimeInvokeMng GetTimer(GameObject go = null) {
        if (go == null) {
            go = CommonProcess.getCommonObject();
        }
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
    public void TimerAction(System.Action callback, float time, float after) {
        StartCoroutine(Invoke(callback, time, after));
    }

    public void TimerActionLoop(System.Action callback, float time, int count) {
        StartCoroutine(InvokeLoop(callback, time, count));
        
    }

    public void FrameAction(System.Action callback) {
        StartCoroutine(Invoke(callback));
    }

    IEnumerator Invoke(System.Action callback, float time) {
        yield return new WaitForSeconds(time);
        callback();
    }
    IEnumerator Invoke(System.Action callback, float time, float after) {
        yield return new WaitForSeconds(time);
        callback();
        yield return new WaitForSeconds(after);
    }

    IEnumerator Invoke(System.Action callback) {
        yield return new WaitForEndOfFrame();
        callback();
    }

    IEnumerator InvokeLoop(System.Action callback, float time, int count) {
        for (var i = 0; i < count; i++) {
            yield return new WaitForSeconds(time);
            callback();
        }
    }
}
