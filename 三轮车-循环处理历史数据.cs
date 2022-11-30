
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D1175025110b1983fa346219bbd707d41dbe558: H3.SmartForm.SmartFormController
{
    public D1175025110b1983fa346219bbd707d41dbe558(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        H3.IEngine engine = this.Request.Engine;
        base.OnLoad(response);
        string SELECTSQL = "SELECT objectId AS OBJ, AfileID AS A, BfileID AS B, CfileID AS C, DfileID AS D FROM I_D117502Sogsss4gokiktazse3ourmyza6 WHERE name = '张之刚'";
        System.Data.DataTable Count = engine.Query.QueryTable(SELECTSQL, null);
        int num = Count.Rows.Count;
        for(int i = 0;i < num; i++)
        {
            string OBJ = Count.Rows[i]["OBJ"].ToString();
            string Aurl = Count.Rows[i]["A"].ToString() == "" ? "" : this.Request.UserContext.GetThumbnailUrl(Count.Rows[i]["A"].ToString());
            string Burl = Count.Rows[i]["B"].ToString() == "" ? "" : this.Request.UserContext.GetThumbnailUrl(Count.Rows[i]["B"].ToString());
            string Curl = Count.Rows[i]["C"].ToString() == "" ? "" : this.Request.UserContext.GetThumbnailUrl(Count.Rows[i]["C"].ToString());
            string Durl = Count.Rows[i]["D"].ToString() == "" ? "" : this.Request.UserContext.GetThumbnailUrl(Count.Rows[i]["D"].ToString());
            string UPDATESQL = "UPDATE I_D117502Sogsss4gokiktazse3ourmyza6 SET AThumbnailUrl = '" + Aurl + "', BThumbnailUrl = '" + Burl + "', CThumbnailUrl = '" + Curl + "', DThumbnailUrl = '" + Durl + "' WHERE objectId = '" + OBJ + "'";
            engine.Query.QueryTable(UPDATESQL, null);
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }
}