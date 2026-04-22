using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 送信するゲームデータ
/// </summary>
[System.Serializable]
public class SendGameData
{
    /// <summary>
    /// GameName
    /// </summary>
    [SerializeField]
    private string m_gameName;
    public string GameName => m_gameName;

    /// <summary>
    /// RoomID
    /// </summary>
    [SerializeField]
    private string m_roomID;
    public string RoomID => m_roomID;

    /// <summary>
    /// User1のPlayerType
    /// 講師なら1、生徒なら2。初期値が0で、そのままだとエラー
    /// </summary>
    [SerializeField]
    private int m_user1PlayerType;
    public int User1PlayerType => m_user1PlayerType;

    /// <summary>
    /// User2のPlayerType
    /// 講師なら1、生徒なら2。初期値が0で、そのままだとエラー
    /// </summary>
    [SerializeField]
    private int m_user2PlayerType;
    public int User2PlayerType => m_user2PlayerType;

    /// <summary>
    /// User01
    /// </summary>
    [SerializeField]
    private string m_userID_01;
    public string UserID_01 => m_userID_01;

    /// <summary>
    /// User02
    /// </summary>
    [SerializeField]
    private string m_userID_02;
    public string UserID_02 => m_userID_02;

    /// <summary>
    /// ターン
    /// </summary>
    [SerializeField]
    private int m_turn;
    public int Turn => m_turn;

    /// <summary>
    /// 選択レベル
    /// </summary>
    [SerializeField]
    private int m_selectLevel;
    public int SelectLevel => m_selectLevel;

    /// <summary>
    /// ステージ番号
    /// </summary>
    [SerializeField]
    private int m_stageNo;
    public int StageNo => m_stageNo;

    /// <summary>
    /// 英単語の配置
    /// </summary>
    [SerializeField]
    private List<int> m_wordPlace;
    public List<int> WordPlace => m_wordPlace;

    /// <summary>
    /// 英単語に割り振られたポイント
    /// </summary>
    [SerializeField]
    private List<int> m_wordPlacePoint;
    public List<int> WordPlacePoint => m_wordPlacePoint;

    /// <summary>
    /// 英単語選択状態
    /// </summary>
    [SerializeField]
    private List<bool> m_wordPlaceAnswer;
    public List<bool> WordPlaceAnswer => m_wordPlaceAnswer;

    /// <summary>
    /// user1得点表示状態
    /// </summary>
    [SerializeField]
    private List<bool> m_wordPlaceUser1Read;
    public List<bool> WordPlaceUser1Read => m_wordPlaceUser1Read;

    /// <summary>
    /// user2得点表示状態
    /// </summary>
    [SerializeField]
    private List<bool> m_wordPlaceUser2Read;
    public List<bool> WordPlaceUser2Read => m_wordPlaceUser2Read;

    /// <summary>
    /// user1合計スコア
    /// </summary>
    [SerializeField]
    private int m_user1TotalScore;
    public int User1TotalScore => m_user1TotalScore;

    /// <summary>
    /// user1今回スコア
    /// </summary>
    [SerializeField]
    private int m_user1Score;
    public int User1Score => m_user1Score;
    
    /// <summary>
    /// user2合計スコア
    /// </summary>
    [SerializeField]
    private int m_user2TotalScore;
    public int User2TotalScore => m_user2TotalScore;
    
    /// <summary>
    /// user2今回スコア
    /// </summary>
    [SerializeField]
    private int m_user2Score;
    public int User2Score => m_user2Score;
    
    /// <summary>
    /// user1が所持しているカード
    /// </summary>
    [SerializeField]
    private List<int> m_user1Card;
    public List<int> User1Card => m_user1Card;
    
    /// <summary>
    /// user2が所持しているカード
    /// </summary>
    [SerializeField]
    private List<int> m_user2Card;
    public List<int> User2Card => m_user2Card;

    /// <summary>
    /// User1がDoubleカードを使用したか？
    /// </summary>
    [SerializeField]
    private bool m_user1IsUseDouble;
    public bool User1IsUseDouble => m_user1IsUseDouble;

    /// <summary>
    /// User2がDoubleカードを使用したか？
    /// </summary>
    [SerializeField]
    private bool m_user2IsUseDouble;
    public bool User2IsUseDouble => m_user2IsUseDouble;

    /// <summary>
    /// User1がRareカードを使用したか？
    /// </summary>
    [SerializeField]
    private bool m_user1IsUseRare;
    public bool User1IsUseRare => m_user1IsUseRare;

    /// <summary>
    /// User2がRareカードを使用したか？
    /// </summary>
    [SerializeField]
    private bool m_user2IsUseRare;
    public bool User2IsUseRare => m_user2IsUseRare;

    /// <summary>
    /// カード使用フラグ
    /// </summary>
    [SerializeField]
    private bool m_isUseCard;
    public bool IsUseCard => m_isUseCard;

    /// <summary>
    /// 使用カード
    /// </summary>
    [SerializeField]
    private int m_useCard;
    public int UseCard => m_useCard;

    /// <summary>
    /// 使用したカードの番号
    /// </summary>
    [SerializeField]
    private int m_selectCardNo;
    public int SelectCardNo => m_selectCardNo;

    /// <summary>
    /// 勝敗
    /// </summary>
    [SerializeField]
    private List<int> m_winOrLose;
    public List<int> WinOrLose => m_winOrLose;

    /// <summary>
    /// 次のゲームに進むか
    /// </summary>
    [SerializeField]
    private bool m_isNextGame;
    public bool IsNextGame => m_isNextGame;

    /// <summary>
    /// ゲームデータが削除されるまでの制限時間
    /// </summary>
    [SerializeField]
    private string m_timeLimit;
    public string TimeLimit => m_timeLimit;

    public SendGameData()
    {
        m_gameName = GameInfo.ApplicationName;
        m_roomID = string.Empty;
        m_user1PlayerType = 0;
        m_user2PlayerType = 0;
        m_userID_01 = string.Empty;
        m_userID_02 = string.Empty;
        m_turn = -1;
        m_selectLevel = -1;
        m_stageNo = -1;
        m_wordPlace = new List<int>();
        m_wordPlacePoint = new List<int>();
        m_wordPlaceAnswer = new List<bool>();
        m_wordPlaceUser1Read = new List<bool>();
        m_wordPlaceUser2Read = new List<bool>();
        m_user1TotalScore = 0;
        m_user1Score = 0;
        m_user2TotalScore = 0;
        m_user2Score = 0;
        m_user1Card = new List<int>();
        m_user2Card = new List<int>();
        m_user1IsUseDouble = false;
        m_user2IsUseDouble = false;
        m_user1IsUseRare = false;
        m_user2IsUseRare = false;
        m_isUseCard = false;
        m_useCard = 0;
        m_selectCardNo = -1;
        m_winOrLose = new List<int>();
        m_isNextGame = false;
        m_timeLimit = string.Empty;
    }

    /// <summary>
    /// ゲームデータのコンバート
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static SendGameData Convert(JsonNode json)
    {
        SendGameData sendGameData = new SendGameData();

        sendGameData.m_gameName = json[0][nameof(m_gameName)].Get<string>();
        sendGameData.m_roomID = json[0][nameof(m_roomID)].Get<string>();
        sendGameData.m_user1PlayerType = (int)json[0][nameof(m_user1PlayerType)].Get<long>();
        sendGameData.m_user2PlayerType = (int)json[0][nameof(m_user2PlayerType)].Get<long>();
        sendGameData.m_userID_01 = json[0][nameof(m_userID_01)].Get<string>();
        sendGameData.m_userID_02 = json[0][nameof(m_userID_02)].Get<string>();
        sendGameData.m_turn = (int)json[0][nameof(m_turn)].Get<long>();
        sendGameData.m_selectLevel = (int)json[0][nameof(m_selectLevel)].Get<long>();
        sendGameData.m_stageNo = (int)json[0][nameof(m_stageNo)].Get<long>();
        foreach (var data in json[0][nameof(m_wordPlace)])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_wordPlace.Add(num);
        }
        foreach (var data in json[0][nameof(m_wordPlacePoint)])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_wordPlacePoint.Add(num);
        }
        foreach (var data in json[0][nameof(m_wordPlaceAnswer)])
        {
            bool num = data.Get<bool>();
            sendGameData.m_wordPlaceAnswer.Add(num);
        }
        foreach (var data in json[0][nameof(m_wordPlaceUser1Read)])
        {
            bool num = data.Get<bool>();
            sendGameData.m_wordPlaceUser1Read.Add(num);
        }
        foreach (var data in json[0][nameof(m_wordPlaceUser2Read)])
        {
            bool num = data.Get<bool>();
            sendGameData.m_wordPlaceUser2Read.Add(num);
        }
        sendGameData.m_user1TotalScore = (int)json[0][nameof(m_user1TotalScore)].Get<long>();
        sendGameData.m_user1Score = (int)json[0][nameof(m_user1Score)].Get<long>();
        sendGameData.m_user2TotalScore = (int)json[0][nameof(m_user2TotalScore)].Get<long>();
        sendGameData.m_user2Score = (int)json[0][nameof(m_user2Score)].Get<long>();
        foreach (var data in json[0][nameof(m_user1Card)])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_user1Card.Add(num);
        }
        foreach (var data in json[0][nameof(m_user2Card)])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_user2Card.Add(num);
        }
        sendGameData.m_user1IsUseDouble = json[0][nameof(m_user1IsUseDouble)].Get<bool>();
        sendGameData.m_user2IsUseDouble = json[0][nameof(m_user2IsUseDouble)].Get<bool>();
        sendGameData.m_user1IsUseRare = json[0][nameof(m_user1IsUseRare)].Get<bool>();
        sendGameData.m_user2IsUseRare = json[0][nameof(m_user2IsUseRare)].Get<bool>();
        sendGameData.m_isUseCard = json[0][nameof(m_isUseCard)].Get<bool>();
        sendGameData.m_useCard = (int)json[0][nameof(m_useCard)].Get<long>();
        sendGameData.m_selectCardNo = (int)json[0][nameof(m_selectCardNo)].Get<long>();
        foreach (var data in json[0][nameof(m_winOrLose)])
        {
            int num = (int)data.Get<long>();
            sendGameData.m_winOrLose.Add(num);
        }
        sendGameData.m_isNextGame = json[0][nameof(m_isNextGame)].Get<bool>();
        sendGameData.m_timeLimit = json[0][nameof(m_timeLimit)].Get<string>();

        return sendGameData;
    }

    /// <summary>
    /// 送信するゲームデータを作成する
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public static SendGameData CreateData(GameData gameData)
    {
        SendGameData sendGameData = new SendGameData();

        sendGameData.m_gameName = gameData.GameName;
        sendGameData.m_roomID = gameData.RoomID;
        sendGameData.m_user1PlayerType = (int)gameData.User1PlayerType;
        sendGameData.m_user2PlayerType = (int)gameData.User2PlayerType;
        sendGameData.m_userID_01 = gameData.UserID_01;
        sendGameData.m_userID_02 = gameData.UserID_02;
        sendGameData.m_turn = (int)gameData.Turn;
        sendGameData.m_selectLevel = gameData.SelectLevel;
        sendGameData.m_stageNo = gameData.StageNo;
        sendGameData.m_wordPlace = gameData.WordDatas.ConvertAll(n => n.Place);
        sendGameData.m_wordPlacePoint = gameData.WordDatas.ConvertAll(n => n.Point);
        sendGameData.m_wordPlaceAnswer = gameData.WordDatas.ConvertAll(n => n.Answer);
        sendGameData.m_wordPlaceUser1Read = gameData.UserData[0].WordPlaceRead;
        sendGameData.m_wordPlaceUser2Read = gameData.UserData[1].WordPlaceRead;
        sendGameData.m_user1TotalScore = gameData.UserData[0].TotalScore;
        sendGameData.m_user1Score = gameData.UserData[0].Score;
        sendGameData.m_user2TotalScore = gameData.UserData[1].TotalScore;
        sendGameData.m_user2Score = gameData.UserData[1].Score;
        sendGameData.m_user1Card = gameData.UserData[0].Card.ConvertAll(n => (int)n);
        sendGameData.m_user2Card = gameData.UserData[1].Card.ConvertAll(n => (int)n);
        sendGameData.m_user1IsUseDouble = gameData.UserData[0].IsUseDouble;
        sendGameData.m_user2IsUseDouble = gameData.UserData[1].IsUseDouble;
        sendGameData.m_user1IsUseRare = gameData.UserData[0].IsUseRare;
        sendGameData.m_user2IsUseRare = gameData.UserData[1].IsUseRare;
        sendGameData.m_isUseCard = gameData.IsUseCard;
        sendGameData.m_useCard = (int)gameData.UseCard;
        sendGameData.m_selectCardNo = gameData.SelectCardNo;
        sendGameData.m_winOrLose = gameData.WinOrLose.ConvertAll(n => (int)n);
        sendGameData.m_isNextGame = gameData.IsNextGame;
        sendGameData.m_timeLimit = gameData.TimeLimit;

        return sendGameData;
    }
}