﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RekognitionExtensions.Const
{
    /// <summary>
    /// JSONキー
    /// </summary>
    public class JsonKeys
    {
        // 汎用キ―
        public const string TYPE = "Type";

        // 座標情報までのキー
        public const string FACE_DETAILS = "FaceDetails";
        public const string LANDMARKS = "Landmarks";

        // 座標情報キー
        public const string X = "X";
        public const string Y = "Y";

        // テキスト検出キー
        public const string TEXT_DETECTIONS = "TextDetections";
        public const string ID = "Id";
        public const string PARENT_ID = "ParentId";
        public const string DETECTED_TEXT = "DetectedText";
    }
}
