using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoopCapture : MonoBehaviour
{
    // Public variables
    public float drawRadius = 0.01f;

    public Color drawColor = Color.white;

    public AudioClip loopClip;
    public AudioClip[] successiveClips;

    public AudioSource audioSource;

    // Private variables
    private bool isDrawing = false;

    private List<Vector3> linePoints = new List<Vector3>();

    private GameObject currentLineObject;

    private LineRenderer currentLineRenderer;

    private Vector3 previousPosition;

    private bool hasSelfIntersection = false;

    private int successiveLoops = 0;

    private List<Vector3> loopPoints;

    private GameObject[] targetObjects;

    private bool loopedATarget = false;

    void Start()
    {
    }

    void OnEnable() {
        targetObjects = GameObject.FindGameObjectsWithTag("Monster");
        audioSource.clip = loopClip;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
            audioSource.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
            audioSource.Stop();
        }
        if (isDrawing)
        {
            Draw();
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        linePoints.Clear();
        linePoints.Add(GetMouseWorldPosition());
        currentLineObject = new GameObject("Line");
        currentLineRenderer = currentLineObject.AddComponent<LineRenderer>();
        currentLineRenderer.positionCount = 0;
        currentLineRenderer.startWidth = drawRadius * 2f;
        currentLineRenderer.endWidth = drawRadius * 2f;
        currentLineRenderer.material =
            new Material(Shader.Find("Sprites/Default"));
        currentLineRenderer.startColor = drawColor;
        currentLineRenderer.endColor = drawColor;

        previousPosition = GetMouseWorldPosition();
        Draw();
    }

    void StopDrawing()
    {
        isDrawing = false;
        successiveLoops = 0;
        Destroy(currentLineObject);
    }

    void OnDisable() {
        StopDrawing();
    }

    void Draw()
    {
        Vector3 currentPosition = GetMouseWorldPosition();

        if (Vector3.Distance(currentPosition, previousPosition) > 0.1f)
        {
            linePoints.Add (currentPosition);
            currentLineRenderer.positionCount++;
            currentLineRenderer
                .SetPosition(currentLineRenderer.positionCount - 1,
                currentPosition);

            if (currentLineRenderer.positionCount > 2)
            {
                CheckSelfIntersection (currentPosition);
                if (hasSelfIntersection)
                {
                    foreach (GameObject targetObject in targetObjects)
                    {
                        if (IsPointInsideLoop(targetObject.transform.position))
                        {
                            loopedATarget = true;
                            successiveLoops++;
                            audioSource.PlayOneShot(successiveClips[Mathf.Min(successiveLoops - 1, 12)]);
                            Debug.Log("Target object is inside the loop!");
                            Debug.Log("Successive loops: " + successiveLoops);
                            gameObject.GetComponent<BattleManager>().TakeDamage(successiveLoops);
                            // Take appropriate action here
                        }
                    }
                    if (!loopedATarget)
                    {
                        successiveLoops = 0;
                        Debug.Log("Target object is outside the loop.");
                        Debug.Log("Successive loops: " + successiveLoops);
                        // Take appropriate action here
                    }
                    linePoints.Clear();
                    loopPoints.Clear();
                    currentLineRenderer.positionCount = 0;
                    hasSelfIntersection = false;
                    loopedATarget = false;
                }
            }

            previousPosition = currentPosition;
        }
    }

    void CheckSelfIntersection(Vector3 currentPosition)
    {
        Vector2[] linePositions =
            new Vector2[currentLineRenderer.positionCount];
        for (int i = 0; i < currentLineRenderer.positionCount; i++)
        {
            linePositions[i] =
                new Vector2(currentLineRenderer.GetPosition(i).x,
                    currentLineRenderer.GetPosition(i).y);
        }

        Vector2 currentPoint =
            new Vector2(currentPosition.x, currentPosition.y);
        for (int i = 0; i < linePositions.Length - 2; i++)
        {
            for (int j = i + 2; j < linePositions.Length - 1; j++)
            {
                if (
                    LineSegmentsIntersect(linePositions[i],
                    linePositions[i + 1],
                    linePositions[j],
                    linePositions[j + 1])
                )
                {
                    hasSelfIntersection = true;
                    loopPoints = new List<Vector3>();

                    // Add each point from the intersection point to the end of linePoints to loopPoints
                    for (int k = i; k < linePoints.Count; k++)
                    {
                        loopPoints.Add(linePoints[k]);
                    }
                    return;
                }
            }
        }
    }

    bool LineSegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        float denominator =
            ((p4.y - p3.y) * (p2.x - p1.x)) - ((p4.x - p3.x) * (p2.y - p1.y));

        if (denominator == 0) return false;

        float ua =
            (
            ((p4.x - p3.x) * (p1.y - p3.y)) - ((p4.y - p3.y) * (p1.x - p3.x))
            ) /
            denominator;
        float ub =
            (
            ((p2.x - p1.x) * (p1.y - p3.y)) - ((p2.y - p1.y) * (p1.x - p3.x))
            ) /
            denominator;

        if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1) return true;

        return false;
    }

    bool IsPointInsideLoop(Vector3 targetPosition)
    {
        if (loopPoints == null || loopPoints.Count < 3)
        {
            // Not enough points to form a loop
            return false;
        }

        int count = 0;
        Vector2 targetPoint = new Vector2(targetPosition.x, targetPosition.y);

        for (int i = 0; i < loopPoints.Count; i++)
        {
            Vector2 p1 = new Vector2(loopPoints[i].x, loopPoints[i].y);
            Vector2 p2 =
                new Vector2(loopPoints[(i + 1) % loopPoints.Count].x,
                    loopPoints[(i + 1) % loopPoints.Count].y);

            if (IsPointOnSegment(p1, p2, targetPoint))
            {
                // Target point lies on an edge of the loop
                return true;
            }

            if (
                ((p1.y > targetPoint.y) != (p2.y > targetPoint.y)) &&
                (
                targetPoint.x <
                (p2.x - p1.x) * (targetPoint.y - p1.y) / (p2.y - p1.y) + p1.x
                )
            )
            {
                count++;
            }
        }

        return count % 2 == 1;
    }

    bool IsPointOnSegment(Vector2 p1, Vector2 p2, Vector2 targetPoint)
    {
        float distance = Vector2.Distance(p1, p2);
        float dist1 = Vector2.Distance(p1, targetPoint);
        float dist2 = Vector2.Distance(p2, targetPoint);

        return Mathf.Approximately(dist1 + dist2, distance);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
