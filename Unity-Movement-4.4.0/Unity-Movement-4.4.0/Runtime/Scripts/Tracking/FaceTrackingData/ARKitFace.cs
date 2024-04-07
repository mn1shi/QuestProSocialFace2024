// Copyright (c) Meta Platforms, Inc. and affiliates.

namespace Oculus.Movement.Tracking
{
    /// <summary>
    /// Version of Correctives mapped to ARKit blend shapes,
    /// via "custom" mapping.
    /// </summary>
    public class ARKitFace : CorrectivesFace
    {
        private static (string, OVRFaceExpressions.FaceExpression)[] ARKitBlendshapesSorted =
            {
            ("A14_Eye_Blink_Left", OVRFaceExpressions.FaceExpression.EyesClosedL),
            ("A08_Eye_Look_Down_Left", OVRFaceExpressions.FaceExpression.EyesLookDownL),
            ("A11_Eye_Look_In_Left", OVRFaceExpressions.FaceExpression.EyesLookRightL),
            ("A10_Eye_Look_Out_Left", OVRFaceExpressions.FaceExpression.EyesLookLeftL),
            ("A06_Eye_Look_Up_Left", OVRFaceExpressions.FaceExpression.EyesLookUpL),
            ("A16_Eye_Squint_Left", OVRFaceExpressions.FaceExpression.LidTightenerL),
            ("A18_Eye_Wide_Left", OVRFaceExpressions.FaceExpression.UpperLidRaiserL),
            ("A15_Eye_Blink_Right", OVRFaceExpressions.FaceExpression.EyesClosedR),
            ("Eye_Look_Down_Right", OVRFaceExpressions.FaceExpression.EyesLookDownR),
            ("Eye_Look_In_Right", OVRFaceExpressions.FaceExpression.EyesLookLeftR),
            ("Eye_Look_Out_Right", OVRFaceExpressions.FaceExpression.EyesLookRightR),
            ("Eye_Look_Up_Right", OVRFaceExpressions.FaceExpression.EyesLookUpR),
            ("Eye_Squint_Right", OVRFaceExpressions.FaceExpression.LidTightenerR),
            ("Eye_Wide_Right", OVRFaceExpressions.FaceExpression.UpperLidRaiserR),
            ("Jaw_Forward", OVRFaceExpressions.FaceExpression.JawThrust),
            ("Jaw_Left", OVRFaceExpressions.FaceExpression.JawSidewaysLeft),
            ("Jaw_Right", OVRFaceExpressions.FaceExpression.JawSidewaysRight),
            ("Jaw_Open", OVRFaceExpressions.FaceExpression.JawDrop),
            ("Mouth_Close", OVRFaceExpressions.FaceExpression.LipsToward),
            ("Mouth_Funnel", OVRFaceExpressions.FaceExpression.LipFunnelerLB),
            ("Mouth_Pucker", OVRFaceExpressions.FaceExpression.LipPuckerL),
            ("Mouth_Left", OVRFaceExpressions.FaceExpression.MouthLeft),
            ("Mouth_Right", OVRFaceExpressions.FaceExpression.MouthRight),
            ("Mouth_Smile_Left", OVRFaceExpressions.FaceExpression.LipCornerPullerL),
            ("Mouth_Smile_Right", OVRFaceExpressions.FaceExpression.LipCornerPullerR),
            ("Mouth_Frown_Left", OVRFaceExpressions.FaceExpression.LipCornerDepressorL),
            ("Mouth_Frown_Right", OVRFaceExpressions.FaceExpression.LipCornerDepressorR),
            ("Mouth_Dimple_Left", OVRFaceExpressions.FaceExpression.DimplerL),
            ("Mouth_Dimple_Right", OVRFaceExpressions.FaceExpression.DimplerR),
            ("Mouth_Stretch_Left", OVRFaceExpressions.FaceExpression.LipStretcherL),
            ("Mouth_Stretch_Right", OVRFaceExpressions.FaceExpression.LipStretcherR),
            ("Mouth_Roll_Lower", OVRFaceExpressions.FaceExpression.LipSuckLB),
            ("Mouth_Roll_Upper", OVRFaceExpressions.FaceExpression.LipSuckLT),
            ("Mouth_Shrug_Lower", OVRFaceExpressions.FaceExpression.ChinRaiserB),
            ("Mouth_Shrug_Upper", OVRFaceExpressions.FaceExpression.ChinRaiserT),
            ("Mouth_Press_Left", OVRFaceExpressions.FaceExpression.LipPressorL),
            ("Mouth_Press_Right", OVRFaceExpressions.FaceExpression.LipPressorR),
            ("Mouth_Lower_Down_Left", OVRFaceExpressions.FaceExpression.LowerLipDepressorL),
            ("Mouth_Lower_Down_Right", OVRFaceExpressions.FaceExpression.LowerLipDepressorR),
            ("Mouth_Upper_Up_Left", OVRFaceExpressions.FaceExpression.UpperLipRaiserL),
            ("Mouth_Upper_Up_Right", OVRFaceExpressions.FaceExpression.UpperLipRaiserR),
            ("Brow_Down_Left", OVRFaceExpressions.FaceExpression.BrowLowererL),
            ("Brow_Down_Right", OVRFaceExpressions.FaceExpression.BrowLowererR),
            ("Brow_Inner_Up", OVRFaceExpressions.FaceExpression.InnerBrowRaiserL),
            ("Brow_Outer_Up_Right", OVRFaceExpressions.FaceExpression.OuterBrowRaiserR),
            ("Cheek_Puff", OVRFaceExpressions.FaceExpression.CheekPuffL),
            ("Cheek_Squint_Left", OVRFaceExpressions.FaceExpression.CheekRaiserL),
            ("Cheek_Squint_Right", OVRFaceExpressions.FaceExpression.CheekRaiserR),
            ("Nose_Sneer_Left", OVRFaceExpressions.FaceExpression.NoseWrinklerL),
            ("Nose_Sneer_Right", OVRFaceExpressions.FaceExpression.NoseWrinklerR)
        };

        /// <summary>
        /// Defines ARKit blend shape name and face expression pair mappings.
        /// </summary>
        /// <returns>Two arrays, each relating a blend shape name with a face expression pair.</returns>
        protected override (string[], OVRFaceExpressions.FaceExpression[]) GetCustomBlendShapeNameAndExpressionPairs()
        {
            string[] arKitBlendShapeNames = new string[ARKitBlendshapesSorted.Length];
            OVRFaceExpressions.FaceExpression[] arKitFaceExpressions =
                new OVRFaceExpressions.FaceExpression[ARKitBlendshapesSorted.Length];
            for (int i = 0; i < ARKitBlendshapesSorted.Length; i++)
            {
                arKitBlendShapeNames[i] = ARKitBlendshapesSorted[i].Item1;
                arKitFaceExpressions[i] = ARKitBlendshapesSorted[i].Item2;
            }
            return (arKitBlendShapeNames, arKitFaceExpressions);
        }
    }
}
