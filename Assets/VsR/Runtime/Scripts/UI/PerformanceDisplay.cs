using UnityEngine;
using Unity.Profiling;
using TMPro;
using System.Text;

namespace VsR {
	public class PerformanceDisplay : MonoBehaviour {
		[SerializeField] private float m_updateInterval = 0.5f;
		private TextMeshProUGUI m_text;
		private ProfilerRecorder m_triangleCountRecorder, m_vertexCountRecorder, m_drawCallCountRecorder;

		private StringBuilder m_str;
		private uint m_frameCount;
		private float m_framesTime, m_lastFps;

		private void Awake() {
			m_text = GetComponent<TextMeshProUGUI>();
			m_str = new StringBuilder(500);
		}

		private void OnEnable() {
			m_triangleCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
			m_vertexCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
			m_drawCallCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
		}

		private void OnDisable() {
			m_triangleCountRecorder.Dispose();
			m_vertexCountRecorder.Dispose();
			m_drawCallCountRecorder.Dispose();
		}

		private void Update() {
			m_frameCount++;
			m_framesTime += Time.unscaledDeltaTime;
			if (m_framesTime >= m_updateInterval)
				Refresh();
		}

		private void Refresh() {
			m_lastFps = m_frameCount / m_framesTime;
			m_frameCount = 0u;
			m_framesTime = 0.0f;

			m_str.Clear();
			m_str.AppendLine($"Fps: {Mathf.RoundToInt(m_lastFps)}");
			m_str.AppendLine($"Vert: {m_vertexCountRecorder.LastValue / 1000}k");
			m_str.AppendLine($"Tris: {m_triangleCountRecorder.LastValue / 1000}k");
			m_str.AppendLine($"Calls: {m_drawCallCountRecorder.LastValue}");

			m_text.text = m_str.ToString();
		}
	}
}
