using System;
using UnityEngine;

public class TargetSkin : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;
	[SerializeField] private String _postfix = "up";
	
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		LoadSkin(ScreenSkins.CurrentTargetId);
	}
	
	private void OnEnable()
	{
		GlobalEvents<OnChangeTargetSkin>.Happened += OnChangeTargetSkin;
	}

	private void OnDisable()
	{
		GlobalEvents<OnChangeTargetSkin>.Happened -= OnChangeTargetSkin;
	}

	private void OnChangeTargetSkin(OnChangeTargetSkin obj)
	{
		LoadSkin(obj.Id);
	}

	private void LoadSkin(int objId)
	{
		if (_spriteRenderer.sprite) Resources.UnloadAsset(_spriteRenderer.sprite);
		_spriteRenderer.sprite = Resources.Load<Sprite>("Gfx/Targets/target_" + (objId + 1) + "_" + _postfix);
	}
}
