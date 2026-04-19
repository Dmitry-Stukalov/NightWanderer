using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialManager
{
	private VisualElement _tutorialPanel;
	private VisualElement[] _tutorialElements;

	public TutorialManager(VisualElement tutorialPanel) 
	{
		_tutorialPanel = tutorialPanel;
		_tutorialElements = _tutorialPanel.Query<VisualElement>("Panel").ToList().ToArray();
	}

	public void OpenPanel(int id)
	{
		_tutorialElements[id].style.display = DisplayStyle.Flex;
		DOTween.To(() => _tutorialElements[id].resolvedStyle.opacity, x => _tutorialElements[id].style.opacity = x, 1, 2f);
	}

	public void ClosePanel(int id)
	{
		DOTween.To(() => _tutorialElements[id].resolvedStyle.opacity, x => _tutorialElements[id].style.opacity = x, 0, 2f).OnComplete(() => _tutorialElements[id].style.display = DisplayStyle.None);
	}
}
