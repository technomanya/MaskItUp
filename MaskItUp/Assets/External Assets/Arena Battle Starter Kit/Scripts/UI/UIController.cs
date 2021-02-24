using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    public static UIController instance;

    /* prefabs */
    public GameOverlayWindowCtrl GameOverlayWindow;
    public LoadingWindowCtrl LoadingWindow;


    /* public vars */
    public GameObject Design;
    public GameObject WindowOverlay;

    void Awake () {
        instance = this;
        this.Design.SetActive(false);

        this.ShowLoadingWindow();
    }
	
	void Update () {
	    
	}

    public void ShowWindow(ClosableWindowCtrl windowPrefab)
    {
        GameObject inst = Utils.CreateInstance(windowPrefab.gameObject, this.WindowOverlay, true);
        ClosableWindowCtrl win = inst.GetComponent<ClosableWindowCtrl>();

        win.Init();
    }

    public void CloseWindow(ClosableWindowCtrl window)
    {
        Destroy(window.gameObject);
    }


    public void ShowLoadingWindow()
    {
        this.ShowWindow(this.LoadingWindow);
    }

    public void ShowGameOverlayWindow()
    {
        this.ShowWindow(this.GameOverlayWindow);

    }
}
