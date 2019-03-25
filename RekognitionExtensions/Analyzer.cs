using RekognitionExtensions.Const;
using RekognitionExtensions.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RekognitionExtensions
{
    /// <summary>
    /// Rekognition結果解析
    /// </summary>
    public class Analyzer
    {
        /// <summary>
        /// detect-textコマンドの結果を解析し、検出した文章、言葉の親子関係を取得する
        /// </summary>
        /// <param name="jsonFilePath">JSONファイルパス</param>
        /// <returns>文章、言葉の親子関係</returns>
        public static Dictionary<ValueDto, List<ValueDto>> AnalyzeDetectText(string jsonFilePath)
        {
            // テキスト検出の結果をDeserialize
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> detectTextResponse =
                serializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(jsonFilePath));

            ArrayList textDetections = detectTextResponse[JsonKeys.TEXT_DETECTIONS] as ArrayList;
            // テキストの紐付けを取得する。
            Dictionary<ValueDto, List<ValueDto>> textRelationships = GetTextRelationships(textDetections);

            return textRelationships;
        }

        /// <summary>
        /// detect-facesコマンドの結果を解析し、顔の座標情報を取得する
        /// </summary>
        /// <param name="jsonFilePath">JSONファイルパス</param>
        /// <returns>顔の座標情報</returns>
        public static List<FaceLandmark> AnalyzeDetectFaces(string jsonFilePath)
        {
            // 顔解析の結果をDeserialize
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> detectFaceResponse =
                serializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(jsonFilePath));

            ArrayList faceDetails = detectFaceResponse[JsonKeys.FACE_DETAILS] as ArrayList;
            List<FaceLandmark> faceLandmarks = new List<FaceLandmark>();
            foreach (Dictionary<string, object> faceDetailsValue in faceDetails)
            {
                if (!faceDetailsValue.ContainsKey(JsonKeys.LANDMARKS))
                {
                    continue;
                }

                // Landmarks内の顔座標を格納する。
                ArrayList landMarks = faceDetailsValue[JsonKeys.LANDMARKS] as ArrayList;
                FaceLandmark faceLandmark = GetLandmarkPoint(landMarks);
                faceLandmarks.Add(faceLandmark);
            }

            return faceLandmarks;
        }

        /// <summary>
        /// 文章、言葉の親子関係を取得する
        /// </summary>
        /// <param name="textDetections">検出テキスト情報</param>
        /// <returns>文章、言葉の親子関係</returns>
        private static Dictionary<ValueDto, List<ValueDto>> GetTextRelationships(ArrayList textDetections)
        {
            // LINEとWORDの紐付けを設定
            Dictionary<ValueDto, List<ValueDto>> textRelationships = new Dictionary<ValueDto, List<ValueDto>>();

            foreach (Dictionary<string, object> detectionsValue in textDetections)
            {
                string type = detectionsValue[JsonKeys.TYPE] as string;
                string id = detectionsValue[JsonKeys.ID].ToString();
                string detectedText = detectionsValue[JsonKeys.DETECTED_TEXT] as string;
                ValueDto valueDto = new ValueDto() { ID = id, Text = detectedText };

                // LINEの場合
                if (JsonValues.TYPE_LINE == type)
                {
                    // LINE情報の設定のみを行う。
                    textRelationships.Add(valueDto, new List<ValueDto>());
                }
                // WORDの場合
                else if (JsonValues.TYPE_WORD == type)
                {
                    string parentId = detectionsValue[JsonKeys.PARENT_ID].ToString();
                    // WORD情報の紐付け設定を行う。
                    foreach (KeyValuePair<ValueDto, List<ValueDto>> relationshipsValue in textRelationships)
                    {
                        if (relationshipsValue.Key.ID == parentId)
                        {
                            relationshipsValue.Value.Add(valueDto);
                            break;
                        }
                    }
                }
            }

            return textRelationships;
        }

        /// <summary>
        /// 顔の座標情報を取得する
        /// </summary>
        /// <param name="landmarks">座標情報</param>
        /// <returns>顔の座標情報</returns>
        private static FaceLandmark GetLandmarkPoint(ArrayList landmarks)
        {
            FaceLandmark faceLandmark = new FaceLandmark()
            {
                Mouth = new Dictionary<string, PointF>(),
                Nose = new Dictionary<string, PointF>(),
                LeftEyeBrow = new Dictionary<string, PointF>(),
                RightEyeBrow = new Dictionary<string, PointF>(),
                LeftEye = new Dictionary<string, PointF>(),
                RightEye = new Dictionary<string, PointF>(),
                Jawline = new Dictionary<string, PointF>()
            };

            foreach (Dictionary<string, object> landmarksValue in landmarks)
            {
                // 座標情報を作成する。
                PointF point = new PointF()
                {
                    X = float.Parse(landmarksValue[JsonKeys.X].ToString()),
                    Y = float.Parse(landmarksValue[JsonKeys.Y].ToString())
                };

                string type = landmarksValue[JsonKeys.TYPE] as string;
                // 設定する座標を決定する。
                switch (type)
                {
                    // 口の場合
                    case Landmarks.MOUTH_LEFT:
                    case Landmarks.MOUTH_RIGHT:
                    case Landmarks.MOUTH_UP:
                    case Landmarks.MOUTH_DOWN:
                        faceLandmark.Mouth.Add(type, point);
                        break;
                    // 鼻の場合
                    case Landmarks.NOSE:
                    case Landmarks.NOSE_LEFT:
                    case Landmarks.NOSE_RIGHT:
                        faceLandmark.Nose.Add(type, point);
                        break;
                    // 左眉の場合
                    case Landmarks.LEFT_EYE_BROW_LEFT:
                    case Landmarks.LEFT_EYE_BROW_RIGHT:
                    case Landmarks.LEFT_EYE_BROW_UP:
                        faceLandmark.LeftEyeBrow.Add(type, point);
                        break;
                    // 右眉の場合
                    case Landmarks.RIGHT_EYE_BROW_LEFT:
                    case Landmarks.RIGHT_EYE_BROW_RIGHT:
                    case Landmarks.RIGHT_EYE_BROW_UP:
                        faceLandmark.RightEyeBrow.Add(type, point);
                        break;
                    // 左目の場合
                    case Landmarks.LEFT_EYE_LEFT:
                    case Landmarks.LEFT_EYE_RIGHT:
                    case Landmarks.LEFT_EYE_UP:
                    case Landmarks.LEFT_EYE_DOWN:
                    case Landmarks.LEFT_PUPIL:
                        faceLandmark.LeftEye.Add(type, point);
                        break;
                    // 右目の場合
                    case Landmarks.RIGHT_EYE_LEFT:
                    case Landmarks.RIGHT_EYE_RIGHT:
                    case Landmarks.RIGHT_EYE_UP:
                    case Landmarks.RIGHT_EYE_DOWN:
                    case Landmarks.RIGHT_PUPIL:
                        faceLandmark.RightEye.Add(type, point);
                        break;
                    // 下あご輪郭の場合
                    case Landmarks.UPPER_JAWLINE_LEFT:
                    case Landmarks.MID_JAWLINE_LEFT:
                    case Landmarks.UPPER_JAWLINE_RIGHT:
                    case Landmarks.MID_JAWLINE_RIGHT:
                    case Landmarks.CHIN_BOTTOM:
                        faceLandmark.Jawline.Add(type, point);
                        break;
                }
            }

            return faceLandmark;
        }
    }
}
