# AWSExtensions
Library using AWS service.<br>
Currently we offer the following services:
* Rekognition
* Textract

## Rekognition
Implements the following features.
* Acquires the parent-child relationship of the detected text from the result of the detect-text command.
  * ```Analyzer#AnalyzeDetectText(string)```
* The outline information of the detected face is acquired from the result of the detect-faces command.
  * ```Analyzer#AnalyzeDetectFaces(string)```

Please refer to the following site for face contour information.
* https://docs.aws.amazon.com/ja_jp/rekognition/latest/dg/API_Landmark.html<br>
  * eyeLeft and eyeRight are not included.

## Textract
Implements the following features.
* Acquires the parent-child relationship of the detected text from the result of the detect-document-text command.
  * ```Analyzer#AnalyzeDetectDocumentText(string)```
