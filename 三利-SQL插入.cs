
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502CalculateSalary: H3.SmartForm.SmartFormController
{
    public D117502CalculateSalary(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
        this.Request.Engine.Query.QueryTable("delete from i_D117502staffSalary where monthSalary = '" + this.Request.BizObject["date"].ToString() + "'", null);
    
        string sql = "INSERT INTO i_D117502staffSalary(i_D117502staffSalary.office , i_D117502staffSalary.objectid  ,i_D117502staffSalary.employeeNames ,"
            + " i_D117502staffSalary.entryDate , i_D117502staffSalary.phoneNumber , i_D117502staffSalary.department , i_D117502staffSalary.rank , i_D117502staffSalary.baeSalary , "
            + "i_D117502staffSalary.weekPerformance , i_D117502staffSalary.monthperformance , i_D117502staffSalary.monthSalary ,i_D117502staffSalary.status) "
            + "SELECT i_D117502JLJUser.jopTitle , replace(uuid(),'-','') , i_D117502JLJUser.objectid , i_D117502JLJUser.entryTime , i_D117502JLJUser.phoneNumber ,"
            + "i_D117502JLJUser.DEPUser , i_D117502JLJUser.rank , i_D117502JLJUser.basicWage , i_D117502JLJUser.standardWeekPerformance , "
            + " i_D117502JLJUser.standardMonthlyPerformance , date_format( date_add( CURDATE() , interval -1 month) ,'%Y-%m')   ,  1  FROM i_D117502JLJUser";

        this.Request.Engine.Query.QueryTable(sql, null);
        //油补
    
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.oilSupplement   =  ifnull ("
            + "      (SELECT   SUM(oilSupplement)  from I_D117502oilSubsidy "
            + "   where I_D117502oilSubsidy.employeeName  =   i_D117502staffSalary.employeeNames  and  I_D117502oilSubsidy.monthSalary  =  i_D117502staffSalary.monthSalary  ) , 0  ) ", null);
        //补资
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.subsidySalary   =  ifnull ("
            + "      (SELECT   SUM(  supplementaryFunds    ) from I_D117502supplementaryDeduction "
            + "   where I_D117502supplementaryDeduction.fullName  =   i_D117502staffSalary.employeeNames  and  I_D117502supplementaryDeduction.payMonth  =  i_D117502staffSalary.monthSalary  ) , 0  ) ", null);
        //扣资
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.buckleSalary   =  ifnull ("
            + "      (SELECT   SUM(  deduction    ) from I_D117502supplementaryDeduction "
            + "   where I_D117502supplementaryDeduction.fullName  =   i_D117502staffSalary.employeeNames  and  I_D117502supplementaryDeduction.payMonth  =  i_D117502staffSalary.monthSalary  ) , 0  ) ", null);
        //周绩效工资
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.weekPerformanceSalary   =  ifnull ("
            + "      (SELECT (I_D117502performanceFactor.weekPerformance*I_D117502performanceFactor.weeklyPerformanceFactor) from I_D117502performanceFactor "
            + "   where I_D117502performanceFactor.fullName  =   i_D117502staffSalary.employeeNames  and  I_D117502performanceFactor.monthSalary  =  i_D117502staffSalary.monthSalary  ) , 0  ) ", null);
        //月绩效工资
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.monthPerformanceSalary   =  ifnull ("
            + "      (SELECT   (I_D117502performanceFactor.monthPerformance*I_D117502performanceFactor.monthPerformanceScore) from I_D117502performanceFactor "
            + "   where I_D117502performanceFactor.fullName  =   i_D117502staffSalary.employeeNames  and  I_D117502performanceFactor.monthSalary  =  i_D117502staffSalary.monthSalary  ) , 0  ) ", null);

        //应出勤天数
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.attendanceDay   =  ifnull ("
            + "      (SELECT  attendanceDay  from I_D117502AttendanceDay where zuZhi like '%总部%') , 0  ) ", null);
        //加班天数
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.overtimeDay   =  ifnull ("
            + "      (SELECT  I_D117502supplementaryDeduction.overtimeDay from I_D117502supplementaryDeduction "
            + "   where I_D117502supplementaryDeduction.fullName  =   i_D117502staffSalary.employeeNames  and  I_D117502supplementaryDeduction.payMonth  =  i_D117502staffSalary.monthSalary  ) , 0  ) ", null);
        //加班工资
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set  overtimePay=overtimeDay*(baeSalary/30)", null);

        //钉钉后台出勤天数
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.attendanceDays   =  ifnull ("
            + "(SELECT  I_D117502DayWork.attendanceDays from I_D117502DayWork where i_D117502staffSalary.phoneNumber=I_D117502DayWork.phoneNumber) , 0  ) ", null);
        
        //正常出勤天数
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.normalAttendance =i_D117502staffSalary.attendanceDay where (i_D117502staffSalary.attendanceDays>=i_D117502staffSalary.attendanceDay)", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.normalAttendance =i_D117502staffSalary.attendanceDays where (i_D117502staffSalary.attendanceDays<i_D117502staffSalary.attendanceDay)", null);
        
        //实际出勤天数
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.attendanceDays =i_D117502staffSalary.normalAttendance+i_D117502staffSalary.overtimeDay", null);

        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.attendanceDays =  i_D117502staffSalary.attendanceDay where "
        +"i_D117502staffSalary.phoneNumber in (select i_D117502monthlyAttendance.phoneNumber from   i_D117502monthlyAttendance where "
        +"i_D117502monthlyAttendance.lateTimes+i_D117502monthlyAttendance.notSigned+i_D117502monthlyAttendance.normal=0)", null);
        
        //全勤奖
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set perfectAttendance=200 where i_D117502staffSalary.attendanceDays>=i_D117502staffSalary.attendanceDay ", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set perfectAttendance=0 where i_D117502staffSalary.attendanceDays<i_D117502staffSalary.attendanceDay ", null);

        //应发基本工资
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set  basicSalary=0 where attendanceDays=0", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set  basicSalary=baeSalary/30*attendanceDays where attendanceDays>0 and attendanceDays<18", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set  basicSalary=(i_D117502staffSalary.baeSalary - (i_D117502staffSalary.baeSalary/30*(i_D117502staffSalary.attendanceDay - i_D117502staffSalary.attendanceDays))) where (attendanceDays>=18 and attendanceDays<attendanceDay) ", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set  basicSalary=baeSalary+ perfectAttendance where attendanceDays>=attendanceDay", null);



        //工龄
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.lengthOfService   =   ifnull("
            + "      (SELECT   i_D117502DayWork.lengthOfService from i_D117502DayWork "
            + "   where i_D117502DayWork.phoneNumber=i_D117502staffSalary.phoneNumber  ), 0  )  ", null);

        //工龄补贴
        //this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.ageSubsidy=i_D117502DayWork.ageSubsidy where i_D117502staffSalary.employeeNames=i_D117502DayWork.fullName", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.ageSubsidy   =  ifnull("
            + "      (SELECT i_D117502DayWork.ageSubsidy from i_D117502DayWork "
            + "   where i_D117502DayWork.phoneNumber=i_D117502staffSalary.phoneNumber) , 0  ) ", null);
        

        //应发工资
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set  i_D117502staffSalary.payable=i_D117502staffSalary.overtimePay+i_D117502staffSalary.basicSalary +i_D117502staffSalary.weekPerformanceSalary+i_D117502staffSalary.monthPerformanceSalary-i_D117502staffSalary.buckleSalary+i_D117502staffSalary.subsidySalary+i_D117502staffSalary.oilSupplement+i_D117502staffSalary.ageSubsidy ", null);
        // //保险
        //  this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set personalInsuranceCosts=0 ", null);
        //个人承担保险
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.personalInsuranceCosts   =  ifnull ("
            + "      (SELECT   I_D117502insurance1.personalInsurance from I_D117502insurance1 "
            + "   where I_D117502insurance1.fullName  =   i_D117502staffSalary.employeeNames  and  I_D117502insurance1.phoneNumber  =  i_D117502staffSalary.phoneNumber  ) , 0  ) ", null);
        //公司承担保险
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set i_D117502staffSalary.theCompanyPaysTheInsuranceCost   =  ifnull ("
            + "      (SELECT   I_D117502insurance1.theCompanyUndertakesInsurance from I_D117502insurance1 "
            + "   where I_D117502insurance1.fullName  =   i_D117502staffSalary.employeeNames  and  I_D117502insurance1.phoneNumber  =  i_D117502staffSalary.phoneNumber  ) , 0  ) ", null);
        //个税
        //<5000
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set personalTax=0 where (i_D117502staffSalary.payable-i_D117502staffSalary.personalInsuranceCosts) <=5000", null);
        //5000-8000
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set personalTax=(payable-personalInsuranceCosts-5000)*0.03 where (i_D117502staffSalary.payable-i_D117502staffSalary.personalInsuranceCosts)"
            + ">5000 and (i_D117502staffSalary.payable-i_D117502staffSalary.personalInsuranceCosts)<=8000 ", null);
        //8000-17000
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set personalTax=(payable-personalInsuranceCosts-8000)*0.1+90 where (i_D117502staffSalary.payable-i_D117502staffSalary.personalInsuranceCosts)"
            + ">=8000 and (i_D117502staffSalary.payable-i_D117502staffSalary.personalInsuranceCosts)<=17000 ", null);
        
        

        //实发工资
        this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set actualSalary=payable-personalInsuranceCosts-personalTax ", null);


        //七方工资计算！！！
        this.Request.Engine.Query.QueryTable("DELETE FROM I_D117502managementSalary",null);

        this.Request.Engine.Query.QueryTable("delete from i_D117502managementSalary where salaryMonth = '" + this.Request.BizObject["date"].ToString() + "'", null);

        string sql1 = "INSERT INTO i_D117502managementSalary(i_D117502managementSalary.objectid  ,i_D117502managementSalary.employeeName ,"
            + "  i_D117502managementSalary.phoneNumber , i_D117502managementSalary.rank , i_D117502managementSalary.basicSalary , "
            + "i_D117502managementSalary.weeklyPerformance , i_D117502managementSalary.performanceSalary , i_D117502managementSalary.salaryMonth ,i_D117502managementSalary.status) "
            + "SELECT replace(uuid(),'-','') , i_D117502QFUser.objectid , i_D117502QFUser.phoneNumber ,"
            + " i_D117502QFUser.rank , i_D117502QFUser.basicWage , i_D117502QFUser.standardWeekPerformance , "
            + " i_D117502QFUser.standardMonthlyPerformance , date_format( date_add( CURDATE() , interval -1 month) ,'%Y-%m')   ,  1  FROM i_D117502QFUser";

        this.Request.Engine.Query.QueryTable(sql1,null);
        //油补
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.oilSupplement   =  ifnull ("
            + "      (SELECT   oilSupplement  from I_D117502OilInformation "
            + "   where I_D117502OilInformation.employeeName  =   i_D117502managementSalary.employeeName  and  I_D117502OilInformation.monthSalary  =  i_D117502managementSalary.salaryMonth  ) , 0  ) ", null);
        //补资
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.supplementarySalary   =  ifnull ("
            + "      (SELECT supplementaryFunds from I_D117502SupplementaryDeduction1 "
            + "   where I_D117502SupplementaryDeduction1.fullName  =   i_D117502managementSalary.employeeName  and  I_D117502SupplementaryDeduction1.payMonth  =  i_D117502managementSalary.salaryMonth  ) , 0  ) ", null);
        //扣资
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.deductionWages   =  ifnull ("
            + "      (SELECT deduction from I_D117502SupplementaryDeduction1 "
            + "   where I_D117502SupplementaryDeduction1.fullName  =   i_D117502managementSalary.employeeName  and  I_D117502SupplementaryDeduction1.payMonth  =  i_D117502managementSalary.salaryMonth  ) , 0  ) ", null);
        //周绩效工资
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.performanceSalaryW   =  ifnull ("
            + "      (SELECT (I_D117502performanceFactor1.weekPerformance*I_D117502performanceFactor1.weeklyPerformanceFactor) from I_D117502performanceFactor1 "
            + "   where I_D117502performanceFactor1.fullName=i_D117502managementSalary.employeeName  and  I_D117502performanceFactor1.monthSalary  =  i_D117502managementSalary.salaryMonth  ) , 0  ) ", null);
        //月绩效工资
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.performanceSalaryM   =  ifnull ("
            + "      (SELECT   (I_D117502performanceFactor1.monthPerformance*I_D117502performanceFactor1.monthPerformanceScore) from I_D117502performanceFactor1 "
            + " where  I_D117502performanceFactor1.fullName  =   i_D117502managementSalary.employeeName  and  I_D117502performanceFactor1.monthSalary  =  i_D117502managementSalary.salaryMonth  ) , 0  ) ", null);

        //应出勤天数
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.attendanceDays   =  ifnull ("
            + "      (SELECT  attendanceDay  from I_D117502AttendanceDay where zuZhi like '%七方%' ), 0  ) ", null);

        //加班天数
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.overTimeDay=ifnull("
            + "      (SELECT I_D117502SupplementaryDeduction1.overtimeDay from I_D117502SupplementaryDeduction1"
            + "   where I_D117502SupplementaryDeduction1.fullName  =   i_D117502managementSalary.employeeName and I_D117502SupplementaryDeduction1.payMonth  =  i_D117502managementSalary.salaryMonth  ) , 0  ) ", null);
        //加班工资
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set  overtimeSalary=overTimeDay*(basicSalary/30)", null);

        //钉钉后台出勤天数
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.normalAttendance   =  ifnull ("
            + "(SELECT  count(1)+overTimeDay from I_D117502monthlyAttendance where i_D117502managementSalary.phoneNumber=I_D117502monthlyAttendance.phoneNumber and I_D117502monthlyAttendance.normal+I_D117502monthlyAttendance.lateTimes>=4  ) , 0  ) ", null);
        //正常出勤天数
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.normalAttendance =i_D117502managementSalary.attendanceDays where i_D117502managementSalary.normalAttendance>i_D117502managementSalary.attendanceDays", null);
        //实际出勤天数
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.actualDttendance =i_D117502managementSalary.normalAttendance+i_D117502managementSalary.overTimeDay", null);
        

        //全勤奖
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set fullAttendance=200 where i_D117502managementSalary.actualDttendance>=i_D117502managementSalary.attendanceDays ", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set fullAttendance=0 where i_D117502managementSalary.actualDttendance<i_D117502managementSalary.attendanceDays ", null);

        //应发基本工资
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set  attendanceSalary=0 where actualDttendance=0", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set  attendanceSalary=basicSalary/30*actualDttendance where actualDttendance>0 and actualDttendance<18", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set  attendanceSalary=(i_D117502managementSalary.basicSalary - (i_D117502managementSalary.basicSalary/30*(i_D117502managementSalary.attendanceDays - i_D117502managementSalary.actualDttendance)))"
        +" where (actualDttendance>=18 and actualDttendance<attendanceDays) ", null);
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set  attendanceSalary=basicSalary+ fullAttendance where actualDttendance>=attendanceDays", null);

        //工龄
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.workingYears   =   ifnull("
            + "      (SELECT   i_D117502DayWork.lengthOfService from i_D117502DayWork "
            + "   where i_D117502DayWork.fullName=i_D117502managementSalary.employeeName ), 0  )  ", null);

        //工龄补贴
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.ageSubsidy   =  ifnull("
            + "      (SELECT i_D117502DayWork.ageSubsidy from i_D117502DayWork "
            + "   where i_D117502DayWork.fullName=i_D117502managementSalary.employeeName ) , 0  ) ", null);
        

        //应发工资
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set  i_D117502managementSalary.salaryPayable="
        +"i_D117502managementSalary.overtimeSalary+i_D117502managementSalary.attendanceSalary +i_D117502managementSalary.performanceSalaryW+i_D117502managementSalary.performanceSalaryM"
        +"-i_D117502managementSalary.deductionWages+i_D117502managementSalary.supplementarySalary+i_D117502managementSalary.oilSupplement+i_D117502managementSalary.ageSubsidy ", null);
        // //保险
        //  this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set personalInsuranceCosts=0 ", null);
        //个人承担保险
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set i_D117502managementSalary.personalInsuranceCosts   =  ifnull ("
            + "      (SELECT   I_D117502insurance2.personalInsurance from I_D117502insurance2 "
            + "   where I_D117502insurance2.fullName  =   i_D117502managementSalary.employeeName) , 0  ) ", null);
        
        //个税
        //<5000
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set personalTax=0 where (i_D117502managementSalary.salaryPayable-i_D117502managementSalary.personalInsuranceCosts) <=5000", null);
        //5000-8000
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set personalTax=(salaryPayable-personalInsuranceCosts-5000)*0.03 where (i_D117502managementSalary.salaryPayable-i_D117502managementSalary.personalInsuranceCosts)"
            + ">5000 and (i_D117502managementSalary.salaryPayable-i_D117502managementSalary.personalInsuranceCosts)<=8000 ", null);
        //8000-17000
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set personalTax=(salaryPayable-personalInsuranceCosts-8000)*0.1+90 where (i_D117502managementSalary.salaryPayable-i_D117502managementSalary.personalInsuranceCosts)"
            + ">=8000 and (i_D117502managementSalary.salaryPayable-i_D117502managementSalary.personalInsuranceCosts)<=17000 ", null);
        
        

        //实发工资
        this.Request.Engine.Query.QueryTable(" update i_D117502managementSalary set netSalary=salaryPayable-personalInsuranceCosts-personalTax ", null);


        //皇城根工资计算！！！
        this.Request.Engine.Query.QueryTable("DELETE FROM I_D117502salaryHCG",null);

        this.Request.Engine.Query.QueryTable("delete from I_D117502salaryHCG where salaryMonth = '" + this.Request.BizObject["date"].ToString() + "'", null);

        string sql2 = "INSERT INTO I_D117502salaryHCG(I_D117502salaryHCG.objectid  ,I_D117502salaryHCG.employeeName ,"
            + "  I_D117502salaryHCG.phoneNumber , I_D117502salaryHCG.attendanceSalary , "
            + "I_D117502salaryHCG.performance , I_D117502salaryHCG.salaryMonth ,I_D117502salaryHCG.status) "
            + "SELECT replace(uuid(),'-','') , i_D117502HCGUser.objectid , i_D117502HCGUser.phoneNumber ,"
            + "i_D117502HCGUser.basicWage , i_D117502HCGUser.standardMonthlyPerformance , "
            + "  date_format( date_add( CURDATE() , interval -1 month) ,'%Y-%m')   ,  1  FROM i_D117502HCGUser";

        this.Request.Engine.Query.QueryTable(sql2,null);
        
        //补资
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.supplementary   =  ifnull ("
            + "      (SELECT supplementaryFunds from i_D117502supplementaryDeduction3 "
            + "   where i_D117502supplementaryDeduction3.fullName  =   I_D117502salaryHCG.employeeName  and  i_D117502supplementaryDeduction3.payMonth  =  I_D117502salaryHCG.salaryMonth  ) , 0  ) ", null);
        //扣资
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.deductions   =  ifnull ("
            + "      (SELECT deduction from i_D117502supplementaryDeduction3 "
            + "   where i_D117502supplementaryDeduction3.fullName  =   I_D117502salaryHCG.employeeName  and  i_D117502supplementaryDeduction3.payMonth  =  I_D117502salaryHCG.salaryMonth  ) , 0  ) ", null);
        //标准绩效工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.performanceShould   =  ifnull ("
            + "      (SELECT  performancePay from I_D117502performanceFactor2 "
            + " where  I_D117502performanceFactor2.fullName  =   I_D117502salaryHCG.employeeName  and  I_D117502performanceFactor2.monthSalary  =  I_D117502salaryHCG.salaryMonth  ) , 0  ) ", null);
        //提成1
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.tc1   =  ifnull ("
            + "      (SELECT tatal from i_D117502Commission "
            + "   where i_D117502Commission.fullName  =   I_D117502salaryHCG.employeeName  and  i_D117502Commission.payMonth  =  I_D117502salaryHCG.salaryMonth  ) , 0  ) ", null);
        //提成2
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.tc2   =  ifnull ("
            + "      (SELECT totalAmount from i_D117502Slhvdvh1jrha7crazjpnert3n0 "
            + "   where i_D117502Slhvdvh1jrha7crazjpnert3n0.fullName  =   I_D117502salaryHCG.employeeName  and  i_D117502Slhvdvh1jrha7crazjpnert3n0.payMonth  =  I_D117502salaryHCG.salaryMonth  ) , 0  ) ", null);
        //总提成
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.commission   =  I_D117502salaryHCG.tc1+I_D117502salaryHCG.tc2", null);
        

        
        //应出勤天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.attendanceDays   =  ifnull ("
            + "(SELECT attendanceDay from I_D117502AttendanceDay where zuZhi like '%皇城根%' ), 0  ) ", null);

        //加班天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.overTimeDay=ifnull("
            + "      (SELECT I_D117502supplementaryDeduction3.overtimeDay from I_D117502supplementaryDeduction3"
            + "   where I_D117502supplementaryDeduction3.fullName  =   I_D117502salaryHCG.employeeName and I_D117502supplementaryDeduction3.payMonth  =  I_D117502salaryHCG.salaryMonth  ) , 0  ) ", null);
        //加班工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set  overtimePay=overTimeDay*(attendanceSalary/30)", null);

        //钉钉后台出勤天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.normalAttendance   =  ifnull ("
            + "(SELECT  count(1)+overTimeDay from I_D117502monthlyAttendance where I_D117502salaryHCG.phoneNumber=I_D117502monthlyAttendance.phoneNumber and I_D117502monthlyAttendance.normal+I_D117502monthlyAttendance.lateTimes>=4  ) , 0  ) ", null);
        //正常出勤天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.normalAttendance =I_D117502salaryHCG.attendanceDays where I_D117502salaryHCG.normalAttendance>I_D117502salaryHCG.attendanceDays", null);
        //实际出勤天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.actualDttendance =I_D117502salaryHCG.normalAttendance+I_D117502salaryHCG.overTimeDay", null);
        

        //全勤奖
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set fullAttendance=100 where I_D117502salaryHCG.actualDttendance>=I_D117502salaryHCG.attendanceDays ", null);
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set fullAttendance=0 where I_D117502salaryHCG.actualDttendance<I_D117502salaryHCG.attendanceDays ", null);

        //应发基本工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set  basicSalary=0 where actualDttendance=0", null);
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set  basicSalary=attendanceSalary/30*actualDttendance where actualDttendance>0 and actualDttendance<18", null);
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set  basicSalary=(I_D117502salaryHCG.attendanceSalary - (I_D117502salaryHCG.attendanceSalary/30*(I_D117502salaryHCG.attendanceDays - I_D117502salaryHCG.actualDttendance)))"
        +" where (actualDttendance>=18 and actualDttendance<attendanceDays) ", null);
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set  basicSalary=attendanceSalary+ fullAttendance where actualDttendance>=attendanceDays", null);

        //工龄
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.lengthOfService   =   ifnull("
            + "      (SELECT   i_D117502DayWork.lengthOfService from i_D117502DayWork "
            + "   where i_D117502DayWork.fullName=I_D117502salaryHCG.employeeName ), 0  )  ", null);

        //工龄补贴
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.senioritySalary   =  ifnull("
            + "      (SELECT i_D117502DayWork.ageSubsidy from i_D117502DayWork "
            + "   where i_D117502DayWork.fullName=I_D117502salaryHCG.employeeName ) , 0  ) ", null);
        

        //应发工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set  I_D117502salaryHCG.payable="
        +"I_D117502salaryHCG.overtimePay+I_D117502salaryHCG.basicSalary "
        +"-I_D117502salaryHCG.deductions+I_D117502salaryHCG.supplementary+I_D117502salaryHCG.senioritySalary+I_D117502salaryHCG.commission ", null);
        // //保险
        //  this.Request.Engine.Query.QueryTable(" update i_D117502staffSalary set personalInsuranceCosts=0 ", null);
        //个人承担保险
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set I_D117502salaryHCG.personalFiveInsurances   =  ifnull ("
            + "      (SELECT   I_D117502insurance3.personalInsurance from I_D117502insurance3 "
            + "   where I_D117502insurance3.fullName  =   I_D117502salaryHCG.employeeName) , 0  ) ", null);
        
        //个税
        //<5000
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set personalIncomeTax=0 where (I_D117502salaryHCG.payable-I_D117502salaryHCG.personalFiveInsurances) <=5000", null);
        //5000-8000
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set personalIncomeTax=(payable-personalFiveInsurances-5000)*0.03 where (I_D117502salaryHCG.payable-I_D117502salaryHCG.personalFiveInsurances)"
            + ">5000 and (I_D117502salaryHCG.payable-I_D117502salaryHCG.personalFiveInsurances)<=8000 ", null);
        //8000-17000
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set personalIncomeTax=(payable-personalFiveInsurances-8000)*0.1+90 where (I_D117502salaryHCG.payable-I_D117502salaryHCG.personalFiveInsurances)"
            + ">=8000 and (I_D117502salaryHCG.payable-I_D117502salaryHCG.personalFiveInsurances)<=17000 ", null);
        
        

        //实发工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salaryHCG set netSalary=payable-personalFiveInsurances-personalIncomeTax ", null);
        

        //源绿薪资核算

        this.Request.Engine.Query.QueryTable("DELETE FROM I_D117502salary",null);

        this.Request.Engine.Query.QueryTable("delete from I_D117502salary where salaryMonth = '" + this.Request.BizObject["date"].ToString() + "'", null);

        string sql3 = "INSERT INTO I_D117502salary(I_D117502salary.objectid,I_D117502salary.employeeName,I_D117502salary.employeeNameT,"
            + "  I_D117502salary.phoneNumber , I_D117502salary.attendanceSalary , "
            + "I_D117502salary.standardWeeklyPerformance , I_D117502salary.standardMonthlyPerformance,I_D117502salary.salaryMonth ,I_D117502salary.status) "
            + "SELECT replace(uuid(),'-','') , I_D117502YLUser.objectid ,I_D117502YLUser.userName , I_D117502YLUser.phoneNumber ,"
            + "I_D117502YLUser.basicWage , I_D117502YLUser.standardWeekPerformance ,I_D117502YLUser.standardMonthlyPerformance ,"
            + "  date_format( date_add( CURDATE() , interval -1 month) ,'%Y-%m'),1 FROM I_D117502YLUser";

        this.Request.Engine.Query.QueryTable(sql3,null);
        
        //补资
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.supplementary   =  ifnull ("
            + "      (SELECT supplementaryFunds from I_D117502supplementaryDeduction2 "
            + "   where I_D117502supplementaryDeduction2.fullName  =   I_D117502salary.employeeName  and  I_D117502supplementaryDeduction2.payMonth  =  I_D117502salary.salaryMonth  ) , 0  ) ", null);
        //扣资
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.deductions   =  ifnull ("
            + "      (SELECT deduction from i_D117502supplementaryDeduction2 "
            + "   where i_D117502supplementaryDeduction2.fullName  =   I_D117502salary.employeeName  and  I_D117502supplementaryDeduction2.payMonth  =  I_D117502salary.salaryMonth  ) , 0  ) ", null);
        //周绩效工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.weeklyPerformanceDue   =  ifnull ("
            + "      (SELECT  (weekPerformance*weeklyPerformanceFactor) from I_D117502performanceFactor3 "
            + " where  I_D117502performanceFactor3.fullName  =   I_D117502salary.employeeName  and  I_D117502performanceFactor3.salaryMonth  =  I_D117502salary.salaryMonth  ) , 0  ) ", null);
        //月绩效工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.desiredMonthlyPerformance   =  ifnull ("
            + "      (SELECT  (monthPerformanceScore*monthperformance) from I_D117502performanceFactor3 "
            + " where  I_D117502performanceFactor3.fullName  =   I_D117502salary.employeeName  and  I_D117502performanceFactor3.salaryMonth  =  I_D117502salary.salaryMonth  ) , 0  ) ", null);

        //应出勤天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.attendanceDays   =  ifnull ("
            + "(SELECT attendanceDay from I_D117502AttendanceDay where zuZhi like '%源绿%' ), 0  ) ", null);

        //加班天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.overTimeDay=ifnull("
            + "      (SELECT I_D117502supplementaryDeduction2.overtimeDay from I_D117502supplementaryDeduction2"
            + "   where I_D117502supplementaryDeduction2.fullName  =   I_D117502salary.employeeName and I_D117502supplementaryDeduction2.payMonth  =  I_D117502salary.salaryMonth  ) , 0  ) ", null);
        //加班工资
        this.Request.Engine.Query.QueryTable("update I_D117502salary set  overtimeSalary=overTimeDay*(attendanceSalary/30)", null);

        //钉钉后台出勤天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.normalAttendance   =  ifnull ("
            + "(SELECT  count(1)+overTimeDay from I_D117502monthlyAttendance where I_D117502salary.phoneNumber=I_D117502monthlyAttendance.phoneNumber and I_D117502monthlyAttendance.normal+I_D117502monthlyAttendance.lateTimes>=4  ) , 0  ) ", null);
        //正常出勤天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.normalAttendance =I_D117502salary.attendanceDays where I_D117502salary.normalAttendance>I_D117502salary.attendanceDays", null);
        //实际出勤天数
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.actualDttendanceDays =I_D117502salary.normalAttendance+I_D117502salary.overTimeDay", null);
        

        //全勤奖
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set fullAttendance=200 where I_D117502salary.normalAttendance>=I_D117502salary.attendanceDays ", null);
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set fullAttendance=0 where I_D117502salary.normalAttendance<I_D117502salary.attendanceDays ", null);
        //安全奖
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.safetyAward   =  ifnull ("
            + "      (SELECT safetyAward from I_D117502SafetyAward "
            + "   where I_D117502SafetyAward.fullName  =   I_D117502salary.employeeName  and  I_D117502SafetyAward.salaryMonth  =  I_D117502salary.salaryMonth  ) , 0  ) ", null);
        //计件工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.countSalary   =  ifnull ("
            + "      (SELECT pieceRate from I_D117502PieceRate "
            + "   where I_D117502PieceRate.fullName  =   I_D117502salary.employeeName  and  I_D117502PieceRate.payMonth  =  I_D117502salary.salaryMonth  ) , 0  ) ", null);

        //应发基本工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set  basicSalary=0 where actualDttendanceDays=0", null);
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set  basicSalary=attendanceSalary/30*actualDttendanceDays where actualDttendanceDays>0 and actualDttendanceDays<18", null);
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set  basicSalary=(I_D117502salary.attendanceSalary - (I_D117502salary.attendanceSalary/30*(I_D117502salary.attendanceDays - I_D117502salary.actualDttendanceDays)))"
        +" where (actualDttendanceDays>=18 and actualDttendanceDays<attendanceDays) ", null);
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set  basicSalary=attendanceSalary+ fullAttendance where actualDttendanceDays>=attendanceDays", null);

        //工龄补贴
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.seniorityAllowance   =  ifnull("
            + "      (SELECT i_D117502DayWork.ageSubsidy*20 from i_D117502DayWork "
            + "   where i_D117502DayWork.fullName=I_D117502salary.employeeName ) , 0  ) ", null);
        

        //应发工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set  I_D117502salary.payable="
        +"I_D117502salary.overtimeSalary+I_D117502salary.basicSalary+I_D117502salary.safetyAward+I_D117502salary.countSalary"
        +"-I_D117502salary.deductions+I_D117502salary.supplementary+I_D117502salary.seniorityAllowance", null);
       
        //个人承担保险
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set I_D117502salary.personalInsuranceCosts   =  ifnull ("
            + "      (SELECT   I_D117502insurance4.personalInsurance from I_D117502insurance4 "
            + "   where I_D117502insurance4.fullName  =   I_D117502salary.employeeName) , 0  ) ", null);
        
        //个税
        //<5000
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set personalIncomeTax=0 where (I_D117502salary.payable-I_D117502salary.personalInsuranceCosts) <=5000", null);
        //5000-8000
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set personalIncomeTax=(payable-personalInsuranceCosts-5000)*0.03 where (I_D117502salary.payable-I_D117502salary.personalInsuranceCosts)"
            + ">5000 and (I_D117502salary.payable-I_D117502salary.personalInsuranceCosts)<=8000 ", null);
        //8000-17000
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set personalIncomeTax=(payable-personalInsuranceCosts-8000)*0.1+90 where (I_D117502salary.payable-I_D117502salary.personalInsuranceCosts)"
            + ">=8000 and (I_D117502salary.payable-I_D117502salary.personalInsuranceCosts)<=17000 ", null);
        
        

        //实发工资
        this.Request.Engine.Query.QueryTable(" update I_D117502salary set netSalary=payable-personalInsuranceCosts-personalIncomeTax ", null);

    }
}