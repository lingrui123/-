using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManagement
{
  public  class MySqlConnectHelper
    {
        MySqlConnection myContree;
        MySqlConnection mysqlcon;
        MySqlConnection myCon;
        string M_str_sqlcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        #region  建立MySql数据库连接
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回MySqlConnection对象</returns>
        public MySqlConnection getmysqlcon()
        {

             myCon = new MySqlConnection(M_str_sqlcon);
            return myCon;
        }
        #endregion

        #region  执行MySqlCommand命令
        /// <summary>
        /// 执行MySqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public bool getmysqlcom(string M_str_sqlstr)
        {
            MySqlConnection mysqlcon = this.getmysqlcon();
            mysqlcon.Open();
            MySqlTransaction myTrans = mysqlcon.BeginTransaction();
            //为事务创建一个命令
            MySqlCommand myCommand = new MySqlCommand(M_str_sqlstr, mysqlcon);  
            try
            {
        
                myCommand.Connection = mysqlcon;
                myCommand.Transaction = myTrans;
                myCommand.ExecuteNonQuery();
                myTrans.Commit();//提交
                myCommand.Dispose();
                mysqlcon.Close();
                mysqlcon.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                myTrans.Rollback();//遇到错误，回滚
                return false;
            }
            finally
            {
                mysqlcon.Close();
             
            }
        }
        
        #endregion

        #region  创建MySqlDataReader对象
        /// <summary>
        /// 创建一个MySqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回MySqlDataReader对象</returns>
        public MySqlDataReader getmysqlread(string M_str_sqlstr)
        {
             mysqlcon = this.getmysqlcon();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            mysqlcom.CommandTimeout = 3600;
            mysqlcon.Open();
          MySqlDataReader mysqlread = mysqlcom.ExecuteReader(CommandBehavior.CloseConnection);//数据读取完成后自动关闭数据库
            return mysqlread;
        }
       

        #endregion
    }
}
