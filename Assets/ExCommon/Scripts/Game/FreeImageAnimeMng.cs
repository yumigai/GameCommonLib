using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FreeImageAnimeMng : MonoBehaviour {

    public enum END_TO
    {
        LOOP,
        STOP,
    }

    [SerializeField]
    public END_TO EndTo;

	[SerializeField]
	public Image AnimeBoard;

	[SerializeField]
	public float Speed;

	[SerializeField]
	public Sprite[] Sprites;

	[SerializeField]
	public bool IsActive = true;

	private int Index;
	private float AnimeTime;

	void Awake(){
		if (AnimeBoard == null) {
			AnimeBoard = GetComponent<Image> ();
		}
	}
	void OnEnable(){
		Index = 0;
		AnimeTime = 0f;
	}
		
	void FixedUpdate(){
		if (Speed > 0f && IsActive ) {
			AnimeTime += Speed * Time.fixedDeltaTime;
			if (AnimeTime >= 1) { //êÿÇËë÷Ç¶íPà Åi1ïbÅj
				AnimeTime = 0f;
				
				if (Index < Sprites.Length) {
					AnimeBoard.sprite = Sprites[Index];
					Index++;
				} else {
					if (EndTo == END_TO.LOOP) {
						Index = 0;
					} else {
						this.gameObject.SetActive(false);
					}
				}
			}
		}
	}

}
