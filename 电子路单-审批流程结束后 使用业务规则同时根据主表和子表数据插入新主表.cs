using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502ElectronicRoadList1: H3.SmartForm.SmartFormController
{
    public D117502ElectronicRoadList1(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
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
            H3.IEngine engine = this.Request.Engine;
            string systemUserId = H3.Organization.User.SystemUserId;
            string user = this.Request.BizObject["OwnerId"].ToString(); // 派车单位拥有者，负责人
            string creteUser = this.Request.BizObject["CreatedBy"].ToString(); // 用车单位的数据创建人
            string useINC = this.Request.BizObject["carUnit"].ToString(); // 用车单位 关联主键
            string carINC = this.Request.BizObject["carDispatchUnit"].ToString(); // 派车单位 关联主键
            string OwnerDeptId = this.Request.BizObject["OwnerDeptId"].ToString(); // 用车单位 所属部门
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502total");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            H3.DataModel.BizObject[] details = (H3.DataModel.BizObject[]) this.Request.BizObject["D117502details1"]; //获取子表属性并强制转换为对象数组
            if(details != null && details.Length > 0)
            {
                foreach(H3.DataModel.BizObject detail in details)
                {
                    string objectid = detail["ObjectId"] + string.Empty;
                    aBo.CreatedBy = systemUserId;
                    aBo.OwnerId = user;
                    aBo.OwnerDeptId = OwnerDeptId;
                    aBo["ObjectId"] = objectid;
                    aBo["useCarInc"] = useINC;
                    aBo["carInc"] = carINC;
                    aBo["crete"] = creteUser;
                    aBo["address"] = detail["address"] + string.Empty;
                    aBo["carModelT"] = detail["car"] + string.Empty;
                    aBo["carId"] = detail["licensePlateNumber"] + string.Empty;
                    aBo["dirver"] = detail["driver"] + string.Empty;
                    aBo["amoust"] = detail["amout"];
                    aBo["beginTime"] = detail["startingTime"] + string.Empty;
                    aBo["endTime"] = detail["endTime"] + string.Empty;
                    aBo["time"] = detail["F0000005"];
                    aBo["time1"] = detail["duration"];
                    aBo["class"] = detail["class"];
                    aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                    aBo.Create();

                    this.Request.Engine.BizObjectManager.CopyFiles("D117502ElectronicRoadList1", "D117502details1", "livePictures", "objectid", "D117502total", "", "pic", "objectid", true, true);
                }
            }
        }
        base.OnWorkflowInstanceStateChanged(oldState, newState);
    }
}