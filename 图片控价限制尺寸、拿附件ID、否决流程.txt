
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502Sa7su0q957ojv0aa6g3y2twrb1: H3.SmartForm.SmartFormController
{
    public D117502Sa7su0q957ojv0aa6g3y2twrb1(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        if(this.Request.ActivityCode == "Activity3")
        {
            // 这里就是流程的审批状态
        };

        base.OnLoad(response);
        response.Actions.Remove("Remove");
        response.Actions.Remove("Print");
        response.Actions.Remove("ViewQrCode");

    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
        /*
        * 每创建一条数据 就把当前图片的附件编码查出来，同时检验附件的大小（如果图片大小超过200Kb 那么否决流程）
        */
        string schemacode = "D117502Sa7su0q957ojv0aa6g3y2twrb1"; // 当前表单的表单编码
        string propertyname = "adImages"; // 图片附件的控件编码
        string bizobjectid = this.Request.BizObjectId; // 当前数据的OBJ
        string selectSql = "SELECT objectid, contentlength FROM H_bizobjectfile WHERE schemacode = '" + schemacode + "' AND propertyname = '" + propertyname + "' AND bizobjectid = '" + bizobjectid + "'";
        System.Data.DataTable Count = this.Engine.Query.QueryTable(selectSql, null);
        int contentlength = Convert.ToInt32(Count.Rows[0]["contentlength"]); // 图片的大小（单位：字节）
        string fileId = Count.Rows[0]["objectid"].ToString(); // 图片的附件ID
        if(contentlength > 200000)
        {
            // 图片的大小超过200KB 否决流程 同时将 是否启用 改为否
            H3.Workflow.Messages.CancelInstanceMessage cancelMessage = new H3.Workflow.Messages.CancelInstanceMessage(this.Request.InstanceId, false);
            this.Request.Engine.WorkflowInstanceManager.SendMessage(cancelMessage);
            String updateSql = "UPDATE I_D117502Sa7su0q957ojv0aa6g3y2twrb1 SET isEnable = 0, remark = '图片超出限制或不符合规范，已被系统否决。当前体积：" + contentlength.ToString() + "' WHERE objectid = '" + bizobjectid + "'";
            this.Engine.Query.QueryTable(updateSql, null);
        }
        else
        {
            // 图片尺寸不超200Kb 修改附件Idd 以供接口调用 同时将 是否启用 改为是
            String updateSql = "UPDATE I_D117502Sa7su0q957ojv0aa6g3y2twrb1 SET fileId = '" + fileId + "', isEnable = 1 WHERE objectid = '" + bizobjectid + "'";
            this.Engine.Query.QueryTable(updateSql, null);
        }
    }
    protected override void OnWorkflowInstanceStateChanged(H3.Workflow.Instance.WorkflowInstanceState   oldState, H3.Workflow.Instance.WorkflowInstanceState newState)
    {
        if(oldState == H3.Workflow.Instance.WorkflowInstanceState.Running && newState == H3.Workflow.Instance.WorkflowInstanceState.Finished) // 流程审批结束事件（先执行业务规则，在执行该方法）。
        {
            // H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
            // H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
            // H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
            // Dictionary < string, string > querys=new Dictionary<string, string>();
            // string objectid = this.Request.BizObjectId;//主键
            // querys.Add("objectid", objectid);
            // //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
            // H3.BizBus.InvokeResult InResult = this.Engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
            //     "adImages", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);

        }
        if(oldState == H3.Workflow.Instance.WorkflowInstanceState.Finished && newState == H3.Workflow.Instance.WorkflowInstanceState.Running) // 流程审批结束后，重新激活流程（先执行业务规则，在执行该方法）。
        {
        }
        base.OnWorkflowInstanceStateChanged(oldState, newState);
    }
}