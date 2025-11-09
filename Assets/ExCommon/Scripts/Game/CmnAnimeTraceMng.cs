using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CmnAnimeTraceMng : MonoBehaviour
{
    public enum END_TO
    {
        NON,
        RESTART,
        REVERSE,
        DESTROY,
    }

    [SerializeField]
    public Transform[] TargetPoints;

    [SerializeField]
    public float Speed;

    [SerializeField]
    public float Interval;

    [SerializeField]
    public END_TO EndTo;

    [SerializeField]
    public float[] MultiSpeeds;

    [SerializeField]
    public UnityEvent FinishEvent;

    [SerializeField]
    private bool IsStop = false;

    [System.NonSerialized]
    public Vector3[] MovePoints;

    public System.Action Callback;

    private Vector3 InitPosition;
    private Vector3 InitAngle;
    private int[] MaxMoveCounts;
    private Vector3[] MoveDirect;
    private Vector3[] AngleDirect;
    private int MoveCount;
    private int NowMoveIndex;
    private int ReverseAdj = 1;

    private float RestInterval = 0;


    private void Awake() {
        initSetting();
        moveSetting();
    }

    protected void initSetting() {
        if (TargetPoints != null && TargetPoints.Length > 0) {
            MovePoints = new Vector3[TargetPoints.Length];
            for (int i = 0; i < MovePoints.Length; i++) {
                MovePoints[i] = TargetPoints[i].position;
            }
            InitPosition = MovePoints[0];
            InitAngle = TargetPoints[0].localEulerAngles;
        }
    }

    protected void moveSetting() {
        MoveCount = 0;
        if (MovePoints.Length > 1) {
            MaxMoveCounts = new int[MovePoints.Length - 1];
            MoveDirect = new Vector3[MovePoints.Length - 1];
            AngleDirect = new Vector3[MovePoints.Length - 1];

            for (int i = 0; i < MaxMoveCounts.Length; i++) {
                float move = Speed;
                if (MultiSpeeds != null && MultiSpeeds.Length > i) {
                    move = MultiSpeeds[i];
                }
                move *= Time.fixedDeltaTime;

                float dist = Vector3.Distance(MovePoints[i], MovePoints[i + 1]);
                MoveDirect[i] = (MovePoints[i + 1] - MovePoints[i]) * (move / dist);
                MaxMoveCounts[i] = (int)Mathf.Ceil(dist / move);

                AngleDirect[i] = (TargetPoints[i + 1].localEulerAngles - TargetPoints[i].localEulerAngles) / MaxMoveCounts[i];
            }
        }

        this.transform.position = InitPosition;
        this.transform.localEulerAngles = InitAngle;
    }

    void FixedUpdate() {
        if (!IsStop && NowMoveIndex < MoveDirect.Length && NowMoveIndex >= 0) {

            if (RestInterval > 0) {
                RestInterval -= Time.fixedDeltaTime;
                return;
            }

            if (MoveCount >= MaxMoveCounts[NowMoveIndex]) {
                NowMoveIndex += ReverseAdj;
                MoveCount = 0;
                RestInterval = Interval;
            } else {
                this.transform.position += MoveDirect[NowMoveIndex] * ReverseAdj;
                this.transform.localEulerAngles += AngleDirect[NowMoveIndex] * ReverseAdj;
                MoveCount++;
            }

            if (NowMoveIndex >= MovePoints.Length - 1 || NowMoveIndex < 0) {

                if (FinishEvent != null && FinishEvent.GetPersistentEventCount() > 0) {
                    FinishEvent.Invoke();
                }
                switch (EndTo) {
                    case END_TO.RESTART:
                        NowMoveIndex = 0;
                        StatusReset();
                        break;
                    case END_TO.REVERSE:
                    ReverseAdj *= -1;
                    if (ReverseAdj > 0) {
                        NowMoveIndex = 0;
                        StatusReset();
                        } else {
                        NowMoveIndex = MoveDirect.Length - 1;
                    }
                    break;
                    case END_TO.DESTROY:
                    Destroy(this.gameObject);
                    break;
                }

            }

        }

    }

    private void StatusReset() {
        this.transform.localPosition = InitPosition;
        this.transform.localEulerAngles = InitAngle;
    }

    public void AnimeStart() {
        IsStop = false;
    }

    public void AnimeStop() {
        IsStop = true;
    }

    public void AnimeSwitch() {
        IsStop = !IsStop;
    }

    public void AnimeRestart() {
        ReverseAdj = 1;
        StatusReset();
        AnimeStart();
    }
}
