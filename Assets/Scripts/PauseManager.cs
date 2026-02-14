using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the pause menu. Builds UI programmatically so no scene setup is needed.
/// Attach to any GameObject, or let PlaneMovement auto-create it.
/// </summary>
public class PauseManager : MonoBehaviour
{
	public static PauseManager Instance { get; private set; }

	private bool isPaused = false;
	private float savedTimeScale = 1f;
	private bool isMuted = false;

	private GameObject pausePanel;
	private Text muteButtonText;
	private Font gameFont;
	private Color textColor = new Color(0.82f, 0.84f, 0.84f, 1f); // light gray matching main menu

	public bool IsPaused { get { return isPaused; } }

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		// Load mute preference
		isMuted = PlayerPrefs.GetInt("muted", 0) == 1;
		AudioListener.volume = isMuted ? 0f : 1f;

		// Load the game's pixel font from Resources
		gameFont = Resources.Load<Font>("8_bit_party");
		if (gameFont == null)
			Debug.LogWarning("PauseManager: 8_bit_party font not found in Resources, using fallback.");

		BuildPauseUI();
		pausePanel.SetActive(false);
	}

	void Update()
	{
		// Allow Escape key to toggle pause
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		if (isPaused)
			Resume();
		else
			Pause();
	}

	public void Pause()
	{
		if (isPaused) return;
		isPaused = true;
		savedTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		pausePanel.SetActive(true);
	}

	public void Resume()
	{
		if (!isPaused) return;
		isPaused = false;
		Time.timeScale = savedTimeScale;
		pausePanel.SetActive(false);
	}

	public void ToggleSound()
	{
		isMuted = !isMuted;
		AudioListener.volume = isMuted ? 0f : 1f;
		PlayerPrefs.SetInt("muted", isMuted ? 1 : 0);
		PlayerPrefs.Save();

		if (muteButtonText != null)
			muteButtonText.text = isMuted ? "UNMUTE SOUND" : "MUTE SOUND";
	}

	public void QuitToMenu()
	{
		isPaused = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	}

	// ─── UI Construction ───────────────────────────────────────────────

	private void BuildPauseUI()
	{
		// Canvas
		GameObject canvasObj = new GameObject("PauseCanvas");
		canvasObj.transform.SetParent(transform);

		Canvas canvas = canvasObj.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 999;

		CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.referenceResolution = new Vector2(1080, 1920);
		scaler.matchWidthOrHeight = 0.5f;

		canvasObj.AddComponent<GraphicRaycaster>();

		pausePanel = canvasObj;

		// Subtle dark overlay
		GameObject overlay = CreateImage(canvasObj.transform, "Overlay",
			new Color(0f, 0f, 0f, 0.35f), Vector2.zero, Vector2.one);

		// Semi-transparent gray bands
		Color bandColor = new Color(0.3f, 0.3f, 0.3f, 0.6f);

		// Title — large text, upper area
		CreateLabel(overlay.transform, "Title", "PAUSED", 80, FontStyle.Bold,
			textColor, new Vector2(0f, 0.70f), new Vector2(1f, 0.90f));

		// Resume button — full-width band
		CreateMenuButton(overlay.transform, "ResumeBtn", "RESUME",
			new Vector2(0f, 0.50f), new Vector2(1f, 0.60f),
			bandColor, Resume);

		// Mute / Unmute button — full-width band
		string muteLabel = isMuted ? "UNMUTE SOUND" : "MUTE SOUND";
		GameObject muteBtn = CreateMenuButton(overlay.transform, "MuteBtn", muteLabel,
			new Vector2(0f, 0.38f), new Vector2(1f, 0.48f),
			bandColor, ToggleSound);
		muteButtonText = muteBtn.GetComponentInChildren<Text>();

		// Quit button — full-width band
		CreateMenuButton(overlay.transform, "QuitBtn", "QUIT",
			new Vector2(0f, 0.26f), new Vector2(1f, 0.36f),
			bandColor, QuitToMenu);
	}

	private GameObject CreateImage(Transform parent, string name, Color color,
		Vector2 anchorMin, Vector2 anchorMax)
	{
		GameObject obj = new GameObject(name);
		obj.transform.SetParent(parent, false);

		Image img = obj.AddComponent<Image>();
		img.color = color;

		RectTransform rect = obj.GetComponent<RectTransform>();
		rect.anchorMin = anchorMin;
		rect.anchorMax = anchorMax;
		rect.offsetMin = Vector2.zero;
		rect.offsetMax = Vector2.zero;

		return obj;
	}

	private GameObject CreateLabel(Transform parent, string name, string content,
		int fontSize, FontStyle style, Color color, Vector2 anchorMin, Vector2 anchorMax)
	{
		GameObject obj = new GameObject(name);
		obj.transform.SetParent(parent, false);

		Text t = obj.AddComponent<Text>();
		t.text = content;
		if (gameFont != null)
			t.font = gameFont;
		else
		{
			t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			if (t.font == null)
				t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}
		t.fontSize = fontSize;
		t.fontStyle = style;
		t.color = color;
		t.alignment = TextAnchor.MiddleCenter;
		t.horizontalOverflow = HorizontalWrapMode.Overflow;
		t.verticalOverflow = VerticalWrapMode.Overflow;

		RectTransform rect = obj.GetComponent<RectTransform>();
		rect.anchorMin = anchorMin;
		rect.anchorMax = anchorMax;
		rect.offsetMin = Vector2.zero;
		rect.offsetMax = Vector2.zero;

		return obj;
	}

	private GameObject CreateMenuButton(Transform parent, string name, string label,
		Vector2 anchorMin, Vector2 anchorMax, Color bgColor,
		UnityEngine.Events.UnityAction onClick)
	{
		// Button background
		GameObject btnObj = CreateImage(parent, name, bgColor, anchorMin, anchorMax);

		Button btn = btnObj.AddComponent<Button>();
		btn.targetGraphic = btnObj.GetComponent<Image>();

		ColorBlock cb = btn.colors;
		cb.normalColor = Color.white;
		cb.highlightedColor = new Color(0.85f, 0.85f, 0.85f);
		cb.pressedColor = new Color(0.65f, 0.65f, 0.65f);
		btn.colors = cb;

		btn.onClick.AddListener(onClick);

		// Button label
		CreateLabel(btnObj.transform, name + "Label", label, 50, FontStyle.Bold,
			textColor, Vector2.zero, Vector2.one);

		return btnObj;
	}
}
