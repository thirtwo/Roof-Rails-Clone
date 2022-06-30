using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private float swerveInput = 0;
    [SerializeField] private float maxSwerveAmount = 1;
    public float SwerveInput { get { return Mathf.Clamp(swerveInput,-maxSwerveAmount,maxSwerveAmount); } }
    void Update()
    {
        MobileControl();
//#if UNITY_EDITOR
//        EditorControl();
//#else
//        MobileControl();
//#endif
    }

    private void MobileControl()
    {
        if (Input.touchCount <= 0) return;
        if (!GameManager.isGameStarted) GameManager.StartGame();
        var touch = Input.GetTouch(0);
        swerveInput = touch.deltaPosition.x;
    }
}
