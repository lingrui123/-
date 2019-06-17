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

namespace FileManagement
{
    public partial class AddUserForm : DevExpress.XtraEditors.XtraForm
    {
        public MySqlConnectHelper mysqlUser =new MySqlConnectHelper();
        Encrypt PwdMD5 = new Encrypt();
        public AddUserForm()
        {
            InitializeComponent();
            Type enumType = typeof(BuMen);
            foreach (string str in Enum.GetNames(enumType))
            {
                this.bumen.Items.Add(str);
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string str1 = password.Text;
            string str2 = Spassword.Text;
            if (str1 != str2)
            {
                MessageBox.Show("两次密码输入不同", "错误");
                return;
            }
            else if (str1 == string.Empty )
            {
                MessageBox.Show("新增用户密码不能为空", "错误");
                return;
            }
            else if (username.Text == string.Empty )
            {
                MessageBox.Show("用户名不能为空", "错误");
                return;
            }
            else if (this.name.Text == string.Empty)
            {
                MessageBox.Show("姓名不能为空", "错误");
                return;
            }
            string Username = this.name.Text;
            string UserLoginName = this.username.Text;
            string Pwd = this.password.Text;
             Pwd = PwdMD5.EncryptPWD(Pwd); 
            string worknum = this.gonghao.Text;
            string personDe = this.bumen.Text;
            string sqlstr = "insert into persons(Name,NameLogin,LoginPwd,WorkNum,PersonDepartment,Position)values('"+ Username + "','"+ UserLoginName + "','"+ Pwd + "','"+ worknum + "','"+ personDe + "',''); ";
            var tem= mysqlUser.getmysqlcom(sqlstr);
            if (!tem)
            {
                MessageBox.Show("添加人员出错！");
                return;
            }
            simpleButton1.DialogResult = DialogResult.OK;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}