using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSubject
{
    /// <summary>
    /// 配置映射
    /// </summary>
    [Serializable]
    public class RuntimeConfig
    {
        string insert2, insert3, select2, select3;
        int lineCount = 5, resultMinValue, resultMaxValue, subjectCount;
        bool containZero;
        Dictionary<string, string[]> buttons2, buttons3;
        /// <summary>
        /// 插入2个数的题 SQL语句
        /// </summary>
        public string Insert2 { get => insert2; set => insert2 = value; }
        /// <summary>
        /// 插入3个数的题 SQL语句
        /// </summary>
        public string Insert3 { get => insert3; set => insert3 = value; }
        /// <summary>
        /// 查询2个数的题 SQL语句
        /// </summary>
        public string Select2 { get => select2; set => select2 = value; }
        /// <summary>
        /// 查询3个数的题 SQL语句
        /// </summary>
        public string Select3 { get => select3; set => select3 = value; }
        /// <summary>
        /// 每行题数
        /// </summary>
        public int LineCount { get => lineCount; set => lineCount = value; }
        /// <summary>
        /// 结果最小值
        /// </summary>
        public int ResultMinValue { get => resultMinValue; set => resultMinValue = value; }
        /// <summary>
        /// 结果最大值
        /// </summary>
        public int ResultMaxValue { get => resultMaxValue; set => resultMaxValue = value; }
        /// <summary>
        /// 每次出题数量
        /// </summary>
        public int SubjectCount { get => subjectCount; set => subjectCount = value; }
        /// <summary>
        /// 出题包含0值
        /// </summary>
        public bool ContainZero { get => containZero; set => containZero = value; }
        /// <summary>
        /// 2个数的出题按钮
        /// </summary>
        public Dictionary<string, string[]> Buttons2 { get => buttons2; set => buttons2 = value; }
        /// <summary>
        /// 3个数的出题按钮
        /// </summary>
        public Dictionary<string, string[]> Buttons3 { get => buttons3; set => buttons3 = value; }
    }
}
