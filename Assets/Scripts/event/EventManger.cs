using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �¼�����
/// </summary>
/// <param name="key"></param>
/// <param name="param"></param>
public delegate void EventMgr(byte[] param);

/// <summary>
/// ��Ա�����ӿ�
/// </summary>
public interface IEventMgr
{
    void Register(string key, EventMgr eventMgr);//ע���¼�

    void UnRegister(string key, EventMgr eventMgr);//����¼�

    void ClearAll();//��������¼�
    void Invoke(string key, byte[] param);//����
}



/// <summary>
/// time:2019/6/7 22:14
/// author:Sun
/// des:�¼�����
///
/// github:https://github.com/KingSun5
/// csdn:https://blog.csdn.net/Mr_Sun88
/// </summary>
public class EventManager : IEventMgr
{

    /// <summary>
    /// �洢ע��õ��¼�
    /// </summary>
    protected readonly Dictionary<string, List<EventMgr>> EventListerDict = new Dictionary<string, List<EventMgr>>();

    /// <summary>
    /// �Ƿ���ͣ���е��¼�
    /// </summary>
    public bool IsPause = false;

    /// <summary>
    /// ע���¼�
    /// </summary>
    /// <param name="key"></param>
    /// <param name="eventMgr"></param>
    public void Register(string key, EventMgr eventMgr)
    {
        if (EventListerDict.ContainsKey(key))
        {
            EventListerDict[key].Add(eventMgr);
        }
        else
        {
            List<EventMgr> list = new List<EventMgr>();
            list.Add(eventMgr);
            EventListerDict.Add(key, list);
        }
    }

    /// <summary>
    /// ȡ���¼���
    /// </summary>
    /// <param name="key"></param>
    public void UnRegister(string key, EventMgr eventMgr)
    {
        if (EventListerDict != null && EventListerDict.ContainsKey(key))
        {
            List<EventMgr> list = EventListerDict[key];
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Equals(eventMgr))
                {
                    list.RemoveAt(i);
                }
            }
            Debug.Log("�Ƴ��¼���" + key);
        }
        else
        {
            Debug.LogError("Key:" + key + "�����ڣ�");
        }
    }

    /// <summary>
    /// ȡ�������¼���
    /// </summary>
    public void ClearAll()
    {
        if (EventListerDict != null)
        {
            EventListerDict.Clear();
            Debug.Log("���ע���¼���");
        }
    }

    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="key"></param>
    /// <param name="param"></param>
    public void Invoke(string key, byte[] param)
    {
        if (!IsPause)
        {
            if (EventListerDict.ContainsKey(key))
            {
                foreach (EventMgr eventMgr in EventListerDict[key]) 
                {
                    eventMgr.Invoke(param);
                }
            }
            else
            {
                Debug.LogError("�¼���" + key + "δע�ᣡ");
            }
        }
        else
        {
            Debug.LogError("�����¼�����ͣ��");
        }

    }


    #region ����

    /*
	//    public delegate void OnEvent<in T1, in T2>();
    /// <summary>
    /// ����ע��õ��¼�
    /// </summary>
//    protected Dictionary<string,Action> ActionDict0 = new Dictionary<string, Action>();
//    protected Dictionary<string,Action<T>> ActionDict1 = new Dictionary<string, Action<T>>();
//    protected Dictionary<string,Action> ActionDict2 = new Dictionary<string, Action>();
    /// <summary>
    /// �Ƿ���ͣ���е��¼�
    /// </summary>
    public static bool IsPause = false;
    /// <summary>
    /// ע���¼��޲���
    /// </summary>
    /// <param name="eventName"></param> �¼���
    /// <param name="func"></param> ����
    /// <typeparam name="T"></typeparam>
    public static void Register(string eventName, Action func)
    {
        
    }
    
    /// <summary>
    /// ע���¼�һ������
    /// </summary>
    /// <param name="eventName"></param> �¼���
    /// <param name="func"></param> ����
    /// <typeparam name="T"></typeparam>
    public static void Register<T>(string eventName, Action<T> func)
    {
    }
    
    /// <summary>
    /// ע���¼���������
    /// </summary>
    /// <param name="eventName"></param> �¼���
    /// <param name="func"></param> ����
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public static void Register<T1,T2>(string eventName, Action<T1,T2> func)
    {
       
    }
    /// <summary>
    /// ע���¼�,������ֵ���޲���
    /// </summary>
    /// <param name="eventName"></param> �¼���
    /// <param name="func"></param> ����
    /// <typeparam name="T"></typeparam>
    public static void Register<T>(string eventName, Func<T> func)
    {
       
    }
    
    /// <summary>
    /// ע���¼���������ֵ��1������
    /// </summary>
    /// <param name="eventName"></param> �¼���
    /// <param name="func"></param> ����
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public static void Register<T1,T2>(string eventName, Func<T1,T2> func)
    {
       
    }
    /// <summary>
    /// ע���¼���������ֵ��2������
    /// </summary>
    /// <param name="eventName"></param> �¼���
    /// <param name="func"></param> ����
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public static void Register<T1,T2,T3>(string eventName, Func<T1,T2,T3> func)
    {
       
    }
    
 
  
    /// <summary>
    /// ȡ���¼���
    /// </summary>
    /// <param name="eventName"></param>
    public static void UnRegister(string eventName)
    {
       
    }
    
 
    /// <summary>
    /// ȡ�������¼���
    /// </summary>
    public static void ClearAll()
    {
        
    }
    /// <summary>
    /// ��ͣ�����¼�����
    /// </summary>
    public static void Pause()
    {
        
    }
    /// <summary>
    /// �¼����Ƿ�ע���
    /// </summary>
    /// <param name="eventName"></param>
    public static void IsRegisterName(string eventName)
    {
        
    }
    /// <summary>
    /// �����Ƿ�ע���
    /// </summary>
    /// <param name="func"></param>
    /// <typeparam name="T"></typeparam>
    public static void IsRegisterFunc<T>(T func)
    {
        
    }
    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="eventName"></param>
    public static void Invoke(string eventName)
    {
        
    }
    
    /// <summary>
    /// �����¼�������ֵ
    /// </summary>
    /// <param name="eventName"></param>
    public static T Invoke<T>(string eventName) where T : new()
    {
        return new T();
    }
    /// <summary>
    /// �����¼������Ȳ���
    /// </summary>
    /// <param name="eventName"></param>
    public static void Invoke(string eventName,params object[] objs)
    {
        
    }
    
    /// <summary>
    /// �����¼������Ȳ����з���ֵ
    /// </summary>
    /// <param name="eventName"></param>
    public static T Invoke<T>(string eventName,params object[] objs) where T : new()
    {
        return new T();
    }
    
    */

    #endregion



}