using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileManagement
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        MySqlConnectHelper mysql = new MySqlConnectHelper();
        public Form1()
        {
            InitializeComponent();
            //设置自动换行
            this.dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //设置自动调整高度
           // this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            datasource();
        }
        /// <summary>
        /// 表数据
        /// </summary>
        private void datasource()
        {
            string sql = "select * from externaldocuments";
            var tempread = mysql.getmysqlread(sql);
            while (tempread.Read())
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell Id = new DataGridViewTextBoxCell();
                Id.Value = tempread["Id"].ToString();
                row.Cells.Add(Id);
                DataGridViewTextBoxCell DispatchUnit = new DataGridViewTextBoxCell();
                DispatchUnit.Value = tempread["DispatchUnit"].ToString();
                row.Cells.Add(DispatchUnit);
                DataGridViewTextBoxCell DispatchDate = new DataGridViewTextBoxCell();
                DispatchDate.Value = tempread["DispatchDate"].ToString();
                row.Cells.Add(DispatchDate);
                DataGridViewTextBoxCell DispatchTheme = new DataGridViewTextBoxCell();
                DispatchTheme.Value = tempread["DispatchTheme"].ToString();
                row.Cells.Add(DispatchTheme);
                DataGridViewTextBoxCell FileName = new DataGridViewTextBoxCell();
                FileName.Value = tempread["FileName"].ToString();
                row.Cells.Add(FileName);
                DataGridViewTextBoxCell FileNum = new DataGridViewTextBoxCell();
                FileNum.Value = tempread["FileNum"].ToString();
                row.Cells.Add(FileNum);
                DataGridViewTextBoxCell ReceivePerson = new DataGridViewTextBoxCell();
                ReceivePerson.Value = tempread["ReceivePerson"].ToString();
                row.Cells.Add(ReceivePerson);
                DataGridViewTextBoxCell DispensePerson = new DataGridViewTextBoxCell();
                DispensePerson.Value = tempread["DispensePerson"].ToString();
                row.Cells.Add(DispensePerson);
                DataGridViewTextBoxCell DispenseUnit = new DataGridViewTextBoxCell();
                DispenseUnit.Value = tempread["DispenseUnit"].ToString();
                row.Cells.Add(DispenseUnit);
                DataGridViewTextBoxCell IsShoukong = new DataGridViewTextBoxCell();
                IsShoukong.Value = tempread["IsShoukong"].ToString();
                row.Cells.Add(IsShoukong);
                DataGridViewTextBoxCell SecretClass = new DataGridViewTextBoxCell();
                SecretClass.Value = tempread["SecretClass"].ToString();
                row.Cells.Add(SecretClass);
                row.Height =50;
                dataGridView1.Rows.Add(row);
            }
            tempread.Close();
        }
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (this.dataGridView1.Columns[e.ColumnIndex].HeaderText == "操作")
                {
                    StringFormat sf = StringFormat.GenericDefault.Clone() as StringFormat;//设置重绘入单元格的字体样式
                    sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.EllipsisCharacter;

                    e.PaintBackground(e.CellBounds, false);//重绘边框
                    //设置要写入字体的大小
                    System.Drawing.Font myFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    SizeF sizeDel = e.Graphics.MeasureString("打开目录", myFont);
                    SizeF sizeMod = e.Graphics.MeasureString("修改文件", myFont);
                    SizeF sizeLook = e.Graphics.MeasureString("删除记录", myFont);

                    float fDel = sizeDel.Width / (sizeDel.Width + sizeMod.Width + sizeLook.Width); //
                    float fMod = sizeMod.Width / (sizeDel.Width + sizeMod.Width + sizeLook.Width);
                    float fLook = sizeLook.Width / (sizeDel.Width + sizeMod.Width + sizeLook.Width);

                    //设置每个“按钮的边界”
                    RectangleF rectDel = new RectangleF(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width * fDel, e.CellBounds.Height);
                    RectangleF rectMod = new RectangleF(rectDel.Right, e.CellBounds.Top, e.CellBounds.Width * fMod, e.CellBounds.Height);
                    RectangleF rectLook = new RectangleF(rectMod.Right, e.CellBounds.Top, e.CellBounds.Width * fLook, e.CellBounds.Height);
                    e.Graphics.DrawString("打开目录", myFont, Brushes.Black, rectDel, sf); //绘制“按钮”
                    e.Graphics.DrawString("修改文件", myFont, Brushes.Black, rectMod, sf);
                    e.Graphics.DrawString("删除记录", myFont, Brushes.Black, rectLook, sf);
                    e.Handled = true;
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {

                Point curPosition = e.Location;//当前鼠标在当前单元格中的坐标
                if (this.dataGridView1.Columns[e.ColumnIndex].HeaderText == "操作")
                {
                    Graphics g = this.dataGridView1.CreateGraphics();
                    System.Drawing.Font myFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    SizeF sizeDel = g.MeasureString("打开目录", myFont);
                    SizeF sizeMod = g.MeasureString("修改文件", myFont);
                    SizeF sizeLook = g.MeasureString("删除记录", myFont);
                    float fDel = sizeDel.Width / (sizeDel.Width + sizeMod.Width + sizeLook.Width);
                    float fMod = sizeMod.Width / (sizeDel.Width + sizeMod.Width + sizeLook.Width);
                    float fLook = sizeLook.Width / (sizeDel.Width + sizeMod.Width + sizeLook.Width);

                    Rectangle rectTotal = new Rectangle(0, 0, this.dataGridView1.Columns[e.ColumnIndex].Width, this.dataGridView1.Rows[e.RowIndex].Height);
                    RectangleF rectPass = new RectangleF(rectTotal.Left, rectTotal.Top, rectTotal.Width * fDel, rectTotal.Height);
                    RectangleF rectRefuse = new RectangleF(rectPass.Right, rectTotal.Top, rectTotal.Width * fMod, rectTotal.Height);
                    RectangleF rectLook = new RectangleF(rectRefuse.Right, rectTotal.Top, rectTotal.Width * fLook, rectTotal.Height);
                    //判断当前鼠标在哪个“按钮”范围内 
                  var Id=  this.dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    string sqlstr = "select * from externaldocuments where Id='"+Id+"'";
                    string fileFullPath = "";
                    var tempread = mysql.getmysqlread(sqlstr);
                    while (tempread.Read())
                    {
                        fileFullPath = tempread["FileRoute"].ToString();
                    }
                    try
                    {

                        if (rectPass.Contains(curPosition))
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(fileFullPath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("目录打开失败");
                            }
                        }
                        else if (rectRefuse.Contains(curPosition))
                        {
                            var Re = new ReceiveFile(Id);
                            Re.ShowDialog();
                            if (Re.DialogResult == DialogResult.OK)
                            {
                                this.dataGridView1.Rows.Clear();
                                datasource();
                            }
                        }
                        else if (rectLook.Contains(curPosition))
                        {
                            var result = MessageBox.Show("是否同时删除文件？", "删除提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                string FilePath = "";
                                string FileName = "";
                                string querystr = "Select * from externaldocuments where Id='" + Id + "'";
                                var quertem = mysql.getmysqlread(querystr);
                                while (quertem.Read())
                                {
                                    FilePath = quertem["FileRoute"].ToString();
                                    FileName = quertem["FileName"].ToString();
                                }
                                string strsql = "DELETE FROM externaldocuments WHERE Id = '" + Id + "';";
                                var tem = mysql.getmysqlcom(strsql);
                                if (tem)
                                {
                                    File.Delete(FilePath+ FileName);
                                    MessageBox.Show("删除成功！");
                                    this.dataGridView1.Rows.Clear();
                                    datasource();
                                }
                            }
                            else if (result == DialogResult.No)
                            {
                                string strsql = "DELETE FROM externaldocuments WHERE Id = '" + Id + "';";
                                var tem = mysql.getmysqlcom(strsql);
                                if (tem)
                                {
                                    MessageBox.Show("删除成功！");
                                    this.dataGridView1.Rows.Clear();
                                    datasource();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("出现错误");
                    }

                }
            }


        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //ToExcel(this.dataGridView1);
            var allopenform = new List<Form>();
            foreach (Form item in Application.OpenForms)
            {
                if (item.Name != "Form1")
                    allopenform.Add(item);
            }
            foreach (var item in allopenform)
            {
                item.Close();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var Re = new ReceiveFile(null);
            Re.ShowDialog();
            if (Re.DialogResult==DialogResult.OK)
            {
                this.dataGridView1.Rows.Clear();
                  datasource();
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("真的要退出程序吗？", "退出程序", MessageBoxButtons.OKCancel,MessageBoxIcon.Stop) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
         
            var ad = new AddUserForm();
            ad.ShowDialog();
            if (ad.DialogResult==DialogResult.OK)
            {
                MessageBox.Show("添加人员成功！"); 
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string defaultPath = "C:\\Users\\Administrator\\Desktop\\";//默认放在桌面
               string file = Application.ExecutablePath;
                Configuration config = ConfigurationManager.OpenExeConfiguration(file);
                foreach (string key in config.AppSettings.Settings.AllKeys)
                {
                    if (key == "path")
                    {
                        if (!string.IsNullOrEmpty(config.AppSettings.Settings["path"].Value.ToString()))
                        {
                            defaultPath = config.AppSettings.Settings["path"].Value.ToString();
                        }
                    }
                }
                FolderBrowserDialog path = new FolderBrowserDialog();
                path.Description = "请选择一个文件夹";
                if (defaultPath != "")
                {
                    //设置此次默认目录为上一次选中目录  
                    path.SelectedPath = defaultPath;
                }
                //按下确定选择的按钮  
                if (path.ShowDialog() == DialogResult.OK)
                {
                    //记录选中的目录  
                   defaultPath = path.SelectedPath;
                }
                else
                {
                    return;
                }
                var Filepath = defaultPath + "\\";
                bool exist = false;
                foreach (string key in config.AppSettings.Settings.AllKeys)
                {
                    if (key == "path")
                    {
                        exist = true;
                    }
                }
                if (exist)
                {
                    config.AppSettings.Settings.Remove("path");
                }
                config.AppSettings.Settings.Add("path", Filepath);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                MessageBox.Show("文件路径保存成功");
            }
            catch (Exception )
            {
                MessageBox.Show("文件路径保存失败，请重试！");
            }
          
        }
        public void ToExcel(DataGridView dataGrid)
        {
            try
            {
                //没有数据的话就不往下执行  
                if (dataGrid.Rows.Count == 0)
                    return;
                //实例化一个Excel.Application对象  
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写  
                excel.Visible = true;

                //新增加一个工作簿，Workbook是直接保存，不会弹出保存对话框，加上Application会弹出保存对话框，值为false会报错  
                excel.Application.Workbooks.Add(true);
                //生成Excel中列头名称  
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (this.dataGridView1.Columns[i].Visible == true)
                    {
                        excel.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                    }

                }
                //把DataGridView当前页的数据保存在Excel中  
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if (this.dataGridView1.Columns[j].Visible == true)
                        {
                            if (dataGridView1[j, i].ValueType == typeof(string))
                            {
                                excel.Cells[i + 2, j + 1] = "'" + dataGridView1[j, i].Value.ToString();
                            }
                            else
                            {
                                excel.Cells[i + 2, j + 1] = dataGridView1[j, i].Value.ToString();
                            }
                        }

                    }
                }

                //设置禁止弹出保存和覆盖的询问提示框  
                excel.DisplayAlerts = false;
                excel.AlertBeforeOverwriting = false;

                //保存工作簿  
                excel.Application.Workbooks.Add(true).Save();
                //保存excel文件  
                excel.Save(ReceiveFile.GetAppConfig());

                //确保Excel进程关闭  
                excel.Quit();
                excel = null;
                GC.Collect();//如果不使用这条语句会导致excel进程无法正常退出，使用后正常退出
                MessageBox.Show(this, "文件已经成功导出！", "信息提示");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误提示");
            }

        }
    }
}
