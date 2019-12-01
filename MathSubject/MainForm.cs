using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MathSubject
{
    public partial class MainForm : Form
    {
        readonly string MathSubject2;//2个数的和
        readonly string MathSubject3;//3个数的和
        readonly int maxValue = 10;
        readonly bool containZero = true;
        readonly int row_cell_count = 5;
        readonly Dictionary<string, object> config = new Dictionary<string, object>();
        readonly bool needInit = false;
        readonly int init_max = 10;
        public MainForm()
        {
            InitializeComponent();
            MathSubject2 = ConfigurationManager.AppSettings["subject2_table"];
            MathSubject3 = ConfigurationManager.AppSettings["subject3_table"];
            maxValue = int.Parse(ConfigurationManager.AppSettings["max"]);
            containZero = bool.Parse(ConfigurationManager.AppSettings["containZero"]);
            row_cell_count = int.Parse(ConfigurationManager.AppSettings["row_cell_count"]);
            needInit = bool.Parse(ConfigurationManager.AppSettings["needInit"]);
            init_max = int.Parse(ConfigurationManager.AppSettings["init_max"]);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (needInit)
            {
                Init2Number(init_max);
                Init3Number(init_max);
                //初始化完成，需要将配置文件修改位false，然后重启程序
                MessageBox.Show("请在下次启动程序前，将配置项needInit修改未false！", "初始化完成");
            }

            config["min"] = int.Parse(ConfigurationManager.AppSettings["min"]);
            config["max"] = maxValue;
            config["count"] = int.Parse(ConfigurationManager.AppSettings["count"]);

            Text = string.Format("{0}(最小和：{1}，最大和：{2}，总题数：{3}，{4})", Text, config["min"], config["max"], config["count"], containZero ? "包含0" : "不含0");

            string subject2 = ConfigurationManager.AppSettings["subject2"];
            Regex regex = new Regex("\"(.+)\":\\[(.+)\\]");
            var group = regex.Matches(subject2);
            var sql = @"SELECT v1,v2,v3 FROM " + MathSubject2 + " WHERE v3 between :min and :max AND ROWNUM <= :count " + (containZero ? "" : " and v1>0 and v2>0 and v3>0 ") + "ORDER BY DBMS_RANDOM.VALUE";
            AddSubjects(group, sql, "\t\t");

            string subject3 = ConfigurationManager.AppSettings["subject3"];
            group = regex.Matches(subject3);
            sql = @"SELECT v1,v2,v3,v4 FROM " + MathSubject3 + " WHERE v2<>v3 AND v4 between :min and :max AND ROWNUM <= :count " + (containZero ? "" : " and v1>0 and v2>0 and v3>0 and v4>0") + "ORDER BY DBMS_RANDOM.VALUE";
            AddSubjects(group, sql, "\t");
        }
        private void btn_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(subBox.Text);
        }

        #region 初始化题库
        /// <summary>
        /// 初始化2个数的加法
        /// </summary>
        private void Init2Number(int max)
        {
            using (var x = new DBContext())
            {
                x.Open();
                x.BeginTransaction();
                var sql = "insert into " + MathSubject2 + "(id,v1,v2,v3) values(:id,:v1,:v2,:v3)";
                var dic = new Dictionary<string, object>();
                for (int i = 0; i <= max; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        dic["id"] = string.Format("{0}_{1}", i, j);
                        dic["v1"] = i;
                        dic["v2"] = j;
                        dic["v3"] = i + j;
                        x.Execute(sql, dic);
                    }
                }
                x.Commit();
                x.Close();
            }
        }
        /// <summary>
        /// 初始化3个数的加法
        /// </summary>
        private void Init3Number(int max)
        {
            using (var x = new DBContext())
            {
                x.Open();
                x.BeginTransaction();
                var sql = "insert into " + MathSubject3 + "(id,v1,v2,v3,v4) values(:id,:v1,:v2,:v3,:v4)";
                var dic = new Dictionary<string, object>();
                for (int i = 0; i <= max; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        for (int k = 0; k <= j; k++)
                        {
                            dic["id"] = string.Format("{0}_{1}_{2}", i, j, k);
                            dic["v1"] = i;
                            dic["v2"] = j;
                            dic["v3"] = k;
                            dic["v4"] = i + j + k;
                            x.Execute(sql, dic);
                        }
                    }
                }
                x.Commit();
                x.Close();
            }
        }
        #endregion

        #region 添加出题按钮

        /// <summary>
        /// 添加出题控件
        /// </summary>
        void AddSubjects(MatchCollection group, string sql, string split)
        {
            foreach (Match item in group)
            {
                var text = item.Groups[1].Value;
                var subjects = item.Groups[2].Value.Split(',');
                for (int i = 0; i < subjects.Length; i++)
                {
                    subjects[i] = subjects[i].Trim().Trim('"');
                }
                btnPanel.Controls.Add(AddButton(string.Format(text, maxValue), sql, split, subjects));
            }
        }
        //按钮宽度
        int ButtonWidth = 140;
        /// <summary>
        /// 创建出题按钮
        /// </summary>
        Button AddButton(string text, string sql, string split, params string[] temp)
        {

            Button btn = new Button();
            btn.Text = text;
            btn.Width = ButtonWidth;
            btn.Click += (e, s) =>
            {
                using (var x = new DBContext())
                {
                    x.Open();
                    var tb = x.Query(sql, config);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(btn.Text);
                    Random r = new Random();
                    int i = 1;
                    foreach (DataRow item in tb.Rows)
                    {
                        var rv = r.Next(temp.Length);

                        sb.AppendFormat(temp[rv], item.ItemArray);

                        if (i % row_cell_count == 0)
                        {
                            sb.AppendLine();
                        }
                        else
                        {
                            sb.Append(split);
                        }
                        i++;
                    }
                    subBox.Text = sb.ToString();
                    x.Close();
                }
            };
            return btn;
        }
        #endregion

    }
}
