using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    //public Vector3[] DrawPos;
    public GameObject obj;
    public GameObject PooledBulletsParent;
    public float scaleFact;
    public List<GameObject> MagicBullets;
    public List<GameObject> PooledBullets;
    public bool shoot = false;
    public float LineRes = 1.0f;

    private float counter;

    /// <summary>
    /// Starting position of the created line
    /// </summary>
    Vector3 startPosition;

    /// <summary>
    /// GameObject of the created line
    /// </summary>
    GameObject currentLineObject;

    /// <summary>
    /// lineRenderer of the created line
    /// </summary>
    LineRenderer currentLineRenderer;

    /// <summary>
    /// Material of the drawn line.
    /// For my example I used a new Material with "UI/Default" shader
    /// </summary>
    public Material lineMaterial;

    /// <summary>
    /// thickness of the drawn line
    /// </summary>
    public float lineThickness;

    /// <summary>
    /// Canvas that you want to draw on
    /// </summary>
    public Canvas parentCanvas;

    void Start()
    {
        PooledBulletsParent = GameObject.FindWithTag("ObjectPool");
    }

    void Update()
    {
        if(Time.timeSinceLevelLoad-counter < 3)
        {
            foreach (var mb in MagicBullets)
            {
                Debug.Log("MB here maan");
                mb.transform.parent = null;
                mb.transform.Translate(Vector3.forward * 10* Time.deltaTime);
            }
            //MagicBullets = null;
        }
        else if (MagicBullets.Count != 0)
        {
            Debug.Log(MagicBullets.Count);
            foreach (var item in MagicBullets)
            {
                PooledBullets.Add(item);
                item.transform.parent = PooledBulletsParent.transform;
                item.SetActive(false);
                item.transform.localPosition = Vector3.zero;
            }

            MagicBullets = null;
            MagicBullets = new List<GameObject>();
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartDrawingLine();
        }
        else if (Input.GetMouseButton(0))
        {
            PreviewLine();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrawingLine();
        }
    }

    /// <summary>
    /// Returns the current mouseposition relative to the canvas.
    /// Modifies the z-value slightly so that stuff will be rendered in front of UI elements.
    /// </summary>
    /// <returns></returns>
    Vector3 GetMousePosition()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);
        Vector3 positionToReturn = parentCanvas.transform.TransformPoint(movePos);
        positionToReturn.z = parentCanvas.transform.position.z - 0.01f;
        return positionToReturn;
    }

    /// <summary>
    /// Starts drawing the line
    /// Creates a new GameObject at the startPosition and adds a LineRenderer to it
    /// The LineRenderer also gets modified with material and thickness here
    /// </summary>
    void StartDrawingLine()
    {
        startPosition = GetMousePosition();
        currentLineObject = new GameObject();
        currentLineObject.name = "Line Clone";
        currentLineObject.tag = "Cut";
        currentLineObject.transform.position = startPosition;
        currentLineRenderer = currentLineObject.AddComponent<LineRenderer>();
        currentLineRenderer.material = lineMaterial;
        currentLineRenderer.startWidth = lineThickness;
        currentLineRenderer.endWidth = lineThickness;
        currentLineRenderer.SetPosition(0,startPosition);
        currentLineRenderer.SetPosition(1, startPosition);
    }

    /// <summary>
    /// Updates the LineRenderer Positions
    /// </summary>
    void PreviewLine()
    {
        if(Vector3.Distance(currentLineRenderer.GetPosition(currentLineRenderer.positionCount-1), currentLineRenderer.GetPosition(currentLineRenderer.positionCount - 2)) > LineRes)
            currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount-1 ,GetMousePosition());
    }

    /// <summary>
    /// Cleans up the used variables as the LineRenderer will not be modified anymore
    /// </summary>
    void EndDrawingLine()
    {
        Vector3[] DrawPos = new Vector3[currentLineRenderer.positionCount];
        Debug.Log(currentLineRenderer.GetPositions(DrawPos));

        
        var tempObj = new GameObject();
        foreach (var pos in DrawPos)
        {
            if (PooledBullets.Count > 0)
            {
                PooledBullets[0].transform.parent = null;
                PooledBullets[0].transform.position = pos;
                PooledBullets[0].SetActive(true);
                MagicBullets.Add(PooledBullets[0]);
                PooledBullets.RemoveAt(0);
            }
            else if (PooledBullets.Count == 0)
            {
                tempObj = Instantiate(obj, pos, Quaternion.identity);
                obj.transform.localScale = Vector3.one * scaleFact;
                MagicBullets.Add(tempObj);
            }
             
        }

        startPosition = Vector3.zero;
        currentLineObject = null;
        currentLineRenderer = null;

        foreach (var item in GameObject.FindGameObjectsWithTag("Cut"))
        {
            Destroy(item);
        }

        counter = Time.timeSinceLevelLoad;
        StartCoroutine(ShootMagic());
    }

    IEnumerator ShootMagic()
    {
        yield return new WaitForSeconds(3);
        
        shoot = true;

        //MagicBullets = null;
    }
}
