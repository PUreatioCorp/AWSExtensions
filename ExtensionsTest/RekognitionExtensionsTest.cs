using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RekognitionExtensions.Dto;
using System.Collections.Generic;
using RekognitionExtensions;
using RekognitionExtensions.Const;

namespace ExtensionsTest
{
    [TestClass]
    public class RekognitionExtensionsTest
    {
        /// <summary>
        /// 解析情報テスト001
        /// </summary>
        [TestMethod]
        public void AnalyzerTest001()
        {
            List<FaceLandmark> faceLandmarks = Analyzer.AnalyzeDetectFaces(@"C:\work\aaaa.txt");

            // インスタンスが生成されているか
            Assert.IsNotNull(faceLandmarks);

            // 口の情報が存在しているか
            Assert.IsTrue(faceLandmarks[0].Mouth.ContainsKey(Landmarks.MOUTH_LEFT));
            Assert.IsTrue(faceLandmarks[0].Mouth.ContainsKey(Landmarks.MOUTH_RIGHT));
            Assert.IsTrue(faceLandmarks[0].Mouth.ContainsKey(Landmarks.MOUTH_UP));
            Assert.IsTrue(faceLandmarks[0].Mouth.ContainsKey(Landmarks.MOUTH_DOWN));

            // 鼻の情報が存在しているか
            Assert.IsTrue(faceLandmarks[0].Nose.ContainsKey(Landmarks.NOSE));
            Assert.IsTrue(faceLandmarks[0].Nose.ContainsKey(Landmarks.NOSE_LEFT));
            Assert.IsTrue(faceLandmarks[0].Nose.ContainsKey(Landmarks.NOSE_RIGHT));

            // 左眉の情報が存在しているか
            Assert.IsTrue(faceLandmarks[0].LeftEyeBrow.ContainsKey(Landmarks.LEFT_EYE_BROW_LEFT));
            Assert.IsTrue(faceLandmarks[0].LeftEyeBrow.ContainsKey(Landmarks.LEFT_EYE_BROW_RIGHT));
            Assert.IsTrue(faceLandmarks[0].LeftEyeBrow.ContainsKey(Landmarks.LEFT_EYE_BROW_UP));

            // 右眉の情報が存在しているか
            Assert.IsTrue(faceLandmarks[0].RightEyeBrow.ContainsKey(Landmarks.RIGHT_EYE_BROW_LEFT));
            Assert.IsTrue(faceLandmarks[0].RightEyeBrow.ContainsKey(Landmarks.RIGHT_EYE_BROW_RIGHT));
            Assert.IsTrue(faceLandmarks[0].RightEyeBrow.ContainsKey(Landmarks.RIGHT_EYE_BROW_UP));

            // 左目の情報が存在しているか
            Assert.IsTrue(faceLandmarks[0].LeftEye.ContainsKey(Landmarks.LEFT_EYE_LEFT));
            Assert.IsTrue(faceLandmarks[0].LeftEye.ContainsKey(Landmarks.LEFT_EYE_RIGHT));
            Assert.IsTrue(faceLandmarks[0].LeftEye.ContainsKey(Landmarks.LEFT_EYE_UP));
            Assert.IsTrue(faceLandmarks[0].LeftEye.ContainsKey(Landmarks.LEFT_EYE_DOWN));
            Assert.IsTrue(faceLandmarks[0].LeftEye.ContainsKey(Landmarks.LEFT_PUPIL));

            // 右目の情報が存在しているか
            Assert.IsTrue(faceLandmarks[0].RightEye.ContainsKey(Landmarks.RIGHT_EYE_LEFT));
            Assert.IsTrue(faceLandmarks[0].RightEye.ContainsKey(Landmarks.RIGHT_EYE_RIGHT));
            Assert.IsTrue(faceLandmarks[0].RightEye.ContainsKey(Landmarks.RIGHT_EYE_UP));
            Assert.IsTrue(faceLandmarks[0].RightEye.ContainsKey(Landmarks.RIGHT_EYE_DOWN));
            Assert.IsTrue(faceLandmarks[0].RightEye.ContainsKey(Landmarks.RIGHT_PUPIL));

            // 下あご輪郭の情報が存在しているか
            Assert.IsTrue(faceLandmarks[0].Jawline.ContainsKey(Landmarks.UPPER_JAWLINE_LEFT));
            Assert.IsTrue(faceLandmarks[0].Jawline.ContainsKey(Landmarks.MID_JAWLINE_LEFT));
            Assert.IsTrue(faceLandmarks[0].Jawline.ContainsKey(Landmarks.UPPER_JAWLINE_RIGHT));
            Assert.IsTrue(faceLandmarks[0].Jawline.ContainsKey(Landmarks.MID_JAWLINE_RIGHT));
            Assert.IsTrue(faceLandmarks[0].Jawline.ContainsKey(Landmarks.CHIN_BOTTOM));
        }
    }
}
