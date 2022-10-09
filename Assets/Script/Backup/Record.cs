// using System;
// using UnityEngine;
//
// public class Record : MonoBehaviour, ILoad
// {
//     #region - Variable Declaration (การประกาศตัวแปร) -
//
//     [Header("Attributes")]
//     public float x;
//     public float y;
//     public float size;
//
//     public float space;
//
//     public int recordLimiter;
//
//     [Header("Object Assign")]
//     //public static RecordInfo[] infos = new RecordInfo[1];
//     public RecordLine recordText;
//
//     public RectTransform puRect;
//     public Flag countryInfo;
//
//     [Header("Debug")]
//     public bool beforeLoop;
//     public bool enterLoop;
//     public bool exitLoop;
//     public bool returnPoint;
//     
//     private RectTransform prRect;
//
//     // private int[] _minute;
//     // private int[] _second;
//     // private int[] _millisecond;
//     // private string[] _name;
//     // private string[] _country;
//     // private Sprite[] _flag;
//     // private string[] _wholeTime;
//
//     private PlayerInfo[] _playerInfos;
//     public PlayerInfo[] PlayerInfos => _playerInfos;
//     
//     // public int[] Minute => _minute;
//     // public int[] Second => _second;
//     // public int[] Millisecond => _millisecond;
//     // public string[] Name => _name;
//     // public string[] Country => _country;
//     // public Sprite[] Flag => _flag;
//     // public string[] WholeTime => _wholeTime;
//
//     public static Record Instance;
//     
//     #endregion
//
//     #region - Unity's Method (คำสั่งของ Unity เอง) -
//
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(Instance);
//         }
//         else
//         {
//             Destroy(Instance);
//         }
//     }
//
//     #endregion
//
//     #region - Custom Method (คำสั่งที่เขียนขึ้นมาเอง) -
//
//     public void LoadData(GameData data)
//     {
//         int dataQuantity = data.time.Length;
//         
//         _playerInfos = new PlayerInfo[dataQuantity];
//         
//         Debug.Log(_playerInfos.Length);
//
//         // _minute = new int[dataQuantity];
//         // _second = new int[dataQuantity];
//         // _millisecond = new int[dataQuantity];
//         // _name = new string[dataQuantity];
//         // _country = new string[dataQuantity];
//         // _flag = new Sprite[dataQuantity];
//         // _wholeTime = new string[dataQuantity];
//
//         for (int i = 0; i < dataQuantity; i++)
//         {
//             if (string.IsNullOrEmpty(data.name[i]))
//             {
//                 Debug.Log("Return");
//                 return;
//             }
//             
//             Debug.Log($"i = {i}");
//             
//             _playerInfos[i].wholeTime = data.time[i].ToString();
//
//             switch (_playerInfos[i].wholeTime.Length)
//             {
//                 case 6:
//                     _playerInfos[i].minute = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(0, 2));
//                     _playerInfos[i].second = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(2, 2));
//                     _playerInfos[i].millisecond = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(4, 2));
//                     break;
//                 case 5:
//                     _playerInfos[i].minute = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(0, 1));
//                     _playerInfos[i].second = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(1, 2));
//                     _playerInfos[i].millisecond = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(3, 2));
//                     break;
//                 case 4:
//                     _playerInfos[i].second = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(0, 2));
//                     _playerInfos[i].millisecond = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(2, 2));
//                     break;
//                 case 3:
//                     _playerInfos[i].second = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(0, 1));
//                     _playerInfos[i].millisecond = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(1, 2));
//                     break;
//                 case 2:
//                     _playerInfos[i].millisecond = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(0, 2));
//                     break;
//                 case 1:
//                     _playerInfos[i].millisecond = Convert.ToInt32(_playerInfos[i].wholeTime.Substring(0, 1));
//                     break;
//             }
//
//             _playerInfos[i].name = data.name[i].Substring(3);
//             _playerInfos[i].country = data.name[i].Substring(0, 3).ToUpper();
//             _playerInfos[i].flag = FindFlag(_playerInfos[i].country);
//
//             Debug.Log($"Load Data #{i}: {_playerInfos[i].name} - {_playerInfos[i].wholeTime}");
//             
//             CreateLeaderboard();
//         }
//     }
//     
//     private void CreateLeaderboard()
//     {
//         Debug.Log("Start Create Leaderboard");
//         
//         prRect = puRect;
//         y = prRect.localPosition.y - space;
//
//         string[] word = new string[recordLimiter];
//         
//         if(beforeLoop)
//             Debug.Log("Before loop");
//         
//         for (int i = 0; i < recordLimiter; i++)
//         {
//             if(enterLoop)
//                 Debug.Log("Enter Loop");
//             
//             if (_playerInfos[i].name == null)
//             {
//                 if(returnPoint)
//                     Debug.Log("Return");
//                 
//                 return;
//             }
//             
//             word[i] =
//                 $"{_playerInfos[i].minute.ToString("00")}:{_playerInfos[i].second.ToString("00")}:{this._playerInfos[i].millisecond.ToString("00")} - {this._playerInfos[i].name}\n";
//
//             var obj = Instantiate(recordText, Vector2.zero, Quaternion.identity);
//             obj.transform.SetParent(this.transform);
//             RectTransform rect = obj.GetComponent<RectTransform>();
//             rect.localPosition = new Vector3(puRect.localPosition.x, y, 0);
//             rect.localScale = new Vector3(size, size, size);
//
//             RecordLine line = obj.GetComponent<RecordLine>();
//
//             line.SetRecord(word[i], _playerInfos[i].flag);
//
//             y = y - space;
//             
//             Debug.Log("Leaderboard Created");
//         }
//         
//         Debug.Log("Stop Create Leaderboard");
//     }
//
//     private Sprite FindFlag(string countryAbbreviate)
//     {
//         foreach (countryInfo info in countryInfo.country)
//         {
//             if (info.abbreviate.Trim().ToUpper() == countryAbbreviate.Trim().ToUpper())
//             {
//                 return info.flag;
//             }
//         }
//
//         return null;
//     }
//
//     #endregion
// }
//
// [Serializable]
// public class PlayerInfo
// {
//     public int minute;
//     public int second;
//     public int millisecond;
//     public string name;
//     public string country;
//     public Sprite flag;
//     public string wholeTime;
// }