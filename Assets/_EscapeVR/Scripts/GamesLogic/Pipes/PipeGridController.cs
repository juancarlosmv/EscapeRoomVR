using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndpointColor { Red, Green, Blue, Yellow, Cyan, Purple};

[System.Serializable]
public struct Endpoint
{
    public Vector3Int cell;
    public Vector3Int direction;
    public EndpointColor color;
    public bool active;
};

public class PipeGridController : MonoBehaviour
{
    enum GrabState : short { Grab, Ungrab, Grab2Ungrab, Ungrab2Grab };
    [SerializeField]
    Vector3Int dimensions;
    [SerializeField]
    float cellSize = 0.2f;
    [SerializeField]
    Endpoint[] entradas;
    [SerializeField]
    Endpoint[] salidas;
    short[,,] occupiedCells;
    Dictionary<short, GrabState> pipeState;
    Dictionary<short, PipeController> insidePipes; // inside, but not attached
    Dictionary<short, PipeController> attachedPipes;
    Dictionary<short, List<Vector3Int>> attachedPositions;
    Dictionary<short, List<Vector3Int>> attachedExits;

    void Start()
    {
        occupiedCells = new short[dimensions.x, dimensions.y, dimensions.z];
        for(int i=0; i<dimensions.x; i++) {
            for(int j=0; j<dimensions.y; j++) {
                for(int k=0; k<dimensions.z; k++) {
                    occupiedCells[i, j, k] = -1;
                }
            }
        }
        insidePipes = new Dictionary<short, PipeController>();
        attachedPipes = new Dictionary<short, PipeController>();
        attachedPositions = new Dictionary<short, List<Vector3Int>>();
        attachedExits = new Dictionary<short, List<Vector3Int>>();
        pipeState = new Dictionary<short, GrabState>();
    }


    void Update()
    {
        List<short> toRemove = new List<short>();

        // loop over pipes inside
        foreach(KeyValuePair<short, PipeController> kv in insidePipes)
        {
            GrabableObj gro = kv.Value.gameObject.GetComponent<GrabableObj>();
            // if change from grab to ungrab
            if (pipeState[kv.Key] == GrabState.Grab && gro.GetGrabber() == null)
                pipeState[kv.Key] = GrabState.Grab2Ungrab;
            if (pipeState[kv.Key] == GrabState.Grab2Ungrab)
            {
                // change pipe state to ungrab
                pipeState[kv.Key] = GrabState.Ungrab;
                // compute destinyP and destinyR
                Vector3 destinyP = GetTargetPosition(kv.Value.gameObject.transform.position);
                Quaternion destinyR = GetTargetRotation(kv.Value.gameObject.transform.rotation);
                // compute new occupied cells
                List<Vector3Int> newCells = GetNewCells(destinyP, destinyR, kv.Value.Positions, true);
                List<Vector3Int> newExits = GetNewCells(destinyP, destinyR, kv.Value.Exits, false);
                // if can fit
                if (CanFit(newCells))
                {
                    // set attached, destinyP and destinyR in PipeController
                    kv.Value.Attach(destinyP, destinyR);
                    // change also occupied cells setting the new occupied cells to the pipe id
                    foreach (Vector3Int v in newCells) occupiedCells[v.x, v.y, v.z] = kv.Key;
                    // move from insidePipes to attachedPipes with the pipe id
                    attachedPipes[kv.Key] = insidePipes[kv.Key];
                    toRemove.Add(kv.Key); // do not delete immediatly because we are inside a loop
                    // initialize new lists
                    attachedPositions[kv.Key] = new List<Vector3Int>();
                    attachedExits[kv.Key] = new List<Vector3Int>();
                    // add the new occupied positions to attachedPositions with the pipe id
                    foreach (Vector3Int v in newCells) attachedPositions[kv.Key].Add(v);
                    // add the exits to attached exists with the pipe id
                    foreach (Vector3Int v in newExits) attachedExits[kv.Key].Add(v);
                    // set distance grab and kinematic to false
                    gro.IsDistanceGrabbable = false;
                    gro.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    // Recompute correct paths
                    Refresh();
                }
            }
        }
        foreach(short s in toRemove) insidePipes.Remove(s);
        toRemove.Clear();

        // loop over pipes attached
        foreach (KeyValuePair<short, PipeController> kv in attachedPipes)
        {
            GrabableObj gro = kv.Value.gameObject.GetComponent<GrabableObj>();
            // if change from ungrab to grab
            if (pipeState[kv.Key] == GrabState.Ungrab && gro.GetGrabber() != null)
                pipeState[kv.Key] = GrabState.Ungrab2Grab;
            if (pipeState[kv.Key] == GrabState.Ungrab2Grab)
            {
                // change pipe state to grab
                pipeState[kv.Key] = GrabState.Grab;
                // set attached to false
                kv.Value.Detach();
                // free occupied cells with -1 using attachedPostitions and pipe id
                foreach (Vector3Int v in attachedPositions[kv.Key]) occupiedCells[v.x, v.y, v.z] = -1;
                // move from attachedPipes to insidePipes
                insidePipes[kv.Key] = attachedPipes[kv.Key];
                toRemove.Add(kv.Key); // do not delete immediatly because we are inside a loop
                // delete from attachedPositions
                attachedPositions.Remove(kv.Key);
                // delete from attachedExits
                attachedExits.Remove(kv.Key);
                // Reset distance grabbable to original value
                gro.IsDistanceGrabbable = gro.DG;
                // Recompute correct paths
                Refresh();
            }
        }
        foreach (short s in toRemove) attachedPipes.Remove(s);
    }

    private void OnTriggerEnter(Collider other)
    {
        PipeController pc = other.gameObject.GetComponent<PipeController>();
        if(pc != null)
        {
            GrabableObj grabable = pc.gameObject.GetComponent<GrabableObj>();
            // Puede ser ungrab porque lo puedes lanzar hacia el grid, no meterlo cogido
            pipeState[pc.id] = grabable.GetGrabber() != null ? GrabState.Grab : GrabState.Ungrab;
            insidePipes[pc.id] = pc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PipeController pc = other.gameObject.GetComponent<PipeController>();
        if (pc != null)
        {
            pipeState.Remove(pc.id);
            insidePipes.Remove(pc.id);
        }
    }

    Vector3 GetTargetPosition(Vector3 initialPosition)
    {
        // relative distance vector to grid (0,0,0) point in world space
        Vector3 position = initialPosition - transform.position;
        // rotate distance vector obtain it in grid space
        position = Quaternion.Inverse(transform.rotation) * position;
        // align to grid cell centers in grid space
        position.x = ToGridPositionF(position.x);
        position.y = ToGridPositionF(position.y);
        position.z = ToGridPositionF(position.z);
        // inverse transform to get world space again
        position = transform.rotation * position;
        position = position + transform.position;
        return position;
    }

    Quaternion GetTargetRotation(Quaternion initialRotation)
    {
        // en grados
        Vector3 eulerObject = (Quaternion.Inverse(transform.rotation) * initialRotation).eulerAngles;
        // transform the relative rotation into multiples of 90
        eulerObject.x = ToGridRotationF(eulerObject.x);
        eulerObject.y = ToGridRotationF(eulerObject.y);
        eulerObject.z = ToGridRotationF(eulerObject.z);
        // return to world space
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(eulerObject);
        return targetRotation;
    }

    List<Vector3Int> GetNewCells(Vector3 destinyP, Quaternion destinyR, List<Vector3Int> positions, bool isPoint)
    {
        List<Vector3> floatPositions = new List<Vector3>();
        List<Vector3Int> newCells = new List<Vector3Int>();
        // convertir cada posicion con la rotacion
        // sumar cada posicion convertida a la posicion de destino
        foreach (Vector3Int position in positions)
        {
            floatPositions.Add((destinyR * position) * cellSize);
            if (isPoint) floatPositions[floatPositions.Count - 1] += destinyP;
        }
        // create cell indices
        for (int i=0; i< floatPositions.Count; i++)
        {
            // transform world space positions to grid space positions
            if (isPoint) floatPositions[i] = floatPositions[i] - transform.position;
            floatPositions[i] = Quaternion.Inverse(transform.rotation) * floatPositions[i];
            // convert float positions in integer positions using the cell size
            newCells.Add(new Vector3Int(
                ToGridPositionI(floatPositions[i].x),
                ToGridPositionI(floatPositions[i].y),
                ToGridPositionI(floatPositions[i].z)));
        }
        return newCells;
    }

    float ToGridPositionF(float val)
    {
        val = val + cellSize / 2.0f;
        // We do not care about negative values since they are out of the grid
        if (val < 0) val = -cellSize;
        else val = val - val % cellSize;
        return val;
    }

    int ToGridPositionI(float val)
    {
        int ind;
        val = val + cellSize / 2.0f;
        // We do not care about negative values since they are out of the grid
        // so we can return -1 always. Exits are unitary vectors, so this shouls be
        // valid in that case too
        if (val < 0.0f) ind = -1;
        else ind = Mathf.RoundToInt((val - val % cellSize) / cellSize);
        return ind;
    }

    float ToGridRotationF(float val)
    {
        if (val < 0)
        {
            int aux = -(int)val;
            aux = (int)(aux / 360.0f) + 1;
            val += 360.0f * aux;
        }
        val = val + 45.0f;
        val = val - val % 90.0f;
        return val;
    }

    bool InsideGrid(Vector3Int v)
    {
        return v.x >= 0 && v.x < dimensions.x &&
            v.y >= 0 && v.y < dimensions.y &&
            v.z >= 0 && v.z < dimensions.z;
    }

    bool CellFull(Vector3Int v)
    {
        return occupiedCells[v.x, v.y, v.z] >= 0;
    }

    bool CanFit(List<Vector3Int> positions)
    {
        foreach(Vector3Int v in positions)
        {
            if (!InsideGrid(v) || CellFull(v)) return false;
        }
        return true;
    }

    void Refresh()
    {
        for(int i=0; i<salidas.Length; i++)
        {
            salidas[i].active = false;
        }
        for (int i = 0; i < entradas.Length; i++)
        {
            HashSet<short> visited = new HashSet<short>();
            DFS(entradas[i].cell - entradas[i].direction, entradas[i].direction, entradas[i].color, ref visited);
        }
    }

    void DFS(Vector3Int cell, Vector3Int direction, EndpointColor color, ref HashSet<short> visited)
    {
        // If the dfs is in an exit cell
        for (int i = 0; i < salidas.Length; i++)
        {
            if (EqualVecI(salidas[i].cell, cell) && Dot(direction, salidas[i].direction) < -0.1f && salidas[i].color == color)
            {
                salidas[i].active = true;
                return;
            }
        }

        cell = cell + direction;
        if (!InsideGrid(cell)) return;
        short id = occupiedCells[cell.x, cell.y, cell.z];
        // No pipe here
        if (id == -1) return;
        // Pipe already visited
        if (visited.Contains(id)) return;
        int ind = GetCellInd(cell, id);
        // If this pipe does not match with the direction passed
        if (Dot(direction, attachedExits[id][ind]) > -0.1f) return;
        
        visited.Add(id);
        // If the dfs is in a new pipe
        for (int i=0; i<attachedPositions[id].Count; i++)
        {
            Vector3Int dir = attachedExits[id][i];
            if(Dot(dir, dir) > 0.1f)
            {
                Vector3Int pos = attachedPositions[id][i];
                DFS(pos, dir, color, ref visited);
            }
        }
    }

    bool EqualVecI(Vector3Int v1, Vector3Int v2)
    {
        return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
    }

    float Dot(Vector3Int v1, Vector3Int v2)
    {
        return (float)(v1.x * v2.x + v1.y * v2.y + v1.z * v2.z);
    }

    int GetCellInd(Vector3Int cell, short id)
    {
        for(int i=0; i<attachedPositions[id].Count; i++)
        {
            if (EqualVecI(cell, attachedPositions[id][i])) return i;
        }
        return -1;
    }

    public bool PathOk(int ind)
    {
        if (ind >= salidas.Length) return false;
        return salidas[ind].active;
    }
}
