using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosableWindowCtrl : MonoBehaviour {

    /* object refs */
    public Animator WindowAnimator;

    /* public vars */
 
    virtual public void Init()
	{
        if (this.WindowAnimator != null)
            this.WindowAnimator.SetTrigger("show");
	}

	virtual public void Close()
	{
        if (this.WindowAnimator != null)
            this.WindowAnimator.SetTrigger("hide");
        else
            this.CloseEvent();

	}

    public void CloseEvent()
    {
        UIController.instance.CloseWindow(this);
    }

    virtual public void Hide()
    {
        gameObject.SetActive(false);
    }

    virtual public void Unhide()
    {
        gameObject.SetActive(true);
    }

  
}
