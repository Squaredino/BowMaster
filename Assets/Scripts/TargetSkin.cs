using System;
using UnityEngine;

public class TargetSkin : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;
	[SerializeField] private String _postfix = "up";
	
	void Awake ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		LoadSkin(ScreenSkins.CurrentTargetId);
	}

	public void LoadSkin(int objId)
	{
//		if (_spriteRenderer.sprite) Resources.UnloadAsset(_spriteRenderer.sprite);
		_spriteRenderer.sprite = Resources.Load<Sprite>("Gfx/Targets/target_" + objId + "_" + _postfix);
	}
}
