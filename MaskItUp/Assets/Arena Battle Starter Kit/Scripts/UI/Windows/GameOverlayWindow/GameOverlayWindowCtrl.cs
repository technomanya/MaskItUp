using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverlayWindowCtrl : ClosableWindowCtrl 
{
    public static GameOverlayWindowCtrl instance;

    /* component refs */
    public JoystickComponentCtrl NavigatorJoystickComponent;
    public JoystickComponentCtrl AttackerJoystickComponent;

    private void Awake()
    {
        instance = this;
    }

    public override void Init()
    {
        base.Init();
    }

    private void Update()
    {
        if (SceneController.instance.player != null)
        {
            if (NavigatorJoystickComponent.Horizontal != 0 || NavigatorJoystickComponent.Vertical != 0)
            {
                Vector3 moveVector = (Vector3.right * NavigatorJoystickComponent.Horizontal + Vector3.forward * NavigatorJoystickComponent.Vertical);
                moveVector.Normalize();
                SceneController.instance.player.Move(moveVector);
            }
            else
                SceneController.instance.player.StopMove();

            if (AttackerJoystickComponent.Horizontal != 0 || AttackerJoystickComponent.Vertical != 0)
            {
                Vector3 directionVector = (Vector3.right * AttackerJoystickComponent.Horizontal + Vector3.forward * AttackerJoystickComponent.Vertical);
                directionVector.Normalize();
                SceneController.instance.player.Aim(directionVector);
            }
            else
                SceneController.instance.player.StopAim();
        }
    }

    public void OnTapAttackerJoystickComponent()
    {
        if (SceneController.instance.player != null)
            SceneController.instance.player.Attack();
    }

}
