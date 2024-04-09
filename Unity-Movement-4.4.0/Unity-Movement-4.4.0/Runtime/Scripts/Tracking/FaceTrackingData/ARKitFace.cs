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
            ("A09_Eye_Look_Down_Right", OVRFaceExpressions.FaceExpression.EyesLookDownR),
            ("A12_Eye_Look_In_Right", OVRFaceExpressions.FaceExpression.EyesLookLeftR),
            ("A13_Eye_Look_Out_Right", OVRFaceExpressions.FaceExpression.EyesLookRightR),
            ("A07_Eye_Look_Up_Right", OVRFaceExpressions.FaceExpression.EyesLookUpR),
            ("A17_Eye_Squint_Right", OVRFaceExpressions.FaceExpression.LidTightenerR),
            ("A19_Eye_Wide_Right", OVRFaceExpressions.FaceExpression.UpperLidRaiserR),
            ("A26_Jaw_Forward", OVRFaceExpressions.FaceExpression.JawThrust),
            ("A27_Jaw_Left", OVRFaceExpressions.FaceExpression.JawSidewaysLeft),
            ("A28_Jaw_Right", OVRFaceExpressions.FaceExpression.JawSidewaysRight),
            ("A25_Jaw_Open", OVRFaceExpressions.FaceExpression.JawDrop),
            ("A37_Mouth_Close", OVRFaceExpressions.FaceExpression.LipsToward),
            ("A29_Mouth_Funnel", OVRFaceExpressions.FaceExpression.LipFunnelerLB),
            ("A30_Mouth_Pucker", OVRFaceExpressions.FaceExpression.LipPuckerL),
            ("A31_Mouth_Left", OVRFaceExpressions.FaceExpression.MouthLeft),
            ("A32_Mouth_Right", OVRFaceExpressions.FaceExpression.MouthRight),
            ("A38_Mouth_Smile_Left", OVRFaceExpressions.FaceExpression.LipCornerPullerL),
            ("A39_Mouth_Smile_Right", OVRFaceExpressions.FaceExpression.LipCornerPullerR),
            ("A40_Mouth_Frown_Left", OVRFaceExpressions.FaceExpression.LipCornerDepressorL),
            ("A41_Mouth_Frown_Right", OVRFaceExpressions.FaceExpression.LipCornerDepressorR),
            ("A42_Mouth_Dimple_Left", OVRFaceExpressions.FaceExpression.DimplerL),
            ("A43_Mouth_Dimple_Right", OVRFaceExpressions.FaceExpression.DimplerR),
            ("A50_Mouth_Stretch_Left", OVRFaceExpressions.FaceExpression.LipStretcherL),
            ("A51_Mouth_Stretch_Right", OVRFaceExpressions.FaceExpression.LipStretcherR),
            ("A34_Mouth_Roll_Lower", OVRFaceExpressions.FaceExpression.LipSuckLB),
            ("A33_Mouth_Roll_Upper", OVRFaceExpressions.FaceExpression.LipSuckLT),
            ("A35_Mouth_Shrug_Lower", OVRFaceExpressions.FaceExpression.ChinRaiserB),
            ("A36_Mouth_Shrug_Upper", OVRFaceExpressions.FaceExpression.ChinRaiserT),
            ("A48_Mouth_Press_Left", OVRFaceExpressions.FaceExpression.LipPressorL),
            ("A49_Mouth_Press_Right", OVRFaceExpressions.FaceExpression.LipPressorR),
            ("A46_Mouth_Lower_Down_Left", OVRFaceExpressions.FaceExpression.LowerLipDepressorL),
            ("A47_Mouth_Lower_Down_Right", OVRFaceExpressions.FaceExpression.LowerLipDepressorR),
            ("A44_Mouth_Upper_Up_Left", OVRFaceExpressions.FaceExpression.UpperLipRaiserL),
            ("A45_Mouth_Upper_Up_Right", OVRFaceExpressions.FaceExpression.UpperLipRaiserR),
            ("A02_Brow_Down_Left", OVRFaceExpressions.FaceExpression.BrowLowererL),
            ("A03_Brow_Down_Right", OVRFaceExpressions.FaceExpression.BrowLowererR),
            ("A01_Brow_Inner_Up", OVRFaceExpressions.FaceExpression.InnerBrowRaiserL),
            ("A05_Brow_Outer_Up_Right", OVRFaceExpressions.FaceExpression.OuterBrowRaiserR),
            ("A20_Cheek_Puff", OVRFaceExpressions.FaceExpression.CheekPuffL),
            ("A21_Cheek_Squint_Left", OVRFaceExpressions.FaceExpression.CheekRaiserL),
            ("A22_Cheek_Squint_Right", OVRFaceExpressions.FaceExpression.CheekRaiserR),
            ("A23_Nose_Sneer_Left", OVRFaceExpressions.FaceExpression.NoseWrinklerL),
            ("A24_Nose_Sneer_Right", OVRFaceExpressions.FaceExpression.NoseWrinklerR)
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
