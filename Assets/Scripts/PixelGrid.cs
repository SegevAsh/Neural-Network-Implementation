using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelGrid : MonoBehaviour
{
    public Vector2Int gridSize;
    List<Vector3> gridPoints;
    [SerializeField] GameObject pixel;
    [SerializeField] AIScript AI;
    Vector3 pos;

    private void Start()
    {
        pos = transform.position;

        // initialise all the points in the grid for later use
        gridSize = new Vector2Int(100, 100);
        gridPoints = new List<Vector3>();
        Vector3 nextPoint = new Vector3(-5f, -5f, 0f);
        Vector3 tempPos = transform.position;
        tempPos.x += 0.05f;
        tempPos.y += 0.1f;

        for (int i = 0; i < 10000; i++)
        {
            if (nextPoint.x > 5f)
            {
                nextPoint.x = -5f;
                nextPoint.y += 0.1f;
            }

            gridPoints.Add(nextPoint + tempPos);

            nextPoint.x += 0.1f;
        }

        //StartCoroutine(BeginRefreshingGrid());
    }

    IEnumerator BeginRefreshingGrid()
    {
        for (int i = 0; i < 20; i++)
        {
            RefreshGrid();
            yield return new WaitForSeconds(2f);
        }
    }

    public void RefreshGrid()
    {
        Network network = AI.currentBestNetwork;

        // delete previous grid
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }


        List<GameObject> children = new List<GameObject>();

        // make new grid
        foreach (Vector3 point in gridPoints)
        {
            children.Add(Instantiate(pixel, point, Quaternion.identity, transform));
        }

        // decide if shown shape is circle or rectangle
        int circleOrRectangle = Random.Range(0, 2);

        // circle
        if (circleOrRectangle == 0)
        {
            float radius = Random.Range(1f, 4f);
            Vector3 circlePosition = new Vector3(Random.Range(-5f + radius, 5f - radius), Random.Range(-5f + radius, 5f - radius), 0f);
            circlePosition += transform.position;

            //for (int i = 0; i < transform.childCount; i++)
            //{
            //    children.Add(transform.GetChild(i).gameObject);
            //}

            List<GameObject> pixelsToDelete = new List<GameObject>();
            int num = 0;
            
            foreach (GameObject pixel in children)
            {
                if (Vector3.Distance(pixel.transform.position, circlePosition) < radius)
                {
                    pixelsToDelete.Add(pixel);
                    network.layers[0].neurons[num].value = 1;
                }
                else
                {
                    
                    network.layers[0].neurons[num].value = 0;
                }
                num++;
            }
            for (int i = pixelsToDelete.Count - 1; i > 0; i--)
            {
                Destroy(pixelsToDelete[i]);
            }
        }

        // rectangle
        if (circleOrRectangle == 1)
        {
            float length = Random.Range(1f, 4f);
            float height = Random.Range(1f, 4f);
            Vector3 rectanglePosition = new Vector3(Random.Range(-5f + length, 5f - length), Random.Range(-5f + height, 5f - height), 0f);
            rectanglePosition += transform.position;

            //for (int i = 0; i < transform.childCount; i++)
            //{
            //    children.Add(transform.GetChild(i).gameObject);
            //}

            List<GameObject> pixelsToDelete = new List<GameObject>();
            int num = 0;
            foreach (GameObject pixel in children)
            {
                if (Mathf.Abs(pixel.transform.position.x - rectanglePosition.x) < length && Mathf.Abs(pixel.transform.position.y - rectanglePosition.y) < height)
                {
                    pixelsToDelete.Add(pixel);
                    network.layers[0].neurons[num].value = 1;
                }
                else
                {
                    network.layers[0].neurons[num].value = 0;
                }
                num++;
            }
            for (int i = pixelsToDelete.Count; i > 0; i--)
            {
                Destroy(pixelsToDelete[i - 1]);
            }
        }
    }

    float GetFloatInRange(System.Random random, double min, double max)
    {
        return (float)(min + (random.NextDouble() * (max - min)));
    }

    public int MakeInvisibleGrid(Network network)
    {
        var rnd = new System.Random();
        int circleOrRectangle = rnd.Next(2);

        if (circleOrRectangle == 0)
        {
            float radius = GetFloatInRange(rnd, 1f, 4f);
            Vector3 circlePosition = new Vector3(GetFloatInRange(rnd, -5f + radius, 5f - radius), GetFloatInRange(rnd, -5f + radius, 5f - radius), 0f);
            circlePosition += pos;

            int num = 0;
            foreach (Vector3 gridPoint in gridPoints)
            {
                if (Vector3.Distance(gridPoint, circlePosition) < radius)
                {
                    network.layers[0].neurons[num].value = 1;
                }
                else
                {
                    network.layers[0].neurons[num].value = 0;
                }
                num++;
            }
        }

        if (circleOrRectangle == 1)
        {
            float length = GetFloatInRange(rnd, 1f, 4f);
            float height = GetFloatInRange(rnd, 1f, 4f);
            Vector3 rectanglePosition = new Vector3(GetFloatInRange(rnd, -5f + length, 5f - length), GetFloatInRange(rnd, -5f + height, 5f - height), 0f);
            rectanglePosition += pos;

            int num = 0;
            foreach (Vector3 gridPoint in gridPoints)
            {
                if (Mathf.Abs(gridPoint.x - rectanglePosition.x) < length && Mathf.Abs(gridPoint.y - rectanglePosition.y) < height)
                {
                    network.layers[0].neurons[num].value = 1;
                }
                else
                {
                    network.layers[0].neurons[num].value = 0;
                }
                num++;
            }
        }

        return circleOrRectangle;
    }
}
