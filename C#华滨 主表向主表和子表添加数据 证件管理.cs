
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502DocumentEntryYT: H3.SmartForm.SmartFormController
{
    public D117502DocumentEntryYT(H3.SmartForm.SmartFormRequest request): base(request)
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

        /* 《向人员证件信息的表单中新增或修改数据》 */

        if(this.Request.BizObject.Status == H3.DataModel.BizObjectStatus.Draft)
        {
            /*            《新增提交》
            * 0 用SQL查出“油田相关证件”的证件大类的objectid
            * 1 检查“人员证件信息”中是否已经有了这个人的信息
            * 2 人如果没有，那么使用业务对象new主表的人和子表的证
            * 3 人如果有，那么使用业务对象new子表的证
            * PRO TIPS: 避免更改数据造成主键找不到，子表赋值list之前应该用SQL查询一遍obj
            *                           -- ZDX SDU
            */
            string thisemem = this.Request.BizObject["emem"].ToString(); // 当前表单中的人员ID
            string selectSQL = "SELECT COUNT(1) AS num, ObjectId, toalGetDocumentsPey FROM I_D11750293ed5b17b80a4c6b8c58d3767b99a2a3 WHERE F0000036 = '" + thisemem + "'";
            System.Data.DataTable Count = this.Request.Engine.Query.QueryTable(selectSQL, null);
            int num = Convert.ToInt32(Count.Rows[0]["num"]);
            int toalGetDocumentsPey = 0; // 取证费用合计 遍历赋值 业务对象更新到人员证件管理的子表
            H3.DataModel.BizObject[] details = (H3.DataModel.BizObject[]) this.Request.BizObject["D117502documentReview"]; // 获取子表属性并强制转换为对象数组
            for(int i = 0;i < details.Length; i++)
            {
                toalGetDocumentsPey += Convert.ToInt32(details[i]["GetDocumentsPey"]);
            }
            if(num == 0)
            {
                string selectSQL2 = "SELECT ObjectId, certificateCategory AS name FROM I_D117502EquipmentStatistics WHERE certificateCategory = '油田相关证件'";
                System.Data.DataTable certificateCategory = this.Request.Engine.Query.QueryTable(selectSQL2, null);
                string certificateCategoryOBJ = certificateCategory.Rows[0]["ObjectId"].ToString();
                string systemUserId = H3.Organization.User.SystemUserId;
                H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D11750293ed5b17b80a4c6b8c58d3767b99a2a3");
                H3.DataModel.BizObject obj = new H3.DataModel.BizObject(this.Request.Engine, schema, this.Request.UserContext.UserId);
                obj.CreatedBy = systemUserId;
                obj["F0000045"] = this.Request.BizObject["firm"]; // 公司
                obj["F0000046"] = this.Request.BizObject["department"]; // 部门
                obj["F0000036"] = this.Request.BizObject["emem"]; // 人员
                obj["toalGetDocumentsPey"] = toalGetDocumentsPey; // 取证费用合计
                List < H3.DataModel.BizObject > lstObject = new List<H3.DataModel.BizObject>(); // new子表数据集合
                H3.DataModel.BizObject zibiao = new H3.DataModel.BizObject(this.Request.Engine, schema.GetChildSchema("D117502Ff79d6e5c3dbc41618862e5f14a2559ce"), H3.Organization.User.SystemUserId); // 子表对象
                zibiao["F0000029"] = certificateCategoryOBJ; // 证件大类
                zibiao["F0000030"] = this.Request.BizObject["documentName"]; // 证件名称
                zibiao["F0000043"] = this.Request.BizObject["SegmentCategories"]; // 细分类别
                zibiao["F0000033"] = details[details.Length - 1]["documentReviewDate"]; // 发证日期
                zibiao["F0000013"] = details[details.Length - 1]["expireNew"]; // 证件到期时间
                zibiao["IssuingAuthority1"] = this.Request.BizObject["IssuingAuthority"]; // 发证机关
                zibiao["GetDocumentsPey"] = toalGetDocumentsPey; // 取证费用合计
                zibiao["F0000044"] = this.Request.BizObject["remark"].ToString(); // 子表的备注
                zibiao["F0000047"] = certificateCategory.Rows[0]["name"].ToString(); // 证件大类T
                zibiao["F0000048"] = this.Request.BizObject["documentNameT"].ToString(); // 证件名称T
                zibiao["F0000049"] = this.Request.BizObject["SegmentCategoriesT"].ToString(); // 细分类别T
                lstObject.Add(zibiao); // 将这个子表业务对象添加至子表数据集合中
                obj["D117502Ff79d6e5c3dbc41618862e5f14a2559ce"] = lstObject.ToArray(); // 子表数据赋值
                obj.Status = H3.DataModel.BizObjectStatus.Effective;
                obj.Create(); // 创建对象
            }
            else
            {
                string selectSQL2 = "SELECT ObjectId, certificateCategory AS name FROM I_D117502EquipmentStatistics WHERE certificateCategory = '油田相关证件'";
                System.Data.DataTable certificateCategory = this.Request.Engine.Query.QueryTable(selectSQL2, null);
                string certificateCategoryOBJ = certificateCategory.Rows[0]["ObjectId"].ToString();
                H3.IEngine engine = this.Engine;
                string masterSchemaCode = "D11750293ed5b17b80a4c6b8c58d3767b99a2a3";
                string masterBoId = Count.Rows[0]["ObjectId"].ToString(); // 查出来的有人的人员证件管理中的对应数据的OBJ 【表单数据Id】
                string childSchemaCode = "D117502Ff79d6e5c3dbc41618862e5f14a2559ce"; // 子表控件编码
                H3.DataModel.BizObject masterBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, engine, masterSchemaCode, masterBoId, false); // 主表业务对象
                H3.DataModel.BizObjectSchema childSchema = masterBo.Schema.GetChildSchema(childSchemaCode); // 子表结构体对象
                List < H3.DataModel.BizObject > newChildBoList = new List<H3.DataModel.BizObject>(); // 定义新的子表数据集合
                H3.DataModel.BizObject[] childBoArray = (H3.DataModel.BizObject[]) masterBo[childSchemaCode]; // 获取子表内已有数据
                if(childBoArray != null && childBoArray.Length > 0)
                {
                    foreach(H3.DataModel.BizObject itemBo in childBoArray)
                    {
                        newChildBoList.Add(itemBo); // 将子表内已有数据循环添加到新的子表数据集合里
                    }
                }
                // new一个子表业务对象，并添加到子表数据第一行
                H3.DataModel.BizObject childBo1 = new H3.DataModel.BizObject(engine, childSchema, H3.Organization.User.SystemUserId);
                childBo1["F0000029"] = certificateCategoryOBJ; // 证件大类
                childBo1["F0000030"] = this.Request.BizObject["documentName"]; // 证件名称
                childBo1["F0000043"] = this.Request.BizObject["SegmentCategories"]; // 细分类别
                childBo1["F0000044"] = this.Request.BizObject["remark"].ToString(); // 子表的备注
                childBo1["F0000033"] = details[details.Length - 1]["documentReviewDate"]; // 发证日期
                childBo1["F0000013"] = details[details.Length - 1]["expireNew"]; // 证件到期时间
                childBo1["GetDocumentsPey"] = toalGetDocumentsPey; // 取证费用合计
                childBo1["IssuingAuthority1"] = this.Request.BizObject["IssuingAuthority"]; // 发证机关                
                childBo1["F0000047"] = certificateCategory.Rows[0]["name"].ToString(); // 证件大类T
                childBo1["F0000048"] = this.Request.BizObject["documentNameT"].ToString(); // 证件名称T
                childBo1["F0000049"] = this.Request.BizObject["SegmentCategoriesT"].ToString(); // 细分类别T
                newChildBoList.Insert(0, childBo1);
                masterBo[childSchemaCode] = newChildBoList.ToArray(); // 将新的子表数据集合赋值到子表控件
                masterBo["toalGetDocumentsPey"] = toalGetDocumentsPey + Convert.ToInt32(Count.Rows[0]["toalGetDocumentsPey"]); // 主表 取证费用合计
                masterBo.Update();  // 修改主表业务对象，系统会自动识别出上面子表数据被修改了，执行完Update方法，新的子表数据就会被保存到数据库
            }
        }

        if(this.Request.BizObject.Status == H3.DataModel.BizObjectStatus.Effective)
        {

        }
        base.OnSubmit(actionName, postValue, response);
    }
}