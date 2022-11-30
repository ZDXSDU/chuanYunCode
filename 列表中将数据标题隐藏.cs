protected override void OnInit(H3.SmartForm.LoadListViewResponse response)
    {
        base.OnInit(response);
        if(response.Columns.ContainsKey("Name"))
        {
            response.Columns.Remove("Name");
        }
    }