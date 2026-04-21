
/// <summary>
/// 使用するURLのタイプ
/// </summary>
public enum URLType
{
    /// <summary>
    /// Info環境
    /// </summary>
    Info,

    /// <summary>
    /// 開発環境
    /// </summary>
    Develop,

    /// <summary>
    /// クアドラのレンタルサーバー環境
    /// </summary>
    Quadra,

    /// <summary>
    /// 本番環境
    /// </summary>
    StudyCompas,
}

public enum PlayerType
{
    /// <summary>
    /// 初期値
    /// </summary>
    None,

    /// <summary>
    /// 講師
    /// </summary>
    Teacher,

    /// <summary>
    /// 生徒
    /// </summary>
    Student
}

/// <summary>
/// ゲーム情報
/// </summary>
public class GameInfo
{
    /// <summary>
    /// フレームレート
    /// </summary>
    public const int FrameRate = 60;

    /// <summary>
    /// アプリケーション名
    /// </summary>
    public static string ApplicationName { get; private set; } = "blackbeard";

    /// <summary>
    /// 自身のユーザーID
    /// </summary>
    public static string MyUserID { get; set; } = "0000";

    /// <summary>
    /// 講師か生徒か
    /// </summary>
    public static PlayerType MyPlayerType { get; set; } = PlayerType.None;

    /// <summary>
    /// PlayerTypeが設定されているのかのチェックをするかしないか
    /// 02_TitleのTitleManagerから変更してください
    /// </summary>
    public static bool CheckPlayerType { get; set; } = false;

    /// <summary>
    /// 一人プレイモード(デバッグ用)
    /// 02_TitleのTitleManagerから変更してください
    /// </summary>
    public static bool IsSingleMode { get; set; } = false;

    /// <summary>
    /// 使用するURLのタイプ
    /// 02_TitleのTitleManagerから変更してください
    /// </summary>
    public static URLType URLType { get; set; } = URLType.Quadra;

    /// <summary>
    /// 再起動されたか？
    /// </summary>
    public static bool IsRestart { get; set; } = false;

    /// <summary>
    /// ゲームデータ
    /// </summary>
    public static GameData Game { get; set; } = default;

    /// <summary>
    /// 自身のターン
    /// </summary>
    public static Turn MyTurn { get; set; } = Turn.None;

    /// <summary>
    /// 対戦相手のターン
    /// </summary>
    public static Turn OpponentTurn => MyTurn == Turn.User01 ? Turn.User02 : Turn.User01;

    /// <summary>
    /// データ取得で使用する自身のuserNo
    /// </summary>
    public static int MyUserNo => MyTurn == Turn.User01 ? 0 : 1;

    /// <summary>
    /// データ取得で使用する対戦相手のuserNo
    /// </summary>
    public static int OpponentUserNo => MyTurn == Turn.User01 ? 1 : 0;

    /// <summary>
    /// 自身のuserデータ
    /// </summary>
    public static UserData MyData => Game.UserData[MyUserNo];

    /// <summary>
    /// 対戦相手のuserデータ
    /// </summary>
    public static UserData OpponentData => Game.UserData[OpponentUserNo];

    public static void Init()
    {
        IsRestart = false;
        Game = new GameData();
        MyTurn = Turn.None;
    }

    /// <summary>
    /// 誰が勝ったかチェック
    /// </summary>
    /// <param name="total"></param>
    /// <returns></returns>
    public static Turn CheckWinner(bool total)
    {
        int myScore = MyData.Score;
        int opponentScore = OpponentData.Score;
        if (total)
        {
            myScore = MyData.TotalScore;
            opponentScore = OpponentData.TotalScore;
        }

        if (myScore == opponentScore)
        {
            return Turn.None;
        }
        else if (myScore > opponentScore)
        {
            return MyTurn;
        }
        else
        {
            return OpponentTurn;
        }
    }
}
