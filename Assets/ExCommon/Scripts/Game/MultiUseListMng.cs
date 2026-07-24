using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiUseListMng : ListItemMng
{

	public enum BUTTON
	{
		SHOW,
		HIDE,
		LOCK,
	}

	[SerializeField]
	public Text ButtonTxt;

	[SerializeField]
	public GamePadButtonMng Btn;

	[SerializeField]
	public Text Name;

	[SerializeField]
	public Text Detail;

	[SerializeField]
	public Text Value;

	[SerializeField]
	public Image Icon;

	[SerializeField]
	public string ImagePath; //画像パス

	[SerializeField]
	public Text TagLabel;

	[SerializeField]
	public Text[] ExtraTxts;

	[SerializeField]
	public Image[] ExtraImgs;

	[System.NonSerialized]
	public string DetailString;

	public UnityAction<MultiUseListMng> Callback{  set { if(Btn!=null)Btn.SetButtonInvoke(value,this); } }

	public UnityAction<MultiUseListMng> SelectedCallback {  set { if(Btn!=null)Btn.SetButtonSelectInvoke(value,this); } }

	public static MultiUseListMng SelectedItem;

	/// <summary>
	/// 
	/// </summary>
	public string ExtraText1 {
		set {
			SetExtraText1(value);
		}
	}
	/// <summary>
	/// 
	/// </summary>
	public string ExtraText2 {
		set {
			SetExtraText2(value);
		}
	}

    protected override void Awake() {
		base.Awake();
		if (Btn == null) {
			Btn = GetComponent<GamePadButtonMng>();
		}
	}

    public Sprite setIcon(string path) {
		if (path.Length == 0) {
			return null;
		}
		Sprite sp = Resources.Load<Sprite>(path);
		if (Icon != null) {
			Icon.sprite = sp;
		}
		return sp;
	}

	public void setButton(BUTTON state) {
		if (Btn == null) {
			return;
		}
		switch (state) {
			case BUTTON.SHOW:
			Btn.gameObject.SetActive(true);
			Btn.interactable = true;
			break;
			case BUTTON.HIDE:
			Btn.gameObject.SetActive(false);
			break;
			case BUTTON.LOCK:
			Btn.gameObject.SetActive(false); //一旦非表示にする  
			Btn.interactable = false;
			Btn.gameObject.SetActive(true);
			break;
		}
	}

	public void SetExtraText1(string str) {
		if (ExtraTxts.Length > 0) {
			ExtraTxts[0].text = str;
		}
	}

	public void SetExtraText2(string str) {
		if (ExtraTxts.Length > 1) {
			ExtraTxts[1].text = str;
		}
	}

	#region
	/// <summary>
	///  ボタンイベント登録
	/// </summary>
	/// <param name="call"></param>
	public void SetButtonInvoke(UnityAction call) {
		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(call);
	}

	public void SetButtonInvoke(UnityAction<MultiUseListMng> call) {
		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => call(this));
	}

	public void SetButtonInvoke(UnityAction<int> call) {
		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => call(this.Id));
	}

	public void SetButtonInvoke(UnityAction<int> call, int value) {
		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => call(value));
	}
	#endregion

	public static MultiUseListMng GetNowListItem() {
		return EventSystem.current.currentSelectedGameObject.GetComponentInChildren<MultiUseListMng>();
	}
}