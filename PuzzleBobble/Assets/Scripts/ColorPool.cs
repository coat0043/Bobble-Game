using UnityEngine;

[CreateAssetMenu(menuName = "Utils", fileName = "ColorPool")]
public class ColorPool : ScriptableObject
{
    [SerializeField] private Color[] m_ColorPool;

    public uint GetPoolSize() { return (uint)m_ColorPool.Length; }
    public Color GetColor(uint index) { Debug.Assert(index < m_ColorPool.Length); return m_ColorPool[index]; }

}
