using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishGeneral : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator animator;
    protected Renderer FishRend;
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float scale = 5f;
    protected Vector3Int StartPos;
    protected Vector3 CalcStarPos;
    protected MapManager Map;
    protected bool Ready = false;
    protected float tc = 0;
    [SerializeField] protected float rad = 2;

    private void Start() {
        FishRend = GetComponent<Renderer> ();
    }

    protected void Swim() {
        if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime > 1 && !animator.IsInTransition (0))
        {
            animator.SetTrigger ("swim");
        }
    }
    protected void GameManagerOnGameStateChanged(GameState state) {
        if (state == GameState.Game)
        {
            Map = FindObjectOfType <MapManager> ();
            StartPos = new Vector3Int (Random.Range (0, Map.GetWidth ()), 0, Random.Range(0, Map.GetHeight ()));
            transform.position = new Vector3((StartPos.x+0.5f)*scale, 0.5f*scale, (StartPos.z+0.5f)*scale);
            CalcStarPos = transform.position;
            
        }
    }

    protected void Circle() {
        Swim ();
        tc += Time.deltaTime*speed;
        float x = Mathf.Cos (tc)*rad;
        float z = Mathf.Sin (tc)*rad;
        transform.position =  CalcStarPos + new Vector3 (x, 0, z);
        transform.rotation = Quaternion.LookRotation (CalcStarPos - transform.position);
    }
    
}
