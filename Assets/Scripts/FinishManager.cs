using System;
using UnityEngine;

public class FinishManager : MonoBehaviour
    {
        protected MapManager Map;
        // Start is called before the first frame update
        private void Awake() {
            GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
        
        }

        private void OnDestroy() {
            GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        private void GameManagerOnGameStateChanged(GameState state)
        {
            if (state == GameState.Game)
            {
                Map = FindObjectOfType<MapManager>();
                var pos = Map.GetEndPoint();
                transform.position = new Vector3((pos.x + 0.5f) * MapManager.Scale, 0.5f * MapManager.Scale, (pos.y + 0.5f) * MapManager.Scale);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                print("You won!");
            }
        }
    }
