using UnityEngine;

public class QuestPanel : MonoBehaviour
{

    Animator QuestPanelAnimator;

	private void Awake()
    {
        QuestPanelAnimator = GetComponent<Animator>();
    }

    public void WrongAnswer()
    {
        QuestPanelAnimator.SetTrigger("WrongAnswer");
	}
}
