
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502974f1fad35864bb887fe751861c29e69_ListViewController: H3.SmartForm.ListViewController
{
    public D117502974f1fad35864bb887fe751861c29e69_ListViewController(H3.SmartForm.ListViewRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadListViewResponse response)
    {
        base.OnLoad(response);
        foreach(Dictionary < string, object > data in response.ReturnData)
        {
            string timesSPAN = data["ExecutionTime"].ToString();
            TimeSpan dt = TimeSpan.Parse(timesSPAN);
            long sss = Convert.ToInt64(dt.TotalMilliseconds);
            if(sss < 700)
            {
                data["ExecutionTime"] = new H3.SmartForm.ListViewCustomCell(data["ExecutionTime"], H3.SmartForm.Color.Green);
            }
            if(sss >= 700 && sss < 1000)
            {
                data["ExecutionTime"] = new H3.SmartForm.ListViewCustomCell(data["ExecutionTime"], H3.SmartForm.Color.Yellow);
            }
            if(sss >= 1000)
            {
                data["ExecutionTime"] = new H3.SmartForm.ListViewCustomCell(data["ExecutionTime"], H3.SmartForm.Color.Red);
            }
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }
    protected override void OnInit(H3.SmartForm.LoadListViewResponse response)
    {
        base.OnInit(response);
        if(response.Columns.ContainsKey("Name"))
        {
            response.Columns.Remove("Name");
        }
    }
}