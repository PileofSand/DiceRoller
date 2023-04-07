using System.Linq;
using TMPro;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro m_textMesh;

    public int SideValue { get; set; }

    public void Setup(int value)
    {
        SideValue = value;
        m_textMesh.text = value.ToString();
    }
}