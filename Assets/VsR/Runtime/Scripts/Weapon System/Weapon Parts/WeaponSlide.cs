using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

namespace VsR {
	public class WeaponSlide : WeaponPart {
		[SerializeField] protected Vector3 m_maxSlidePosition;
		[SerializeField] protected float m_releaseAnimationSpeed = 0.03f;

		protected Vector3 m_initPosition;
		protected Vector3 m_startHandLocalPosition;
		protected float m_initToMaxSlideDistance;
		protected bool m_racked = false;
		protected Hand m_hand;

		protected virtual bool CanRelease => true;
		public bool Racked => m_racked;
		protected Vector3 getHandLocalPosition() => transform.InverseTransformPoint(m_hand.attachTransform.position);

		protected override void Awake() {
			base.Awake();
			m_initPosition = transform.localPosition;
			m_initToMaxSlideDistance = Vector3.Distance(m_initPosition, m_maxSlidePosition);
		}

		protected virtual void UpdateSlideMovement() {
			transform.localPosition = m_initPosition;
			float handDifference = m_weapon.transform.InverseTransformDirection(transform.TransformDirection(m_startHandLocalPosition - getHandLocalPosition())).z;
			float slidePercentage = Mathf.Clamp01(handDifference / m_initToMaxSlideDistance);

			transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePosition, slidePercentage);

			if (m_racked && slidePercentage < 0.6f)
				RackBack();
			else if (!m_racked && slidePercentage >= 0.99f)
				Rack();
		}

		protected void Rack() {
			SoundPoolManager.Instance.PlaySound(m_weapon.Data.rackSound, transform.position, Random.Range(0.9f, 1.1f));
			m_weapon.TryToCock();
			m_racked = true;
		}

		protected void RackBack() {
			SoundPoolManager.Instance.PlaySound(m_weapon.Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));
			m_racked = false;
		}

		protected IEnumerator ReleaseAnimation() {
			while (Vector3.Distance(transform.localPosition, m_initPosition) != 0) {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_initPosition, m_releaseAnimationSpeed * Time.deltaTime);
				yield return null;
			}
		}

		public void OnRelease() {
			if (m_racked)
				RackBack();

			StartCoroutine(ReleaseAnimation());
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (!m_weapon.isSelected)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (Hand)args.interactorObject;

			m_startHandLocalPosition = getHandLocalPosition();
		}

		protected override void OnSelectExiting(SelectExitEventArgs args) {
			base.OnSelectExiting(args);
			m_hand = null;

			if (CanRelease)
				OnRelease();
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (isSelected && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
				UpdateSlideMovement();
		}
	}
}