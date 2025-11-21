using UnityEngine;

/// <summary>
/// Centralized constants for managing Z-depths and sorting orders in the game.
/// This ensures consistent layering across all visual elements.
/// </summary>
public static class LayerConstants
{
    // Z-Depth constants for 3D positioning
    // Lower values are further from camera (background)
    // Higher values are closer to camera (foreground)
    public const float Z_BACKGROUND = -10f;
    public const float Z_CLOUDS = -5f;
    public const float Z_OBSTACLES = 0f;
    public const float Z_COLLECTIBLES = 0f;
    public const float Z_PLAYER = 0f;
    public const float Z_PLAYER_EFFECTS = 0f; // Same Z as player, use sorting order instead
    public const float Z_UI_WORLD = 5f; // For world-space UI elements

    // Sorting Layer names (must be configured in Unity's Tags and Layers settings)
    public const string SORT_BACKGROUND = "Background";
    public const string SORT_ENVIRONMENT = "Environment";
    public const string SORT_GAMEPLAY = "Gameplay";
    public const string SORT_PLAYER = "Player";
    public const string SORT_EFFECTS = "Effects";
    public const string SORT_UI = "UI";

    // Sorting Orders within each layer
    // Background layer
    public const int ORDER_BACKGROUND_BASE = 0;

    // Environment layer (clouds, birds)
    public const int ORDER_CLOUDS = 0;
    public const int ORDER_BIRDS = 1;

    // Gameplay layer (obstacles, collectibles)
    public const int ORDER_OBSTACLES = 0;
    public const int ORDER_COLLECTIBLES = 1;

    // Player layer
    public const int ORDER_PLAYER_SHADOW = 0;
    public const int ORDER_PLAYER = 1;

    // Effects layer (explosions, particles)
    public const int ORDER_EFFECTS_BASE = 0;
    public const int ORDER_EFFECTS_FRONT = 10;

    // UI layer
    public const int ORDER_UI_ARROWS = 0;
    public const int ORDER_UI_LABELS = 10;

    /// <summary>
    /// Helper method to set both Z position and sorting layer/order for a sprite
    /// </summary>
    public static void SetSpriteLayer(SpriteRenderer renderer, float zDepth, string sortingLayer, int sortingOrder)
    {
        if (renderer == null)
        {
            Debug.LogWarning("SetSpriteLayer: SpriteRenderer is null");
            return;
        }

        Transform transform = renderer.transform;
        Vector3 pos = transform.position;
        pos.z = zDepth;
        transform.position = pos;

        renderer.sortingLayerName = sortingLayer;
        renderer.sortingOrder = sortingOrder;
    }

    /// <summary>
    /// Helper method to set Z position for a transform
    /// </summary>
    public static void SetZDepth(Transform transform, float zDepth)
    {
        if (transform == null)
        {
            Debug.LogWarning("SetZDepth: Transform is null");
            return;
        }

        Vector3 pos = transform.position;
        pos.z = zDepth;
        transform.position = pos;
    }
}
