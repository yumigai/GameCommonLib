using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMaskMng : MonoBehaviour
{
    public const float Speed = 10f;

    [SerializeField, Header("開くパターンか")]
    public bool IsOpen;

    private RectTransform MaskRect;

    private Vector2 CanvasSize;

    private Vector2 MinSize;

    private bool IsActive = false;

    public static HoleMaskMng Singleton;

    private void Awake() {

        Singleton = this;

        MaskRect = this.GetComponent<RectTransform>();
        CanvasSize = GetComponentInParent<Canvas>().rootCanvas.GetComponent<RectTransform>().sizeDelta;

        if (CanvasSize.x > CanvasSize.y) {
            MinSize = new Vector2(CanvasSize.x - CanvasSize.y, 0.001f);
        } else {
            MinSize = new Vector2(0.001f, CanvasSize.y - CanvasSize.x);
        }
    }

    private void Start() {
    }

    private void OnEnable() {
        init();
    }

    void init() {
        if (IsOpen) {
            MaskRect.sizeDelta = MinSize;
        } else {
            MaskRect.sizeDelta = CanvasSize;
        }
    }

    // Update is called once per frame
    void Update() {

        if (!IsActive) {
            return;
        }

        var size = MaskRect.sizeDelta;
        size.x = IsOpen ? size.x += Speed : size.x -= Speed;
        size.y = IsOpen ? size.y += Speed : size.y -= Speed;
        if (IsOpen) {
            if (size != CanvasSize) {
                if (size.x > CanvasSize.x || size.y > CanvasSize.y) {
                    size = CanvasSize;
                    IsActive = false;
                }
                MaskRect.sizeDelta = size;
            }
        } else {
            if (size != MinSize) {
                if (size.x < MinSize.x || size.y < MinSize.y) {
                    size = MinSize;
                    IsActive = false;
                }
                MaskRect.sizeDelta = size;
            }
        }
    }

    public void show(bool isShow, bool isOpen) {
        IsOpen = isOpen;
        this.gameObject.SetActive(isShow);
        //少し待ってからアクション開始
        TimeInvokeMng.TimerAction(() => { Singleton.IsActive = true; }, 0.5f, Singleton.gameObject);
    }

    public static void showStatic(bool isShow, bool isOpen) {

        Singleton.show(isShow, isOpen);
    }
}
