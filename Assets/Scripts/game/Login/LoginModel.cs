using Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginModel : BaseModel
{
    public string localAcc;
    public string localPass;
    public bool isReconnect=false;
    public event EventHandler loginFail;
    public LoginModel()
    {
        App.Ins.net.Register(MsgMainID.C2L , MsgEnum.LOGIN, ResLogin);
    }

    private void ResLogin(byte[] param)
    {
        throw new NotImplementedException();
    }

    private enum MsgEnum: byte
    {
        LOGIN = 1,
        ENTERGAME = 3,
    }
    public void SendLogin(string name,string pass)
    {
        LOGIN.ReqLogin login = new LOGIN.ReqLogin();
        login.Usermame = name;
        login.Passworld = pass;
        App.Ins.net.SendMessage(MsgMainID.C2L, MsgEnum.LOGIN, login);
    }
}
