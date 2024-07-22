using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]

public class MyARExperienceManager : MonoBehaviour
{
    //[SerializeField]
    //private UnityEvent OnInitialized = null;

    //[SerializeField]
    //private UnityEvent OnRetarted = null;

    [SerializeField]
    private Camera arCamera = null;

    public GameObject cubePrefab = null;
    public GameObject reticle;

    private ARPlaneManager arPlaneManager = null;
    MyARDrawManager myDraw;
    bool canDraw = false;

    private bool Initialized { get; set; }

    void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        myDraw = GetComponent<MyARDrawManager>();
        //Anchor�� �����ϱ� ���ؼ��� ���� �÷����� ����Ǿ�� �Ѵ�.
        //�÷����� ����Ǹ� planesChanged�� �߰��� �Լ����� ����ȴ�.
        arPlaneManager.planesChanged += PlanesChanged;
    }

    void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!Initialized)
        {
            Activate();
        }
    }

    private void Activate()
    {
        ARDebugManager.Instance.LogInfo("Activate Experience");
        //OnInitialized?.Invoke();
        Initialized = true;
        arPlaneManager.enabled = false;
        reticle.SetActive(false);
        //myDraw.AllowDraw(true);
        canDraw = true;
    }

    private void Update()
    {
        DrawOnTouch();   
    }

    void DrawOnTouch()
    {
        if (!canDraw) return;

        int tapCount = Input.touchCount;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 1));

            ARDebugManager.Instance.LogInfo($"{touch.fingerId}");

            if (touch.phase == TouchPhase.Began)
            {
                GameObject go = Instantiate(cubePrefab, touchPosition, transform.rotation);
                go.AddComponent<ARAnchor>();
                go.transform.position = touchPosition;
            }

        }
    }
}