using DoozyUI;
using UnityEngine;

public class ScreenItem : MonoBehaviour {
	protected UIElement[] elements;
	protected UIButton[] buttons;

	protected void InitUi()
	{
		elements = GetComponentsInChildren<UIElement>();
		buttons = GetComponentsInChildren<UIButton>();
	}
	
	public virtual void Show()
	{
		foreach (UIElement element in elements)
			UIManager.ShowUiElement(element.elementName);
		EnableButtons();
	}
	
	public virtual void Hide()
	{
		foreach (UIElement element in elements)
		{
			UIManager.HideUiElement(element.elementName);
		}
		DisableButtons();
	}
	
	public void EnableButtons()
	{
		foreach (UIButton button in buttons)
			button.EnableButton();
	}
	
	public void DisableButtons()
	{
		foreach (UIButton button in buttons)
			button.DisableButton();
	}
}
