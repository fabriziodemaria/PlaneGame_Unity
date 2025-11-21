using UnityEngine;
using System.Collections;

/// <summary>
/// Generic object spawner that instantiates prefabs at regular intervals
/// with randomized horizontal positions. Assigns proper Z-depth and sorting layers.
/// </summary>
public class ObjSpawner : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private GameObject inputPrefab;
	[SerializeField] private float frequency = 2f;
	[SerializeField] private float firstDelay = 1f;

	[Header("Layer Settings")]
	[SerializeField] private string sortingLayerName = LayerConstants.SORT_GAMEPLAY;
	[SerializeField] private int sortingOrder = 0;
	[SerializeField] private float zDepth = LayerConstants.Z_OBSTACLES;

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

		// Auto-detect layer settings from prefab name if not explicitly set
		if (sortingLayerName == LayerConstants.SORT_GAMEPLAY && sortingOrder == 0)
		{
			AssignLayerFromPrefabName();
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
		spawnPos.z = zDepth;

		// Instantiate the object
		GameObject spawnedObj = Instantiate(inputPrefab, spawnPos, transform.rotation);

		// Set proper sorting layer and order
		SpriteRenderer renderer = spawnedObj.GetComponent<SpriteRenderer>();
		if (renderer != null)
		{
			renderer.sortingLayerName = sortingLayerName;
			renderer.sortingOrder = sortingOrder;
		}
	}

	/// <summary>
	/// Automatically assigns appropriate sorting layer based on prefab name
	/// </summary>
	private void AssignLayerFromPrefabName()
	{
		string prefabName = inputPrefab.name.ToLower();

		if (prefabName.Contains("cloud"))
		{
			sortingLayerName = LayerConstants.SORT_ENVIRONMENT;
			sortingOrder = LayerConstants.ORDER_CLOUDS;
			zDepth = LayerConstants.Z_CLOUDS;
		}
		else if (prefabName.Contains("bird"))
		{
			sortingLayerName = LayerConstants.SORT_ENVIRONMENT;
			sortingOrder = LayerConstants.ORDER_BIRDS;
			zDepth = LayerConstants.Z_CLOUDS;
		}
		else if (prefabName.Contains("meteorite") || prefabName.Contains("meteor"))
		{
			sortingLayerName = LayerConstants.SORT_GAMEPLAY;
			sortingOrder = LayerConstants.ORDER_OBSTACLES;
			zDepth = LayerConstants.Z_OBSTACLES;
		}
		else if (prefabName.Contains("wrench") || prefabName.Contains("tank"))
		{
			sortingLayerName = LayerConstants.SORT_GAMEPLAY;
			sortingOrder = LayerConstants.ORDER_COLLECTIBLES;
			zDepth = LayerConstants.Z_COLLECTIBLES;
		}
	}
}

