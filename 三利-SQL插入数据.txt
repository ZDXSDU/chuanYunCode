string sql2 = "INSERT INTO i_D117502managementSalary(i_D117502managementSalary.objectid  ,i_D117502managementSalary.employeeNameT ,i_D117502managementSalary.employeeName ,"
            + "  i_D117502managementSalary.phoneNumber , i_D117502managementSalary.rank , i_D117502managementSalary.basicSalary , "
            + "i_D117502managementSalary.weeklyPerformance , i_D117502managementSalary.performanceSalary , i_D117502managementSalary.salaryMonth ,i_D117502managementSalary.status) "
            + "SELECT replace(uuid(),'-','') , i_D117502QFUser.userName ,i_D117502QFUser.objectid, i_D117502QFUser.phoneNumber , "
            + " i_D117502QFUser.rank , i_D117502QFUser.basicWage , i_D117502QFUser.standardWeekPerformance , "
            + " i_D117502QFUser.standardMonthlyPerformance , date_format( date_add( CURDATE() , interval -1 month) ,'%Y-%m')   ,  1  FROM i_D117502QFUser";
