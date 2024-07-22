using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARAnchorManager))]
public class MyARDrawManager : MonoBehaviour
{
    [SerializeField]
    private LineSettings lineSettings = null;

    //[SerializeField]
    //private ARAnchorManager anchorManager = null;

    [SerializeField]
    private Camera arCamera = null;

    public GameObject cubePrefab = null;

    //private List<ARAnchor> anchors = new List<ARAnchor>();

    private Dictionary<int, ARLine> Lines = new Dictionary<int, ARLine>();

    private bool CanDraw { get; set; }

    void Update()
    {
#if !UNITY_EDITOR
        DrawOnTouch();
#endif
    }

    public void AllowDraw(bool isAllow)
    {
        CanDraw = isAllow;
    }


    void DrawOnTouch()
    {
        if (!CanDraw) return;

        int tapCount = Input.touchCount > 1 && lineSettings.allowMultiTouch ? Input.touchCount : 1;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, lineSettings.distanceFromCamera));

            ARDebugManager.Instance.LogInfo($"{touch.fingerId}");

            if (touch.phase == TouchPhase.Began)
            {
                //OnDraw?.Invoke();

                //ARAnchor anchor = anchorManager.AddAnchor(new Pose(touchPosition, Quaternion.identity));
                //ARAnchor anchor = anchorManager.AddAnchor(new Pose(touchPosition, Quaternion.identity));
                //if (anchor == null) 
                //    Debug.LogError("Error creating reference point");
                //else 
                //{
                //    //anchors.Add(anchor);
                //    //ARDebugManager.Instance.LogInfo($"Anchor created & total of {anchors.Count} anchor(s)");
                //    ARDebugManager.Instance.LogInfo($"Anchor created");
                //}

                ARLine line = new ARLine(lineSettings);

                Lines.Add(touch.fingerId, line);
                //line.AddNewLineRenderer(transform, touchPosition);
                GameObject go = Instantiate(cubePrefab, touchPosition, transform.rotation);
                //line.AddNewCube(transform, touchPosition, go);
                go.AddComponent<ARAnchor>();
                go.transform.position = touchPosition;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Lines[touch.fingerId].AddPoint(touchPosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Lines.Remove(touch.fingerId);
            }
        }
    }

    GameObject[] GetAllLinesInScene()
    {
        return GameObject.FindGameObjectsWithTag("Line");
    }

    public void ClearLines()
    {
        GameObject[] lines = GetAllLinesInScene();
        foreach (GameObject currentLine in lines)
        {
            LineRenderer line = currentLine.GetComponent<LineRenderer>();
            Destroy(currentLine);
        }
    }
}