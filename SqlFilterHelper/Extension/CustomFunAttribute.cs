using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// 自定义新特性
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class CustomFunAttribute:Attribute
{
    [Display(Name = "名称")]
    public string FunName { get; set; }

    [Display(Name = "类型")]
    public string FunType { get; set; }

    [Display(Name = "说明")]
    public string FunRemark { get; set; }

    [Display(Name = "参数列表")]
    public string ParaList { get; set; }
}