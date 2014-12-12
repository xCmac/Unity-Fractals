using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{
    private int depth;
	private Material[,] materials;
	private static Vector3[] childDirections = { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
	private static Quaternion[] childOrientations = {
				Quaternion.identity,
				Quaternion.Euler (0f, 0f, -90f),
				Quaternion.Euler (0f, 0f, 90f),
				Quaternion.Euler(90f, 0f, 0f),
				Quaternion.Euler(-90f, 0f, 0f)
		};


    public Mesh[] meshes;
	public Material material;
	public int maxDepth;
	public float childScale;
	public float spawnProbability;

	// Use this for initialization
	void Start ()
	{
		if (materials == null)
		{
			InitializeMaterials();
		}

	    AddMeshAndMaterialComponents ();

	    if (depth < maxDepth)
	    {
			StartCoroutine(CreateChildren());
	    }
	}

	void Update()
	{
		transform.Rotate (0f, 30f * Time.deltaTime, 0f);
	}

	void AddMeshAndMaterialComponents ()
	{
		gameObject.AddComponent<MeshFilter> ().mesh = meshes [Random.Range (0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = materials [depth, Random.Range (0, 2)];
	}

	private void Initialize (Fractal parent, int childIndex)
	{
		meshes = parent.meshes;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		spawnProbability = parent.spawnProbability;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections [childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations [childIndex];
	}

	private IEnumerator CreateChildren ()
	{
		for (int i = 0; i < childDirections.Length; i++) 
		{
			if(Random.value < spawnProbability)
			{
				yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
				new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
			}
		}
	}

	private void InitializeMaterials()
	{
		materials = new Material[maxDepth +1, 2];
		for(int i = 0; i <=maxDepth; i++)
		{
			float t = i / (maxDepth -1f);
			t *=t;
			materials[i, 0] = new Material(material);
			materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
			materials[i, 1] = new Material(material);
			materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
		}
		materials[maxDepth, 0].color = Color.magenta;
		materials[maxDepth, 1].color = Color.red;
	}
}
