using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressPanelCtrl : MonoBehaviour {

	/* component references */
	public RectTransform ProgressFiller;
	public TextMeshProUGUI Text;
    public TextMeshProUGUI PercentageText;

    public float fillerFullWidth = 225.0f;

	/* private variables */


	private void Awake()
	{
	}

	public void SetText(string text)
	{
		Text.text = text;
	}

    public void SetProgress(float val)
	{
		val = Mathf.Clamp(val, 0.0f, 1.0f);
		var newWidth = fillerFullWidth * val;
		Vector2 sizeDelta = ProgressFiller.sizeDelta;
		sizeDelta.x = newWidth;
		ProgressFiller.sizeDelta = sizeDelta;

        if(this.PercentageText != null)
            this.PercentageText.text = ((int)(val * 100)).ToString() + "%";

    }
}
