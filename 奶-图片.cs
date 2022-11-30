base.OnSubmit(actionName, postValue, response);
        string schemacode = "D117502MerchandiseManagement"; // 当前表单的表单编码
        string propertyname = "pic"; // 图片附件的控件编码【主题图片】
        string bizobjectid = this.Request.BizObjectId; // 当前数据的OBJ
        
        string selectSql = "SELECT objectid, contentlength FROM H_bizobjectfile WHERE schemacode = '" + schemacode + "' AND propertyname = '" + propertyname + "' AND bizobjectid = '" + bizobjectid + "'";
        System.Data.DataTable Count = this.Engine.Query.QueryTable(selectSql, null);
        int contentlength = Convert.ToInt32(Count.Rows[0]["contentlength"]); // 图片的大小（单位：字节）
        string fileId = Count.Rows[0]["contentlength"].ToString(); // 主题图片的附件ID       

        if(contentlength > 200000)
        {
            // 图片的大小超过200KB 将 是否启用 改为否
            String updateSql = "UPDATE I_D117502MerchandiseManagement SET isEnabled = 0, remark = '图片超出限制或不符合规范。当前体积：" + contentlength.ToString() + "；' WHERE objectid = '" + bizobjectid + "'";
            this.Engine.Query.QueryTable(updateSql, null);
        }
        else
        {
            // 图片尺寸不超200Kb 修改附件Id 以供接口调用 同时将 是否启用 改为是
            String updateSql = "UPDATE I_D117502Sa7su0q957ojv0aa6g3y2twrb1 SET fileId = '" + fileId + "', isEnable = 1, WHERE objectid = '" + bizobjectid + "'";
            this.Engine.Query.QueryTable(updateSql, null);
        }