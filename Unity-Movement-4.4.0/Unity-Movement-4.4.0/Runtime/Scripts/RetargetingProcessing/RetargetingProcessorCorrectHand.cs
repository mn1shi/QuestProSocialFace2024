// Copyright (c) Meta Platforms, Inc. and affiliates.

using Oculus.Interaction.Input;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Movement.AnimationRigging
{
    /// <summary>
    /// Retargeting processor used to fix the arm via an IK algorithm so that the retargeted hand position matches
    /// the tracked hand position.
    /// </summary>
    [CreateAssetMenu(fileName = "Correct Hand", menuName = "Movement Samples/Data/Retargeting Processors/Correct Hand", order = 1)]
    public sealed class RetargetingProcessorCorrectHand : RetargetingProcessor
    {
        /// <summary>
        /// The types of IK available to be used.
        /// </summary>
        public enum IKType
        {
            None,
            CCDIK,
            FABRIK
        }

        /// <summary>
        /// The hand that this is correcting.
        /// </summary>
        [SerializeField]
        private Handedness _handedness = Handedness.Left;
        /// <inheritdoc cref="_handedness" />
        public Handedness Handedness
        {
            get => _handedness;
            set => _handedness = value;
        }

        /// <summary>
        /// The type of IK that should be applied to modify the arm bones toward the
        /// correct hand target.
        /// </summary>
        [Tooltip(RetargetingLayerTooltips.HandIKType)]
        [SerializeField]
        private IKType _handIKType = IKType.None;
        /// <inheritdoc cref="_handIKType" />
        public IKType HandIKType
        {
            get => _handIKType;
            set => _handIKType = value;
        }

        /// <summary>
        /// If true, use the world hand position for placing the hand instead of the scaled position.
        /// </summary>
        [Tooltip(RetargetingLayerTooltips.UseWorldHandPosition)]
        [SerializeField]
        private bool _useWorldHandPosition = true;
        /// <inheritdoc cref="_useWorldHandPosition" />
        public bool UseWorldHandPosition
        {
            get => _useWorldHandPosition;
            set => _useWorldHandPosition = value;
        }

        /// <summary>
        /// If true, use the custom hand target position for the target position.
        /// </summary>
        [Tooltip(RetargetingLayerTooltips.UseCustomHandTargetPosition)]
        [SerializeField]
        private bool _useCustomHandTargetPosition = true;
        /// <inheritdoc cref="_useCustomHandTargetPosition" />
        public bool UseCustomHandTargetPosition
        {
            get => _useCustomHandTargetPosition;
            set => _useCustomHandTargetPosition = value;
        }

        /// <summary>
        /// The custom hand target position.
        /// </summary>
        private Vector3? _customHandTargetPosition;
        /// <inheritdoc cref="_customHandTargetPosition" />
        public Vector3? CustomHandTargetPosition
        {
            get => _customHandTargetPosition;
            set => _customHandTargetPosition = value;
        }

        /// <summary>
        /// The maximum stretch for the hand to reach the target position that is allowed.
        /// </summary>
        [Tooltip(RetargetingLayerTooltips.MaxHandStretch)]
        [SerializeField]
        private float _maxHandStretch = 0.01f;
        /// <inheritdoc cref="_maxHandStretch" />
        public float MaxHandStretch
        {
            get => _maxHandStretch;
            set => _maxHandStretch = value;
        }

        /// <summary>
        /// The maximum stretch for the shoulder to help the hand reach the target position that is allowed.
        /// </summary>
        [Tooltip(RetargetingLayerTooltips.MaxShoulderStretch)]
        [SerializeField]
        private float _maxShoulderStretch;
        /// <inheritdoc cref="_maxShoulderStretch" />
        public float MaxShoulderStretch
        {
            get => _maxShoulderStretch;
            set => _maxShoulderStretch = value;
        }

        /// <summary>
        /// The maximum distance between the resulting position and target position that is allowed.
        /// </summary>
        [Tooltip(RetargetingLayerTooltips.IKTolerance)]
        [SerializeField]
        private float _ikTolerance = 1e-6f;
        /// <inheritdoc cref="_ikTolerance" />
        public float IKTolerance
        {
            get => _ikTolerance;
            set => _ikTolerance = value;
        }

        /// <summary>
        /// The maximum number of iterations allowed for the IK algorithm.
        /// </summary>
        [Tooltip(RetargetingLayerTooltips.IKIterations)]
        [SerializeField]
        private int _ikIterations = 10;
        /// <inheritdoc cref="_ikIterations" />
        public int IKIterations
        {
            get => _ikIterations;
            set => _ikIterations = value;
        }

        private Transform[] _armBones;
        private Transform[] _armBonesEndEffectorlast;
        private float[] _distanceToNextBoneEndEffectorLast;
        private Vector3 _originalHandPosition;

        /// <inheritdoc />
        public override void CopyData(RetargetingProcessor source)
        {
            base.CopyData(source);
            var sourceCorrectHand = source as RetargetingProcessorCorrectHand;
            if (sourceCorrectHand == null)
            {
                Debug.LogError($"Failed to copy properties from {source.name} processor to {name} processor");
                return;
            }
            _handedness = sourceCorrectHand.Handedness;
            _handIKType = sourceCorrectHand.HandIKType;
            _ikTolerance = sourceCorrectHand.IKTolerance;
            _ikIterations = sourceCorrectHand.IKIterations;
            _useWorldHandPosition = sourceCorrectHand.UseWorldHandPosition;
            _useCustomHandTargetPosition = sourceCorrectHand.UseCustomHandTargetPosition;
        }

        /// <inheritdoc />
        public override void SetupRetargetingProcessor(RetargetingLayer retargetingLayer)
        {
            // Skip the finger bones.
            var armBones = new List<Transform>();
            var animator = retargetingLayer.GetAnimatorTargetSkeleton();

            // We iterate from the jaw downward, as the first bone is the effector, which is the hand.
            // Hand -> Lower Arm -> Upper Arm -> Shoulder.
            for (var i = HumanBodyBones.Jaw; i >= HumanBodyBones.Hips; i--)
            {
                var boneTransform = animator.GetBoneTransform(i);
                if (boneTransform == null)
                {
                    continue;
                }

                if ((Handedness == Handedness.Left &&
                     BoneMappingsExtension.HumanBoneToAvatarBodyPart[i] == AvatarMaskBodyPart.LeftArm) ||
                    (Handedness == Handedness.Right &&
                     BoneMappingsExtension.HumanBoneToAvatarBodyPart[i] == AvatarMaskBodyPart.RightArm))
                {
                    armBones.Add(boneTransform);
                }
            }
            _armBones = armBones.ToArray();
            armBones.Reverse();
            // some IK solutions require end effector to be last.
            _armBonesEndEffectorlast = armBones.ToArray();
            _distanceToNextBoneEndEffectorLast = new float[_armBones.Length];
            UpdateBoneDistances();
        }

        /// <inheritdoc />
        public override void PrepareRetargetingProcessor(RetargetingLayer retargetingLayer, IList<OVRBone> ovrBones)
        {
            _originalHandPosition = _armBones[0].position;
        }

        /// <inheritdoc />
        public override void ProcessRetargetingLayer(RetargetingLayer retargetingLayer, IList<OVRBone> ovrBones)
        {
            if (Weight < float.Epsilon)
            {
                return;
            }

            var isFullBody = retargetingLayer.GetSkeletonType() == OVRSkeleton.SkeletonType.FullBody;
            var leftHandWristIndex = isFullBody ? (int)OVRSkeleton.BoneId.FullBody_LeftHandWrist :
                (int)OVRSkeleton.BoneId.Body_LeftHandWrist;
            var rightHandWristIndex = isFullBody ? (int)OVRSkeleton.BoneId.FullBody_RightHandWrist :
                (int)OVRSkeleton.BoneId.Body_RightHandWrist;

            if ((Handedness == Handedness.Left &&
                 ovrBones.Count < leftHandWristIndex) ||
                (Handedness == Handedness.Right &&
                 ovrBones.Count < rightHandWristIndex))
            {
                return;
            }

            var targetHand = ovrBones[Handedness == Handedness.Left ?
                leftHandWristIndex :
                rightHandWristIndex]?.Transform;
            if (targetHand == null)
            {
                return;
            }

            var targetHandPosition = targetHand.position;
            if (_useWorldHandPosition)
            {
                var localScale = retargetingLayer.transform.localScale;
                // divide by absolute value of scale to avoid sign of scale
                localScale = new Vector3 (Mathf.Abs(localScale.x), Mathf.Abs(localScale.y), Mathf.Abs(localScale.z));
                targetHandPosition = RiggingUtilities.DivideVector3(targetHandPosition, localScale);
            }
            if (_useCustomHandTargetPosition && _customHandTargetPosition.HasValue)
            {
                targetHandPosition = _customHandTargetPosition.Value;
            }

            var handBone = _armBones[0];
            handBone.position = _originalHandPosition;
            var handRotation = handBone.rotation;
            Vector3 targetPosition = Vector3.Lerp(handBone.position, targetHandPosition, Weight);
            bool solvedIK = false;
            if (Weight > 0.0f)
            {
                if (HandIKType == IKType.CCDIK)
                {
                    solvedIK = AnimationUtilities.SolveCCDIK(_armBones, targetPosition, IKTolerance, IKIterations);
                }
                else if (HandIKType == IKType.FABRIK)
                {
                    UpdateBoneDistances();
                    solvedIK = AnimationUtilities.SolveFABRIK(
                        _armBonesEndEffectorlast, _distanceToNextBoneEndEffectorLast,
                        targetPosition, IKTolerance, IKIterations, true);
                }
            }
            handBone.position = Vector3.MoveTowards(handBone.position, targetPosition, MaxHandStretch);
            handBone.rotation = handRotation;

            if (!solvedIK && MaxShoulderStretch > 0.0f)
            {
                var shoulderStretchDirection = targetPosition - handBone.position;
                var shoulderStretchMagnitude = Mathf.Clamp(shoulderStretchDirection.magnitude, 0, MaxShoulderStretch);
                var shoulderStretchVector = shoulderStretchDirection.normalized * shoulderStretchMagnitude;
                _armBones[^1].position += shoulderStretchVector;
            }
        }

        private void UpdateBoneDistances()
        {
            for (int i = 0; i < _armBonesEndEffectorlast.Length; i++)
            {
                bool lastJoint = i == _armBonesEndEffectorlast.Length - 1;
                if (lastJoint)
                {
                    _distanceToNextBoneEndEffectorLast[i] = 0f;
                }
                else
                {
                    _distanceToNextBoneEndEffectorLast[i] = (_armBonesEndEffectorlast[i + 1].position -
                        _armBonesEndEffectorlast[i].position).magnitude;
                }
            }
        }
    }
}
