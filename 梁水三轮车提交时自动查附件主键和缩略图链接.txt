base.OnSubmit(actionName, postValue, response);
        string schemacode = "D11750242f8dcf539074e93abb57a584208d56a"; // 当前表单的表单编码
        string propertynameA = "F0000003"; // A图片附件的控件编码
        string propertynameB = "F0000004"; // B图片附件的控件编码
        string propertynameC = "F0000005"; // C图片附件的控件编码
        string propertynameD = "F0000006"; // D图片附件的控件编码
        string bizobjectid = this.Request.BizObjectId; // 当前数据的OBJ
        string AfileID = "";
        string BfileID = "";
        string CfileID = "";
        string DfileID = "";
        string AThumbnailUrl = "";
        string BThumbnailUrl = "";
        string CThumbnailUrl = "";
        string DThumbnailUrl = "";
        string selectSql = "SELECT objectid, propertyname FROM H_bizobjectfile WHERE schemacode = '" + schemacode + "' AND bizobjectid = '" + bizobjectid + "'";
        System.Data.DataTable Count = this.Engine.Query.QueryTable(selectSql, null);
        for(int i = 0;i < Count.Rows.Count; i++)
        {
            switch(Count.Rows[i]["propertyname"].ToString())
            {
                case "F0000003":
                    AfileID = Count.Rows[i]["objectid"].ToString();
                    break;
                case "F0000004":
                    BfileID = Count.Rows[i]["objectid"].ToString();
                    break;
                case "F0000005":
                    CfileID = Count.Rows[i]["objectid"].ToString();
                    break;
                case "F0000006":
                    DfileID = Count.Rows[i]["objectid"].ToString();
                    break;
            }
        }
        AThumbnailUrl = AfileID == "" ? "" : this.Request.UserContext.GetThumbnailUrl(AfileID);
        BThumbnailUrl = BfileID == "" ? "" : this.Request.UserContext.GetThumbnailUrl(BfileID);
        CThumbnailUrl = CfileID == "" ? "" : this.Request.UserContext.GetThumbnailUrl(CfileID);
        DThumbnailUrl = DfileID == "" ? "" : this.Request.UserContext.GetThumbnailUrl(DfileID);
        String updateSql = "UPDATE I_D11750242f8dcf539074e93abb57a584208d56a SET AfileID = '" + AfileID + "', BfileID = '" + BfileID + "', CfileID = '" + CfileID + "', DfileID = '" + DfileID + "', AThumbnailUrl = '" + AThumbnailUrl + "', BThumbnailUrl = '" + BThumbnailUrl + "', CThumbnailUrl = '" + CThumbnailUrl + "', DThumbnailUrl = '" + DThumbnailUrl + "' WHERE objectid = '" + bizobjectid + "'";
        this.Engine.Query.QueryTable(updateSql, null);