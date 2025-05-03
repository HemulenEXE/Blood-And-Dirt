using UnityEngine;

public class GlobalFPSLimiter
{
    [RuntimeInitializeOnLoadMethod]
    static void SetFPSLimit()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
}
