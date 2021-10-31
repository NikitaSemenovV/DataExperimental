using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Numpy;


namespace DataExperimentalNew
{
    public partial class DataExperimentalNew : Form
    {
        public DataExperimentalNew()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Задание 1
        /// </summary>
        private List<double> Linear(int N, double a, double b)
        {
            List<double> x = new List<double>();
            for (double t = 1; t < N; t++)
            {
                double temp = a * t + b;
                x.Add(temp);
            }
            return x;
        }
        private List<double> Exponential(int N, double a, double b)
        {
            List<double> x = new List<double>();
            for (double t = 1; t < N; t++)
            {
                double temp = b * Math.Exp(-a * t);
                x.Add(temp);
            }
            return x;
        }
        /// <summary>
        /// Задание 2
        /// </summary>
        private List<double> EmbedRandom()
        {
            var min = -1;
            var max = 1;
            int N = 10000, S = 1;
            List<double> lis = new List<double>();
            Random rnd = new Random();
            for (double x = 0; x < N; x += 10)
            {
                double y = rnd.NextDouble() * (S * max - S * min) + S * min;
                lis.Add(y);
            }
            return lis;

        }
        private List<double> SelfRandom()
        {
            var min = -1;
            var max = 1;
            double mt = 0;
            int N = 10000, S = 1;
            double milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            double remainder = milliseconds % 10000;
            List<double> lis = new List<double>();
            for (double x = 0; x <= N; x += 10)
            {
                mt = mt + Math.Pow(remainder, 2) * 3 * 787;
                remainder = mt % 1000000000;
                double y = (remainder / 999999999) * (S * max - S * min) + S * min;
                lis.Add(y);
            }
            return lis;

        }
        /// <summary>
        /// Задание 3
        /// </summary>
        private void Harmonic()
        {
            List<double> x = EmbedRandom();
            var min = x.Min();
            textBox1.Text = min.ToString();
            var max = x.Max();
            textBox2.Text = max.ToString();
            var N = x.Count();
            ////////////////////////////////////////////
            double m = 0;
            for (int k = 0; k < N; k++)
            {
                m = x[k] + m;
            }
            double average = 1.0 / N * m;
            textBox3.Text = average.ToString();
            //////////////////////////////////////////// мат ожидание
            double Dn = 0;
            for (int k = 0; k < N; k++)
            {
                Dn = Dn + Math.Pow((x[k] - average), 2);
            }
            double D = 1.0 / N * Dn;
            textBox4.Text = D.ToString();
            //////////////////////////////////////////// дисперсия
            double gamma = Math.Sqrt(D);
            textBox5.Text = gamma.ToString();
            //////////////////////////////////////////// Стандартное отклонение
            double psin = 0;
            for (int k = 0; k < N; k++)
            {
                psin = psin + Math.Pow((x[k]), 2);
            }
            double psi = 1.0 / N * psin;
            textBox6.Text = psi.ToString();
            //////////////////////////////////////////// Средний квадрат
            double E = Math.Sqrt(psi);
            //////////////////////////////////////////// Средне квадрт ошибк
            //////////////////////////////////////////// 
            double asymmn = 0;
            for (int k = 0; k < N; k++)
            {
                asymmn = asymmn + Math.Pow((x[k] - average), 3);
            }
            double asymm = 1.0 / N * asymmn;
            textBox7.Text = asymm.ToString();
            //////////////////////////////////////////// Ассиметрия
            double asymm_k = asymm / Math.Pow(gamma, 3);
            textBox8.Text = asymm_k.ToString();
            //////////////////////////////////////////// коэфициент ассиметрии
            ///  //////////////////////////////////////////// 
            double excessn = 0;
            for (int k = 0; k < N; k++)
            {
                excessn = excessn + Math.Pow((x[k] - average), 4);
            }
            double excess = 1.0 / N * excessn;
            textBox9.Text = excess.ToString();
            //////////////////////////////////////////// эксцесс
            ////////////////////////////////////////////
            double curtosis = (excess / Math.Pow(gamma, 4)) - 3;
            textBox10.Text = curtosis.ToString();
            //////////////////////////////////////////// куртозис
            //LinearUpward(); /// график
        }

        private bool stationarity()
        {
            int M = 1000;
            int e = 10;
            List<double> x = EmbedRandom();
            var N = x.Count();
            // var prev_average = 0.0;
            //var prev_D = 0.0;
            List<double> Mlist = new List<double>();
            List<double> Dlist = new List<double>();
            double m = 0;
            double Dn = 0;
            for (int i = 0; i < M; i++)
            {
                //0-10 10-20 20-30
                for (int k = i * (N / M); k < i * N / M + N / M; k++)
                {
                    m = x[k] + m;
                }
                double average = m / N;
                Mlist.Add(average);
                //////////////////////////////////////////// мат ожидание
                for (int k = i * M; k < i * N / M + N / M; k++)
                {
                    Dn = Dn + Math.Pow((x[k] - average), 2);
                }
                double D = Dn / N;
                Dlist.Add(D);
                for (int j = 0; j < i - 1; j++)
                {
                    if (i != 0)
                    {
                        if ((Math.Max(Mlist[i], Mlist[j]) - Math.Min(Mlist[i], Mlist[j])) / Math.Max(Mlist[i], Mlist[j]) * 100 > e)
                        {
                            return false;
                        }
                        if ((Math.Max(Dlist[i], Dlist[j]) - Math.Min(Dlist[i], Dlist[j])) / Math.Max(Dlist[i], Dlist[j]) * 100 > e)
                        {
                            return false;
                        }

                    }
                }
            }
            return true;
        }
        
        /// <summary>
        /// Задание 4
        /// </summary>
        private List<double> avtocovariance(List<double> x)
        {
            double avgN = x.Average();
            double Num = x.Count();
            List<double> Y = new List<double>();

            for (int i = 0; i < Num; i++)
            {
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j < Num - i; j++)
                {
                    sum1 += (x[j] - avgN) * (x[j + i] - avgN);
                }
                for (int k = 0; k < Num; k++)
                {
                    sum2 += (x[k] - avgN) * (x[k] - avgN);
                }
                Y.Add(sum1 / sum2);
            }
            return Y;
        }
        private List<double> covariance(List<double> x, List<double> y)
        {
            double avgX = x.Average();
            double avgY = y.Average();
            double Num = x.Count();
            List<double> L = new List<double>();
            for (int i = 0; i < Num; i++)
            {
                double sum1 = 0;
                for (int j = 0; j < Num - i; j++)
                {
                    sum1 += (x[j] - avgX) * (y[j + i] - avgY);
                }
                L.Add(sum1 / Num);
            }
            return L;
        }
        private void density(List<double> prc, ref System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {

            int M = 20;
            var gist = np.histogram(np.array(prc.ToArray()), M);
            chart.Series[0].Points.Clear();
            for (int i = 0; i < gist.Item1.size; i++)
                chart.Series[0].Points.AddXY(gist.Item2[i].asscalar<double>(), gist.Item1[i].asscalar<double>());
        }

        /// <summary>
        /// Задание 5
        /// </summary>
        private List<double> Harm()
        {
            double N = 1000;
            double A = 5;
            double f = 5;
            double dt = 0.001;
            double y = 0;
            List<double> y1 = new List<double>();
            for (double i = 0; i <= N * dt; i += dt)
            {
                y = A * Math.Sin(2 * Math.PI * f * i);
                y1.Add(y);

            }
            return y1;
        }

        private List<double> polyHarm3()
        {
            double N = 1000;
            double[] A = new double[3] { 10, 100, 15 };
            double[] f = new double[3] { 15, 37, 134 };
            double dt = 0.001;
            double y = 0;
            List<double> y2 = new List<double>();
            for (double i = 0; i <= N * dt; i += dt)
            {
                y = 0;
                for (int j = 0; j < A.Length; j++)
                {
                    y = y + A[j] * Math.Sin(2 * Math.PI * f[j] * i);
                }
                y2.Add(y);
            }
            return y2;
        }
        /// <summary>
        /// Задание 6
        /// </summary>
        private List<double> Shift(List<double> x)
        {
            List<double> x2 = new List<double>();
            int cs = 10;
            for (int i = 0; i < x.Count; i++)
            {
                double g = x[i] + cs;
                x2.Add(g);
            }
            return x2;
        }

        private List<double> spikes(List<double> lst)
        {
            Random rnd = new Random();
            int cnt = rnd.Next(1, 5);
            for (int x = 0; x < cnt; x++)
            {
                double tmp = rnd.NextDouble() * (10000.0 - 0.2) + 0.2;
                lst[rnd.Next(0, lst.Count())] = tmp * (rnd.Next(1, 3) == 1 ? 1 : -1);
            }
            return lst;
        }
        /// <summary>
        /// Задание 7
        /// </summary>
        private List<double> antishift(List<double> x)
        {
            var tmp = Shift(x);
            List<double> x2 = new List<double>();
            double average = tmp.Average();
            for (int i = 0; i < x.Count; i++)
            {
                double t = tmp[i] - average;
                x2.Add(t);
            }
            return x2;
        }
        private List<double> antispike(List<double> x)
        {
            var tmp = spikes(x);
            for (int i = 1; i < tmp.Count - 1; i++)
            {
                if (Math.Abs(tmp[i] / tmp[i - 1]) > 50)
                    tmp[i] = (tmp[i - 1] + tmp[i + 1]) / 2.0;
                else
                    tmp[i] = tmp[i];
            }
            return tmp;
        }
        private List<double> additiveTrRand(List<double> x)
        {
            Random rnd = new Random();
            List<double> x2 = new List<double>();
            for (int i = 0; i < x.Count; i++)
            {
                int value = rnd.Next(30, 324);
                x[i] += value;
                x2.Add(x[i]);
            }
            return x2;
        }
        private List<double> HighlightingTrend(List<double> tmp)
        {
            double L = 10;
            int u = tmp.Count;
            List<double> y = new List<double>();
            for (int i = 0; i < tmp.Count - L; i++)
            {
                double sum = 0;
                for (int k = i; k < i + L; k++)
                {
                    sum += tmp[k];
                }
                y.Add(sum / L);
            }
            return y;
        }

        private List<double> HarmZad3(List<double> x)
        {
            int NS=10000;
            int N = 1000;
            Random rnd = new Random();
            
            for (int k=0; k<NS; k++)
            {
                double cnt = rnd.Next(-5, 5);
                List<double> p = new List<double>();
                List<double> tmp = EmbedRandom();
                for (int i=0; i<N;i++)
                {
                    p.Add(cnt + tmp[i]); 
                }
                for (int i = 0; i <N; i++)
                {
                    x[i] += p[i];
                }

            }
            for (int i = 0; i <N; i++)
            {
                x[i] /= NS;
            }
            return x;
        }


        //private List<double> Zad2() Смотри в фкнкции button8_Click
        //  private List<double> AntiTrend() Смотри в фкнкции button8_Click
        private void button1_Click(object sender, EventArgs e)
        {
            int N = 20;
            double a = 5, b = 3;
            List<double> answer1 = Linear(N, a, b);
            N = 20;
            a = -5; b = 3;
            List<double> answer2 = Linear(N, a, b);
            N = 20;
            a = 0.2; b = 1;
            List<double> answer3 = Exponential(N, a, b);
            N = 20;
            a = -0.2; b = 1;
            List<double> answer4 = Exponential(N, a, b);
            this.chart1.Series[0].Points.Clear();
            this.chart2.Series[0].Points.Clear();
            this.chart3.Series[0].Points.Clear();
            this.chart4.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart1.Series[0].Points.AddXY(i, answer1[i]);
            for (int i = 0; i < answer2.Count(); i++)
                this.chart2.Series[0].Points.AddXY(i, answer2[i]);
            for (int i = 0; i < answer3.Count(); i++)
                this.chart3.Series[0].Points.AddXY(i, answer3[i]);
            for (int i = 0; i < answer4.Count(); i++)
                this.chart4.Series[0].Points.AddXY(i, answer4[i]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<double> answer1 = EmbedRandom();
            List<double> answer2 = SelfRandom();
            this.chart5.Series[0].Points.Clear();
            this.chart6.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart5.Series[0].Points.AddXY(i, answer1[i]);
            for (int i = 0; i < answer2.Count(); i++)
                this.chart6.Series[0].Points.AddXY(i, answer2[i]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Harmonic();
            List<double> answer1 = EmbedRandom();
            this.chart7.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart7.Series[0].Points.AddXY(i, answer1[i]);
            if (stationarity())
            {
                MessageBox.Show("Стационарный");
            }
            else
            {
                MessageBox.Show("Не стационарный");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<double> answer1 = EmbedRandom();
            this.chart19.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart19.Series[0].Points.AddXY(i, answer1[i]);
            List<double> answer2 = EmbedRandom();
            this.chart20.Series[0].Points.Clear();
            for (int i = 0; i < answer2.Count(); i++)
                this.chart20.Series[0].Points.AddXY(i, answer2[i]);
            List<double> x1 = EmbedRandom();
            System.Threading.Thread.Sleep(50);
            List<double> y = EmbedRandom();
            List<double> L = covariance(x1, y);
            double Num1 = L.Count();
            this.chart10.Series[0].Points.Clear();
            for (int t = 0; t < Num1; t++)
            {
                this.chart10.Series[0].Points.AddXY(t, L[t]);
            }
            List<double> x = EmbedRandom();
            List<double> Y = avtocovariance(x);
            double Num = Y.Count();
            this.chart12.Series[0].Points.Clear();
            for (int t = 0; t < Num; t++)
            {
                textBox21.Text += Y[t].ToString() + " / ";
                this.chart12.Series[0].Points.AddXY(t, Y[t]);
            }
            density(EmbedRandom(), ref this.chart11);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<double> answer1 = Harm();
            this.chart14.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart14.Series[0].Points.AddXY(i, answer1[i]);
            List<double> answer2 = polyHarm3();
            this.chart15.Series[0].Points.Clear();
            for (int i = 0; i < answer2.Count(); i++)
                this.chart15.Series[0].Points.AddXY(i, answer2[i]);
            List<double> x = Harm();
            List<double> answer3 = avtocovariance(x);
            this.chart16.Series[0].Points.Clear();
            for (int i = 0; i < answer3.Count(); i++)
                this.chart16.Series[0].Points.AddXY(i, answer3[i]);
            List<double> x1 = Harm();
            List<double> y = polyHarm3();
            List<double> answer4 = covariance(x1, y);
            this.chart17.Series[0].Points.Clear();
            for (int i = 0; i < answer4.Count(); i++)
                this.chart17.Series[0].Points.AddXY(i, answer4[i]);
            List<double> x2 = polyHarm3();
            List<double> answer5 = avtocovariance(x2);
            this.chart13.Series[0].Points.Clear();
            for (int i = 0; i < answer5.Count(); i++)
                this.chart13.Series[0].Points.AddXY(i, answer5[i]);
            density(Harm(), ref this.chart18);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.chart22.Series[0].Points.Clear();
            this.chart21.Series[0].Points.Clear();
            List<double> x = Harm();
            var tmp = Shift(x);
            for (int i = 0; i < tmp.Count(); i++)
                this.chart22.Series[0].Points.AddXY(i, tmp[i]);
            tmp = spikes(tmp);
            for (int i = 0; i < tmp.Count(); i++)
                this.chart21.Series[0].Points.AddXY(i, tmp[i]);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.chart23.Series[0].Points.Clear();
            List<double> answer1 = Harm();
            var tmp = antishift(answer1);
            for (int i = 0; i < tmp.Count(); i++)
                this.chart23.Series[0].Points.AddXY(i, tmp[i]);

            this.chart24.Series[0].Points.Clear();
            List<double> answer2 = Harm();
            var tmp2 = antispike(answer2);
            for (int i = 0; i < tmp2.Count(); i++)
                this.chart24.Series[0].Points.AddXY(i, tmp2[i]);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.chart25.Series[0].Points.Clear();
            int N = 1000;
            double a = 4, b = 5;
            List<double> answer1 = Linear(N, a, b);
            var tmp = additiveTrRand(answer1);
            for (int i = 0; i < tmp.Count(); i++)
                this.chart25.Series[0].Points.AddXY(i, tmp[i]);
            this.chart26.Series[0].Points.Clear();
            var tmp2 = HighlightingTrend(answer1);
            for (int i = 0; i < tmp2.Count(); i++)
                this.chart26.Series[0].Points.AddXY(i, tmp2[i]);
            this.chart27.Series[0].Points.Clear();
            for (int i=0; i<tmp2.Count; i++) // antiTrend
            {
                var tmp3 = tmp[i] - tmp2[i];
                this.chart27.Series[0].Points.AddXY(i, tmp3);
            }
            this.chart28.Series[0].Points.Clear();
            List<double> answer2 = Harm();
            tmp2 = HarmZad3(answer2);
            for (int i = 0; i < tmp2.Count(); i++)
                this.chart28.Series[0].Points.AddXY(i, tmp2[i]);
        }
    }
}
