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
    protected MapManager Map;
    protected bool Ready = false;
    protected float tc = 0;

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
            StartPos = new Vector3Int (Random.Range (0, Map.GetWidth ()), Random.Range (0, Map.GetHeight ()), 0);
        }
    }

    protected void Circle() {
        tc += Time.deltaTime*speed;
        float x = (Mathf.Cos (tc)/2+StartPos.x)*scale;
        float z = (Mathf.Sin (tc)/2+StartPos.y)*scale;
        float y = (0.5f+StartPos.z)*scale;
        transform.position = new Vector3 (x, y, z);
    }
    
}
