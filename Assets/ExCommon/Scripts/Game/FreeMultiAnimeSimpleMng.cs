using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMultiAnimeSimpleMng : MonoBehaviour
{
    public enum END_TO
    {
        RESTART,
        REVERSE,
        STOP,
        BREAK,
        REPEAT,
    }

    [SerializeField]
    public Vector3 RoleLimit;

    [SerializeField]
    public Vector3 MoveLimit;

    [SerializeField]
    public Vector3 ScaleLimit;

    [SerializeField]
    public float AnimeTime = 1f;

    [SerializeField]
    public END_TO EndTo;

    [SerializeField]
    public float IntervalTime = 0f;

    [SerializeField]
    private bool IsStop = false;

    [SerializeField]
    public bool IsRole;

    [SerializeField]
    public bool IsMove;

    [SerializeField]
    public bool IsScale;


    private float ReverseAdj = 1;

    private float RestTime = 0f;

    private float RestInterval = 0f;

    private Vector3 InitRole;
    private Vector3 InitPosition;
    private Vector3 InitScale;

    private Vector3 RoleSpeed;
    private Vector3 MoveSpeed;
    private Vector3 ScaleSpeed;

    private void Awake() {
        InitRole = this.transform.localEulerAngles;
        InitPosition = this.transform.localPosition;
        InitScale = this.transform.localScale;
        StatusReset();
    }

    void FixedUpdate() {

        if (IsStop) {
            return;
        }

        if (RestInterval > 0) {
            RestInterval -= Time.fixedDeltaTime;
            if (RestInterval <= 0 && EndTo == END_TO.RESTART) {
                RestTime = AnimeTime;
                PositionReset();
            }
            return;
        }

        RestTime -= Time.fixedDeltaTime;

        Transform t = this.transform;

        if (IsRole) {
            t.Rotate(RoleSpeed * ReverseAdj);
        }
        if (IsMove) {
            t.localPosition += MoveSpeed * ReverseAdj;
        }

        if (IsScale) {
            t.localScale += ScaleSpeed * ReverseAdj;
        }
            

        if (RestTime <= 0f) {
            switch (EndTo) {
                case END_TO.RESTART:
                StatusReset();
                PositionReset();
                break;
                case END_TO.REVERSE:
                RestTime = AnimeTime;
                RestInterval = IntervalTime;
                ReverseAdj *= -1;
                if (ReverseAdj > 0) {
                    PositionReset();
                }
                break;
                case END_TO.STOP:
                IsStop = true;
                break;
                case END_TO.BREAK:
                Destroy(this.gameObject);
                break;
                case END_TO.REPEAT:
                RestTime = AnimeTime;
                RestInterval = IntervalTime;
                break;
            }
        }
    }

    private void PositionReset() {
        if (IsRole) {
            this.transform.localEulerAngles = InitRole;
        }
        if (IsMove) {
            this.transform.localPosition = InitPosition;
        }
        if (IsScale) {
            this.transform.localScale = InitScale;
        }
    }
    private void StatusReset() {

        RoleSpeed = RoleLimit / AnimeTime * Time.fixedDeltaTime;
        MoveSpeed = MoveLimit / AnimeTime * Time.fixedDeltaTime;
        ScaleSpeed = ScaleLimit / AnimeTime * Time.fixedDeltaTime;

        RestTime = AnimeTime;
        RestInterval = IntervalTime;

    }

    public void AnimeStart() {
        IsStop = false;
    }

    public void AnimeStop() {
        IsStop = true;
    }

    public void AnimeRestart() {
        ReverseAdj = 1;
        StatusReset();
        AnimeStart();
    }

    public void Forward() {
        if (ReverseAdj < 0) {
            ReversePlane();
            ReverseAdj = 1;
        }
    }

    public void Reverse() {
        if (ReverseAdj > 0) {
            ReversePlane();
            ReverseAdj = -1;
        }
    }

    private void ReversePlane() {
        RoleLimit = -RoleLimit;
        MoveLimit = -MoveLimit;
        ScaleLimit = -ScaleLimit;
        PositionReset();
        StatusReset();
    }
}
