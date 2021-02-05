using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingWindowCtrl : ClosableWindowCtrl 
{

    /* component refs */
    public ProgressPanelCtrl ProgressPanel;

    public override void Init()
    {
        this.StartCoroutine(_LoadScene());
        base.Init();
    }

    private void Update()
    {

    }

    private IEnumerator _LoadScene()
    {
        ProgressPanel.SetProgress(0f);
        yield return new WaitForSeconds(0.5f);

        //DO LOADING STUFFS YOU WANT

        ProgressPanel.SetProgress(0.3f);
        yield return new WaitForSeconds(0.2f);

        ProgressPanel.SetProgress(0.4f);
        yield return new WaitForSeconds(0.2f);

        ProgressPanel.SetProgress(0.8f);
        yield return new WaitForSeconds(0.2f);

        ProgressPanel.SetProgress(1.0f);
        yield return new WaitForSeconds(0.2f);

        SceneController.instance.InitStage();
        this.Close();
    }
}
