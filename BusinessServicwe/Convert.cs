/********************************************************************************
** Copyright(c) 2016  All Rights Reserved. 
** auth：索俊杰
** mail：suojunjie@hotmail.com
** date： 2016/5/9 17:29:45 
** desc：  
** Ver : V1.0.0 
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService
{
    /// <summary>
    /// CRM实体转化
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class ConvertClass<T> where T : new()
    {

        #region 变量
        // public OrganizationServiceProxy Service;

      

        /// <summary>
        /// 全部属性
        /// </summary>
        private PropertyInfo[] _propertyinfo;
        #endregion

        #region 构造函数
        public ConvertClass()
        {
            T t = new T();
            //this.Service = service;
            _propertyinfo = t.GetType().GetProperties();
        }
        #endregion

        #region 批量转换
        /// <summary>
        /// 将Table转化为List
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<T> ToList(DataTable table)
        {
            if (table == null)
                return null;
            List<T> list = new List<T>();
            T t;


            //循环表
            for (int count = 0; count < table.Rows.Count; count++)
            {
                t = new T();
                t = ToT(table.Rows[count]);
                if (t != null)
                    list.Add(t);
            }
            return list;
        }



       

        #endregion

        #region 单个转换
        /// <summary>
        /// 将dataRow转化为T
        /// </summary>
        /// <param name="drRow"></param>
        /// <returns></returns>
        public T ToT(DataRow drRow)
        {
            T t = new T();
            foreach (PropertyInfo property in _propertyinfo)
            {
                //T 属性名称
                string propertyName = property.Name;

                string readerValue = drRow[propertyName].ToString();

                //T 属性类型
                string propertyTypeName = property.PropertyType.Name;
                if (propertyTypeName != "String")
                {
                    if (readerValue == "")
                    {
                        property.SetValue(t, null, null);

                    }
                    else
                    {
                        try
                        {
                            if (propertyTypeName == "Guid")
                            {
                                Guid accountID = new Guid(readerValue);
                                property.SetValue(t, accountID, null);
                                continue;
                            }
                            if (propertyTypeName == "Boolean")
                            {
                                bool bools = System.Convert.ToBoolean(readerValue);
                                property.SetValue(t, bools, null);
                                continue;
                            }
                            if (propertyTypeName == "DateTime")
                            {
                                DateTime dt = DateTime.Parse(readerValue);
                                property.SetValue(t, dt, null);
                                continue;
                            }
                            if (propertyTypeName == "Int32")
                            {
                                int proccessdata = System.Convert.ToInt32(readerValue);
                                property.SetValue(t, proccessdata, null);
                                continue;
                            }
                            if (propertyTypeName == "Decimal")
                            {
                                decimal proccessdata = System.Convert.ToDecimal(readerValue);
                                property.SetValue(t, proccessdata, null);
                                continue;
                            }
                            //整数Int,用于区别选项集如果为空
                            if (propertyTypeName == "Nullable`1")
                            {

                                int? proccessdata = System.Convert.ToInt32(readerValue);
                                property.SetValue(t, proccessdata, null);
                                continue;
                            }



                        }
                        catch
                        {
                            property.SetValue(t, null, null);
                            continue;
                        }
                    }

                }
                else
                {

                    property.SetValue(t, readerValue, null);
                }
            }

            return t;
        }

      

        #endregion

        #region 私有方法

        private void WriteLog(string message)
        {
            //LogHelper.Error(message);
        }

        private void WriteDBLog(string message, T t)
        {

            //DBLogHelper.Log();
        }


      

        #endregion
    }
}
