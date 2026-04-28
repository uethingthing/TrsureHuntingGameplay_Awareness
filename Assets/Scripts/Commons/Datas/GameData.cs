using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターン
/// </summary>
public enum Turn
{
    None = -1,
    User01,
    User02,
    Result,
};

public enum CardType
{
    None,
    Double,
    Combo,
    Rare,
    Protect,
    ReadingEye,
}

public class WordData
{
    /// <summary>
    /// 配置
    /// </summary>
    public int Place { get; private set; }

    /// <summary>
    /// 割り振られたポイント
    /// </summary>
    public int Point { get; private set; }

    /// <summary>
    /// 選択状態
    /// </summary>
    public bool Answer { get; set; }

    public WordData(int place,int point,bool answer)
    {
        Place = place;
        Point = point;
        Answer = answer;
    }
}

/// <summary>
/// 各userデータ
/// </summary>
public class UserData
{
    /// <summary>
    /// 得点表示状態
    /// </summary>
    public List<bool> WordPlaceRead { get; set; }

    /// <summary>
    /// 合計スコア
    /// </summary>
    public int TotalScore { get; set; }

    /// <summary>
    /// 今回スコア
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// 所持しているカード
    /// </summary>
    public List<CardType> Card { get; set; }

    /// <summary>
    /// Doubleカードを使用したか？
    /// </summary>
    public bool IsUseDouble { get; set; }

    /// <summary>
    /// Rareカードを使用したか？
    /// </summary>
    public bool IsUseRare { get; set; }

    public int RemainComboCount { get; set; }

    public UserData()
    {
        WordPlaceRead = new List<bool>();
        TotalScore = 0;
        Score = 0;
        Card = new List<CardType>();
        IsUseDouble = false;
        IsUseRare = false;
        RemainComboCount = 0;
    }
}

/// <summary>
/// ゲームデータ
/// </summary>
public class GameData
{
    /// <summary>
    /// GameName
    /// </summary>
    public string GameName { get; private set; }

    /// <summary>
    /// RoomID
    /// </summary>
    public string RoomID { get; set; }

    /// <summary>
    /// User1のPlayerType
    /// 講師なら1、生徒なら2。初期値が0で、そのままだとエラー
    /// </summary>
    public PlayerType User1PlayerType { get; set; }

    /// <summary>
    /// User2のPlayerType
    /// 講師なら1、生徒なら2。初期値が0で、そのままだとエラー
    /// </summary>
    public PlayerType User2PlayerType { get; set; }

    /// <summary>
    /// User01
    /// </summary>
    public string UserID_01 { get; set; }

    /// <summary>
    /// User02
    /// </summary>
    public string UserID_02 { get; set; }

    /// <summary>
    /// ターン
    /// </summary>
    public Turn Turn { get; set; }

    /// <summary>
    /// 選択レベル
    /// </summary>
    public int SelectLevel { get; set; }

    /// <summary>
    /// ステージ番号
    /// </summary>
    public int StageNo { get; set; }

    /// <summary>
    /// 英単語データ
    /// </summary>
    public List<WordData> WordDatas { get; set; }

    /// <summary>
    /// 各userデータ
    /// </summary>
    public UserData[] UserData { get; set; }

    /// <summary>
    /// カード使用フラグ
    /// </summary>
    public bool IsUseCard { get; set; }

    /// <summary>
    /// 使用カード
    /// </summary>
    public CardType UseCard { get; set; }

    /// <summary>
    /// 使用したカードの番号
    /// </summary>
    public int SelectCardNo { get; set; }

    /// <summary>
    /// 勝敗
    /// </summary>
    public List<Turn> WinOrLose { get; set; }

    /// <summary>
    /// 次のゲームに進むか
    /// </summary>
    public bool IsNextGame { get; set; }

    /// <summary>
    /// ゲームデータが削除されるまでの制限時間
    /// </summary>
    public string TimeLimit { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GameData()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        GameName = GameInfo.ApplicationName;
        RoomID = string.Empty;
        User1PlayerType = PlayerType.None;
        User2PlayerType = PlayerType.None;
        UserID_01 = string.Empty;
        UserID_02 = string.Empty;
        Turn = Turn.None;
        SelectLevel = -1;
        StageNo = -1;
        WordDatas = new List<WordData>();

        UserData = new UserData[2];
        for(int i = 0;i< UserData.Length; i++)
        {
            UserData[i] = new UserData();
        }

        IsUseCard = false;
        UseCard = CardType.None;
        SelectCardNo = -1;
        WinOrLose = new List<Turn>();
        IsNextGame = false;
        TimeLimit = string.Empty;
    }

    /// <summary>
    /// ゲームデータをリセットする
    /// </summary>
    public void ResetData()
    {
        Turn = Turn.None;
        WordDatas.Clear();
        for (int i = 0; i < UserData.Length; i++)
        {
            UserData[i].WordPlaceRead.Clear();
            UserData[i].Score = 0;
            UserData[i].Card.Clear();
            UserData[i].IsUseDouble = false;
            UserData[i].IsUseRare = false;
            UserData[i].RemainComboCount = 0;
        }
        IsUseCard = false;
        UseCard = CardType.None;
        SelectCardNo = -1;
    }

    /// <summary>
    /// Jsonデータからゲームデータに変換
    /// </summary>
    /// <param name="json">Jsonデータ</param>
    /// <returns>ゲームデータ</returns>
    public static GameData FromJsonConvert(JsonNode json)
    {
        SendGameData sendGameData = SendGameData.Convert(json);
        GameData gameData = new GameData();
        
        gameData.GameName = sendGameData.GameName;
        gameData.RoomID = sendGameData.RoomID;
        gameData.User1PlayerType = (PlayerType)sendGameData.User1PlayerType;
        gameData.User2PlayerType = (PlayerType)sendGameData.User2PlayerType;
        gameData.UserID_01 = sendGameData.UserID_01;
        gameData.UserID_02 = sendGameData.UserID_02;
        gameData.Turn = (Turn)sendGameData.Turn;
        gameData.SelectLevel = sendGameData.SelectLevel;
        gameData.StageNo = sendGameData.StageNo;

        List<WordData> wordDatas = new List<WordData>();
        for (int i=0;i< sendGameData.WordPlace.Count; i++)
        {
            WordData wordData = new WordData(sendGameData.WordPlace[i], sendGameData.WordPlacePoint[i], sendGameData.WordPlaceAnswer[i]);
            wordDatas.Add(wordData);
        }
        gameData.WordDatas = wordDatas;

        gameData.UserData[0].WordPlaceRead = sendGameData.WordPlaceUser1Read;
        gameData.UserData[1].WordPlaceRead = sendGameData.WordPlaceUser2Read;
        gameData.UserData[0].TotalScore = sendGameData.User1TotalScore;
        gameData.UserData[0].Score = sendGameData.User1Score;
        gameData.UserData[1].TotalScore = sendGameData.User2TotalScore;
        gameData.UserData[1].Score = sendGameData.User2Score;
        gameData.UserData[0].Card = sendGameData.User1Card.ConvertAll(n => (CardType)Enum.ToObject(typeof(CardType), n));
        gameData.UserData[1].Card = sendGameData.User2Card.ConvertAll(n => (CardType)Enum.ToObject(typeof(CardType), n));
        gameData.UserData[0].IsUseDouble = sendGameData.User1IsUseDouble;
        gameData.UserData[1].IsUseDouble = sendGameData.User2IsUseDouble;
        gameData.UserData[0].IsUseRare = sendGameData.User1IsUseRare;
        gameData.UserData[1].IsUseRare = sendGameData.User2IsUseRare;
        gameData.UserData[0].RemainComboCount = sendGameData.User1RemainComboCount;
        gameData.UserData[1].RemainComboCount = sendGameData.User2RemainComboCount;
        gameData.IsUseCard = sendGameData.IsUseCard;
        gameData.UseCard = (CardType)sendGameData.UseCard;
        gameData.SelectCardNo = sendGameData.SelectCardNo;
        gameData.WinOrLose = sendGameData.WinOrLose.ConvertAll(n => (Turn)Enum.ToObject(typeof(Turn), n));
        gameData.IsNextGame = sendGameData.IsNextGame;
        gameData.TimeLimit = sendGameData.TimeLimit;

        return gameData;
    }

    /// <summary>
    /// GameデータからJsonデータに変換
    /// </summary>
    /// <param name="gameData"><ゲームデータ/param>
    /// <returns>Jsonデータ</returns>
    public static string ToJsonConvert(GameData gameData)
    {
        SendGameData sendGameData = SendGameData.CreateData(gameData);
        string json = JsonUtility.ToJson(sendGameData);
        json = "[" + json + "]";
        return json;
    }

    /// <summary>
    /// ゲームデータの情報をstring型にして返す
    /// </summary>
    /// <returns></returns>
    public string GetStr()
    {
        string str =
            $"GameData\n" +
            $" {nameof(RoomID)}: {RoomID}\n" +
            $" {nameof(User1PlayerType)}: {User1PlayerType}\n" +
            $" {nameof(User2PlayerType)}: {User2PlayerType}\n" +
            $" {nameof(UserID_01)}: {UserID_01}\n" +
            $" {nameof(UserID_02)}: {UserID_02}\n" +
            $" {nameof(Turn)}: {Turn}\n" +
            $" {nameof(SelectLevel)}: {SelectLevel}\n"+
            $" {nameof(StageNo)}: {StageNo}\n" +
            $" {nameof(WordData)}\n" +
            $"  {nameof(WordData.Place)}: {string.Join(",", WordDatas.ConvertAll(n => n.Place))}\n"+
            $"  {nameof(WordData.Point)}: {string.Join(",", WordDatas.ConvertAll(n => n.Point))}\n"+
            $"  {nameof(WordData.Answer)}: {string.Join(",", WordDatas.ConvertAll(n => n.Answer))}\n"+
            $" User1Data\n" +
            $"  {nameof(global::UserData.WordPlaceRead)}: {string.Join(",", UserData[0].WordPlaceRead)}\n" +
            $"  {nameof(global::UserData.TotalScore)}: {UserData[0].TotalScore}\n" +
            $"  {nameof(global::UserData.Score)}: {UserData[0].Score}\n" +
            $"  {nameof(global::UserData.Card)}: {string.Join(",", UserData[0].Card)}\n" +
            $"  {nameof(global::UserData.IsUseDouble)}: {UserData[0].IsUseDouble}\n" +
            $"  {nameof(global::UserData.IsUseDouble)}: {UserData[0].IsUseRare}\n" +
            $"  {nameof(global::UserData.RemainComboCount)}: {UserData[0].RemainComboCount}\n" +
            $" User2Data\n" +
            $"  {nameof(global::UserData.WordPlaceRead)}: {string.Join(",", UserData[1].WordPlaceRead)}\n" +
            $"  {nameof(global::UserData.TotalScore)}: {UserData[1].TotalScore}\n" +
            $"  {nameof(global::UserData.Score)}: {UserData[1].Score}\n" +
            $"  {nameof(global::UserData.Card)}: {string.Join(",", UserData[1].Card)}\n" +
            $"  {nameof(global::UserData.IsUseDouble)}: {UserData[1].IsUseDouble}\n" +
            $"  {nameof(global::UserData.IsUseDouble)}: {UserData[1].IsUseRare}\n" +
            $"  {nameof(global::UserData.RemainComboCount)}: {UserData[1].RemainComboCount}\n" +
            $" {nameof(IsUseCard)}: {IsUseCard}\n" +
            $" {nameof(UseCard)}: {UseCard}\n" +
            $" {nameof(SelectCardNo)}: {SelectCardNo}\n" +
            $" {nameof(WinOrLose)}: {string.Join(",", WinOrLose)}\n" +
            $" {nameof(IsNextGame)}: {IsNextGame}\n" +
            $" {nameof(TimeLimit)}: {TimeLimit}\n";

        return str;
    }
}