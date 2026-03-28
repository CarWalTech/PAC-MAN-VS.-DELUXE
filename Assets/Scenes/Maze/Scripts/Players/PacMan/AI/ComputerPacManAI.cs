using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class ComputerPacManAI : PacManAI
{
    public GameObject pellets;
    public GameObject[] ghosts;

    public float retreatDistance = 3f;
    public float edibleChaseDistance = 12f;

    /* ============================================================
     * MOOD + RANDOMNESS
     * ============================================================ */

    public enum PacMood
    {
        Calm,
        Curious,
        Greedy,
        Scared,
        Chaotic
    }

    [Range(0f, 1f)]
    public float globalRandomness = 0.15f;

    public PacMood currentMood = PacMood.Calm;

    private float MoodRandomnessMultiplier()
    {
        switch (currentMood)
        {
            case PacMood.Calm: return 0.5f;
            case PacMood.Curious: return 1.0f;
            case PacMood.Greedy: return 0.8f;
            case PacMood.Scared: return 0.6f;
            case PacMood.Chaotic: return 2.0f;
            default: return 1f;
        }
    }

    private bool ShouldApplyRandomness(float baseChance)
    {
        float moodFactor = MoodRandomnessMultiplier();
        float finalChance = baseChance * moodFactor * globalRandomness;
        return Random.value < finalChance;
    }

    private bool AreGhostsEdible()
    {
        if (ghosts == null) return false;
        return ghosts.Any(x => x.GetComponent<Ghost>().isFrightened == true);
    }

    /* ============================================================
     * MAIN AI UPDATE
     * ============================================================ */
    protected override void InputUpdate()
    {
        Vector2 pos = transform.position;
        Vector2 nearestGhost = FindNearestHarmfulGhost();
        float ghostDist = Vector2.Distance(pos, nearestGhost);

        // If ghosts are edible → try to chase one
        if (AreGhostsEdible())
        {
            Vector2 edibleGhost = FindNearestEdibleGhost();
            if (edibleGhost != Vector2.zero && IsSafeToChaseGhost(edibleGhost))
            {
                ChaseTarget(edibleGhost);
                return;
            }
        }

        // Normal ghost avoidance
        if (ghostDist < retreatDistance)
        {
            EscapeFromGhost(nearestGhost);
            return;
        }

        // Power pellet priority when ghosts are near
        if (ghostDist < retreatDistance * 1.5f)
        {
            Vector2 powerPellet = FindNearestPowerPellet();
            if (powerPellet != Vector2.zero)
            {
                ChaseTarget(powerPellet);
                return;
            }
        }

        // Default: chase pellets
        ChaseTarget(FindNearestPellet());
    }


    /* ============================================================
     * TARGET CHASING (A* + MOOD RANDOMNESS)
     * ============================================================ */
    private void ChaseTarget(Vector2 targetPos)
    {
        if (targetPos == Vector2.zero)
            return;

        Vector2Int startCell = WorldToGrid(transform.position);
        Vector2Int goalCell = WorldToGrid(targetPos);

        // Try A*
        List<Vector2Int> path = AStar(startCell, goalCell);

        // If A* fails, fallback to a single-step neighbor list
        if (path == null || path.Count < 2)
        {
            path = ReconstructPath(null, startCell);
            if (path == null || path.Count < 2)
                return;
        }

        Vector2Int nextCell = path[1];
        Vector2Int delta = nextCell - startCell;

        // Desired direction in grid space
        Vector2 desiredDir = new Vector2(delta.x, delta.y);

        if (desiredDir == Vector2.zero)
            return;

        List<Vector2> possible = FindPossibleDirections();
        if (possible.Count == 0)
            return;

        // Remove reverse direction unless forced
        if (possible.Count > 1)
            possible.RemoveAll(d => d == -direction);

        // Pick the direction closest to desiredDir using dot product
        float bestScore = float.NegativeInfinity;
        Vector2 bestDir = Vector2.zero;

        foreach (var dir in possible)
        {
            float score = Vector2.Dot(desiredDir.normalized, dir.normalized);
            if (score > bestScore)
            {
                bestScore = score;
                bestDir = dir;
            }
        }

        // Mood-based randomness: sometimes pick the second-best direction
        if (possible.Count > 1 && ShouldApplyRandomness(0.25f))
        {
            var sorted = possible
                .OrderByDescending(d => Vector2.Dot(desiredDir.normalized, d.normalized))
                .ToList();

            if (sorted.Count > 1)
                bestDir = sorted[1];
        }

        SetInputDirection(bestDir);
    }

    private List<Vector2> FindPossibleDirections()
    {
        List<Vector2> dirs = new List<Vector2>();

        if (!DetectWallBorder(Vector2.up)) dirs.Add(Vector2.up);
        if (!DetectWallBorder(Vector2.down)) dirs.Add(Vector2.down);
        if (!DetectWallBorder(Vector2.left)) dirs.Add(Vector2.left);
        if (!DetectWallBorder(Vector2.right)) dirs.Add(Vector2.right);

        return dirs;
    }
    /* ============================================================
     * ESCAPE LOGIC (MOOD RANDOMNESS + JITTER)
     * ============================================================ */
    private void EscapeFromGhost(Vector2 ghostPos)
    {
        Vector2 pos = transform.position;
        Vector2 fleeDir = (pos - ghostPos).normalized;

        var possible = FindPossibleDirections();
        if (possible.Count == 0)
            return;

        float best = float.MaxValue;
        Vector2 bestDir = Vector2.zero;

        foreach (var dir in possible)
        {
            float dist = Vector2.Distance(fleeDir, dir);
            if (dist < best)
            {
                best = dist;
                bestDir = dir;
            }
        }

        // Mood-based jitter
        if (possible.Count > 1 && ShouldApplyRandomness(0.20f))
        {
            var sorted = possible
                .OrderBy(d => Vector2.Distance(fleeDir, d))
                .ToList();

            bestDir = sorted[Random.Range(0, Mathf.Min(2, sorted.Count))];
        }

        SetInputDirection(bestDir);
    }


    /* ============================================================
     * PELLET DETECTION (MOOD RANDOMNESS)
     * ============================================================ */
    private Vector2 FindNearestPellet()
    {
        Vector2 pacPos = transform.position;
        float best = float.MaxValue;
        Vector2 bestPellet = Vector2.zero;

        foreach (Transform pellet in pellets.transform)
        {
            if (!pellet.gameObject.activeSelf)
                continue;

            if (!pellet.GetComponent<Pellet>())
                continue;

            Vector2 pos = pellet.position;
            float dist = Vector2.Distance(pos, pacPos + direction);

            if (dist < best)
            {
                best = dist;
                bestPellet = pos;
            }
        }

        // Mood-based pellet randomness
        if (ShouldApplyRandomness(0.15f))
        {
            float radius = 3f;
            var nearby = new List<Vector2>();

            foreach (Transform pellet in pellets.transform)
            {
                if (!pellet.gameObject.activeSelf) continue;
                if (!pellet.GetComponent<Pellet>()) continue;

                if (Vector2.Distance(pellet.position, pacPos) < radius)
                    nearby.Add(pellet.position);
            }

            if (nearby.Count > 0)
                return nearby[Random.Range(0, nearby.Count)];
        }

        return bestPellet;
    }


    private Vector2 FindNearestPowerPellet()
    {
        Vector2 pacPos = transform.position;
        float best = float.MaxValue;
        Vector2 bestPellet = Vector2.zero;

        foreach (Transform pellet in pellets.transform)
        {
            if (!pellet.gameObject.activeSelf)
                continue;

            if (!pellet.GetComponent<PowerPellet>())
                continue;

            Vector2 pos = pellet.position;
            float dist = Vector2.Distance(pos, pacPos);

            if (dist < best)
            {
                best = dist;
                bestPellet = pos;
            }
        }

        return bestPellet;
    }


    /* ============================================================
     * GHOST DETECTION
     * ============================================================ */
    private Vector2 FindNearestHarmfulGhost()
    {
        Vector2 pacPos = transform.position;
        float best = float.MaxValue;
        Vector2 bestGhost = Vector2.zero;

        foreach (GameObject ghost in ghosts)
        {
            if (!ghost.activeSelf)
                continue;

            Ghost g = ghost.GetComponent<Ghost>();
            if (g == null || g.isFrightened)
                continue;

            Vector2 pos = ghost.transform.position;
            float dist = Vector2.Distance(pos, pacPos);

            if (dist < best)
            {
                best = dist;
                bestGhost = pos;
            }
        }

        return bestGhost;
    }


    private Vector2 FindNearestEdibleGhost()
    {
        if (!AreGhostsEdible())
            return Vector2.zero;

        Vector2 pacPos = transform.position;
        float best = float.MaxValue;
        Vector2 bestGhost = Vector2.zero;

        foreach (GameObject ghost in ghosts)
        {
            if (!ghost.activeSelf)
                continue;

            Ghost g = ghost.GetComponent<Ghost>();
            if (g == null || !g.isFrightened)
                continue;

            Vector2 pos = ghost.transform.position;
            float dist = Vector2.Distance(pos, pacPos);

            if (dist < best)
            {
                best = dist;
                bestGhost = pos;
            }
        }

        return bestGhost;
    }


    private bool IsSafeToChaseGhost(Vector2 ghostPos)
    {
        float dist = Vector2.Distance(transform.position, ghostPos);

        if (dist < 2f)
            return false;

        if (dist > edibleChaseDistance)
            return false;

        Vector2Int start = WorldToGrid(transform.position);
        Vector2Int goal = WorldToGrid(ghostPos);

        var path = AStar(start, goal);
        return path != null && path.Count > 1;
    }


    /* ============================================================
     * DEAD-END DETECTION
     * ============================================================ */
    private bool IsDeadEnd(Vector2Int cell)
    {
        int walkable = 0;

        foreach (var dir in directions)
        {
            if (IsWalkable(cell + dir))
                walkable++;
        }

        return walkable <= 1;
    }

    private static readonly Vector2Int[] directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };


    /* ============================================================
     * GRID HELPERS
     * ============================================================ */
    private Vector2Int WorldToGrid(Vector2 world)
    {
        return (Vector2Int)tilemap.WorldToCell(world);
    }

    private Vector2 GridToWorld(Vector2Int cell)
    {
        return tilemap.CellToWorld((Vector3Int)cell);
    }

    private bool IsWalkable(Vector2Int cell)
    {
        return !tilemap.HasTile((Vector3Int)cell);
    }


    /* ============================================================
     * A* PATHFINDING
     * ============================================================ */
    private List<Vector2Int> AStar(Vector2Int start, Vector2Int goal)
    {
        if (!IsWalkable(start) || !IsWalkable(goal))
            return null;

        var open = new PriorityQueue<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int?>();
        var gScore = new Dictionary<Vector2Int, float>();

        open.Enqueue(start, 0);
        cameFrom[start] = null;
        gScore[start] = 0;

        while (open.Count > 0)
        {
            Vector2Int current = open.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;

                if (!IsWalkable(next))
                    continue;

                float tentative = gScore[current] + 1;

                if (!gScore.ContainsKey(next) || tentative < gScore[next])
                {
                    gScore[next] = tentative;
                    float priority = tentative + Heuristic(next, goal);
                    open.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return null;
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }


    /* ============================================================
     * RANDOM FALLBACK PATH
     * ============================================================ */
    private List<Vector2Int> ReconstructPath(
        Dictionary<Vector2Int, Vector2Int?> cameFrom,
        Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        foreach (var dir in directions)
        {
            Vector2Int next = current + dir;
            if (IsWalkable(next))
                neighbors.Add(next);
        }

        if (neighbors.Count == 0)
            return new List<Vector2Int> { current };

        Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];

        return new List<Vector2Int> { current, chosen };
    }


    /* ============================================================
     * SIMPLE PRIORITY QUEUE
     * ============================================================ */
    private class PriorityQueue<T>
    {
        private readonly List<(T item, float priority)> elements = new();

        public int Count => elements.Count;

        public void Enqueue(T item, float priority)
        {
            elements.Add((item, priority));
        }

        public T Dequeue()
        {
            int best = 0;

            for (int i = 1; i < elements.Count; i++)
                if (elements[i].priority < elements[best].priority)
                    best = i;

            T item = elements[best].item;
            elements.RemoveAt(best);
            return item;
        }
    }
}
