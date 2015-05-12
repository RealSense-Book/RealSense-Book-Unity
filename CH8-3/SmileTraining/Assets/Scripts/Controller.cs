using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
    /// <summary>
    /// ゴールに必要なグッドなスマイル回数
    /// </summary>
    public int goalGoodSmileCount = 5;

    /// <summary>
    /// スマイル時に再生するパーティクル
    /// </summary>
    public ParticleSystem smileParticle;

    /// <summary>
    /// メッセージ表示用テキスト01
    /// </summary>
    public Text messageText01;

    /// <summary>
    /// メッセージ表示用テキスト02
    /// </summary>
    public Text messageText02;

    /// <summary>
    /// ステージ定義
    /// </summary>
    public enum Stage
    {
        None,
        Start,
        Practice,
        Goal
    }

    /// <summary>
    /// 現在のステージ
    /// </summary>
    public Stage CurrentStage { get; private set; }

    /// <summary>
    /// グッドなスマイルフラグ
    /// </summary>
    public bool IsGoodSmiling { get; private set; }

    /// <summary>
    /// グッドなスマイル回数
    /// </summary>
    public int GoodSmileCount { get; private set; }

	// Use this for initialization
	void Start () 
    {
        this.CurrentStage = Stage.None;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (this.CurrentStage == Stage.None)
        {
            //一連のステージ進行を繰り返す
            this.StartCoroutine(this.ProcessStart());
        }
	}

    /// <summary>
    /// グッドなスマイル検出時
    /// </summary>
    void OnGoodSmile()
    {
        if ((this.CurrentStage == Stage.Practice) && !this.IsGoodSmiling)
        {
            //スマイル回数を増やしてパーティクルを再生
            this.GoodSmileCount++;
            this.smileParticle.Play();
        }
        this.IsGoodSmiling = true;
    }

    /// <summary>
    /// グッドじゃないスマイル検出時
    /// </summary>
    void OnBadSmile()
    {
        this.IsGoodSmiling = false;
    }

    /// <summary>
    /// スタートステージ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProcessStart()
    {
        this.CurrentStage = Stage.Start;
        this.GoodSmileCount = 0;
        this.messageText01.color = Color.white;
        this.messageText02.color = Color.white;
        this.messageText01.text = "スマイルの練習を始めるよ！";
        this.messageText02.text = "Enterを押したらスタートだよ！";
        while (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            yield return 0;
        }
        yield return this.StartCoroutine(this.ProcessPractice());
    }

    /// <summary>
    /// 笑顔練習ステージ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProcessPractice()
    {
        this.CurrentStage = Stage.Practice;
        this.messageText01.color = Color.white;
        this.messageText02.color = Color.yellow;
        while (this.GoodSmileCount < this.goalGoodSmileCount)
        {
            this.messageText01.text = string.Format("あと{0}回", this.goalGoodSmileCount - this.GoodSmileCount);
            this.messageText02.text = this.IsGoodSmiling ? "素敵なスマイルだね！" : "にっこり笑って！";
            yield return 0;
        }
        yield return this.StartCoroutine(this.ProcessGoal());
    }

    /// <summary>
    /// ゴール後ステージ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProcessGoal()
    {
        this.CurrentStage = Stage.Goal;
        this.messageText01.color = Color.yellow;
        this.messageText02.color = Color.yellow;
        this.messageText01.text = "お疲れ様でした！";
        this.messageText02.text = "素敵なスマイルをありがとう！";
        yield return new WaitForSeconds(3.0f);
        //最初へ戻る
        this.CurrentStage = Stage.None;
        yield break;
    }
}