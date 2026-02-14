using UnityEngine;
using System.Collections;

/// <summary>
/// Generic object spawner that instantiates prefabs at regular intervals
/// with randomized horizontal positions.
/// </summary>
public class ObjSpawner : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private GameObject inputPrefab;
	[SerializeField] private float frequency = 2f;
	[SerializeField] private float firstDelay = 1f;

	private float spawnTimer;
	private float spawnRangeX;

	void Start()
	{
		if (inputPrefab == null)
		{
			Debug.LogError($"ObjSpawner on {gameObject.name}: No prefab assigned!");
			enabled = false;
			return;
		}

		spawnTimer = firstDelay;

		// Cache spawn range based on camera orthographic size
		if (Camera.main != null)
		{
			spawnRangeX = Camera.main.orthographicSize / 2f;
		}
		else
		{
			Debug.LogWarning("ObjSpawner: Main camera not found, using default spawn range");
			spawnRangeX = 2.5f;
		}
	}

	void FixedUpdate()
	{
		spawnTimer -= Time.deltaTime;

		if (spawnTimer <= 0f)
		{
			spawnTimer = frequency;
			SpawnObject();
		}
	}

	private void SpawnObject()
	{
		// Calculate spawn position with random X offset
		Vector3 spawnPos = transform.position;
		spawnPos.x += Random.Range(-spawnRangeX, spawnRangeX);

		// Instantiate the object — prefab keeps its own sorting layer settings
		Instantiate(inputPrefab, spawnPos, transform.rotation);
	}
}

