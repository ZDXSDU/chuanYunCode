
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502d3f67c8bb71d453c8d8942b37b4eee7f_ListViewController: H3.SmartForm.ListViewController
{
    public D117502d3f67c8bb71d453c8d8942b37b4eee7f_ListViewController(H3.SmartForm.ListViewRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadListViewResponse response)
    {
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
        if(actionName == "smallLabel") //打印小标签
        {
            response.ReturnData = new Dictionary<string, object>();
            string OBJs = this.Request["OBJs"] + string.Empty;
            if(string.IsNullOrEmpty(OBJs))
            {
                response.ReturnData.Add("Message", "AJAX 未收到任何数据！");
                return;
            }
            else
            {
                string[] ObjectIds = OBJs.Split(',');
                List < Dictionary < string, object >> RES = new List<Dictionary<string, object>>();
                H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Request.Engine, "D117502d3f67c8bb71d453c8d8942b37b4eee7f", ObjectIds[0], false);
                for(int i = 0;i < ObjectIds.Length; i++)
                {
                    accountBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Request.Engine, "D117502d3f67c8bb71d453c8d8942b37b4eee7f", ObjectIds[i], false);
                    RES.Add(new Dictionary < string, object >
                        {
                            { "PartNo", string.IsNullOrEmpty(accountBo["PartNoT"].ToString()) ? "--" : accountBo["PartNoT"].ToString() }, // 配件号
                            { "PartName", string.IsNullOrEmpty(accountBo["Nameofaccessories"].ToString()) ? "--" : accountBo["Nameofaccessories"].ToString() }, // 配件名称
                            { "Cargospace", string.IsNullOrEmpty(accountBo["CargospaceT"].ToString()) ? "--" : accountBo["CargospaceT"].ToString() }, // 货位号
                            { "Model", string.IsNullOrEmpty(accountBo["Model"].ToString()) ? "--" : accountBo["Model"].ToString() }, // 适用车型
                            { "OutPrice", string.IsNullOrEmpty(accountBo["F0000006"].ToString()) ? "--" : accountBo["F0000006"].ToString() }, // 出库价
                        }
                    );
                }
                response.ReturnData.Add("res", RES);
            }
        }
        if(actionName == "BigLabel") // 打印大标签
        {
            response.ReturnData = new Dictionary<string, object>();
            string OBJs = this.Request["OBJs"] + string.Empty;
            if(string.IsNullOrEmpty(OBJs))
            {
                response.ReturnData.Add("Message", "AJAX 未收到任何数据！");
                return;
            }
            else
            {
                string[] ObjectIds = OBJs.Split(',');
                List < Dictionary < string, object >> RES = new List<Dictionary<string, object>>();
                H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Request.Engine, "D117502d3f67c8bb71d453c8d8942b37b4eee7f", ObjectIds[0], false);
                for(int i = 0;i < ObjectIds.Length; i++)
                {
                    accountBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Request.Engine, "D117502d3f67c8bb71d453c8d8942b37b4eee7f", ObjectIds[i], false);
                    RES.Add(new Dictionary < string, object >
                        {
                            { "PartNo", string.IsNullOrEmpty(accountBo["PartNoT"].ToString()) ? "--" : accountBo["PartNoT"].ToString() }, // 配件号
                            { "PartName", string.IsNullOrEmpty(accountBo["Nameofaccessories"].ToString()) ? "--" : accountBo["Nameofaccessories"].ToString() }, // 配件名称
                            { "Cargospace", string.IsNullOrEmpty(accountBo["CargospaceT"].ToString()) ? "--" : accountBo["CargospaceT"].ToString() }, // 货位号
                            { "Model", string.IsNullOrEmpty(accountBo["Model"].ToString()) ? "--" : accountBo["Model"].ToString() }, // 适用车型
                            { "OutPrice", string.IsNullOrEmpty(accountBo["F0000006"].ToString()) ? "--" : accountBo["F0000006"].ToString() }, // 出库价
                            // { "remarks", string.IsNullOrEmpty(accountBo["F0000002"].ToString()) ? "--" : accountBo["F0000002"].ToString() }, // 备注
                            { "BU", string.IsNullOrEmpty(accountBo["BusinessDivisionT"].ToString()) ? "--" : accountBo["BusinessDivisionT"].ToString() }, // 事业部
                            { "BR", string.IsNullOrEmpty(accountBo["BranchT"].ToString()) ? "--" : accountBo["BranchT"].ToString() }, // 分店
                        }
                    );
                }
                response.ReturnData.Add("res", RES);
            }
        }
    }
}