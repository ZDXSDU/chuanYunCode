protected override void OnWorkflowInstanceStateChanged(H3.Workflow.Instance.WorkflowInstanceState oldState, H3.Workflow.Instance.WorkflowInstanceState newState)
    {
        
          
        if(oldState == H3.Workflow.Instance.WorkflowInstanceState.Running && newState == H3.Workflow.Instance.WorkflowInstanceState.Canceled)
        {
            H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Request.Engine, "D117502ElectronicRoadList1", this.Request.BizObjectId, false);
            accountBo["statusNow"] = "已取消";
            accountBo.Status = H3.DataModel.BizObjectStatus.Effective;
            accountBo.Update();
        }
        base.OnWorkflowInstanceStateChanged(oldState, newState);
    }