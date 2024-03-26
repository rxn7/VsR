using TriInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/MagazineData", fileName = "magazine")]
	[System.Serializable]
    [ExecuteInEditMode]
	public class MagazineData : ScriptableObject {
		[Header("General")]
		[Range(1, 500)] public int capacity;

		[Header("Audio")]
		public AudioClip slideInSound;
		public AudioClip slideOutSound;

        [Header("Stacking")]
        public Vector3 stackingOrigin;
        public Vector3 stackingRotationOrigin;
        public Vector3 stackingOffset;
        public int offsetZStackStartIdx = 0;
        public int offsetZStackEndIdx = -1;
        public Vector3 stackingRotationOffset;
        public int rotateStackStartIdx = 0;
        public int rotateStackEndIdx = -1;
        public bool isDoubleStack = true;
        public bool isPistolMag = false;
        [ShowInInspector] public MagazineCartridgeSlot[] calculatedCartridgeSlots = new MagazineCartridgeSlot[0];

		[Header("References")]
        [Required] [AssetsOnly] public Magazine prefab;
		[Required] public CartridgeData defaultCartridgeData;

        private void OnEnable() {
            CalculateCartridgeSlots();
        }

        public void CalculateCartridgeSlots() {
            calculatedCartridgeSlots = new MagazineCartridgeSlot[capacity];

            Vector3 positionOffset = stackingOrigin;
            Vector3 rotationOffset = stackingRotationOrigin;

            calculatedCartridgeSlots[0] = new MagazineCartridgeSlot {
                position = stackingOrigin,
                rotation = stackingRotationOrigin
            };

            if(isPistolMag && isDoubleStack) {
                positionOffset.y += stackingOffset.y * 0.5f;
            }

            for(int i=1; i<capacity; ++i) {
                if(isDoubleStack) {
                    positionOffset.x = stackingOffset.x * (i % 2 == 0 ? -1 : 1);
                } else {
                    positionOffset.x += stackingOffset.x;
                }

                positionOffset.y += stackingOffset.y;

                if(i >= offsetZStackStartIdx && (i <= offsetZStackEndIdx || offsetZStackEndIdx == -1)) {
                    positionOffset.z += stackingOffset.z;
                }

                if(i >= rotateStackStartIdx && (i <= rotateStackEndIdx || rotateStackEndIdx == -1)) {
                    rotationOffset += stackingRotationOffset;
                }

                calculatedCartridgeSlots[i] = new MagazineCartridgeSlot {
                    position = positionOffset,
                    rotation = rotationOffset
                };
            }

    #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
    #endif
        }
	}

    public struct MagazineCartridgeSlot {
        public Vector3 position;
        public Vector3 rotation;
    }
}