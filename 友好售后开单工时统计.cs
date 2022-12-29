
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502FirstInsurance: H3.SmartForm.SmartFormController
{
    public D117502FirstInsurance(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
        if(actionName == "IncreaseMaintenanceItems")
        {
            // 增修工时 子表发生了变化
        }
    }
}