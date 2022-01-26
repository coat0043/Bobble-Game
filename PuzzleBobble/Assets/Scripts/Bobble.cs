using UnityEngine;

public struct Index 
{
    public int x, y; 
    public Index(int a_x, int a_y) { x = a_x; y = a_y; }
    public string ToDebugString() { return "(" + x + " ," + y + ")"; }
}

public class Bobble : MonoBehaviour
{
    public Index Index;
    public float Diameter;
    public uint ColorID;

    public void SetMaterialColor(Color color, uint colorID)
    {
        gameObject.GetComponent<MeshRenderer>().material.color =  color;
        ColorID = colorID;
    }
}