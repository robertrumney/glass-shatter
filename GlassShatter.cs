using UnityEngine;
using System.Collections;

public class GlassShatter : MonoBehaviour
{
    [SerializeField]
    private AudioClip glass;
    private bool once = false;

    public void ApplyDamage(float x)
    {
        if (!once)
        {
            AudioSource.PlayClipAtPoint(glass, transform.position, 0.01f);
            StartCoroutine(SplitMesh(true));
            once = true;
        }
    }

    public IEnumerator SplitMesh(bool destroy)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // If no MeshFilter or SkinnedMeshRenderer, do nothing.
        if (meshFilter == null && skinnedMeshRenderer == null)
        {
            yield break;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        Mesh M = meshFilter != null ? meshFilter.mesh : skinnedMeshRenderer.sharedMesh;

        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = renderer != null ? renderer.materials : new Material[0];

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;

        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {
            int[] indices = M.GetTriangles(submesh);

            for (int i = 0; i < indices.Length; i += 3)
            {
                // Create a new triangle
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];

                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh mesh = new Mesh
                {
                    vertices = newVerts,
                    normals = newNormals,
                    uv = newUvs,
                    triangles = new int[] { 0, 1, 2, 2, 1, 0 }
                };

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.AddComponent<MeshRenderer>().material = materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();

                // Apply explosion force
                Vector3 explosionPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f), Random.Range(-0.5f, 0.5f));
                GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300, 500), explosionPos, 5);

                // Destroy the triangle after a random time
                Destroy(GO, 5 + Random.Range(0.0f, 5.0f));
            }
        }

        // Disable the renderer after splitting
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Wait for 2 seconds before destroying the object
        yield return new WaitForSeconds(2.0f);

        if (destroy)
        {
            Destroy(gameObject);
        }
    }
}
