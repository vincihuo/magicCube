using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Network;

public class LoginV : MonoBehaviour
{
    public InputField inputPass;

    public InputField inputName;


    // Start is called before the first frame update
    void Start()
    {

        //LOGIN.ReqLogin login = new LOGIN.ReqLogin();
        //login.Passworld = "12313";
        //login.Usermame = "ะกอ๕";

        //byte[] vv= MsgTool.Serialize(login);
        //NetStream stream = new NetStream();

        ////int len = vv.Length;
        ////stream.WriteBytes(vv);
        //stream.WriteByte(1);
        //stream.WriteByte(2);
        //stream.WriteByte(3);
        //stream.WriteByte(4);
        //stream.WriteByte(5);
        //byte[] pp= stream.GetBuffer();
        //NetStream rr = new NetStream(pp);
        //byte[] pro = rr.ReadBytes(len);
        //byte b = rr.ReadByte();
        //rr.SetPos(0);
        //b = rr.ReadByte();
        //b = rr.ReadByte();
        //b = rr.ReadByte();
        //var mm= LOGIN.ReqLogin.Parser.ParseFrom(pro);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onLogin()
    {
        App.Ins.getModel<LoginModel>().SendLogin(inputName.text, inputPass.text);
    }
}
