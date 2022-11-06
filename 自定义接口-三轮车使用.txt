
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D150862567866aa29cb4cdf8445a8f845ae790e: H3.SmartForm.SmartFormController
{
    public D150862567866aa29cb4cdf8445a8f845ae790e(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }
}

public class MyApiController: H3.SmartForm.RestApiController
{
    public MyApiController(H3.SmartForm.RestApiRequest request): base(request) { }
    protected override void OnInvoke(string actionName, H3.SmartForm.RestApiResponse response)
    {
        try
        {
            if(actionName == "ZhaoZhaiZi") // 赵寨子
            {
                H3.IEngine engine = this.Request.Engine;
                string selectSQL = "SELECT COUNT(1) as num FROM I_D11750242f8dcf539074e93abb57a584208d56a WHERE ThumbnaiStatus = 'down' AND intervalStatus <> 'down'";
                System.Data.DataTable Count = engine.Query.QueryTable(selectSQL, null);
                int num = Convert.ToInt32(Count.Rows[0]["num"]);
                if(num > 0)
                {
                    string selectSQL2 = "SELECT objectId AS OBJ, AfileID AS A, BfileID AS B, CfileID AS C, DfileID AS D FROM I_D11750242f8dcf539074e93abb57a584208d56a WHERE ThumbnaiStatus = 'down' AND intervalStatus <> 'down'";
                    System.Data.DataTable RES = engine.Query.QueryTable(selectSQL2, null);
                    for(int i = 0;i < RES.Rows.Count; i++)
                    {
                        if(RES.Rows[i]["A"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["A"].ToString(), true);
                        }
                        if(RES.Rows[i]["B"].ToString() != "")
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["B"].ToString(), true);
                        }
                        if(RES.Rows[i]["C"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["C"].ToString(), true);
                        }
                        if(RES.Rows[i]["D"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["D"].ToString(), true);
                        }
                        engine.Query.QueryTable("UPDATE I_D11750242f8dcf539074e93abb57a584208d56a SET intervalStatus = 'down' WHERE objectId = '" + RES.Rows[i]["OBJ"].ToString() + "'", null);
                    }
                    string systemUserId = H3.Organization.User.SystemUserId;
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D150862567866aa29cb4cdf8445a8f845ae790e");
                    H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                    aBo.CreatedBy = systemUserId;
                    aBo["F0000001"] = DateTime.Now;
                    aBo["F0000002"] = num;
                    aBo["F0000003"] = "赵寨子中队";
                    aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                    aBo.Create();
                }
                else
                {
                    string systemUserId = H3.Organization.User.SystemUserId;
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D150862567866aa29cb4cdf8445a8f845ae790e");
                    H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                    aBo.CreatedBy = systemUserId;
                    aBo["F0000001"] = DateTime.Now;
                    aBo["F0000002"] = num;
                    aBo["F0000003"] = "赵寨子中队";
                    aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                    aBo.Create();
                }
                response.ReturnData.Add("result", "success");
            }

            if(actionName == "LiangCun") // 梁村中队
            {
                H3.IEngine engine = this.Request.Engine;
                string selectSQL = "SELECT COUNT(1) as num FROM I_D150862Sro8gdnf971a6wezhd5xgviws0 WHERE ThumbnaiStatus = 'down' AND intervalStatus <> 'down'";
                System.Data.DataTable Count = engine.Query.QueryTable(selectSQL, null);
                int num = Convert.ToInt32(Count.Rows[0]["num"]);
                if(num > 0)
                {
                    string selectSQL2 = "SELECT objectId AS OBJ, AfileID AS A, BfileID AS B, CfileID AS C, DfileID AS D FROM I_D150862Sro8gdnf971a6wezhd5xgviws0 WHERE ThumbnaiStatus = 'down' AND intervalStatus <> 'down'";
                    System.Data.DataTable RES = engine.Query.QueryTable(selectSQL2, null);
                    for(int i = 0;i < RES.Rows.Count; i++)
                    {
                        if(RES.Rows[i]["A"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["A"].ToString(), true);
                        }
                        if(RES.Rows[i]["B"].ToString() != "")
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["B"].ToString(), true);
                        }
                        if(RES.Rows[i]["C"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["C"].ToString(), true);
                        }
                        if(RES.Rows[i]["D"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["D"].ToString(), true);
                        }
                        engine.Query.QueryTable("UPDATE I_D150862Sro8gdnf971a6wezhd5xgviws0 SET intervalStatus = 'down' WHERE objectId = '" + RES.Rows[i]["OBJ"].ToString() + "'", null);
                    }
                    string systemUserId = H3.Organization.User.SystemUserId;
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D150862567866aa29cb4cdf8445a8f845ae790e");
                    H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                    aBo.CreatedBy = systemUserId;
                    aBo["F0000001"] = DateTime.Now;
                    aBo["F0000002"] = num;
                    aBo["F0000003"] = "梁村中队";
                    aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                    aBo.Create();
                }
                else
                {
                    string systemUserId = H3.Organization.User.SystemUserId;
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D150862567866aa29cb4cdf8445a8f845ae790e");
                    H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                    aBo.CreatedBy = systemUserId;
                    aBo["F0000001"] = DateTime.Now;
                    aBo["F0000002"] = num;
                    aBo["F0000003"] = "梁村中队";
                    aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                    aBo.Create();
                }
                response.ReturnData.Add("result", "success");
            }

            if(actionName == "JiangDian") // 姜店中队
            {
                H3.IEngine engine = this.Request.Engine;
                string selectSQL = "SELECT COUNT(1) as num FROM I_D150862Sd7t61t99yfmnmocnjnxw7z093 WHERE ThumbnaiStatus = 'down' AND intervalStatus <> 'down'";
                System.Data.DataTable Count = engine.Query.QueryTable(selectSQL, null);
                int num = Convert.ToInt32(Count.Rows[0]["num"]);
                if(num > 0)
                {
                    string selectSQL2 = "SELECT objectId AS OBJ, AfileID AS A, BfileID AS B, CfileID AS C, DfileID AS D FROM I_D150862Sd7t61t99yfmnmocnjnxw7z093 WHERE ThumbnaiStatus = 'down' AND intervalStatus <> 'down'";
                    System.Data.DataTable RES = engine.Query.QueryTable(selectSQL2, null);
                    for(int i = 0;i < RES.Rows.Count; i++)
                    {
                        if(RES.Rows[i]["A"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["A"].ToString(), true);
                        }
                        if(RES.Rows[i]["B"].ToString() != "")
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["B"].ToString(), true);
                        }
                        if(RES.Rows[i]["C"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["C"].ToString(), true);
                        }
                        if(RES.Rows[i]["D"].ToString() != "") 
                        {
                            engine.BizObjectManager.RemoveFile(RES.Rows[i]["D"].ToString(), true);
                        }
                        engine.Query.QueryTable("UPDATE I_D150862Sd7t61t99yfmnmocnjnxw7z093 SET intervalStatus = 'down' WHERE objectId = '" + RES.Rows[i]["OBJ"].ToString() + "'", null);
                    }
                    string systemUserId = H3.Organization.User.SystemUserId;
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D150862567866aa29cb4cdf8445a8f845ae790e");
                    H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                    aBo.CreatedBy = systemUserId;
                    aBo["F0000001"] = DateTime.Now;
                    aBo["F0000002"] = num;
                    aBo["F0000003"] = "姜店中队";
                    aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                    aBo.Create();
                }
                else
                {
                    string systemUserId = H3.Organization.User.SystemUserId;
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D150862567866aa29cb4cdf8445a8f845ae790e");
                    H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                    aBo.CreatedBy = systemUserId;
                    aBo["F0000001"] = DateTime.Now;
                    aBo["F0000002"] = num;
                    aBo["F0000003"] = "姜店中队";
                    aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                    aBo.Create();
                }
                response.ReturnData.Add("result", "success");
            }
        } catch(Exception ex)
        {
            response.ReturnData.Add("result", "error");
            response.ReturnData.Add("message", ex.Message);
        }
    }
}