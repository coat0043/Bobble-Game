using System.Collections.Generic;

public class DFS
{
    public Dictionary<Index, bool> m_IsBobbleVisited;
    public DFS() { m_IsBobbleVisited = new Dictionary<Index, bool>(); }

    public List<Index> GetClusterToRemove(Bobble[] bobbles, Bobble interestedBobble)
    {
        m_IsBobbleVisited.Clear();
        List<Index> openBobbles = new List<Index>();
        List<Index> m_cluster = new List<Index>();

        foreach (Bobble bobble in bobbles)
        {
            //For optimization, ingnores all the empty cells.
            if (bobble != null)
            {
                //For optimization, ignores all the bobbles with different colorID.
                if (interestedBobble.ColorID == bobble.ColorID)
                {
                    //Intializing all the relevant bobbles to unvisited.
                    m_IsBobbleVisited.Add(bobble.Index, false);
                }
            }
        }

        //For optimization, starts with the most recently shooted bobble.
        openBobbles.Add(interestedBobble.Index);
        
        while(openBobbles.Count > 0)
        {
            int indexToRemove = openBobbles.Count - 1;
            Index currentCoords = openBobbles[indexToRemove];
            openBobbles.RemoveAt(indexToRemove);
            
            m_IsBobbleVisited[currentCoords] = true;
            m_cluster.Add(currentCoords);

            AddCoordsIfNeeded(currentCoords, new Index( 1,  0), ref openBobbles);  //right
            AddCoordsIfNeeded(currentCoords, new Index(-1,  0), ref openBobbles);  //left
            AddCoordsIfNeeded(currentCoords, new Index( 0,  1), ref openBobbles);  //up 
            AddCoordsIfNeeded(currentCoords, new Index( 0, -1), ref openBobbles);  //down
        }

        return m_cluster;
    }
    void AddCoordsIfNeeded(Index coords, Index checkDir, ref List<Index> coordsToVisit)
    {
        Index nextCoords = new Index(coords.x + checkDir.x, coords.y + checkDir.y);

        if(m_IsBobbleVisited.ContainsKey(nextCoords)) //if the tile is relevant and valid.
        {
            if(!m_IsBobbleVisited[nextCoords]) //if the tile is not visited already.
            {
                coordsToVisit.Add(nextCoords);
            }
        }
    }
}
