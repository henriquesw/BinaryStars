using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	private MeshFilter[] meshFilter;
	private Mesh mesh;
	private List<Vector3> points;

	public bool enableGeneration;
	public float circleRay;
	public float quantidadeDePontos;

	// Use this for initialization
	void Start () {
		meshFilter = GetComponents<MeshFilter> ();
		mesh = new Mesh ();
		points = new List<Vector3> ();
		quantidadeDePontos = (2*circleRay)/0.01f;
		enableGeneration = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (enableGeneration) {
			points = circleGenerator ();
			meshFilter[0].mesh = generateMesh (points); 
			enableGeneration = false;
		}
	}

	public Mesh generateMesh(List<Vector3> points) {

		int size = points.Count;
		List<Vector3> vertices = new List<Vector3> ();
		//Remove ultimo ponto para ser adicionado no final
		Vector3 lastPoint = points[points.Count-1];
		points.RemoveAt (points.Count-1);

		vertices = points;
		int count = 0;

		//Rotaciona os pontos
		for (int rotation = 1; rotation < 13; rotation++) {
			for (int i = 1; i < size-1; i++) {
				Vector3 vertice = Quaternion.AngleAxis (15 * rotation, Vector3.right) * vertices [i];
				vertices.Add (vertice);
				count++;
			}	 
		}
		vertices.Add (lastPoint);
		//Debug.Log ("Ultimo ponto: " + lastPoint);
		//Duplica pontos para face externa
		int semiCount = vertices.Count;
		for (int i = 0; i < semiCount; i++) {
			vertices.Add (vertices[i]);
		}

		mesh.vertices = vertices.ToArray ();

		List<int> triangles = new List<int> ();
		int jump = (size - 2);
		//Face interna
		//Ponta da mesh
		for (int i = 0; i < 12; i++) {
			triangles.Add (0);
			triangles.Add ((i*jump) + 1);
			triangles.Add ((i+1)*jump + 1);

			triangles.Add (semiCount-1);
			triangles.Add ((i+2)*jump);
			triangles.Add ((i+1)*jump);
		}
		//Meio da mesh
		for (int i = 1; i < ((vertices.Count)/2 - size); i++) {
			if (i % jump != 0) {
				triangles.Add (i);
				triangles.Add (i + 1);
				triangles.Add (i + jump);

				triangles.Add (i + 1);
				triangles.Add (i + 1 + jump);
				triangles.Add (i + jump);
			}
		}

		//Face externa
		int semiTriangles = triangles.Count;
		//Ponta da mesh
		for (int i = 0; i < 12; i++) {
			triangles.Add (semiCount);
			triangles.Add (semiCount+(i+1)*jump + 1);
			triangles.Add (semiCount+(i*jump) + 1);

			triangles.Add (vertices.Count-1);
			triangles.Add (semiCount+(i+1)*jump);
			triangles.Add (semiCount+(i+2)*jump);
		}
		//Meio da mesh
		for (int i = semiCount; i < (vertices.Count - size); i++) {
			if (i % jump != 2) {
				triangles.Add (i);
				triangles.Add (i + jump);
				triangles.Add (i + 1);

				triangles.Add (i + 1);
				triangles.Add (i + jump);
				triangles.Add (i + 1 + jump);
			}
		}

		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();

		return mesh;
	}

	public List<Vector3> circleGenerator () {
		
		float x;
		float y;
		float jump = (2 * circleRay) / quantidadeDePontos;
		List<Vector3> points = new List<Vector3> ();
		//Gera pontos
		for (x = -circleRay; x <= circleRay; x+=jump) {
			y = Mathf.Sqrt (Mathf.Pow (circleRay, 2) - Mathf.Pow (x, 2));
			points.Add (new Vector3(x, y, 0));
		}
		Debug.Log ("Quantidade pontos: " + points.Count);
		return points;
	}

}
