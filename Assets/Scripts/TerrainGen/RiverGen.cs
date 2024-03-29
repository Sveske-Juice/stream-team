using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class RiverGen : MonoBehaviour
{
    public static event Action<BezierKnot> NewPieceSpawned;
    public Transform boat;
    public float spawnNewDistThreshold = 15f;
    public SplineContainer sc;
    public BezierKnot prevKnot;
    float timer = 0;
    float cooldown = 30;
    float curveAmount = 5;
    int idx = 0;

    // Start is called before the first frame update
    void Awake()
    {
        sc = GetComponent<SplineContainer>();
    }

    private void Start()
    {
        BezierKnot startPoint = new BezierKnot(new Vector3(4f, 1f, 3.5f-10f));
        sc.Spline.Add(startPoint);
        prevKnot = startPoint;
        BezierKnot secondPoint = new BezierKnot(new Vector3(4f, 1f, 13.5f));
        sc.Spline.Add(secondPoint);
        prevKnot = secondPoint;
    }

    bool ShouldSpawnNewPiece()
    {
        float dist = Vector3.Distance(sc.Spline.Last().Position, boat.position);
        return dist <= spawnNewDistThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ShouldSpawnNewPiece()) return;
        BezierKnot nextKnot;
        if (idx == 0)
        {
            nextKnot = new BezierKnot(new Vector3(prevKnot.Position.x + 1f, prevKnot.Position.y, prevKnot.Position.z + 1f));
        }
        else if (idx < 4)
        {
            nextKnot = new BezierKnot(new Vector3(prevKnot.Position.x + 5, prevKnot.Position.y, prevKnot.Position.z + 5));
        }
        else
        {
            nextKnot = new BezierKnot(new Vector3(prevKnot.Position.x + UnityEngine.Random.Range(-curveAmount, curveAmount), prevKnot.Position.y, prevKnot.Position.z + 10));
        }
        // sc.AddSpline(new Spline());

        sc.Spline.Add(nextKnot);
        NewPieceSpawned?.Invoke(nextKnot);

        // Connect the knots of each spline
        // sc.KnotLinkCollection.Link(new SplineKnotIndex(sc.Splines.Count - 1, 1), new SplineKnotIndex(sc.Splines.Count, 0));

        prevKnot = nextKnot;
        timer = 0; //Reset time
        idx++;

        if (idx > 10)
            sc.Spline.RemoveAt(0);
    }
}
