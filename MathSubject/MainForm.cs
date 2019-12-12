using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MathSubject
{
    /// <summary>
    /// 主程序窗口
    /// </summary>
    public partial class MainForm : Form
    {
        readonly bool needInit = false;
        readonly RuntimeConfig config;
        readonly Dictionary<string, object> valuePairs = new Dictionary<string, object>();
        /// <summary>
        /// 主程序窗口 构造函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            needInit = bool.Parse(ConfigurationManager.AppSettings["needInit"]);
            using (var sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "RuntimeConfig.json"))
            {
                try
                {
                    config = Newtonsoft.Json.JsonConvert.DeserializeObject<RuntimeConfig>(sr.ReadToEnd());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }
            }
            valuePairs["min"] = config.ResultMinValue;
            valuePairs["max"] = config.ResultMaxValue;
            valuePairs["count"] = config.SubjectCount;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (needInit)
            {
                Init2Number(config.ResultMaxValue);
                Init3Number(config.ResultMaxValue);
                //初始化完成，需要将配置文件修改位false，然后重启程序
                MessageBox.Show("请在下次启动程序前，将配置项needInit修改未false！", "初始化完成");
            }


            Text = string.Format("{0}(最小和：{1}，最大和：{2}，总题数：{3}，{4})", Text, config.ResultMinValue, config.ResultMaxValue, config.SubjectCount, config.ContainZero ? "包含0" : "不含0");

            AddSubjects(config.Buttons2, config.Select2, "\t\t");

            AddSubjects(config.Buttons3, config.Select3, "\t");
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

                var dic = new Dictionary<string, object>();
                for (int i = 0; i <= max; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        dic["id"] = string.Format("{0}_{1}", i, j);
                        dic["v1"] = i;
                        dic["v2"] = j;
                        dic["v3"] = i + j;
                        x.Execute(config.Insert2, dic);
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
                            x.Execute(config.Insert3, dic);
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
        void AddSubjects(Dictionary<string, string[]> buttons, string sql, string split)
        {
            foreach (var item in buttons)
            {
                btnPanel.Controls.Add(AddButton(string.Format(item.Key, config.ResultMaxValue), sql, split, item.Value));
            }
        }
        //按钮宽度
        int ButtonWidth = 140;
        /// <summary>
        /// 创建出题按钮
        /// </summary>
        Button AddButton(string text, string sql, string split, string[] temp)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Width = ButtonWidth;
            btn.Click += (e, s) =>
            {
                using (var x = new DBContext())
                {
                    x.Open();
                    var tb = x.Query(sql, valuePairs);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(btn.Text);
                    Random r = new Random();
                    int i = 1;
                    foreach (DataRow item in tb.Rows)
                    {
                        var rv = r.Next(temp.Length);

                        sb.AppendFormat(temp[rv], item.ItemArray);

                        if (i % config.LineCount == 0)
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
