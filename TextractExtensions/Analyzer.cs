using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TextractExtensions.Const;
using TextractExtensions.Dto;

namespace TextractExtensions
{
    /// <summary>
    /// Textract結果解析
    /// </summary>
    public class Analyzer
    {
        /// <summary>
        /// detect-document-textコマンドの結果を解析し、親子関係を取得する
        /// </summary>
        /// <param name="jsonFilePath">JSONファイルパス</param>
        /// <returns>テキストの紐付け</returns>
        public static Dictionary<string, Dictionary<ValueDto, List<ValueDto>>> AnalyzeDetectDocumentText(string jsonFilePath)
        {
            // テキスト抽出の結果をDeserialize
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> detectTextResponse =
                serializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(jsonFilePath));

            ArrayList blocks = detectTextResponse[JsonKey.BLOCKS] as ArrayList;
            // 紐付けを取得する。
            Dictionary<string, Dictionary<ValueDto, List<ValueDto>>> textRelationships = GetTextRelationships(blocks);

            return textRelationships;
        }

        /// <summary>
        /// テキスト紐付け取得
        /// </summary>
        /// <param name="blocks">BlocksのJSON</param>
        /// <returns>テキスト紐付け</returns>
        private static Dictionary<string, Dictionary<ValueDto, List<ValueDto>>> GetTextRelationships(ArrayList blocks)
        {
            // Relationships格納用Dictionary
            Dictionary<string, List<string>> pageRelationships = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> lineRelationships = new Dictionary<string, List<string>>();

            Dictionary<string, Dictionary<ValueDto, List<ValueDto>>> textRelationships = new Dictionary<string, Dictionary<ValueDto, List<ValueDto>>>();

            // blocksをループ
            foreach (Dictionary<string, object> blocksValue in blocks)
            {
                string blockType = blocksValue[JsonKey.BLOCK_TYPE] as string;
                string id = blocksValue[JsonKey.ID] as string;

                // Relationshipsが存在する場合、RelationshipsのDictionaryに格納する。
                if (blocksValue.ContainsKey(JsonKey.RELATIONSHIPS))
                {
                    ArrayList relationships = blocksValue[JsonKey.RELATIONSHIPS] as ArrayList;
                    if (JsonValue.BLOCK_TYPE_PAGE == blockType)
                    {
                        SetRelationships(ref pageRelationships, relationships, id);
                    }
                    else if (JsonValue.BLOCK_TYPE_LINE == blockType)
                    {
                        SetRelationships(ref lineRelationships, relationships, id);
                    }
                }

                // Pageの場合、文字列の紐付けは存在しないので、以降の処理はなし。
                if (JsonValue.BLOCK_TYPE_PAGE == blockType)
                {
                    // 結果Dictionaryのインスタンスのみ設定しておく。
                    textRelationships.Add(id, new Dictionary<ValueDto, List<ValueDto>>());
                    continue;
                }

                string text = blocksValue[JsonKey.TEXT] as string;
                ValueDto valueDto = new ValueDto() { ID = id, Text = text };
                // Relationshipsから紐付け元を特定し、結果に設定する。
                SetTextRelationships(ref textRelationships, pageRelationships, lineRelationships, valueDto, blockType);
            }

            return textRelationships;
        }

        /// <summary>
        /// Relationships設定
        /// </summary>
        /// <param name="relationshipsDic">設定先のRelationships</param>
        /// <param name="relationships">RelationshipsのJSON</param>
        /// <param name="id">キーとなるID</param>
        private static void SetRelationships(ref Dictionary<string, List<string>> relationshipsDic, ArrayList relationships, string id)
        {
            foreach (Dictionary<string, object> relationshipsValue in relationships)
            {
                string type = relationshipsValue[JsonKey.TYPE] as string;
                // TypeがCHILDではない場合、何もしない。
                if (JsonValue.TYPE_CHILD != type)
                {
                    continue;
                }

                // Relationshipsを設定する。
                ArrayList relationshipsIds = relationshipsValue[JsonKey.IDS] as ArrayList;
                List<string> tempIds = null;
                if (relationshipsDic.ContainsKey(id))
                {
                    tempIds = relationshipsDic[id];
                    tempIds.AddRange(relationshipsIds.Cast<string>());
                }
                else
                {
                    tempIds = relationshipsIds.Cast<string>().ToList();
                    relationshipsDic.Add(id, tempIds);
                }
            }
        }

        /// <summary>
        /// テキスト紐付け設定
        /// </summary>
        /// <param name="textRelationships">設定先テキスト紐付け</param>
        /// <param name="pageRelationships">PAGEとLINEのRelationships</param>
        /// <param name="lineRelationships">LINEとWORDのRelationships</param>
        /// <param name="valueDto">設定するID、Text</param>
        /// <param name="blockType">BlockType</param>
        private static void SetTextRelationships(ref Dictionary<string, Dictionary<ValueDto, List<ValueDto>>> textRelationships,
            Dictionary<string, List<string>> pageRelationships, Dictionary<string, List<string>> lineRelationships, ValueDto valueDto, string blockType)
        {
            // LINEの場合
            if (JsonValue.BLOCK_TYPE_LINE == blockType)
            {
                // PAGEのIDを特定する。
                string pageId = GetSourceID(pageRelationships, valueDto.ID);

                // LINEの内容を設定する。
                Dictionary<ValueDto, List<ValueDto>> lineTextRelationships = textRelationships[pageId];
                lineTextRelationships.Add(valueDto, new List<ValueDto>());
            }
            // WORDの場合
            else if (JsonValue.BLOCK_TYPE_WORD == blockType)
            {
                // LINEのIDを特定する。
                string lineId = GetSourceID(lineRelationships, valueDto.ID);
                // 更にPAGEのIDを特定する。
                string pageId = GetSourceID(pageRelationships, lineId);

                // WORDの内容を設定する。
                Dictionary<ValueDto, List<ValueDto>> lineTextRelationships = textRelationships[pageId];
                foreach(KeyValuePair<ValueDto, List<ValueDto>> lineTextKeyPair in lineTextRelationships)
                {
                    if (lineTextKeyPair.Key.ID == lineId)
                    {
                        lineTextKeyPair.Value.Add(valueDto);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 紐付け元のIDを取得
        /// </summary>
        /// <param name="relationships">取得対象Relationships</param>
        /// <param name="id">自身のID</param>
        /// <returns>ID</returns>
        private static string GetSourceID(Dictionary<string, List<string>> relationships, string id)
        {
            string resultId = null;
            // 紐付け元のキーを特定する（WORDならLINEのID、LINEならPAGEのIDを特定）。
            foreach (KeyValuePair<string, List<string>> relationshipsPair in relationships)
            {
                if (relationshipsPair.Value.Contains(id))
                {
                    resultId = relationshipsPair.Key;
                    break;
                }
            }

            return resultId;
        }
    }
}
