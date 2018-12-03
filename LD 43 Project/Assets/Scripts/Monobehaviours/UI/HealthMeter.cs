using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMeter : MonoBehaviour {

    public RectTransform HeartPrefab;
    public float heartSpacing = 20f;

    private List<RectTransform> hearts;

    private PlayerController player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();

        hearts = new List<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        // need to add more
        while (player.hp > hearts.Count) {
            RectTransform heart = Instantiate(HeartPrefab, transform, false);
            heart.localPosition = hearts.Count * heartSpacing * Vector3.right;
            hearts.Add(heart);
        }

        while (player.hp < hearts.Count && hearts.Count > 0) {
            // need to remove some
            RectTransform heart = hearts[hearts.Count - 1];
            Destroy(heart.gameObject);
            hearts.RemoveAt(hearts.Count - 1);
        }
	}
}
