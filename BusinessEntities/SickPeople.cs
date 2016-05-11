/********************************************************************************
** Copyright(c) 2016  All Rights Reserved. 
** auth：索俊杰
** mail：suojunjie@hotmail.com
** date： 2016/5/9 23:29:04 
** desc：  
** Ver : V1.0.0 
*********************************************************************************/

namespace BusinessEntities
{
    public class SickPeople : User
    {
        public string SickName { get; set; }

        public string Remark { get; set; }
    }
}