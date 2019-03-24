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

            foreach (Dictionary<string, object> landMarksValue in landmarks)
            {
                // 座標情報を作成する。
                PointF point = new PointF()
                {
                    X = float.Parse(landMarksValue[JsonKeys.LANDMARKS_X].ToString()),
                    Y = float.Parse(landMarksValue[JsonKeys.LANDMARKS_Y].ToString())
                };

                string type = landMarksValue[JsonKeys.LANDMARKS_TYPE] as string;
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
