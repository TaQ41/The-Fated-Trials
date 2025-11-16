using PlayerFunctionsCore;
using UnityEngine;

public sealed class ThirdPersonCameraPivotChecker : MonoBehaviour
{
    public PlayerCamera playerCamera;
    public GameObject PivotObj;

    [Tooltip ("Should the placing method call on loop until this is disabled?")]
    public bool loopToggle = false;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    public void PlacePivotAtInstant()
    {
        if (playerCamera == null || PivotObj == null)
            throw new System.NullReferenceException("Either the p-camera or the pivot obj weren't set to a valid object for the test.");
        
        var position = playerCamera.PlayerPivot.transform.position + playerCamera.RelativePivot;
        PivotObj.transform.position = position;
    }

    public void LoopPivotCheck()
    {
        try { PlacePivotAtInstant(); }
        catch { return; }
        
        loopToggle = true;
        StartCoroutine(
            Loop()
        );
    }

    public void EndLoop() => loopToggle = false;

    private System.Collections.IEnumerator Loop()
    {
        while (loopToggle)
        {
            var position = playerCamera.PlayerPivot.transform.position + playerCamera.RelativePivot;
            PivotObj.transform.position = position;
            yield return null;
        }
    }
}
