using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RekognitionExtensions.Dto
{
    /// <summary>
    /// 顔座標情報
    /// </summary>
    public class FaceLandmark
    {
        /// <summary>
        /// 口の情報を取得・設定する
        /// </summary>
        public Dictionary<string, PointF> Mouth { get; set; } = null;

        /// <summary>
        /// 鼻の情報を取得・設定する
        /// </summary>
        public Dictionary<string, PointF> Nose { get; set; } = null;

        /// <summary>
        /// 左眉の情報を取得・設定する
        /// </summary>
        public Dictionary<string, PointF> LeftEyeBrow { get; set; } = null;

        /// <summary>
        /// 右眉の情報を取得・設定する
        /// </summary>
        public Dictionary<string, PointF> RightEyeBrow { get; set; } = null;

        /// <summary>
        /// 左目の情報を取得・設定する
        /// </summary>
        public Dictionary<string, PointF> LeftEye { get; set; } = null;

        /// <summary>
        /// 右目の情報を取得・設定する
        /// </summary>
        public Dictionary<string, PointF> RightEye { get; set; } = null;

        /// <summary>
        /// 下あご輪郭の情報を取得・設定する
        /// </summary>
        public Dictionary<string, PointF> Jawline { get; set; } = null;
    }
}
