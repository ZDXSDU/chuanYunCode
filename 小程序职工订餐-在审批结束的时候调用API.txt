
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502ad: H3.SmartForm.SmartFormController
{
    public D117502ad(H3.SmartForm.SmartFormRequest request): base(request)
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
    }
    protected override void OnWorkflowInstanceStateChanged(H3.Workflow.Instance.WorkflowInstanceState   oldState, H3.Workflow.Instance.WorkflowInstanceState newState)
    {
        if(oldState == H3.Workflow.Instance.WorkflowInstanceState.Running && newState == H3.Workflow.Instance.WorkflowInstanceState.Finished) // 流程审批结束事件（先执行业务规则，在执行该方法）。
        {
            /*
            * TO: 李杰
            * 0. 在这里ru(三声)
            * 1. 流程为结束状态
            * 2. 控件“是否启用”为是的
            * 3. 送至OSS
            * 4. FROM: ZDX SDU
            */
            H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
            H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
            H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
            Dictionary < string, string > querys=new Dictionary<string, string>();
            string objectid = this.Request.BizObjectId;//主键
            querys.Add("objectid", objectid);
            //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
            H3.BizBus.InvokeResult InResult = this.Engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
                "adImages", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);

        }
        if(oldState == H3.Workflow.Instance.WorkflowInstanceState.Finished && newState == H3.Workflow.Instance.WorkflowInstanceState.Running) // 流程审批结束后，重新激活流程（先执行业务规则，在执行该方法）。
        {
            /*
            * TO: 李杰
            * 这里用不到的，咱们不会有重新激活流程的，我在流程里面放了不同意的时候直接作废并且不再发起
            * FROM: ZDX SDU
            */
        }
        base.OnWorkflowInstanceStateChanged(oldState, newState);
    }
}