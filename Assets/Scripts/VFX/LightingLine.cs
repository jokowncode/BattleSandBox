using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingLine : MonoBehaviour
{
    
    LineRenderer lineRenderer;
    [SerializeField] private int vertexCount = 10;
    [SerializeField] private float noiseRange = 0.2f;
    [SerializeField] private float dur = 0.2f;
    
    // [SerializeField]private Transform StartPos;
    // [SerializeField]private Transform TargetPos;
    
    
    private Vector3 startPos;
    private Vector3 targetPos;

    public Vector3 StartPos { get => startPos; set => startPos = value; }
    public Vector3 TargetPos { get => targetPos; set => targetPos = value; }

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        targetPos = this.transform.position;    
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = vertexCount;
        StartCoroutine(Lighting());
    }

    // Update is called once per frame
    void Update()
    {
        TargetPos = this.transform.position;
        timer += Time.deltaTime;
    }

    IEnumerator Lighting()
    {
        while (timer <= 1)
        {
            float distance = Vector3.Distance(TargetPos, StartPos);
            float distanceDir = distance / lineRenderer.positionCount;
            Vector3 dir = (TargetPos - StartPos).normalized;
            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 vertexPos =  StartPos + dir * distanceDir * i;
                Vector3 noise = new Vector3(Random.Range(-noiseRange, noiseRange),
                    Random.Range(-noiseRange, noiseRange), Random.Range(-noiseRange, noiseRange));
                vertexPos += noise;
                lineRenderer.SetPosition(i, vertexPos);
            }
            yield return new WaitForSeconds(dur);

        }
        gameObject.SetActive(false);
    }
}
