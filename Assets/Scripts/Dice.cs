using System.Linq;
using TMPro;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public GameObject textPrefab; // Text prefab for displaying the numbers
    public float yOffset = 0.1f; // Distance between the dice and the text

    // Array of numbers to display on the dice
    private int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    void Start()
    {
        // Get the mesh of the dice
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Create the text objects for each face
        for (int i = 0; i < numbers.Length; i++)
        {
            // Find the center of the face by averaging the vertices
            int[] triangles = mesh.triangles.Where((tri, idx) => idx / 3 == i).ToArray();
            Vector3 center = Vector3.zero;
            for (int j = 0; j < triangles.Length; j++)
            {
                center += mesh.vertices[triangles[j]];
            }
            center /= triangles.Length;

            // Calculate the position for the text object
            Vector3 pos = transform.TransformPoint(center);
            pos.y += yOffset;

            // Create a new text object and set its position and rotation
            GameObject textObj = Instantiate(textPrefab, pos, Quaternion.identity);
            textObj.transform.parent = transform;

            // Get the normal of the face and calculate the rotation of the text object
            Vector3 normal = mesh.normals[triangles[0]];
            Vector3 forward = -normal;
            Vector3 up = Vector3.up;
            if (normal == up || normal == -up)
            {
                up = Vector3.forward;
            }
            Quaternion rotation = Quaternion.LookRotation(forward, up);

            // Set the rotation and text of the object
            textObj.transform.rotation = rotation;
            textObj.GetComponent<TextMeshPro>().text = numbers[i].ToString();
        }
    }
}