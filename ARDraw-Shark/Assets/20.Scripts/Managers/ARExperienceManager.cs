using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class ARExperienceManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnInitialized = null;

    [SerializeField]
    private UnityEvent OnRetarted = null;

    private ARPlaneManager arPlaneManager = null;

    private bool Initialized { get; set; }
    
    void Awake() 
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        //Anchor를 설정하기 위해서는 먼저 플레인이 검출되어야 한다.
        //플레인이 검출되면 planesChanged에 추가된 함수들이 실행된다.
        arPlaneManager.planesChanged += PlanesChanged;

        #if UNITY_EDITOR
            OnInitialized?.Invoke();
            Initialized = true;
            arPlaneManager.enabled = false;
        #endif
    }

    void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        if(!Initialized)
        {
            Activate();
        }
    }

    private void Activate()
    {
        ARDebugManager.Instance.LogInfo("Activate Experience");
        OnInitialized?.Invoke();
        Initialized = true;
        arPlaneManager.enabled = false;
    }

    public void Restart()
    {
        ARDebugManager.Instance.LogInfo("Restart Experience");
        OnRetarted?.Invoke();
        Initialized = false;
        arPlaneManager.enabled = true;
    }
}
