using UnityEngine;
using System;

public class CloseBinarySimulator
{

    private float primaryTemp;
    private float secondaryTemp;

    private bool rocheLobule;
    private bool particles;
    private bool axes;
    private bool labels;
    private bool orbit;

    public BinarySystemData data;
    private Table table;

    private GameObject redGiantVerticalLines;
    private GameObject redGiantHorizontalLines;
    private GameObject whiteDwarfVerticalLines;
    private GameObject whiteDwarfHorizontalLines;
    private GameObject redGiantTriangles;
    private GameObject whiteDwarfTriangles;
    private GameObject redGiantRings;
    private GameObject whiteDwarfRings;
    private GameObject particlesPath;
    private GameObject L1, L2, L3;

    public CloseBinarySimulator()
    {
        initializeObjects();
    }

    public void setSystem(int indice, int countParticles, float primaryTemp, float secondaryTemp)
    {
        this.primaryTemp = primaryTemp;
        this.secondaryTemp = secondaryTemp;

        data = new BinarySystemData((float)table.getC1()[indice],
            (float)table.getM()[indice],
            (float)table.getL1()[indice],
            (float)table.getL2()[indice],
            (float)table.getL3()[indice]);

        instanciateLagrangePoints();

        data.generateRedGiant();
        data.generateWhiteDwarf();
        data.generateAnimatedParticles();

        drawRedGiantRocheLobule();
        drawStarTrianglesGiant();
        drawStarRingsGiant();

        drawWhiteDwarfRocheLobule();
        drawStarTrianglesDwarf();
        drawStarRingsDwarf();

        drawParticles(countParticles);

        hideObjects();
    }

    private void initializeObjects()
    {
        L1 = new GameObject("L1");
        L2 = new GameObject("L2");
        L3 = new GameObject("L3");

        redGiantVerticalLines = instanciateObject(redGiantVerticalLines, "Red Giant Vertical Lines", "Red Giant");
        redGiantHorizontalLines = instanciateObject(redGiantHorizontalLines, "Red Giant Horizontal Lines", "Red Giant");
        whiteDwarfVerticalLines = instanciateObject(whiteDwarfVerticalLines, "White Dwarf Vertical Lines", "White Dwarf");
        whiteDwarfHorizontalLines = instanciateObject(whiteDwarfHorizontalLines, "White Dwarf Horizontal Lines", "White Dwarf");
        redGiantTriangles = instanciateObject(redGiantTriangles, "Red Giant Points", "Red Giant");
        whiteDwarfTriangles = instanciateObject(whiteDwarfTriangles, "White Dwarf Points", "White Dwarf");
        redGiantRings = instanciateObject(redGiantRings, "Red Giant Rings", "Red Giant");
        whiteDwarfRings = instanciateObject(whiteDwarfRings, "White Dwarf Rings", "White Dwarf");
        particlesPath = instanciateObject(particlesPath, "Particles Path", "White Dwarf");

        table = new Table();
    }

    private GameObject instanciateObject(GameObject obj, string name, string parent)
    {
        obj = null;
        obj = new GameObject(name);
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
		obj.GetComponent<MeshRenderer>().material = new Material(Shader.Find ("Vertex color unlit"));
        obj.transform.parent = GameObject.Find(parent).transform;

        return obj;
    }

    private void instanciateLagrangePoints()
    {
        L1.transform.position = new Vector3(data.getL1(), 0, 0);
        L1.transform.parent = GameObject.Find("LagrangePoints").transform;

        L2.transform.position = new Vector3(data.getL2(), 0, 0);
        L2.transform.parent = GameObject.Find("LagrangePoints").transform;

        L3.transform.position = new Vector3(data.getL3(), 0, 0);
        L3.transform.parent = GameObject.Find("LagrangePoints").transform;
    }

    private void hideObjects()
    {
        setRocheLobule(getRocheLobule());
        setParticles(getParticles());
        setAxes(getAxes());
        setLabels(getLabels());
        setOrbit(getOrbit());
        //setClockWise(getClockWise());
    }

    private void drawRedGiantRocheLobule()
    {
        Point p;
        /*
         * RocheRedGiant -- Vertical lines
         */
        int i = 0;
        Vector3[] vertices = new Vector3[585];
        int[] indices = new int[vertices.Length];

        for (int cont1 = 1; cont1 < data.getEquipotentialRocheRedGiant().Count - data.getSlices(); cont1 += data.getSlices())
        {
            for (int cont2 = 1; cont2 < data.getSlices(); cont2++)
            {
                p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + cont2 - 1];
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
                p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + cont2];
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
            }
            p = (Point)data.getEquipotentialRocheRedGiant()[cont1];
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
        }

        Mesh mesh = redGiantVerticalLines.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.LineStrip, 0);

        Color32[] colors = new Color32[vertices.Length];
        for (int j = 0; j < colors.Length; j++)
        {
            colors[j] = new Color(0.3f, 0, 0);
        }
        mesh.colors32 = colors;

        /*
         * RocheRedGiant -- Horizontal lines
         */
        i = 0;
        vertices = new Vector3[340];
        indices = new int[340];
        for (int cont1 = 1; cont1 < data.getSlices() + 1; cont1++)
        {
            p = (Point)data.getEquipotentialRocheRedGiant()[0];
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
            for (int cont2 = 1; cont2 < data.getEquipotentialRocheRedGiant().Count - data.getSlices(); cont2 += data.getSlices())
            {
                p = (Point)data.getEquipotentialRocheRedGiant()[cont2 + cont1 - 1];
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
            }
            p = (Point)data.getEquipotentialRocheRedGiant()[data.getSize() - 1];
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
        }

        mesh = redGiantHorizontalLines.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.LineStrip, 0);

        colors = new Color32[vertices.Length];
        for (int j = 0; j < colors.Length; j++)
        {
            colors[j] = new Color(0.3f, 0, 0);
        }
        mesh.colors32 = colors;
    }

    private void drawWhiteDwarfRocheLobule()
    {
        GameObject.Find("White Dwarf").GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.195f + (primaryTemp - 5) * 0.065f, 0.333f + (primaryTemp - 5) * 0.065f);

        Point p;

        /*
         * RocheWhiteDwarf -- Vertical lines
         */
        int i = 0;
        Vector3[] vertices = new Vector3[585];
        int[] indices = new int[vertices.Length];
        for (int cont1 = 1; cont1 < data.getEquipotentialRocheDwarf().Count - data.getSlices(); cont1 += data.getSlices())
        {
            for (int cont2 = 1; cont2 < data.getSlices(); cont2++)
            {
                p = (Point)data.getEquipotentialRocheDwarf()[cont1 + cont2 - 1];
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
                p = (Point)data.getEquipotentialRocheDwarf()[cont1 + cont2];
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
            }
            p = (Point)data.getEquipotentialRocheDwarf()[cont1];
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
        }

        Mesh mesh = whiteDwarfVerticalLines.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.LineStrip, 0);

        Color32[] colors = new Color32[vertices.Length];
        for (int j = 0; j < colors.Length; j++)
        {
            colors[j] = Color.red;
        }
        mesh.colors32 = colors;

        /*
         * RocheWhiteDwarf -- Horizontal lines
         */
        i = 0;
        vertices = new Vector3[357];
        indices = new int[vertices.Length];
        for (int cont1 = 1; cont1 < data.getSlices() + 1; cont1++)
        {
            p = (Point)data.getEquipotentialRocheDwarf()[0];
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
            for (int cont2 = 1; cont2 < data.getEquipotentialRocheDwarf().Count - data.getSlices(); cont2 += data.getSlices())
            {
                p = (Point)data.getEquipotentialRocheDwarf()[cont2 + cont1 - 1];
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
            }
            p = (Point)data.getEquipotentialRocheDwarf()[data.getSize() - 1];
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
        }

        mesh = whiteDwarfHorizontalLines.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.LineStrip, 0);

        colors = new Color32[vertices.Length];
        for (int j = 0; j < colors.Length; j++)
        {
            colors[j] = Color.red;
        }
        mesh.colors32 = colors;
    }

    private void drawStarTrianglesGiant()
    {
        Point p;

        /*
		 * Drawing points for the Red Giant near from the White Dwarf
		 */
        int j = 0;
        Vector3[] vertices = new Vector3[120];
        Color32[] colors = new Color32[vertices.Length];
        int[] indices = new int[vertices.Length];

        float numColorTempX;
        for (int i = 1; i <= data.getSlices() - 1; i++)
        {

            p = (Point)data.getEquipotentialRocheRedGiant()[0];
            numColorTempX = corTempRedGiant(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheRedGiant()[++i];
            numColorTempX = corTempRedGiant(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheRedGiant()[--i];
            numColorTempX = corTempRedGiant(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());
        }

        p = (Point)data.getEquipotentialRocheRedGiant()[0];
        numColorTempX = corTempRedGiant(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        p = (Point)data.getEquipotentialRocheRedGiant()[1];
        numColorTempX = corTempRedGiant(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        p = (Point)data.getEquipotentialRocheRedGiant()[data.getSlices()];
        numColorTempX = corTempRedGiant(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        /*
		 * Drawing points for the Red Giant far from the White Dwarf
		 */
        for (int k = (data.getSize() - 2); k >= (data.getSize() - data.getSlices()); k--)
        {

            p = (Point)data.getEquipotentialRocheRedGiant()[data.getSize() - 1];
            numColorTempX = corTempRedGiant(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheRedGiant()[--k];
            numColorTempX = corTempRedGiant(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheRedGiant()[++k];
            numColorTempX = corTempRedGiant(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());
        }

        p = (Point)data.getEquipotentialRocheRedGiant()[data.getSize() - 1];
        numColorTempX = corTempRedGiant(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        p = (Point)data.getEquipotentialRocheRedGiant()[data.getSize() - 2];
        numColorTempX = corTempRedGiant(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        p = (Point)data.getEquipotentialRocheRedGiant()[data.getSize() - data.getSlices() - 1];
        numColorTempX = corTempRedGiant(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        Mesh mesh = redGiantTriangles.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.colors32 = colors;

    }

    private void drawStarRingsGiant()
    {
        Point p;

        int i = 0;
        Vector3[] vertices = new Vector3[1120];
        Color32[] colors = new Color32[vertices.Length];
        int[] indices = new int[vertices.Length];

        int slices = (data.getSlices());
        float numColorTempX;
        for (int cont1 = 1; cont1 < data.getSize() - 2 * slices; cont1 += data.getSlices())
        {
            for (int cont2 = 1; cont2 < slices; cont2 += 1)
            {
                p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + cont2 - 1];
                numColorTempX = corTempRedGiant(p.getX());
                colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

                p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + cont2];
                numColorTempX = corTempRedGiant(p.getX());
                colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

                p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + slices + cont2];
                numColorTempX = corTempRedGiant(p.getX());
                colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

                p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + slices + cont2 - 1];
                numColorTempX = corTempRedGiant(p.getX());
                colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
            }

            p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + slices - 1];
            numColorTempX = corTempRedGiant(p.getX());
            colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheRedGiant()[cont1];
            numColorTempX = corTempRedGiant(p.getX());
            colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + slices];
            numColorTempX = corTempRedGiant(p.getX());
            colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheRedGiant()[cont1 + slices + slices - 1];
            numColorTempX = corTempRedGiant(p.getX());
            colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

        }

        Mesh mesh = redGiantRings.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Quads, 0);
        mesh.colors32 = colors;
    }

    private void drawStarTrianglesDwarf()
    {
        Point p;

        /*
		 * Drawing points for the Red Giant near from the White Dwarf
		 */
        int j = 0;
        Vector3[] vertices = new Vector3[120];
        Color32[] colors = new Color32[vertices.Length];
        int[] indices = new int[vertices.Length];

        float numColorTempX;
        for (int i = 1; i <= data.getSlices() - 1; i++)
        {

            p = (Point)data.getEquipotentialRocheDwarf()[0];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheDwarf()[++i];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheDwarf()[--i];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());
        }

        p = (Point)data.getEquipotentialRocheDwarf()[0];
        numColorTempX = corTempWhiteDwarf(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        p = (Point)data.getEquipotentialRocheDwarf()[1];
        numColorTempX = corTempWhiteDwarf(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        p = (Point)data.getEquipotentialRocheDwarf()[data.getSlices()];
        numColorTempX = corTempWhiteDwarf(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        /*
		 * Drawing points for the Red Giant far from the White Dwarf
		 */
        for (int k = (data.getSize() - 2); k >= (data.getSize() - data.getSlices()); k--)
        {

            p = (Point)data.getEquipotentialRocheDwarf()[data.getSize() - 1];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheDwarf()[--k];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheDwarf()[++k];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[j] = j;
            vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());
        }

        p = (Point)data.getEquipotentialRocheDwarf()[data.getSize() - 1];
        numColorTempX = corTempWhiteDwarf(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        p = (Point)data.getEquipotentialRocheDwarf()[data.getSize() - 2];
        numColorTempX = corTempWhiteDwarf(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        p = (Point)data.getEquipotentialRocheDwarf()[data.getSize() - data.getSlices() - 1];
        numColorTempX = corTempWhiteDwarf(p.getX());
        colors[j] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
        indices[j] = j;
        vertices[j++] = new Vector3(p.getX(), p.getY(), p.getZ());

        Mesh mesh = whiteDwarfTriangles.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.colors32 = colors;

    }

    private void drawStarRingsDwarf()
    {
        Point p;

        int i = 0;
        Vector3[] vertices = new Vector3[1120];
        Color32[] colors = new Color32[vertices.Length];
        int[] indices = new int[vertices.Length];

        int slices = (data.getSlices());
        float numColorTempX;
        for (int cont1 = 1; cont1 < data.getSize() - 2 * slices; cont1 += data.getSlices())
        {
            for (int cont2 = 1; cont2 < slices; cont2 += 1)
            {
                p = (Point)data.getEquipotentialRocheDwarf()[cont1 + cont2 - 1];
                numColorTempX = corTempWhiteDwarf(p.getX());
                colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

                p = (Point)data.getEquipotentialRocheDwarf()[cont1 + cont2];
                numColorTempX = corTempWhiteDwarf(p.getX());
                colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

                p = (Point)data.getEquipotentialRocheDwarf()[cont1 + slices + cont2];
                numColorTempX = corTempWhiteDwarf(p.getX());
                colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

                p = (Point)data.getEquipotentialRocheDwarf()[cont1 + slices + cont2 - 1];
                numColorTempX = corTempWhiteDwarf(p.getX());
                colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());
            }

            p = (Point)data.getEquipotentialRocheDwarf()[cont1 + slices - 1];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheDwarf()[cont1];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheDwarf()[cont1 + slices];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

            p = (Point)data.getEquipotentialRocheDwarf()[cont1 + slices + slices - 1];
            numColorTempX = corTempWhiteDwarf(p.getX());
            colors[i] = new Color(numColorTempX, 0.0f, 1 - numColorTempX);
            indices[i] = i;
            vertices[i++] = new Vector3(p.getX(), p.getY(), p.getZ());

        }

        Mesh mesh = whiteDwarfRings.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Quads, 0);
        mesh.colors32 = colors;
    }

    public void drawParticles(int countParticles)
    {
        Point p;

        Vector3[] vertices = new Vector3[countParticles];
        Color32[] colors = new Color32[countParticles];
        int[] indices = new int[countParticles];

        float numColorTempX;
        if (countParticles < data.getCaminho().Count)
        {
            for (int i = 0; i < countParticles; i++)
            {
                p = (Point)data.getCaminho()[i];
                numColorTempX = corTempParticles(p.getX(), p.getY());
                colors[i] = new Color(numColorTempX, 0.4f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i] = new Vector3(p.getX(), p.getZ(), p.getY());
            }
        }
        else
        {
            for (int i = 0; i < data.getCaminho().Count; i++)
            {
                p = (Point)data.getCaminho()[i];
                numColorTempX = corTempParticles(p.getX(), p.getY());
                colors[i] = new Color(numColorTempX, 0.4f, 1 - numColorTempX);
                indices[i] = i;
                vertices[i] = new Vector3(p.getX(), p.getZ(), p.getY());
            }
        }

        Mesh mesh = particlesPath.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
        mesh.colors32 = colors;
    }

    /*
     * Method used to modify the color of the secondary star from the
     * temperature variation along the axis x
     */
    public float corTempRedGiant(float valorX)
    {
        return -secondaryTemp / 100 * 5 + 0.3f + (valorX - data.getL1()) / (data.getL2() - data.getL1());
    }

    public float corTempWhiteDwarf(float valorX)
    {
        return -primaryTemp / 100 * 3 + 0.3f + (valorX - data.getL1()) / (data.getL3() - data.getL1());
    }

    public float corTempParticles(float valorX, float valorY)
    {
        return (float)(-primaryTemp / 100 * 2 + Math.Sqrt(Math.Pow(valorX, 2) + Math.Pow(valorY, 2)));
    }

    public bool getRocheLobule()
    {
        return this.rocheLobule;
    }

    public void setRocheLobule(bool rocheLobule)
    {
        this.rocheLobule = rocheLobule;

        if (this.rocheLobule)
        {
            redGiantVerticalLines.GetComponent<Renderer>().enabled = true;
            redGiantHorizontalLines.GetComponent<Renderer>().enabled = true;
            whiteDwarfVerticalLines.GetComponent<Renderer>().enabled = true;
            whiteDwarfHorizontalLines.GetComponent<Renderer>().enabled = true;
            whiteDwarfRings.GetComponent<Renderer>().enabled = true;
            whiteDwarfTriangles.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            redGiantVerticalLines.GetComponent<Renderer>().enabled = false;
            redGiantHorizontalLines.GetComponent<Renderer>().enabled = false;
            whiteDwarfVerticalLines.GetComponent<Renderer>().enabled = false;
            whiteDwarfHorizontalLines.GetComponent<Renderer>().enabled = false;
            whiteDwarfRings.GetComponent<Renderer>().enabled = false;
            whiteDwarfTriangles.GetComponent<Renderer>().enabled = false;
        }
    }

    public bool getAxes()
    {
        return this.axes;
    }

    public void setAxes(bool axes)
    {
        this.axes = axes;

        if (this.axes)
        {
            GameObject.Find("ConeGreen").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("ConeRed").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("ConeBlue").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("CylinderGreen").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("CylinderRed").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("CylinderBlue").GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            GameObject.Find("ConeGreen").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("ConeRed").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("ConeBlue").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("CylinderGreen").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("CylinderRed").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("CylinderBlue").GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public bool getLabels()
    {
        return this.labels;
    }

    public void setLabels(bool labels)
    {
        this.labels = labels;
    }

    public bool getOrbit()
    {
        return this.orbit;
    }

    public void setOrbit(bool orbit)
    {
        this.orbit = orbit;
    }

    //public bool getClockWise()
    //{
    //    return this.clockWise;
    //}

    //public void setClockWise(bool clockWise)
    //{
    //    this.clockWise = clockWise;
    //}

    public bool getParticles()
    {
        return this.particles;
    }

    public void setParticles(bool particles)
    {
        this.particles = particles;

        if (this.particles)
        {
            particlesPath.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            particlesPath.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
