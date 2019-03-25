using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RekognitionExtensions.Const
{
    /// <summary>
    /// JSONキー
    /// </summary>
    public class JsonKey
    {
        // 座標情報までのキー
        public const string FACE_DETAILS = "FaceDetails";
        public const string LANDMARKS = "Landmarks";

        // 座標情報キー
        public const string LANDMARKS_TYPE = "Type";
        public const string LANDMARKS_X = "X";
        public const string LANDMARKS_Y = "Y";

        // テキスト検出キー
        public const string TEXT_DETECTIONS = "TextDetections";
    }
}
