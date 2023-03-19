using UnityEngine;
using UnityEngine.XR;
using Unity.Profiling;
using TMPro;
using System.Text;
using VsR.Math;

namespace VsR {
	public class PerformanceDisplay : MonoBehaviour {
		[SerializeField] private float m_updateInterval = 0.5f;
		private FloatRange m_fpsRange = new FloatRange(60, 120);
		private TextMeshProUGUI m_text;
		private ProfilerRecorder m_triangleCountRecorder, m_vertexCountRecorder, m_drawCallCountRecorder;
		private InputDevice m_device;

		private StringBuilder m_str;
		private uint m_frameCount;
		private float m_framesTime;

		private void Awake() {
			m_text = GetComponent<TextMeshProUGUI>();
			m_str = new StringBuilder(500);
			m_device = InputDevices.GetDeviceAtXRNode(XRNode.Head);
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
			int fps = Mathf.RoundToInt(m_frameCount / m_framesTime);
			m_frameCount = 0u;
			m_framesTime = 0.0f;

			Color fpsColor = Color.Lerp(Color.red, Color.green, m_fpsRange.Percentage(fps));

			m_str.Clear();
			m_str.AppendLine($"ver: {BuildDataManager.Data.version}.{BuildDataManager.Data.gitBranch}.{BuildDataManager.Data.gitCommit}");
			m_str.AppendLine($"fps: <color=#{ColorUtility.ToHtmlStringRGB(fpsColor)}>{fps}</color>");
			m_str.Append($"v: {m_vertexCountRecorder.LastValue / 1000}k ");
			m_str.Append($"t: {m_triangleCountRecorder.LastValue / 1000}k ");
			m_str.Append($"c: {m_drawCallCountRecorder.LastValue}");

			m_text.text = m_str.ToString();
		}
	}
}
