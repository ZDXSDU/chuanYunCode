# 前端
- HTML 标记语言 <html></html>
- CSS 层叠样式表 html {width:100px;}
- JavaScript 脚本语言

# JavaScript ( JS )与名词解释
    1. 弱类型的动态的运行在客户端上的脚本语言（NODE）
    2. ECMAScript: 是一个标准，不是一个语言（ES）
    3. 变量：储存在内存中的数据（断电、重启 数据丢失）
    4. 内存堆栈与值类型和引用类型
    5. JS代码的结尾可以加分号也可以不加分号，但应保证相同的语法风格，下面的单引号双引号相同
    6. 调试JS代码：在浏览器中按  ⬇  唤起开发者工具，断点在“源代码选项卡”中对应代码的行号处点击即可设置断点，调试过程分单步、步入、步出、跳过方法体等；

<kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>I</kbd>

## 关键字

```JavaScript
console.log("123"); // 将123打印到开发者工具的输出窗口
alert("123"); // 将123以弹窗的形式弹出
```
#### 变量声明
- var ：声明一个变量（极其不推荐使用）。
- const、let
能用const就用const，不能用const就用let；极少数情况下使用var。
- 判断变量的类型
    控制台看颜色，typeof()

```JavaScript
    // 声明变量的方式
    let 变量名 = 变量的值; // 声明并赋值（变量初始化）
    let 变量名; // 仅声明
    // 变量名的命名规范--小驼峰、_、常量（const）大写
    var that = this; // 不标准
    const THAT = this; // 标准
    let inputDate;
    let input_date;

    let Mystring3; // 声明了一个变量 不推荐使用这种写法
    Mystring3 = true; // 布尔 值：true
    Mystring3 = 3.141592654; // 数字，值：3.141592654
    // 异常（BUG）
    let Mystring4 = {}; // 数据类型
```
## 简单数据类型

#### 数字类型
- 1  120 1.256 0.9 
```JavaScript
let num = 100;
let num = 100.23;
let num = 0.85;
```
- 运算符 + - * / % ** ++ -- 
```JavaScript
let num = 10;
console.log(num%3);//1
```
#### 字符串类型
尤其注意：当使用+运算符的表达式中 有一方是string类型，那么结果也会变成string类型，
```JavaScript
let Mystring = "大家上午好！"; //等价于:let Mystring = '大家上午好！'; 推荐使用单引号
let char = '';
let SFSDAUIHIUSADFH = "";
// 字符串的拼接
let b = '上午好';
let c = '中午好';
let d = '晚上好';
let r = '老商'
let a = '神仙大人' + r + ''; // 引引加
let total = a + b + c + d;
console.log(total)
let yu = 'Markdown';
let ui = '使用' + yu + '实现键盘按钮和流程图';
console.log(ui);

// 转义      转义符：\
let b = "'a'\"b\""; 
console.log(b); // 'a'"b"
let a = `/style><div class='b'>`;
console.log(typeof(a));
```

#### 布尔类型

###### 逻辑运算符汇总
· > >= < <= != : 依次是大于、大于等于、小于、小于等于、不等于
· == 等于 （1==1 //true；       '2'== 2 //true)
· === 严格等于 （严格等于：数据类型一致，值相等）
· != 不等于
· !== 严格不等于
· || 逻辑“或”
· && 逻辑“与”（并且）
· !  逻辑“非”（取反）

- 逻辑运算符与关系表达式
```JavaScript
let bool = false;
let a = 3 > 5 || 5 <= 5; // true
let b = 3 > 5 && 5 <= 5; // false
let c = !true;
// || 逻辑“或”
// && 逻辑“与”（并且）
// !  逻辑“非”（取反）
// ||、&&、！：逻辑运算符，条件表达式的运算符
let a = '3' == 3; // 隐式类型转换
let b = 30 + '5'; // + ''   
console.log(1 != '1'); //t
console.log(1 !== '1'); // 
console.log((typeof (1) == typeof ('1')) && (1 == '1'));
```
- if - else
```JavaScript
if (条件表达式或者布尔类型的变量) {
   // 当条件表达式的结果为true 或者布尔类型的变量值为true时执行这里的代码
 else {
   // 当。。。。。false
//示例：
let a = 8;
let b = 1;
let c = b >= a;

if (c) {
    niaoniao();
} else {
    heshui();
}
function niaoniao() {
    // 尿尿
    console.log("niaoniao");
}
function heshui() {
    // 喝水
    console.log("heshui");
}
```

#### undefined

```JavaScript
let a;
console.log(a); // 声明但没有初始化时
```

## 复杂数据类型

### 数组 arr

```javaScript
        let PJH_A = "1";
        let PJH_B = "2";
        let PJH_C = "3";
        let PJH_D = "4";
        let PJH_E = "5";
        let PJH_F = "6";


        let MY_Array = []; // 定义了一个空数组
        let MY_Array2 = ["1", "1", "1", "1", "1", "1"]; // 定义了一个数组 "1": 数组里面的【元素】
        // 数组的长度：数组中元素的个数就是数组的长度  MY_Array2的长度是 6 Length
        console.log(MY_Array2);
        console.log(MY_Array2.length); // 获取MY_Array2数组的长度 语法：数组名.length

        // 数组的索引（又名下标）  从数组的开始一直到数组的结尾 排排坐 从0开始到数组长度-1结束
        // 取 MY_Array2 第三个元素的值（访问数组中的元素）
        // 语法： 数组名[元素的索引]
        console.log(MY_Array2[3]);

        // 获取脑袋和腚
        console.log(MY_Array2[0]);
        console.log(MY_Array2[MY_Array2.length-1]);
```
用于存储很多相同数据类型的值的集合