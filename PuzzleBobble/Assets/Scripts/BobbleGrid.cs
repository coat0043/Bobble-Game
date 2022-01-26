using System.Collections.Generic;
using UnityEngine;

public class BobbleGrid : MonoBehaviour
{
    [SerializeField] private GameObject m_BobblePrefab;
    [SerializeField] private Vector3 m_Offset;
    [SerializeField] private uint m_Rows;
    [SerializeField] private uint m_Columns;
    [SerializeField] private ColorPool m_ColorPool;

    private Bobble[] m_BobbleGrid;

    private DFS m_DFS = new DFS();

    private void Awake()
    {
        Debug.Assert(m_BobblePrefab != null);
        Debug.Assert(m_Rows != 0);
        Debug.Assert(m_Columns != 0);
        Debug.Assert(m_ColorPool.GetPoolSize() > 0);

        m_BobbleGrid = new Bobble[11 * m_Columns]; //Adding some extra rows.

        //Initializing Grid.
        for(int y = 0; y < m_Rows; y++)
        {
            for(int x = 0; x < m_Columns; x++)
            {
                Bobble bobble = GameObject.Instantiate(m_BobblePrefab).GetComponent<Bobble>();
                
                bobble.transform.position = new Vector3(x * bobble.Diameter, -y * bobble.Diameter, 0.0f) + m_Offset;
                bobble.Index.x = x;
                bobble.Index.y = y;

                bobble.name = "[" + GetGridIndex(bobble.Index) + "]" + bobble.Index.ToDebugString(); //For Debug

                uint randomColorIndex = (uint)Random.Range(0, (int)m_ColorPool.GetPoolSize());
                bobble.SetMaterialColor(m_ColorPool.GetColor(randomColorIndex), randomColorIndex);

                bobble.gameObject.transform.parent = gameObject.transform;

                m_BobbleGrid[GetGridIndex(bobble.Index)] = (bobble);
            }
        }
    }

    public void UpdateGrid(Index newIndexToAdd, uint colorID)
    {
        AttachBobble(newIndexToAdd, colorID);
        RemoveCluster(m_BobbleGrid[GetGridIndex(newIndexToAdd)]);
        
        Debug.Assert(!CheckForWin(), "You Win!");
    }

    private bool AttachBobble(Index index, uint colorID)
    {
        Debug.Assert(index.y < 10, "GameOver!!");

        if(GetGridIndex(index) >= m_BobbleGrid.Length) { return false; }

        Bobble bobble = GameObject.Instantiate(m_BobblePrefab).GetComponent<Bobble>();
        bobble.Index = index;
        bobble.transform.position = new Vector3(index.x * bobble.Diameter, -index.y * bobble.Diameter, 0.0f) + m_Offset;

        bobble.name = "[" + GetGridIndex(bobble.Index) + "]" + bobble.Index.ToDebugString(); //For Debug

        bobble.SetMaterialColor(m_ColorPool.GetColor(colorID), colorID);

        bobble.gameObject.transform.parent = gameObject.transform;

        m_BobbleGrid[GetGridIndex(bobble.Index)] = bobble;
        
        return true;
    }

    private bool RemoveCluster(Bobble startPoint)
    {
        List<Index> clustertoRemove = m_DFS.GetClusterToRemove(m_BobbleGrid, startPoint);

        if(clustertoRemove.Count < 3) { return false; }

        foreach(Index index in clustertoRemove)
        {
            if (m_BobbleGrid[GetGridIndex(index)] != null)
            {
                Destroy(m_BobbleGrid[GetGridIndex(index)].gameObject);
                m_BobbleGrid[GetGridIndex(index)] = null;
            }
        }

        return true;
    }

    bool CheckForWin()
    {
        foreach (Bobble bobble in m_BobbleGrid)
        {
            if (bobble != null) { return false; }
        }
        return true;
    }
    public int GetGridIndex(Index index) { return (int)(index.x + (index.y * m_Columns)); }
}