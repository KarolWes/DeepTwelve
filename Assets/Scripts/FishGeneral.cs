using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public abstract class FishGeneral : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator animator;
    [FormerlySerializedAs("rad")][SerializeField] protected float radius = 2;
    [SerializeField] protected float speed = 1f;
    //[SerializeField] protected float scale = 5f;
    protected Vector3Int StartPosFixed;
    protected Vector3 StartPosRelative;
    protected MapManager Map;
    protected bool Ready = false;
    protected float Tc = 0;
    protected Renderer FishRend;

    private void Start()
    {
        FishRend = GetComponent<Renderer>();
    }

    protected void SwimAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            animator.SetTrigger("swim");
        }
    }
    protected void GameManagerOnGameStateChanged(GameState state)
    {
        if (state == GameState.Game)
        {
            Map = FindObjectOfType<MapManager>();
            
            StartPosFixed = new Vector3Int(Random.Range(1, Map.GetWidth()+1), 0, Random.Range(1, Map.GetHeight()+1));
            while (StartPosFixed == Map.GetEndPoint())
            {
                StartPosFixed = new Vector3Int(Random.Range(1, Map.GetWidth()+1), 0, Random.Range(1, Map.GetHeight()+1));
            }
            transform.position = new Vector3((StartPosFixed.x + 0.5f) * MapManager.Scale, 0.5f * MapManager.Scale, (StartPosFixed.z + 0.5f) * MapManager.Scale);
            StartPosRelative = transform.position;

        }
    }

    protected void Circle()
    {
        SwimAnimation();
        Tc += Time.deltaTime * speed;
        float x = Mathf.Cos(Tc) * radius;
        float z = Mathf.Sin(Tc) * radius;
        transform.position = StartPosRelative + new Vector3(x, 0, z);
        transform.rotation = Quaternion.LookRotation(StartPosRelative - transform.position);
        transform.rotation *= Quaternion.Euler(0, -90, 0);
    }

    protected void CalculateStartPosRelative()
    {
        StartPosRelative = (StartPosFixed + new Vector3(0.5f, 0.5f, 0.5f)) * MapManager.Scale;
    }

    protected void CalculateStartPosFixed()
    {
        StartPosFixed = Vector3Int.FloorToInt(StartPosRelative / MapManager.Scale - new Vector3(0.5f, 0.5f, 0.5f));
    }
}
