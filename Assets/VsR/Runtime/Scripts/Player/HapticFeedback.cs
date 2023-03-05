using UnityEngine;

[System.Serializable]
public struct HapticFeedback {
	[Range(0.0f, 1.0f)] public float intensity;
	[Range(0.0f, 2.0f)] public float duration;

	public HapticFeedback(float intensity, float duration) {
		this.intensity = intensity;
		this.duration = duration;
	}
}