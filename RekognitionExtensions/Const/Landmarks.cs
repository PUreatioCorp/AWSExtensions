using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RekognitionExtensions.Const
{
    /// <summary>
    /// 座標情報キー
    /// </summary>
    public class Landmarks
    {
        // 口（MOUTH）
        public const string MOUTH_LEFT = "mouthLeft";
        public const string MOUTH_RIGHT = "mouthRight";
        public const string MOUTH_UP = "mouthUp";
        public const string MOUTH_DOWN = "mouthDown";

        // 鼻（NOSE）
        public const string NOSE = "nose";
        public const string NOSE_LEFT = "noseLeft";
        public const string NOSE_RIGHT = "noseRight";

        // 左眉（Left Eye Brow）
        public const string LEFT_EYE_BROW_LEFT = "leftEyeBrowLeft";
        public const string LEFT_EYE_BROW_RIGHT = "leftEyeBrowRight";
        public const string LEFT_EYE_BROW_UP = "leftEyeBrowUp";

        // 右眉（Right Eye Brow）
        public const string RIGHT_EYE_BROW_LEFT = "rightEyeBrowLeft";
        public const string RIGHT_EYE_BROW_RIGHT = "rightEyeBrowRight";
        public const string RIGHT_EYE_BROW_UP = "rightEyeBrowUp";

        // 左目（Left Eye）
        public const string LEFT_EYE_LEFT = "leftEyeLeft";
        public const string LEFT_EYE_RIGHT = "leftEyeRight";
        public const string LEFT_EYE_UP = "leftEyeUp";
        public const string LEFT_EYE_DOWN = "leftEyeDown";
        public const string LEFT_PUPIL = "leftPupil";

        // 右目（Right Eye）
        public const string RIGHT_EYE_LEFT = "rightEyeLeft";
        public const string RIGHT_EYE_RIGHT = "rightEyeRight";
        public const string RIGHT_EYE_UP = "rightEyeUp";
        public const string RIGHT_EYE_DOWN = "rightEyeDown";
        public const string RIGHT_PUPIL = "rightPupil";

        // 下あご輪郭（Jawline）
        public const string UPPER_JAWLINE_LEFT = "upperJawlineLeft";
        public const string MID_JAWLINE_LEFT = "midJawlineLeft";
        public const string UPPER_JAWLINE_RIGHT = "upperJawlineRight";
        public const string MID_JAWLINE_RIGHT = "midJawlineRight";
        public const string CHIN_BOTTOM = "chinBottom";
    }
}
