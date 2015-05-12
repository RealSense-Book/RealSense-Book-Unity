using RSUnityToolkit;
using UnityEngine;

public class FaceSync : MonoBehaviour {
    public SkinnedMeshRenderer faceMeshRenderer;    //顔のメッシュオブジェクト
    private int mouthSmileIndex = 17;               //口の笑顔ブレンドシェイプ
    private int mouthLeftIndex = 24;                //口の左引っ張りブレンドシェイプ
    private int mouthRightIndex = 25;               //口の右引っ張りブレンドシェイプ
    private int eyeSmileIndex = 1;                  //目の笑顔ブレンドシェイプ
    private int browSmileIndex = 2;                 //眉の笑顔ブレンドシェイプ

	// Use this for initialization
	void Start () {
        var senseToolkitManager = GameObject.FindObjectOfType(typeof(SenseToolkitManager));
        if (senseToolkitManager == null)
        {
            Debug.LogWarning("Sense Manager Object not found and was added automatically");
            senseToolkitManager = (GameObject)Instantiate(Resources.Load("SenseManager"));
            senseToolkitManager.name = "SenseManager";
        }
        SenseToolkitManager.Instance.SetSenseOption(SenseOption.SenseOptionID.Face);	
	}
	
	// Update is called once per frame
	void Update () {
        if (SenseToolkitManager.Instance.FaceModuleOutput == null)
        {
            return;
        }

        // 検出した顔の一覧を取得する
        var faces = SenseToolkitManager.Instance.FaceModuleOutput.QueryFaces();
        if (faces.Length == 0)
        {
            return;
        }

        // 最初の顔の表情を取得する
        var face = faces[0];
        var expression = face.QueryExpressions();
        if (expression == null)
        {
            return;
        }

        if (this.faceMeshRenderer != null)
        {
            this.SyncMouth(expression);
            this.SyncEye(expression);
        }	
	}

    /// <summary>
    /// 口の表情シンクロ
    /// </summary>
    /// <param name="expression">表情オブジェクト</param>
    private void SyncMouth(PXCMFaceData.ExpressionsData expression)
    {
        // 口を動かす
        PXCMFaceData.ExpressionsData.FaceExpressionResult result;
        expression.QueryExpression(PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_SMILE, out result);
        var level = (int)result.intensity;
        this.faceMeshRenderer.SetBlendShapeWeight(this.mouthSmileIndex, level);
        this.faceMeshRenderer.SetBlendShapeWeight(this.mouthLeftIndex, level);
        this.faceMeshRenderer.SetBlendShapeWeight(this.mouthRightIndex, level);
    }

    /// <summary>
    /// 目と眉の表情シンクロ
    /// </summary>
    /// <param name="expression">表情オブジェクト</param>
    private void SyncEye(PXCMFaceData.ExpressionsData expression)
    {
        PXCMFaceData.ExpressionsData.FaceExpressionResult left;
        expression.QueryExpression(PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_CLOSED_LEFT, out left);

        PXCMFaceData.ExpressionsData.FaceExpressionResult right;
        expression.QueryExpression(PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_CLOSED_RIGHT, out right);

        var level = (int)((left.intensity + right.intensity) / 2);
        this.faceMeshRenderer.SetBlendShapeWeight(this.eyeSmileIndex, level);
        this.faceMeshRenderer.SetBlendShapeWeight(this.browSmileIndex, level);
    }
}
