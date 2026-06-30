using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MultiUseScrollMng : MonoBehaviour
{

    [SerializeField]
    public string IconDirectory;

    [SerializeField]
    public GameObject ListItem;

    [SerializeField]
    public GamePadListRecivMng Recive;

    [SerializeField,Tooltip("MultiUseListMngのDetailを表示するText。GamePadButtnRecvと競合しないように注意")]
    public Text GuidMessage;

    [System.NonSerialized]
    public List<MultiUseListMng> ItemList = new List<MultiUseListMng>();


    void Awake() {
        if (Recive == null) {
            Recive = GetComponent<GamePadListRecivMng>();
        }
        ListItem.SetActive(false);
    }

    public List<MultiUseListMng> makeList(MulitiUseListMast[] list) {

        clear();

        for (int i = 0; i < list.Length; i++) {
            makeListItemMasterId(i, list[i]);
        }

        return ItemList;
    }

    /// <summary>
    /// IDを独自採番する（マスタによるIDではなくトランザクションのIDを使用）
    /// </summary>
    /// <param name="i"></param>
    /// <param name="id"></param>
    /// <param name="mst"></param>
    /// <param name="button_txt"></param>
    /// <param name="btn_state"></param>
    /// <returns></returns>
    public MultiUseListMng makeListItem(int i, int id, MulitiUseListMast mst, string button_txt = "選択", MultiUseListMng.BUTTON btn_state = MultiUseListMng.BUTTON.SHOW) {
        MultiUseListMng mng = makeListItemMasterId(i, mst, button_txt, btn_state);
        mng.Id = id;
        return mng;
    }

    /// <summary>
    /// マスターIDで採番する
    /// </summary>
    /// <param name="i"></param>
    /// <param name="mst"></param>
    /// <param name="button_txt"></param>
    /// <param name="btn_state"></param>
    /// <returns></returns>
    public MultiUseListMng makeListItemMasterId(int i, MulitiUseListMast mst, string button_txt = "選択", MultiUseListMng.BUTTON btn_state = MultiUseListMng.BUTTON.SHOW) {

        MultiUseListMng mng = makeListItem(mst.Id, mst.Name, mst.Icon, mst.Detail);
        mng.Index = i;

        if (mng.ButtonTxt != null) {
            mng.ButtonTxt.text = button_txt;
        }

        mng.setButton(btn_state);
        mng.ImagePath = mst.ImagePath;
        mng.Callback = pushList;

        return mng;
    }

    public MultiUseListMng makeListItem(int id, string name, string icon = "", string detail = "", string value = "", UnityAction<int> action = null) {

        MultiUseListMng mng = makeListItem();
        mng.Id = id;

        if (mng.Name != null) {
            mng.Name.text = name;
        }
        if (icon != null && icon.Length > 0) {
            mng.setIcon(IconDirectory + icon);
        }
        if (mng.Detail != null) {
            mng.Detail.text = detail;
        }
        mng.DetailString = detail;
        if (GuidMessage != null && mng.SelectedCallback == null) {
            //個別コールバックが設定されていない場合、ガイドメッセージ更新を登録
            mng.SelectedCallback = ChangeSelectedGuidMessage;
        }

        if (mng.Value != null) {
            mng.Value.text = value;
        }
        if (mng.Btn != null && action != null) {
            mng.SetButtonInvoke(action);
        }

        return mng;
    }

    public MultiUseListMng makeListItem() {
        GameObject item = Instantiate(ListItem) as GameObject;
        item.SetActive(true);
        item.transform.SetParent(ListItem.transform.parent);
        item.transform.localScale = ListItem.transform.localScale;
        MultiUseListMng mng = item.GetComponent<MultiUseListMng>();
        ItemList.Add(mng);
        return mng;

    }

    virtual public void pushList(MultiUseListMng mng) {
    }

    /// <summary>
    /// リストクリア
    /// </summary>
    public void clear() {
        ListItem.SetActive(false);
        ItemList.ForEach(it => it.gameObject.SetActive(false));
        ItemList.ForEach(it => Destroy(it.gameObject));
        ItemList.Clear();
    }

    /// <summary>
    /// アクティブ状態のカウント
    /// </summary>
    /// <returns></returns>
    public int activeCount() {
        return ItemList.Where(it => it.gameObject.activeSelf && it.Btn.interactable).Count();
    }

    /// <summary>
    /// 指定したリストアイテムを取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MultiUseListMng getListItem( int id ) {
        return ItemList.Find(it => it.Id == id);
    }

    /// <summary>
    /// リスト入力準備
    /// </summary>
    /// <param name="over_ride"></param>
    public void ReadyInputGamePad(bool over_ride = false) {
        ItemList.ForEach(it => it.check(false));
        Recive.initSetupWithFrameEnd(over_ride);
    }

    private void ChangeSelectedGuidMessage(MultiUseListMng list ) {
        if (GuidMessage != null) {//念のため
            GuidMessage.text = list.DetailString;
        }
    }
}