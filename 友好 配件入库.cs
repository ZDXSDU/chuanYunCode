
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502193aa0bb6bfd46068889878c7309d74d: H3.SmartForm.SmartFormController
{
    public D117502193aa0bb6bfd46068889878c7309d74d(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if(this.Request.BizObject["InType"].ToString() == "采购入库")
        {
            string BU = this.Request.BizObject["BusinessDivision"].ToString();
            string ST = this.Request.BizObject["store"].ToString();
            string BR = this.Request.BizObject["Branch"].ToString();
            H3.IEngine engine = this.Request.Engine;//得到一个iengine的参数
            string systemUserId = H3.Organization.User.SystemUserId;//获取系统虚拟用户的人员ID
            string currentUserId = this.Request.UserContext.UserId;//获得当前登录人的人员ID
            bool type = this.Request.IsCreateMode;//判断当前是否创建模式
            if(type)//判断是创建模式
            {
                H3.DataModel.BizObject[] details = (H3.DataModel.BizObject[]) this.Request.BizObject["D117502detailsOfIn"];//把子表的数据存在details[]
                if(details != null && details.Length > 0)//判断子表数据不等于空存储的长度大于0
                {
                    H3.DataModel.BulkCommit commit = new H3.DataModel.BulkCommit();//批量操作
                    H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502d3f67c8bb71d453c8d8942b37b4eee7f");//库存明细
                    string  S1 = "SELECT objectId AS OBJ FROM I_D1175025db04169747948658cdde2231ac4bd2c WHERE  max <= 90 AND min < 0";//获取的挤压天数区间的区间名称
                    System.Data.DataTable S1Value = this.Request.Engine.Query.QueryTable(S1, null);//自定义sql
                    string backlogDays = S1Value.Rows[0]["OBJ"].ToString();
                    foreach(H3.DataModel.BizObject detail in details)
                    {
                        string warehouse = detail["warehouse"] + string.Empty;
                        string PartNumber = detail["PartNumber"] + string.Empty;
                        string S2 = "SELECT Categoryofaccessories AS C1, F0000003 AS C2, Accessories AS C3, Deliveryprice AS APRICE FROM I_D11750283bfe708de2c42799b86babb9d98a488 WHERE objectId = '" + PartNumber + "'";
                        System.Data.DataTable S2Value = this.Request.Engine.Query.QueryTable(S2, null);
                        string C1 = S2Value.Rows[0]["C1"] + string.Empty;
                        string C2 = S2Value.Rows[0]["C2"] + string.Empty;
                        string C3 = S2Value.Rows[0]["C3"] + string.Empty;
                        decimal APRICE = Convert.ToInt32(S2Value.Rows[0]["APRICE"]);
                        string BOOLSQL = "SELECT COUNT(1) AS NUM FROM I_D117502d3f67c8bb71d453c8d8942b37b4eee7f WHERE BusinessDivision = '" + BU + "' AND store = '" + ST + "' AND Branch = '" + BR + "' AND PartNo = '" + PartNumber + "' AND Warehousename = '" + warehouse + "'";
                        System.Data.DataTable SQLValue = this.Request.Engine.Query.QueryTable(BOOLSQL, null);
                        int count = Convert.ToInt32(SQLValue.Rows[0]["NUM"]);
                        if(count > 1)
                        {
                            response.Errors.Add("库存重复！请检查存放仓库是否正确或联系管理员处理（ERR:count>1）");
                            return;
                        }
                        else
                        {
                            if(count == 1)
                            {
                                H3.Data.Filter.Filter   filter = new H3.Data.Filter.Filter();
                                H3.Data.Filter.And   andMatcher = new H3.Data.Filter.And();
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("BusinessDivision", H3.Data.ComparisonOperatorType.Equal, BU));
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("store", H3.Data.ComparisonOperatorType.Equal, ST));
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("Branch", H3.Data.ComparisonOperatorType.Equal, BR));
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("PartNo", H3.Data.ComparisonOperatorType.Equal, PartNumber));
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("Warehousename", H3.Data.ComparisonOperatorType.Equal, warehouse));
                                filter.Matcher = andMatcher;
                                string detailSQL = "SELECT objectId AS OBJ FROM I_D117502d3f67c8bb71d453c8d8942b37b4eee7f WHERE BusinessDivision = '" + BU + "' AND store = '" + ST + "' AND Branch = '" + BR + "' AND PartNo = '" + PartNumber + "' AND Warehousename = '" + warehouse + "'";
                                System.Data.DataTable detailSQLVALUE = this.Request.Engine.Query.QueryTable(detailSQL, null);
                                string flagOBJ = detailSQLVALUE.Rows[0]["OBJ"].ToString();
                                H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502d3f67c8bb71d453c8d8942b37b4eee7f", flagOBJ, false);
                                accountBo["quantity"] = Convert.ToDecimal(accountBo["quantity"]) + Convert.ToDecimal(detail["Incount"]);//库存数量
                                accountBo["F0000011"] = detail["vendor"] + string.Empty; //最近一次供货单位
                                accountBo["F0000007"] = (Convert.ToDecimal(accountBo["quantity"]) * Convert.ToDecimal(accountBo["F0000007"]) + Convert.ToDecimal(detail["totalPrice"])) / (Convert.ToDecimal(accountBo["quantity"]) + Convert.ToDecimal(detail["Incount"])); //平均购入价（当前表单中不含税的总价）【（库存数量 * 库存平均购入价 + 入库总价）/ （库存数量 + 入库数量）】
                                accountBo["F0000005"] = Convert.ToDecimal(detail["uninPrice"]); //入库价格（当前表单中不含税的总价）
                                accountBo["F0000006"] = APRICE == 0 ? (Convert.ToDecimal(detail["totalPrice"]) + (Convert.ToDecimal(0.5) * Convert.ToDecimal(detail["totalPrice"]))) : APRICE; //出库价：出库价默认等于配件号的A价 如果配件号的A价为0，则出库价等于入库价加价50%
                                accountBo["Taxpoint"] = Convert.ToDecimal(detail["fax"]); //税点
                                accountBo["Totalprice"] = Convert.ToDecimal(detail["totalInFaxPrice"]); //含税总价格
                                accountBo["F0000015"] = Convert.ToDecimal(detail["totalPrice"]) + Convert.ToDecimal(accountBo["F0000015"]); //不含税总价格
                                accountBo["F0000003"] = 0; //库存挤压天数
                                accountBo["F0000004"] = backlogDays; //库存挤压天数区间
                                accountBo["F0000001"] = DateTime.Now; //最近一次入库日期
                                accountBo.Status = H3.DataModel.BizObjectStatus.Effective; // 将对象状态设为生效
                                accountBo.Update(commit);  //更新对象
                            }
                            if(count == 0)
                            {
                                H3.DataModel.BizObject amao = new H3.DataModel.BizObject(engine, schema, systemUserId);
                                amao.CreatedBy = currentUserId;
                                amao.OwnerId = currentUserId;
                                amao["BusinessDivision"] = BU; //事业部
                                amao["store"] = ST; //门店
                                amao["Branch"] = BR; //分店
                                amao["Warehousename"] = warehouse; //存放仓库
                                // amao["Cargospace"] = detail["PartNumber"] + string.Empty; //货位号
                                amao["PartNo"] = PartNumber; //配件号
                                amao["Nameofaccessories"] = detail["PartName"] + string.Empty; //配件名称
                                amao["bind"] = detail["branch2"] + string.Empty; //适用品牌
                                amao["Model"] = detail["carModel"] + string.Empty; //适用车型
                                amao["F0000008"] = C1; //配件大类
                                amao["F0000009"] = C2; //配件子类
                                amao["F0000010"] = C3; //配件小类
                                amao["Company"] = detail["unit"] + string.Empty; //单位
                                amao["quantity"] = detail["Incount"] + string.Empty; //库存数量
                                amao["F0000011"] = detail["vendor"] + string.Empty; //最近一次供货单位
                                amao["F0000007"] = Convert.ToDecimal(detail["totalPrice"]) / Convert.ToDecimal(detail["Incount"]); //平均购入价（当前表单中不含税的总价）
                                amao["F0000005"] = Convert.ToDecimal(detail["uninPrice"]); //入库价格（当前表单中不含税的总价）
                                amao["F0000006"] = APRICE == 0 ? (Convert.ToDecimal(detail["uninPrice"]) * Convert.ToDecimal(1.5)) : APRICE; //出库价：出库价默认等于配件号的A价 如果配件号的A价为0，则出库价等于入库价加价50%
                                amao["Taxpoint"] = Convert.ToDecimal(detail["fax"]); //税点
                                amao["Totalprice"] = Convert.ToDecimal(detail["totalInFaxPrice"]); //含税总价格
                                amao["F0000003"] = 0; //库存积压天数
                                amao["F0000004"] = backlogDays; //库存积压天数区间
                                amao["F0000012"] = 0; //月消耗
                                amao["F0000013"] = 0; //年消耗
                                amao["F0000001"] = DateTime.Now; //最近一次入库日期
                                amao["OwnerDeptId"] = 0; //所属部门
                                amao["F0000015"] = Convert.ToDecimal(detail["totalPrice"]); //不含税总价格
                                amao["F0000014"] = BU + ST + BR + PartNumber + warehouse + "存放货位号"; //重复校验
                                amao.Status = H3.DataModel.BizObjectStatus.Effective;
                                amao.Create(commit);
                            }
                        }
                        string UpdateSql1 = "SELECT objectId AS OBJ FROM I_D117502F46113d8470aa484783a009ae9dceb8c9 WHERE parentobjectid = '" + this.Request.BizObject["F0000017"].ToString() + "' AND F0000004 = '" + PartNumber + "'";// 采购申请子表中的一行
                        System.Data.DataTable U1Value = this.Request.Engine.Query.QueryTable(UpdateSql1, null);
                        this.Request.Engine.Query.QueryTable("UPDATE I_D117502F46113d8470aa484783a009ae9dceb8c9 SET F0000017 = '已入库' WHERE objectId = '" + U1Value.Rows[0]["OBJ"].ToString() + "' AND parentobjectid = '" + this.Request.BizObject["F0000017"].ToString() + "'", null);
                        string UpdateSql2 = "SELECT COUNT(1) AS NUM FROM I_D117502F46113d8470aa484783a009ae9dceb8c9 WHERE parentobjectid = '" + this.Request.BizObject["F0000017"].ToString() + "' AND F0000017 = '已入库'";
                        System.Data.DataTable U2Value = this.Request.Engine.Query.QueryTable(UpdateSql2, null);
                        H3.DataModel.BizObject account2 = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502Suefsqsppwj048lqgwz9f00nw0", this.Request.BizObject["F0000017"].ToString(), false);
                        account2["allStatus"] = Convert.ToInt32(U2Value.Rows[0]["NUM"]) == 0 ? "均已入库" : "部分入库";
                        account2.Update(commit);
                    }
                    string errorMsg = null;
                    commit.Commit(this.Request.Engine.BizObjectManager, out errorMsg);
                }
            }
        }
        if(this.Request.BizObject["InType"].ToString() == "其他入库") 
        {
            string BU = this.Request.BizObject["BusinessDivision"].ToString();
            string ST = this.Request.BizObject["store"].ToString();
            string BR = this.Request.BizObject["Branch"].ToString();
            H3.IEngine engine = this.Request.Engine;//得到一个iengine的参数
            string systemUserId = H3.Organization.User.SystemUserId;//获取系统虚拟用户的人员ID
            string currentUserId = this.Request.UserContext.UserId;//获得当前登录人的人员ID
            bool type = this.Request.IsCreateMode;//判断当前是否创建模式
            if(type)//判断是创建模式
            {

                H3.DataModel.BizObject[] details = (H3.DataModel.BizObject[]) this.Request.BizObject["D117502detailsOfIn"];//把子表的数据存在details[]
                if(details != null && details.Length > 0)//判断子表数据不等于空存储的长度大于0
                {
                    H3.DataModel.BulkCommit commit = new H3.DataModel.BulkCommit();//批量操作
                    H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502d3f67c8bb71d453c8d8942b37b4eee7f");//库存明细
                    string  S1 = "SELECT objectId AS OBJ FROM I_D1175025db04169747948658cdde2231ac4bd2c WHERE  max <= 90 AND min < 0";//获取的挤压天数区间的区间名称
                    System.Data.DataTable S1Value = this.Request.Engine.Query.QueryTable(S1, null);//自定义sql
                    string backlogDays = S1Value.Rows[0]["OBJ"].ToString();
                    foreach(H3.DataModel.BizObject detail in details)
                    {
                        string warehouse = detail["warehouse"] + string.Empty;
                        string PartNumber = detail["PartNumber"] + string.Empty;
                        string S2 = "SELECT Categoryofaccessories AS C1, F0000003 AS C2, Accessories AS C3, Deliveryprice AS APRICE FROM I_D11750283bfe708de2c42799b86babb9d98a488 WHERE objectId = '" + PartNumber + "'";
                        System.Data.DataTable S2Value = this.Request.Engine.Query.QueryTable(S2, null);
                        string C1 = S2Value.Rows[0]["C1"] + string.Empty;
                        string C2 = S2Value.Rows[0]["C2"] + string.Empty;
                        string C3 = S2Value.Rows[0]["C3"] + string.Empty;
                        decimal APRICE = Convert.ToInt32(S2Value.Rows[0]["APRICE"]);
                        string BOOLSQL = "SELECT COUNT(1) AS NUM FROM I_D117502d3f67c8bb71d453c8d8942b37b4eee7f WHERE BusinessDivision = '" + BU + "' AND store = '" + ST + "' AND Branch = '" + BR + "' AND PartNo = '" + PartNumber + "' AND Warehousename = '" + warehouse + "'";
                        System.Data.DataTable SQLValue = this.Request.Engine.Query.QueryTable(BOOLSQL, null);
                        int count = Convert.ToInt32(SQLValue.Rows[0]["NUM"]);
                        if(count > 1)
                        {
                            response.Errors.Add("库存重复！请检查存放仓库是否正确或联系管理员处理（ERR:count>1）");
                            return;
                        }
                        else
                        {
                            if(count == 1)
                            {
                                H3.Data.Filter.Filter   filter = new H3.Data.Filter.Filter();
                                H3.Data.Filter.And   andMatcher = new H3.Data.Filter.And();
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("BusinessDivision", H3.Data.ComparisonOperatorType.Equal, BU));
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("store", H3.Data.ComparisonOperatorType.Equal, ST));
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("Branch", H3.Data.ComparisonOperatorType.Equal, BR));
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("PartNo", H3.Data.ComparisonOperatorType.Equal, PartNumber));
                                andMatcher.Add(new H3.Data.Filter.ItemMatcher("Warehousename", H3.Data.ComparisonOperatorType.Equal, warehouse));
                                filter.Matcher = andMatcher;
                                string detailSQL = "SELECT objectId AS OBJ FROM I_D117502d3f67c8bb71d453c8d8942b37b4eee7f WHERE BusinessDivision = '" + BU + "' AND store = '" + ST + "' AND Branch = '" + BR + "' AND PartNo = '" + PartNumber + "' AND Warehousename = '" + warehouse + "'";
                                System.Data.DataTable detailSQLVALUE = this.Request.Engine.Query.QueryTable(detailSQL, null);
                                string flagOBJ = detailSQLVALUE.Rows[0]["OBJ"].ToString();
                                H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502d3f67c8bb71d453c8d8942b37b4eee7f", flagOBJ, false);
                                accountBo["quantity"] = Convert.ToDecimal(accountBo["quantity"]) + Convert.ToDecimal(detail["Incount"]);//库存数量
                                accountBo["F0000011"] = detail["vendor"] + string.Empty; //最近一次供货单位
                                accountBo["F0000007"] = (Convert.ToDecimal(accountBo["quantity"]) * Convert.ToDecimal(accountBo["F0000005"]) + Convert.ToDecimal(detail["Incount"])*Convert.ToDecimal(detail["uninPrice"])) / (Convert.ToDecimal(accountBo["quantity"]) + Convert.ToDecimal(detail["Incount"])); //平均购入价（当前表单中不含税的总价）【（库存数量 * 库存平均购入价 + 入库总价）/ （库存数量 + 入库数量）】
                                accountBo["F0000005"] = Convert.ToDecimal(detail["uninPrice"]); //入库价格（当前表单中不含税的总价）
                                accountBo["F0000006"] = APRICE == 0 ? (Convert.ToDecimal(detail["totalPrice"]) + (Convert.ToDecimal(0.5) * Convert.ToDecimal(detail["totalPrice"]))) : APRICE; //出库价：出库价默认等于配件号的A价 如果配件号的A价为0，则出库价等于入库价加价50%
                                accountBo["Taxpoint"] = Convert.ToDecimal(detail["fax"]); //税点
                                accountBo["Totalprice"] = Convert.ToDecimal(detail["totalInFaxPrice"]); //含税总价格
                                accountBo["F0000015"] = Convert.ToDecimal(detail["totalPrice"]) + Convert.ToDecimal(accountBo["F0000015"]); //不含税总价格
                                accountBo["F0000003"] = 0; //库存挤压天数
                                accountBo["F0000004"] = backlogDays; //库存挤压天数区间
                                accountBo["F0000001"] = DateTime.Now; //最近一次入库日期
                                accountBo.Status = H3.DataModel.BizObjectStatus.Effective; // 将对象状态设为生效
                                accountBo.Update(commit);  //更新对象
                            }
                            if(count == 0)
                            {
                                H3.DataModel.BizObject amao = new H3.DataModel.BizObject(engine, schema, systemUserId);
                                amao.CreatedBy = currentUserId;
                                amao.OwnerId = currentUserId;
                                amao["BusinessDivision"] = BU; //事业部
                                amao["store"] = ST; //门店
                                amao["Branch"] = BR; //分店
                                amao["Warehousename"] = warehouse; //存放仓库
                                // amao["Cargospace"] = detail["PartNumber"] + string.Empty; //货位号
                                amao["PartNo"] = PartNumber; //配件号
                                amao["Nameofaccessories"] = detail["PartName"] + string.Empty; //配件名称
                                amao["bind"] = detail["branch2"] + string.Empty; //适用品牌
                                amao["Model"] = detail["carModel"] + string.Empty; //适用车型
                                amao["F0000008"] = C1; //配件大类
                                amao["F0000009"] = C2; //配件子类
                                amao["F0000010"] = C3; //配件小类
                                amao["Company"] = detail["unit"] + string.Empty; //单位
                                amao["quantity"] = detail["Incount"] + string.Empty; //库存数量
                                amao["F0000011"] = detail["vendor"] + string.Empty; //最近一次供货单位
                                amao["F0000007"] = Convert.ToDecimal(detail["totalPrice"]) / Convert.ToDecimal(detail["Incount"]); //平均购入价（当前表单中不含税的总价）
                                amao["F0000005"] = Convert.ToDecimal(detail["uninPrice"]); //入库价格（当前表单中不含税的总价）
                                amao["F0000006"] = APRICE == 0 ? (Convert.ToDecimal(detail["uninPrice"]) * Convert.ToDecimal(1.5)) : APRICE; //出库价：出库价默认等于配件号的A价 如果配件号的A价为0，则出库价等于入库价加价50%
                                amao["Taxpoint"] = Convert.ToDecimal(detail["fax"]); //税点
                                amao["Totalprice"] = Convert.ToDecimal(detail["totalInFaxPrice"]); //含税总价格
                                amao["F0000003"] = 0; //库存积压天数
                                amao["F0000004"] = backlogDays; //库存积压天数区间
                                amao["F0000012"] = 0; //月消耗
                                amao["F0000013"] = 0; //年消耗
                                amao["F0000001"] = DateTime.Now; //最近一次入库日期
                                amao["OwnerDeptId"] = 0; //所属部门
                                amao["F0000015"] = Convert.ToDecimal(detail["totalPrice"]); //不含税总价格
                                amao["F0000014"] = BU + ST + BR + PartNumber + warehouse + "存放货位号"; //重复校验
                                amao.Status = H3.DataModel.BizObjectStatus.Effective;
                                amao.Create(commit);
                            }
                        }
                    }
                    string errorMsg = null;
                    commit.Commit(this.Request.Engine.BizObjectManager, out errorMsg);
                }

            }


        }
        base.OnSubmit(actionName, postValue, response);

    }
}