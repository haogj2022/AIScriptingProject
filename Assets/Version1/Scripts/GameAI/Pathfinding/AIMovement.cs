using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI.PathFinding;

public class AIMovement : MonoBehaviour
{
    public float Speed = 5.0f;
    Queue<Vector2> aiWayPoints = new Queue<Vector2>();

    PathFinder<Vector2Int> aiPathFinder = new AStarPathFinder<Vector2Int>();

    public PathFinderStatus GetStatus()
    {
        return aiPathFinder.Status;
    }

    // Start is called before the first frame update
    void Start()
    {
        aiPathFinder.onSuccess = OnSuccessPathFinding;
        aiPathFinder.onFailure = OnFailurePathFinding;
        aiPathFinder.HeuristicCost = GridVisualize.GetManhattanCost;
        aiPathFinder.NodeTraversalCost = GridVisualize.GetEuclideanCost;

        StartCoroutine(Coroutine_MoveTo());
    }

    public void AddWayPoint(Vector2 pt)
    {
        aiWayPoints.Enqueue(pt);
    }

    public IEnumerator Coroutine_MoveTo()
    {
        while (true)
        {
            while (aiWayPoints.Count > 0)
            {
                yield return StartCoroutine(
                  Coroutine_MoveToPoint(
                    aiWayPoints.Dequeue(),
                    Speed));
            }
            yield return null;
        }
    }

    // coroutine to move smoothly
    private IEnumerator Coroutine_MoveOverSeconds(
      GameObject objectToMove,
      Vector3 endPos,
      float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position =
              Vector3.Lerp(startingPos, endPos, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = endPos;
    }

    IEnumerator Coroutine_MoveToPoint(Vector2 position, float speed)
    {
        Vector3 endPos = new Vector3(position.x, position.y, transform.position.z);
        float duration = (transform.position - endPos).magnitude / speed;
        yield return StartCoroutine(
          Coroutine_MoveOverSeconds(
            transform.gameObject,
            endPos,
            duration));
    }

    public void SetDestination(
    GridVisualize map,
    GridCell destination)
    {
        //// we do not have pathfinding yet, so
        //// we just add the destination as a waypoint.
        //AddWayPoint(destination.Value);
        // Now we have a pathfinder.
        if (aiPathFinder.Status == PathFinderStatus.RUNNING)
        {
            //Debug.Log("Pathfinder already running. Cannot set destination now");
            return;
        }
        // remove all waypoints from the queue.
        aiWayPoints.Clear();
        // new start location is previous destination.
        GridCell start = map.GetGridCell(
          (int)transform.position.x,
          (int)transform.position.y);
        if (start == null) return;
        aiPathFinder.Initialize(start, destination);
        StartCoroutine(Coroutine_FindPathSteps());
    }

    IEnumerator Coroutine_FindPathSteps()
    {
        while (aiPathFinder.Status == PathFinderStatus.RUNNING)
        {
            aiPathFinder.Step();
            yield return null;
        }
    }

    void OnSuccessPathFinding()
    {
        PathFinder<Vector2Int>.PathFinderNode node = aiPathFinder.CurrentNode;
        List<Vector2Int> reverse_indices = new List<Vector2Int>();
        while (node != null)
        {
            reverse_indices.Add(node.Location.Value);
            node = node.Parent;
        }
        for (int i = reverse_indices.Count - 1; i >= 0; i--)
        {
            AddWayPoint(new Vector2(reverse_indices[i].x, reverse_indices[i].y));
        }
    }

    void OnFailurePathFinding()
    {
        //Debug.Log("Error: Cannot find path");
    }
}
