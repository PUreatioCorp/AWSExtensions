using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextractExtensions;
using TextractExtensions.Dto;
using System.Collections.Generic;

namespace ExtensionsTest
{
    [TestClass]
    public class TextractExtensionsTest
    {
        /// <summary>
        /// 解析情報テスト001
        /// </summary>
        [TestMethod]
        public void AnalyzerTest001()
        {
            Dictionary<string, Dictionary<ValueDto, List<ValueDto>>> textRelationships =
                Analyzer.AnalyzeDetectDocumentText(@"C:\work\bbbb.txt");

            // PAGEのキーが含まれているか
            Assert.IsTrue(textRelationships.Keys.Count > 0);

            foreach (KeyValuePair<string, Dictionary<ValueDto, List<ValueDto>>> pair in textRelationships)
            {
                // LINEのキーが含まれているか
                Assert.IsTrue(pair.Value.Keys.Count > 0);
                foreach(KeyValuePair<ValueDto, List<ValueDto>> linePair in pair.Value)
                {
                    Assert.IsNotNull(linePair.Key);

                    // WORDが含まれているか
                    Assert.IsTrue(linePair.Value.Count > 0);
                }
            }
        }
    }
}
