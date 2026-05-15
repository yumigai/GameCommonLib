using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect2dMng : MonoBehaviour
{
    [SerializeField]
    public GameObject EffectPrefab;
    [SerializeField]
    private GameObject[] Effects;

    private int EffectCount = 0;

    public GameObject effect(Vector3 posi )
    {
        if (EffectCount >= Effects.Length)
        {
            EffectCount = 0;
        }

        GameObject eff;

        if (Effects[EffectCount] == null)
        {
            eff = Instantiate(EffectPrefab) as GameObject;
            Effects[EffectCount] = eff;
            eff.transform.localScale = EffectPrefab.transform.localScale;
            eff.transform.parent = this.transform;
            eff.layer = this.gameObject.layer;

        }
        else
        {
            eff = Effects[EffectCount];
        }

        eff.transform.position = posi;
        eff.SetActive(true);

        var anime = eff.GetComponent<FreeImageAnimeMng>();
        if (anime != null) {
            anime.IsActive = true;
        }

        EffectCount++;

        return eff;
    }
}
