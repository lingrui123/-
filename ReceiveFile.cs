using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Configuration;
using System.Transactions;

namespace FileManagement
{
    public partial class ReceiveFile : DevExpress.XtraEditors.XtraForm
    {
        private List<FileInfo> FinfoList = new List<FileInfo>();
        MySqlConnectHelper mysql = new MySqlConnectHelper();
        string OldId = "";
        public ReceiveFile(string Id)
        {
            InitializeComponent();
            OldId = Id;
            string sqlperstr = "select * from persons";
            var persons = mysql.getmysqlread(sqlperstr);
            while (persons.Read())
            {
                this.RecivePerCB.Properties.Items.Add(persons["Name"].ToString());
                this.fafangrenCB.Properties.Items.Add(persons["Name"].ToString());
            }
            Type enumType = typeof(BuMen);
            foreach (string str in Enum.GetNames(enumType))
            {
                this.fafangbumenCB.Properties.Items.Add(str);
            }
            if (Id != null)
            {
                string sqlstr = "select * from externaldocuments where Id='" + Id + "'";
                var doc = mysql.getmysqlread(sqlstr);
                while (doc.Read())
                {
                    this.SendMailPer.Text = doc["DispatchUnit"].ToString();
                    this.SendDate.Text = doc["DispatchDate"].ToString();
                    this.SendMethod.Text = doc["DispatchTheme"].ToString();
                    this.FileCode.Text = doc["FileNum"].ToString();
                    this.Miji.Text = doc["SecretClass"].ToString();
                    if (doc["IsShoukong"].ToString() == "是")
                    {
                        this.radioY.Checked = true;
                    }
                    else
                    {
                        this.radioN.Checked = true;
                    }


                    this.FileName.Text = doc["FileName"].ToString();
                }
                this.button1.Visible = false;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(this.SendMailPer.Text))
            {
                MessageBox.Show("发件人不能为空");
                return;
            }
            if (string.IsNullOrEmpty(this.SendMethod.Text))
            {
                MessageBox.Show("发件主题不能为空");
                return;
            }
            if (string.IsNullOrEmpty(this.FileCode.Text))
            {
                MessageBox.Show("文件编号不能为空");
                return;
            }
            if (string.IsNullOrEmpty(OldId) && FinfoList.Count <= 0)
            {
                MessageBox.Show("请上传相应文件！");
                return;
            }
            //using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码
            //{
            if (string.IsNullOrEmpty(OldId))
            {
                foreach (var item in FinfoList)
                {
                    try
                    {
                        var doucument = new externaldocuments();
                        doucument.DispatchDate = this.SendDate.Text;
                        doucument.DispatchTheme = this.SendMethod.Text;
                        doucument.DispatchUnit = this.SendMailPer.Text;
                        doucument.DispensePerson = this.fafangrenCB.Text;
                        doucument.DispenseUnit = this.fafangbumenCB.Text;
                        doucument.FileNum = this.FileCode.Text;
                        if (this.radioY.Checked)
                        {
                            doucument.IsShoukong = "是";
                        }
                        else
                        {
                            doucument.IsShoukong = "否";
                        }
                        doucument.ReceivePerson = this.RecivePerCB.Text;
                        doucument.SecretClass = this.Miji.Text;
                        doucument.FileName = item.fileName;
                        doucument.FileRoute = GetAppConfig();
                        doucument.FileRoute = doucument.FileRoute.Replace("\\", "\\\\");
                        string sqlstr = "";
                        
                            sqlstr = "insert into externaldocuments(DispatchUnit,DispatchDate,DispatchTheme,FileName,FileRoute,FileNum,ReceivePerson,DispensePerson,DispenseUnit,IsShoukong,SecretClass)values('{0}','{1}','{2}','{3}','" + doucument.FileRoute + "','{4}','{5}','{6}','{7}','{8}','{9}'); ";
                        sqlstr = string.Format(sqlstr, doucument.DispatchUnit, doucument.DispatchDate, doucument.DispatchTheme, doucument.FileName, doucument.FileNum, doucument.ReceivePerson, doucument.DispensePerson, doucument.DispenseUnit, doucument.IsShoukong, doucument.SecretClass);
                        var persons = mysql.getmysqlcom(sqlstr);
                        if (persons)
                        {
                            File.Copy(item.fileAllname, GetAppConfig() + item.fileName, true);
                        }
                        else
                        {
                            throw new Exception("存储基本信息出错");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("文件保存出错:" + ex.Message);
                        return;
                    }
                }
            }
            else
            {
                try
                {
                    var doucument = new externaldocuments();
                    doucument.DispatchDate = this.SendDate.Text;
                    doucument.DispatchTheme = this.SendMethod.Text;
                    doucument.DispatchUnit = this.SendMailPer.Text;
                    doucument.DispensePerson = this.fafangrenCB.Text;
                    doucument.DispenseUnit = this.fafangbumenCB.Text;
                    doucument.FileNum = this.FileCode.Text;
                    if (this.radioY.Checked)
                    {
                        doucument.IsShoukong = "是";
                    }
                    else
                    {
                        doucument.IsShoukong = "否";
                    }
                    doucument.ReceivePerson = this.RecivePerCB.Text;
                    doucument.SecretClass = this.Miji.Text;
                    string sqlstr = "";
                    sqlstr = "update externaldocuments set DispatchUnit='{0}',DispatchDate='{1}',DispatchTheme='{2}', FileNum='{3}',ReceivePerson='{4}',DispensePerson='{5}',DispenseUnit='{6}',IsShoukong='{7}',SecretClass='{8}' where Id='" + OldId + "' ";
                    sqlstr = string.Format(sqlstr, doucument.DispatchUnit, doucument.DispatchDate, doucument.DispatchTheme, doucument.FileNum, doucument.ReceivePerson, doucument.DispensePerson, doucument.DispenseUnit, doucument.IsShoukong, doucument.SecretClass);
                    var persons = mysql.getmysqlcom(sqlstr);
                    if (persons)
                    {
                    }
                    else
                    {
                        throw new Exception("存储基本信息出错");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("文件保存出错:" + ex.Message);
                    return;
                }

            }

            MessageBox.Show("保存成功！");
            this.DialogResult = DialogResult.OK;
            this.Close();

            //    ts.Complete();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //创建一个对话框对象
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "请选择上传的文件";

            ofd.Filter = "所有文件|*";

            ofd.Multiselect = true;
            //如果你点了“确定”按钮
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var item in ofd.FileNames)
                {
                    var Finfo = new FileInfo();
                    var position = item.LastIndexOf("\\");
                    var fileName = item.Substring(position + 1);
                    foreach (var file in FinfoList)
                    {
                        if (file.fileName == fileName)
                        {
                            MessageBox.Show("已存在相同的文件名");
                            return;
                        }
                    }
                    Finfo.fileName = item.Substring(position + 1);
                    Finfo.fileAllname = item;
                    FinfoList.Add(Finfo);
                    this.FileName.Text += fileName + "\n\r";
                }
            }
        }
        public static string GetAppConfig()
        {
            string file = Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == "path")
                {
                    return config.AppSettings.Settings["path"].Value.ToString();
                }
            }
            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确定关闭吗？", "关闭窗口", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

        }
    }
    public class FileInfo
    {
        public string fileAllname { get; set; }
        public string fileName { get; set; }
    }
    public enum BuMen
    {
        技术部,
        工程部,
        财务部,
        行政部,
        销售部,
        研发部,
        证券部
    }
}