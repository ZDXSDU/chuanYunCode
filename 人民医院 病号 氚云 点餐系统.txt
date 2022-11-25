C# >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502Skiwm6kh44wpt2v6e71rnw9cp6: H3.SmartForm.SmartFormController
{
    public D117502Skiwm6kh44wpt2v6e71rnw9cp6(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
        response.Actions.Remove("Remove");
        response.Actions.Remove("Save");
        response.Actions.Remove("Print");
        response.Actions.Remove("ViewQrCode");
        response.Actions.Remove("Edit");
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
        if(actionName == "GetTime")
        {
            string customer = this.Request["Meals"] + string.Empty;
            string SQL = "";
            response.ReturnData = new Dictionary<string, object>();
            if(customer == "早餐")
            {
                SQL = "SELECT BFbeginTime AS beginTime, BREndTime AS endTime FROM I_D117502sendTimeZYZGCT";
            }
            if(customer == "午餐")
            {
                SQL = "SELECT LCBeginTime AS beginTime, LCEndTime AS endTime FROM I_D117502sendTimeZYZGCT";
            }
            if(customer == "晚餐")
            {
                SQL = "SELECT DNBeginTime AS beginTime, DNEndTime AS endTime FROM I_D117502sendTimeZYZGCT";
            }
            System.Data.DataTable dtAccount = this.Request.Engine.Query.QueryTable(SQL, null);
            int begin = Convert.ToInt32(Convert.ToDateTime(dtAccount.Rows[0]["beginTime"].ToString()).Hour) * 60 + Convert.ToInt32(Convert.ToDateTime(dtAccount.Rows[0]["beginTime"].ToString()).Minute);
            int end = Convert.ToInt32(Convert.ToDateTime(dtAccount.Rows[0]["endTime"].ToString()).Hour) * 60 + Convert.ToInt32(Convert.ToDateTime(dtAccount.Rows[0]["endTime"].ToString()).Minute);
            response.ReturnData.Add("begin", begin);
            response.ReturnData.Add("end", end);
        }
    }

    protected override void OnWorkflowInstanceStateChanged(H3.Workflow.Instance.WorkflowInstanceState oldState, H3.Workflow.Instance.WorkflowInstanceState newState)
    {
        if(oldState == H3.Workflow.Instance.WorkflowInstanceState.Running && newState == H3.Workflow.Instance.WorkflowInstanceState.Finished)
        {
            H3.BizBus.BizStructureSchema paramSchema = new H3.BizBus.BizStructureSchema();
            paramSchema.Add(new H3.BizBus.ItemSchema("OrderInfo", "打印内容", H3.Data.BizDataType.ShortString, 200, null));
            paramSchema.Add(new H3.BizBus.ItemSchema("Machine", "机号", H3.Data.BizDataType.ShortString, 200, null));
            string OrderInfo = "";
            OrderInfo = "<CB>中裕职工餐厅消费凭证</CB><BR>";
            OrderInfo += "订单号：" + this.Request.BizObject["SeqNo"].ToString() + "<BR>";
            OrderInfo += "下单时间：" + this.Request.BizObject["CreatedTime"].ToString() + "<BR>";
            OrderInfo += "<B><BOLD>" + this.Request.BizObject["F0000013"].ToString() + "</BOLD></B><BR>";
            OrderInfo += "--------------------------------<BR>";
            H3.DataModel.BizObject[] details = (H3.DataModel.BizObject[]) this.Request.BizObject["D117502Fzpc1jdy9ff7tkvoxsb9ul4297"];
            if(details != null && details.Length > 0)
            {
                foreach(H3.DataModel.BizObject detail in details)
                {
                    OrderInfo += "菜品：<B><BOLD>" + detail["detailT"].ToString().Substring(0, 1) + "</BOLD></B>" + detail["detailT"].ToString() + "<BR>";
                    OrderInfo += "订购数量：" + detail["num"].ToString() + "<BR>";
                    OrderInfo += "单价：" + detail["price"].ToString() + "<BR>";
                    OrderInfo += "金额小计" + detail["AmountSubtotal"].ToString() + "<BR>";
                    OrderInfo += "--------------------------------<BR>";
                    this.Request.Engine.Query.QueryTable("UPDATE I_D117502St7zmydx08cdwtbl3yb8yatur3 SET inventoryQuantity = inventoryQuantity - " + Convert.ToInt32(detail["num"]) + " WHERE F0000001 = '" + detail["detailT"].ToString() + "'", null);
                }
            }
            OrderInfo += "<B><BOLD>总金额：" + this.Request.BizObject["TotalAmount"].ToString() + "</BOLD></B><BR>";
            OrderInfo += "<B><BOLD>联系人：" + this.Request.BizObject["DeliveryContacts"].ToString() + "</BOLD></B><BR>";
            OrderInfo += "<B><BOLD>联系方式：" + this.Request.BizObject["DeliveryTelphoneNumber"].ToString() + "</BOLD></B><BR>";
            OrderInfo += "<B><BOLD>地址：" + this.Request.BizObject["address"].ToString() + " " + this.Request.BizObject["F0000012"].ToString() + "</BOLD></B><BR>";
            // OrderInfo += "顾客备注：" + this.Request.BizObject["remarks"].ToString() + "<BR>";
            OrderInfo += "--加链加技术支持--                                <BR>";
            string Machine = "921528091";
            H3.BizBus.BizStructure paramData = new H3.BizBus.BizStructure(paramSchema);
            paramData["OrderInfo"] = OrderInfo;
            paramData["Machine"] = Machine;
            H3.BizBus.InvokeResult InResult = this.Engine.BizBus.Invoke(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.Legacy, this.Request.SchemaCode, "Print", paramData);
        }
    }
}

JS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
/* 控件接口说明：
 * 1. 读取控件: this.***,*号输入控件编码;
 * 2. 读取控件的值： this.***.GetValue();
 * 3. 设置控件的值： this.***.SetValue(???);
 * 4. 绑定控件值变化事件： this.***.BindChange(key,function(){})，key是定义唯一的方法名;
 * 5. 解除控件值变化事件： this.***.UnbindChange(key);
 * 6. CheckboxList、DropDownList、RadioButtonList: $.***.AddItem(value,text),$.***.ClearItems();
 */
/* 公共接口：
 * 1. ajax：$.SmartForm.PostForm(actionName,data,callBack,errorBack,async),
 *          actionName:提交的ActionName;data:提交后台的数据;callback:回调函数;errorBack:错误回调函数;async:是否异步;
 * 2. 打开表单：$.IShowForm(schemaCode, objectId, checkIsChange)，
 *          schemaCode:表单编码;objectId;表单数据Id;checkIsChange:关闭时，是否感知变化;
 * 3. 定位接口：$.ILocation();
 */
// 表单插件代码
$.extend( $.JForm, {

    // 加载事件
    OnLoad: function() {
        if( $.SmartForm.ResponseContext.FormMode == 2 ) {
            this.D117502Fzpc1jdy9ff7tkvoxsb9ul4297.ClearRows();
        }
        const THAT = this;
        const myDate = new Date();
        let Meals = '';
        let now = parseInt( myDate.getHours() * 60 + myDate.getMinutes() );
        let beginTime = 0;
        let endTime = 0;
        this.F0000013.BindChange( "订餐时间校验", () => {
            Meals = this.F0000013.GetValue().substring( 0, 2 );
            if( Meals != "" && Meals.length == 2 ) {
                $.SmartForm.PostForm( "GetTime", {
                    Meals: Meals,
                }, ( data ) => {
                    if( data.Errors && data.Errors.length ) {
                        $.IShowError( "错误", "AJAX错误" );
                    } else {
                        let begin = data.ReturnData[ "begin" ] + 0;
                        let end = data.ReturnData[ "end" ] + 0;
                        let currentTimes = data.ReturnData[ "end" ] - 30; //停止下单的时间
                        if( now > currentTimes ) {
                            $.IConfirm( "提示", Meals + "点餐时间已过，请在配送截止停止前30分钟完成点餐", function( data ) {
                                THAT.F0000013.SetValue();
                                return false;
                            });
                        } else {
                            return;
                        }
                    }
                }, ( error ) => {
                    $.IShowError( "错误", "AJAX错误" );
                }, false )
            }
        })
    },

    // 按钮事件
    OnLoadActions: function( actions ) {
    },

    // 提交校验
    OnValidate: function( actionControl ) {
        let STATUS = true;
        if( actionControl.Action == "Submit" ) {
            var childTableData = this.D117502Fzpc1jdy9ff7tkvoxsb9ul4297.GetValue();
            for( var i = 0;i < childTableData.length;i++ ) {
                var itemData = childTableData[ i ];
                if( childTableData[ i ].num <= 0 ) {
                    STATUS = false;
                    $.IShowWarn( "警告", "请检查订餐数量" );
                    return false;
                }
            }
            if( STATUS ) {
                return true;
            }
        }
    },

    // 提交前事件
    BeforeSubmit: function( action, postValue ) {

    },

    // 提交后事件
    AfterSubmit: function( action, responseValue ) {
    }
});