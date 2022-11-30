class Med1
{
    string dbCode = "";//要查的字段名
    string controlCode = "";// 当前表的字段名
    public Med1(string dbCode, string controlCode)
    {
        this.dbCode = dbCode;
        this.controlCode = controlCode;
    }
    public string DbCode
    {
        set { dbCode = value; }
        get { return dbCode; }
    }
    public string ControlCode
    {
        set { controlCode = value; }
        get { return controlCode; }
    }

}




--------------------------------------------------
System.Data.DataTable dt = this.Engine.Query.QueryTable("select * from i_D117502SummaryOfST where DateBelong = '2021/12/01'", null);
        if(dt != null && dt.Rows.Count > 0)
        {
            for(int i = 0;i < lst.Count; i++)
            {
                SetMed1(dt, lst[i].DbCode, lst[i].ControlCode);
            }
        }