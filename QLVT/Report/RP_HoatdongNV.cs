using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.Report
{
    public partial class RP_HoatdongNV : DevExpress.XtraReports.UI.XtraReport
    {
        public RP_HoatdongNV()
        {
            InitializeComponent();
        }

        private void SummaryCalculatedAmount(object sender, DevExpress.XtraReports.UI.TextFormatEventArgs e)
        {
            decimal totalValue = Convert.ToDecimal(e.Value);
            e.Text = $"{totalValue:N0}.đ\n ({NumberToText(totalValue)} đồng)";
        }

        private string NumberToText(decimal input)
        {
            // định nghĩa kí tự
            string sNumber = input.ToString("#");
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };

            int ones, tens, hundreds;
            // duyệt số từ cuối quay lại đầu
            int digitPosition = sNumber.Length;

            // chuyển số sang chuỗi.
            string output = " ";
            if (digitPosition == 0) output = unitNumbers[0] + output;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;
                while (digitPosition > 0)
                {
                    // kiểm tra 3 số cuối cùng còn lại
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(digitPosition - 1, 1));
                    digitPosition--;
                    if (digitPosition > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(digitPosition - 1, 1));
                        digitPosition--;
                        if (digitPosition > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(digitPosition - 1, 1));
                            digitPosition--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        output = placeValues[placeValue] + output;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1)) output = "một " + output;
                    else
                    {
                        if ((ones == 5) && (tens > 0)) output = "lăm " + output;
                        else if (ones > 0) output = unitNumbers[ones] + " " + output;
                    }

                    if (tens < 0) break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) output = "lẻ " + output;
                        if (tens == 1) output = "mười " + output;
                        if (tens > 1) output = unitNumbers[tens] + " mươi " + output;
                    }

                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            output = unitNumbers[hundreds] + " trăm " + output;
                    }

                    output = " " + output;
                }
            }

            return CapitalizeFirstLetter(output.Trim());
        }

        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char firstChar = char.ToUpper(input[0]);

            if (input.Length > 1)
            {
                return firstChar + input.Substring(1);
            }
            else
            {
                return firstChar.ToString();
            }
        }


        public RP_HoatdongNV(string maNhanVien, DateTime fromDate, DateTime toDate)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.conStr;
            this.sqlDataSource1.Queries[0].Parameters[0].Value = maNhanVien;
            this.sqlDataSource1.Queries[0].Parameters[1].Value = fromDate;
            this.sqlDataSource1.Queries[0].Parameters[2].Value = toDate;
            this.sqlDataSource1.Fill();
            xrDate.Text = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + "đến ngày " + toDate.ToString("dd/MM/yyyy");
            DateTime currentDay = DateTime.Now;
            string formattedDate = "Ngày tạo báo cáo: " + currentDay.ToString("dd/MM/yyyy");
            xrReportDate.Text = formattedDate;
        }
    }

}
