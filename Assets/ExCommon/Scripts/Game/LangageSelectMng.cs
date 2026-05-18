using UnityEngine;
using System.Collections;

public class LangageSelectMng : MonoBehaviour {


    static LangageSelectMng Singleton;

    private void Awake() {
        Singleton = this;
    }

    public static LangageSelectMng show(GameObject prefab, Transform pare)
    {
        if (Singleton != null) {
            Destroy(Singleton.gameObject);
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = pare;
        obj.transform.localPosition = new Vector3();
        obj.transform.localScale = prefab.transform.localScale;
        return obj.GetComponent<LangageSelectMng>();

    }

    public void pushJp()
    {
		changeLang(CmnSaveProc.GameConfig.LANG.JP);
    }
    public void pushEn()
    {
		changeLang(CmnSaveProc.GameConfig.LANG.ENG);
    }

	public void changeLang(CmnSaveProc.GameConfig.LANG lang)
    {
		CmnBaseProcessMng.playClickSe ();
		CmnSaveProc.Conf.SelectLang = (int)lang;
		CmnSaveProc.Conf.Standby = true;
		CmnSaveProc.saveConfig();
		if (SceneManagerWrap.NowScheneIs(CmnConst.SCENE.SelectLangScene)) {
            SceneManagerWrap.loadBefore();
		} else {
            this.gameObject.SetActive(false);
			//Destroy (this.gameObject);
		}
    }

    public void show() {
        this.gameObject.SetActive(true);
    }
}
