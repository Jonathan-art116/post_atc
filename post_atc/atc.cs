using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace post_atc
{
    public partial class atc : Form
    {
        public atc()
        {
            InitializeComponent();
        }

        class read_config
        {
            public static string tbname;
            public static string result1 = "'Pass'";
            //public static string result2 = "'Fail'";
        }
  
        string connectString = null;
        MySqlConnection cnn;
        public void atc_Load(object sender, EventArgs e)
        {
            //string[] config = File.ReadAllLines(@"C:\config\config.ini");
            string ip = "10.18.255.116";
            string dbname = "deming";
            string usname = "deming";
            string pswd = "A8ZtK*7febEX";
            string pot = "3306";
            read_config.tbname = "rosetta_US130_TMO_2000_0917";
            connectString = "server=" + ip + ";database=" + dbname + ";uid=" + usname + ";pwd=" + pswd + ";port=" + pot;
            cnn = new MySqlConnection(connectString);
        }
    
       
        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("请输入正确的SN");
            }
            else
            {
                string _file = "C:\\orion\\rosetta-atc\\logs\\complete";
                string sn = textBox1.Text;
                try
                {
                    string[] files = Directory.GetFiles(_file, (sn + "*_APxResult.csv"));
                    List<string> ssss = files.OrderBy(ss => new FileInfo(ss).CreationTime).ToList();
                    string report = ssss.Last();
                    //MessageBox.Show(report);
                    string[] test = File.ReadAllLines(report);
                    string[] haha = test[3].Split(',');
                    if (haha[4] == "Failed")
                    {
                        //MessageBox.Show(haha[4]);
                        try
                        {
                            cnn.Open();
                            //string sn = textBox1.Text;
                            string select_sn = "select * from " + read_config.tbname + " where SN = " + "'" + sn + "'" + " and FullTestResult =" + read_config.result1;
                            MySqlCommand cmd1 = new MySqlCommand(select_sn, cnn);
                            if (cmd1.ExecuteScalar() == null)
                            {
                                MessageBox.Show("整机功能测试未完成");
                            }
                            else
                            {
                                string post = "update " + read_config.tbname + " set TestCaseATC = 'Fail' where SN=" + "'" + sn + "'";
                                MySqlCommand cmd = new MySqlCommand(post, cnn);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("上传成功!测试结果为Fail.");
                                textBox1.Text = "";
                            }
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            cnn.Close();
                        }
                    }
                    else if (haha[4] == "Passed")
                    {
                        //MessageBox.Show(haha[4]);
                        try
                        {
                            cnn.Open();
                            //string sn = textBox1.Text;
                            string select_sn = "select * from " + read_config.tbname + " where SN=" + "'" + sn + "'" + " and FullTestResult=" + read_config.result1;
                            MySqlCommand cmd1 = new MySqlCommand(select_sn, cnn);
                            if (cmd1.ExecuteScalar() == null)
                            {
                                MessageBox.Show("整机功能测试未完成");
                            }
                            else
                            {
                                string post = "update " + read_config.tbname + " set TestCaseATC = 'Pass' where SN=" + "'" + sn + "'";
                                MySqlCommand cmd = new MySqlCommand(post, cnn);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("上传成功!测试结果为Pass.");
                                textBox1.Text = "";
                            }
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            cnn.Close();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("请检查complete.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("请输入正确SN号");
            }
            else
            {
                try
                {
                    cnn.Open();
                    string sn = textBox1.Text;
                    string select_sn = "select * from "+ read_config.tbname + " where SN = " + "'" + sn + "'" + " and FullTestResult =" + read_config.result1;
                    MySqlCommand cmd1 = new MySqlCommand(select_sn, cnn);
                    if (cmd1.ExecuteScalar() == null)
                    {
                        MessageBox.Show("整机功能测试未完成");
                    }
                    else
                    {
                        string post = "update "+ read_config.tbname + " set TestCaseATC = 'Fail' where SN=" + "'" + sn +"'";
                        MySqlCommand cmd = new MySqlCommand(post, cnn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("上传成功!测试结果为Fail.");
                        textBox1.Text = "";
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cnn.Close();
                }
            }
        }
    }
}
