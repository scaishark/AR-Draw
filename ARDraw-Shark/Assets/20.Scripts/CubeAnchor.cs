using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARAnchorManager))]
public class CubeAnchor : MonoBehaviour
{
    public GameObject bulletSpawner;
    public GameObject reticle;

    [SerializeField]
    private Camera arCamera = null;

    ARPlaneManager arPlaneManager = null;
    bool isAnchored = false;

    // Start is called before the first frame update
    void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += BulletAnchorActivate;
    }

    void BulletAnchorActivate(ARPlanesChangedEventArgs args)
    {
        if (!isAnchored)
        {
            arPlaneManager.enabled = false;
            isAnchored = true;
            reticle.SetActive(false);
        }
    }

    private void Update()
    {
        //if (arPlaneManager.trackables.count < 2) return;
        GenerateSpawnerOnTouch();
    }

    void GenerateSpawnerOnTouch()
    {

        int tapCount = Input.touchCount;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0.3f));

            if (touch.phase == TouchPhase.Began)
            {
                GameObject go = Instantiate(bulletSpawner, touchPosition, transform.rotation);
                go.transform.parent = transform;
                go.AddComponent<ARAnchor>();
                go.transform.position = touchPosition;

                break;
            }
        }
    }
}


